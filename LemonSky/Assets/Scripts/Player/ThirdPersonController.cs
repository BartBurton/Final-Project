using UnityEngine;
using System;
using System.Collections;
using Unity.Netcode;
using Cinemachine;

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
        public Vector3 GroundedOffset;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;


        private bool ForceGrounded = true;

        public Vector3 ForceGroundedOffset;

        public float ForceGroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Space(10)]
        public float ImpulseHeight;

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
        private PlayerAnimationManager _animationManager;
        private Player _player;

        private const float _threshold = 0.01f;


        [HideInInspector] public NetworkVariable<int> SkinType = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);


        private bool IsCurrentDeviceMouse => true;

        bool CanControl => GameManager.Instance.IsGamePlaying() && LocalUIManager.Instance.CurrentUIState == LocalUIManager.UIState.GamePlay;


        void InitSkin()
        {
            var skin = PlayerInitializer.Instance.GetSkin((PlayerType)SkinType.Value);
            Instantiate(skin, transform.GetChild(1));

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
        }

        private void Start()
        {
            InitSkin();

            _player = GetComponent<Player>();

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

            if (!CanControl)
            {
                VoidTransform();
                _animationManager.VoidAnimations(ForceGrounded);
            }
            else
            {
                HandleJumpServerAuth();
                HandleMovementServerAuth();
            }
        }

        private void LateUpdate()
        {
            if (CanControl)
            {
                CameraRotation();
            }

#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.F1))
            {
                Impulse(new Vector2(0, 1), 23.5f);
            }
#endif
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
            Vector3 groundedPosition = new(position.x - GroundedOffset.x, position.y - GroundedOffset.y, position.z - GroundedOffset.z);
            Vector3 hillDownCheckerPosition = new(position.x - ForceGroundedOffset.x, position.y - ForceGroundedOffset.y, position.z - ForceGroundedOffset.z);

            Grounded = Physics.CheckSphere(
                groundedPosition,
                GroundedRadius,
                GroundLayers,
                QueryTriggerInteraction.Ignore
            );

            ForceGrounded = Physics.CheckSphere(
                hillDownCheckerPosition,
                ForceGroundedRadius,
                GroundLayers,
                QueryTriggerInteraction.Ignore
            );

            if (Grounded)
            {
                _useJumpSpeedFactor = false;
            }

            if (ForceGrounded)
            {
                _isImpulsed = false;
            }

            _animationManager.PlayGrounded(ForceGrounded);
        }

        //[ServerRpc(RequireOwnership = false)]
        void HandleJumpServerRpcc(bool isJump)
        {
            if (ForceGrounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                _animationManager.PlayJump(false);
                _animationManager.PlayFreeFall(false);

                // Jump
                if (isJump && _jumpTimeoutDelta <= 0.0f)
                {
                    _useJumpSpeedFactor = true;
                    Jump(_player.JumpHeight * (GameInputs.Instance.IsSprint() ? JumpHeightSprintFactor : 1f));
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
                    _animationManager.PlayFreeFall(!ForceGrounded);
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
            if (ForceGrounded && !_isImpulsed)
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
            else
            {
                _currentDirection = new Vector2(direction.x, _currentDirection.y);
            }
        }

        public void SetSpeed(float speed)
        {
            _currentSpeed = speed;
        }

        public void Impulse(Vector2 direction, float speed)
        {
            _isImpulsed = true;
            ForceGrounded = false;
            Grounded = false;

            _currentDirection = direction;
            Jump(ImpulseHeight);
            SetSpeed(speed);

            HandleMovementServerAuth();
        }


        private void VoidTransform()
        {
            _controller.Move(new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            ApplyGravity();
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
            Gizmos.color = new(0.0f, 1.0f, 0.0f, 0.35f);

            Gizmos.DrawSphere(
                new Vector3(transform.position.x - GroundedOffset.x, transform.position.y - GroundedOffset.y, transform.position.z - GroundedOffset.z),
                GroundedRadius
            );


            Gizmos.color = new(1.0f, 0.0f, 0.0f, 0.35f);

            Gizmos.DrawSphere(
                new Vector3(transform.position.x - ForceGroundedOffset.x, transform.position.y - ForceGroundedOffset.y, transform.position.z - ForceGroundedOffset.z),
                ForceGroundedRadius
            );
        }
    }
}