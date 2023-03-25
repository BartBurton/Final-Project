using System;
using System.Threading.Tasks;
using UnityEngine;

public class Levitate : MonoBehaviour
{
    private event Action OnMove;
    private event Action OnRotate;

    [Header("Rotate")]

    [SerializeField]
    [Tooltip("Использовать вращение")]
    private bool isRotate = false;

    [SerializeField]
    [ConditionalHide("isRotate", true)]
    [Tooltip("Использовать случайную задержку от 0.1 до 1.5 сек")]
    private bool randomRotateDelay = true;

    [SerializeField]
    [ConditionalHide("randomRotateDelay", true, true)]
    [Tooltip("Величина задержки в .сек")]
    private int rotateDelay = 0;

    [SerializeField]
    [ConditionalHide("isRotate", true)]
    [Tooltip("Направление вращения")]
    private Vector3 rotateDirection = new Vector3(0, 1, 0);

    [SerializeField]
    [ConditionalHide("isRotate", true)]
    [Tooltip("Скорость вращения")]
    private int rotateSpeed = 40;

    [SerializeField]
    [ConditionalHide("isRotate", true)]
    [Tooltip("Пространство, относительно которого происходит вращение")]
    private Space space = Space.Self;

    [Header("Move")]

    [SerializeField]
    [Tooltip("Использовать перемещение")]
    private bool isMove = false;

    [SerializeField]
    [ConditionalHide("isMove", true)]
    [Tooltip("Использовать случайную задержку от 0.1 до 1.5 сек")]
    private bool randomMoveDelay = true;

    [SerializeField]
    [ConditionalHide("randomMoveDelay", true, true)]
    [Tooltip("Величина задержки в .сек")]
    private int moveDelay = 0;

    [SerializeField]
    [ConditionalHide("isMove", true)]
    [Tooltip("Направление и дистанция перемещения")]
    private Vector3 move = new Vector3(0, 1, 0);

    [SerializeField]
    [ConditionalHide("isMove", true)]
    [Tooltip("Время до инверсии движения в .ceк")]
    private float moveRepeatTime = 100;
    private Vector3 rotate;
    private float repeatTime;


    private void Start()
    {
        if (isRotate)
        {
            RotateDelay();
            OnRotate += Rotate;
        }
        if (isMove)
        {
            MoveDelay();
            OnMove += Move;
        }
    }

    void Update()
    {
        OnRotate?.Invoke();
        OnMove?.Invoke();
    }

    void Rotate()
    {
        transform.Rotate(rotateDirection.normalized * rotateSpeed * Time.deltaTime, space);
    }
    void Move()
    {
        transform.Translate(move * Time.deltaTime);
        repeatTime -= Time.deltaTime;
        if (repeatTime <= 0f)
        {
            repeatTime = moveRepeatTime;
            move = -move;
        }
    }

    void RotateDelay()
    {
        var targetSpeed = rotateSpeed;
        rotateSpeed = 0;
        Task.Delay(randomRotateDelay ? new System.Random().Next(100, 1500) : rotateDelay * 1000).ContinueWith(t =>
        {
            rotateSpeed = targetSpeed;
        });
    }

    void MoveDelay()
    {
        var targetMove = move;
        move = new Vector3(0, 0, 0);
        repeatTime = moveRepeatTime;
        Task.Delay(randomMoveDelay ? new System.Random().Next(100, 1500) : moveDelay * 1000).ContinueWith(t =>
        {
            move = targetMove;
            repeatTime = moveRepeatTime;
        });
    }
}
