using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hud : MonoBehaviour
{
    public CoinsBar CoinsBar;
    public SliderBar JumpUpBar;
    public SliderBar ProtectUpBar;
    public SliderBar PowerUpBar;

    public static Hud Instance;

    private void Awake()
    {
        Instance = this;

    }

    private void Start()
    {
        CoinsBar.SetCount(0);
        JumpUpBar.Set(0);
        ProtectUpBar.Set(0);
        PowerUpBar.Set(0);
    }
}
