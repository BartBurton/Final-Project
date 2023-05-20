using StarterAssets;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Animations;

public class SpinningPlatform : AutomaticPlatform
{
    private enum Axis { X, Y, Z }

    [SerializeField] private int _turnsCount = 0;
    [SerializeField][Min(1)] private int _turnSpeed = 1;
    [SerializeField][Min(1)] private float _directionValue = 1;
    [SerializeField] private Axis _directionAxis = Axis.X;

    private float _currentTurnsCount = 0;
    private Vector3 _direction;
    private Quaternion _originalRotation;

    private void Start()
    {
        _direction = _directionAxis switch
        {
            Axis.X => new(_directionValue, 0, 0),
            Axis.Y => new(0, _directionValue, 0),
            Axis.Z => new(0, 0, _directionValue),
            _ => new(_directionValue, 0, 0),
        };

        _directionValue = Mathf.Abs(_directionValue);   

        _originalRotation = transform.rotation;
    }


    protected override void HandleAction()
    {
        transform.Rotate(_turnSpeed * Time.deltaTime * _direction);

        _currentTurnsCount += (_directionValue * _turnSpeed * Time.deltaTime);

        if (_currentTurnsCount / 360 >= _turnsCount)
        {
            _currentTurnsCount = 0;
            transform.rotation = _originalRotation;
            base.HandleAction();
        }
    }
}
