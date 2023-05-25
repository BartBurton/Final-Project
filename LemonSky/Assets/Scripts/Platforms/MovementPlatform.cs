using UnityEngine;

public class MovementPlatform : AutomaticPlatform
{
    [SerializeField][Min(1)] private float _movementSpeed = 1f;
    [SerializeField] private Vector3 _movementDirection = new(1, 0, 0);
    [SerializeField] private float _distance = 0;
    [SerializeField] private float _endpointCalmDelay = 0;

    private Vector3 _backMovementDirection;
    private Vector3 _currentMovementDirection;
    private float _currentDistance = 0;
    private float _currentEndpointCalmDelay = 0;
    private Vector3 _originalPosition;

    private MovementPlatformMovePlayer _playersMover;

    private void Awake()
    {
        _playersMover = GetComponent<MovementPlatformMovePlayer>();

        _backMovementDirection = new Vector3(
            -_movementDirection.x,
            -_movementDirection.y,
            -_movementDirection.z
        );

        _currentMovementDirection = _movementDirection;

        _originalPosition = transform.position;
    }

    protected override void HandleAction()
    {
        if (_currentEndpointCalmDelay != 0)
        {
            _currentEndpointCalmDelay += Time.deltaTime;
            if (_currentEndpointCalmDelay >= _endpointCalmDelay)
            {
                _currentEndpointCalmDelay = 0;
            }
            else
            {
                return;
            }
        };

        _playersMover.OffPlayersMovement();
        transform.Translate(_movementSpeed * Time.deltaTime * _currentMovementDirection);
        _playersMover.OnPlayersMovement(_movementSpeed * Time.deltaTime * _currentMovementDirection);

        var tempDistance = Vector3.Distance(_originalPosition, transform.position);

        if (_currentMovementDirection == _backMovementDirection && tempDistance > _currentDistance)
        {
            _currentDistance = 0;
            _currentMovementDirection = _movementDirection;
            transform.position = _originalPosition;
            base.HandleAction();
        }

        if (tempDistance >= _distance)
        {
            _currentEndpointCalmDelay += Time.deltaTime;
            _currentMovementDirection = _backMovementDirection;
        }

        _currentDistance = tempDistance;
    }

    public override void ActionStateUpdate(float actionStateDeltaTime)
    {
        _playersMover.Clear();
        base.ActionStateUpdate(actionStateDeltaTime);
    }
}
