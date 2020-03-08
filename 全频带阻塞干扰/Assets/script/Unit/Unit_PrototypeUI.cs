using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 对指定单位共有的UI信息进行管理
/// </summary>
public abstract class Unit_PrototypeUI : Unit_Prototype
{
    public Transform hp_Slider_Create;
    public Transform sliderDisplay_Position;

    public GameObject hp_Slider_Prefabe;//血量条预制体

    protected GameObject hp_Slider;
    protected Slider slider;
    protected Image effect;
    protected float Hurtspeed = 0.01f;//缓冲速度

    public abstract void Create_UnityUI();

    public void Update_HP()//更新血量
    {
        slider.value = hp / max_HP;
    }
}
