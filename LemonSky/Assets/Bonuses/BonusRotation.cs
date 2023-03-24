using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BonusRotation : MonoBehaviour
{
    [SerializeField] private int speed = 100;
    [SerializeField] private int speedAfterDelay = 40;

    private void Start()
    {
        var rand = new System.Random();
        int delay = rand.Next(100, 1500);
        Task.Delay(delay).ContinueWith(t =>
        {
            speed = speedAfterDelay;
        });
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, speed, 0) * Time.deltaTime);
    }
}
