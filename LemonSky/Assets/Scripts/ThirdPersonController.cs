using UnityEngine;
using System;
using System.Collections;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using Unity.Netcode;
using Cinemachine;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
    public class ThirdPersonController : NetworkBehaviour
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        public float JumpMoveSpeedFactor = 2.0f;

        public float JumpSprintSpeedFactor = 1.2f;

        public float JumpHeightSprintFactor = 1.26f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;

        private float _currentSpeed = 0f;
        private bool _useJumpSpeedFactor = false;
        private Vector2 _currentDirection = Vector2.zero;

        private bool _isImpulsed = false;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;


        private CharacterController _controller;
        private GameObject _mainCamera;
        private Player _player;
        private PlayerSkills _playerSkills;
        private PlayerAnimationManager _animationManager;

        private const float _threshold = 0.01f;

        [HideInInspector] public NetworkVariable<int> SkinType = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        private bool IsCurrentDeviceMouse
        {
            get
            {
                return true;
            }
        }



        void InitSkin()
        {
            var skin = PlayerInitializer.Instance.GetSkin((PlayerType)SkinType.Value);
            Instantiate(skin, transform.GetChild(1));

            GetComponent<PlayerEffectsManager>().SetModelSkin(skin);

            var animator = gameObject.AddComponent<Animator>();
            animator.avatar = skin.GetComponent<Animator>().avatar;
            animator.runtimeAnimatorController = skin.GetComponent<Animator>().runtimeAnimatorController;
        }



        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }

            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            _controller = GetComponent<CharacterController>();
            _player = GetComponent<Player>();
            _playerSkills = GetComponent<PlayerSkills>();
        }

        private void Start()
        {
            InitSkin();

            _animationManager = GetComponent<PlayerAnimationManager>();
            _animationManager.AssignAnimations();

            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

            if (IsOwner)
            {
                GameObject.FindWithTag("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>().Follow =
                    transform.GetChild(0).transform;
            }
        }

        private void Update()
        {
            if (!IsOwner) return;

            GroundedCheckServerAuth();

            if (!GameManager.Instance.IsGamePlaying() || LocalUIManager.Instance.CurrentUIState == LocalUIManager.UIState.Paused)
            {
                VoidTransform();
                _animationManager.VoidAnimations(Grounded);
                return;
            }

            HandleJumpServerAuth();
            HandleMovementServerAuth();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }



        void GroundedCheckServerAuth()
        {
            GroundedCheckServerRpcc(transform.position);
        }

        void HandleJumpServerAuth()
        {
            HandleJumpServerRpcc(GameInputs.Instance.IsJump());
        }

        void HandleMovementServerAuth()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            ApplyMove(GameInputs.Instance.MoveVector(), GameInputs.Instance.IsSprint());

            HandleMovementServerRpcc(
                _currentDirection,
                _currentSpeed,
                _mainCamera.transform.eulerAngles.y
            );
        }


        //[ServerRpc(RequireOwnership = false)]
        void GroundedCheckServerRpcc(Vector3 position)
        {
            // set sphere position, with offset
            Vector3 spherePosition = new(position.x, position.y - GroundedOffset, position.z);

            Grounded = Physics.CheckSphere(
                spherePosition,
                GroundedRadius,
                GroundLayers,
                QueryTriggerInteraction.Ignore
            );

            if (Grounded)
            {
                _isImpulsed = false;
                _useJumpSpeedFactor = false;
            }

            _animationManager.PlayGrounded(Grounded);
        }

        //[ServerRpc(RequireOwnership = false)]
        void HandleJumpServerRpcc(bool isJump)
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                _animationManager.PlayJump(false);
                _animationManager.PlayFreeFall(false);

                // Jump
                if (isJump && _jumpTimeoutDelta <= 0.0f)
                {
                    _useJumpSpeedFactor = true;
                    Jump(JumpHeight * (GameInputs.Instance.IsSprint() ? JumpHeightSprintFactor : 1f));
                    _animationManager.PlayJump(true);
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    _animationManager.PlayFreeFall(true);
                }
            }

            ApplyGravity();
        }

        //[ServerRpc(RequireOwnership = false)]
        void HandleMovementServerRpcc(Vector2 direction, float targetSpeed, float cameraRotation)
        {
            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (direction == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationManager.AnimationBlend = Mathf.Lerp(_animationManager.AnimationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationManager.AnimationBlend < 0.01f) _animationManager.AnimationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(direction.x, 0.0f, direction.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (direction != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  cameraRotation;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(
                targetDirection.normalized *
                (_speed * Time.deltaTime) +
                new Vector3(0.0f, _verticalVelocity, 0.0f) *
                Time.deltaTime
            );

            _animationManager.PlayMove();
        }



        private void ApplyGravity()
        {
            if (Grounded)
            {
                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }
            }
            else
            {
                // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        public void Jump(float height)
        {
            // the square root of H * -2 * G = how much velocity needed to reach desired height
            _verticalVelocity = Mathf.Sqrt(height * -2f * Gravity);
        }


        private void ApplyMove(Vector2 direction, bool isSprint)
        {

            if (Grounded)
            {
                if (isSprint)
                {
                    SetSpeed(SprintSpeed * (_useJumpSpeedFactor ? JumpSprintSpeedFactor : 1));
                }
                else
                {
                    SetSpeed(MoveSpeed * (_useJumpSpeedFactor ? JumpMoveSpeedFactor : 1));
                }
            }

            if (!_isImpulsed)
            {
                _currentDirection = direction;
            }
        }

        public void SetSpeed(float speed)
        {
            _currentSpeed = speed;
        }


        [ServerRpc(RequireOwnership = false)]
        public void ImpulseServerRpc(Vector3 direction, ulong clientId)
        {
            if (!IsServer) return;

            ClientRpcParams clientRpcParams = new()
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { clientId }
                }
            };
            ImpulseClientRpc(direction, clientRpcParams);
        }

        [ClientRpc]
        public void ImpulseClientRpc(Vector3 direction, ClientRpcParams clientRpcParams = default)
        {
            if (!IsOwner) return;
            Impulse(direction, 20);
        }

        public void Impulse(Vector2 direction, float speed)
        {
            _isImpulsed = true;
            _currentDirection = direction;
            SetSpeed(speed);
        }


        private void VoidTransform()
        {
            _controller.Move(new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            ApplyGravity();
        }

        public void Teleportation(Vector3 newPos)
        {
#warning Добавить мигание персонажа
            _controller.enabled = false;
            transform.position = newPos;
            _controller.enabled = true;
        }


        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (GameInputs.Instance.LookVector().sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += GameInputs.Instance.LookVector().x * deltaTimeMultiplier;
                _cinemachineTargetPitch += GameInputs.Instance.LookVector().y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }


        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }
    }
}