using UnityEngine;

public class ImpulsedPlatform : AutomaticPlatform
{
    [SerializeField][Min(1)] private float _fadeSpeed = 1;
    [SerializeField] private float _invisibilityDelay = 0;

    private float _currentInvisibilityDelay = 0;
    private Vector3 _disappearanceVector = new(-1, -1, -1);

    private Vector3 _originalScale;

    private void Start()
    {
        _originalScale = transform.localScale;
    }

    protected override void HandleAction()
    {
        if (_currentInvisibilityDelay < _invisibilityDelay)
        {
            if (transform.localScale.x > 0)
            {
                transform.localScale += _fadeSpeed * Time.deltaTime * _disappearanceVector;
            }
            else
            {
                gameObject.SetActive(false);
                _currentInvisibilityDelay += Time.deltaTime;
            }
        }
        else
        {
            gameObject.SetActive(true);

            if (transform.localScale.x < _originalScale.x)
            {
                transform.localScale += _fadeSpeed * Time.deltaTime * Vector3.one;
            }
            else
            {
                transform.localScale = _originalScale;
                _currentInvisibilityDelay = 0;
                base.HandleAction();
            }
        }
    }
}
