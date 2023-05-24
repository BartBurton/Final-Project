using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffectsHud : MonoBehaviour
{
    public SliderBar JumpUpBar;
    public SliderBar ProtectUpBar;
    public SliderBar PowerUpBar;

    public static AffectsHud Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        JumpUpBar.Set(0);
        ProtectUpBar.Set(0);
        PowerUpBar.Set(0);
    }
}
