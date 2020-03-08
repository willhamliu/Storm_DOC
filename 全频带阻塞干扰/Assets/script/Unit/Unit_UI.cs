using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 对指定单位特殊的UI信息进行管理
/// </summary>
public class Unit_UI : Unit_PrototypeUI
{
    public Transform morale_Create;
    public Transform moraleDisplay_Position;

    public GameObject morale_Prefabe;
    protected GameObject morale;

    public override void Create_UnityUI()
    {
        hp_Slider = Instantiate(hp_Slider_Prefabe, hp_Slider_Create);
        hp_Slider.transform.position = sliderDisplay_Position.position;
        slider = hp_Slider.GetComponent<Slider>();
        foreach (Transform effect in hp_Slider.transform)
        {
            if (effect.name== "Effect")
            {
                this.effect = effect.GetComponent<Image>();
            }
        }
        slider.value = effect.fillAmount = hp / max_HP;

        morale = Instantiate(morale_Prefabe, morale_Create);
        morale.transform.position = moraleDisplay_Position.position;
        morale.SetActive(false);
    }

    public void Update_UnityUIPosition()//更新血量条位置
    {
        hp_Slider.transform.position = sliderDisplay_Position.position;
        morale.transform.position = moraleDisplay_Position.position;
    }
}
