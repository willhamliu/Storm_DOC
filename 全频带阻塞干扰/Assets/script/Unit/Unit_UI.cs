using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit_UI : MonoBehaviour
{
    public Transform hp_Slider_Create;
    public Transform sliderDisplay_Position;
    public Transform morale_Create;
    public Transform moraleDisplay_Position;

    public GameObject hp_Slider_Prefabe;//血量条预制体
    public GameObject morale_Prefabe;
    public float hp;
    public float max_HP;

    protected GameObject hp_Slider;
    protected GameObject morale;
    protected Slider slider;
    protected Image effect;
    float Hurtspeed = 0.01f;//缓冲速度


    public void Create_HP()
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

    public void Update_Slider_Position()//更新血量条位置
    {
        hp_Slider.transform.position = sliderDisplay_Position.position;
        morale.transform.position = moraleDisplay_Position.position;
    }

    public void Update_HP()//更新血量
    {
        slider.value = hp / max_HP;
        StartCoroutine(Hit());
    }

    IEnumerator Hit()
    {
        while (effect.fillAmount > slider.value)
        {
            effect.fillAmount -= Hurtspeed;
            yield return null;
        }
        effect.fillAmount = slider.value;
        if (effect.fillAmount == 0)
        {
            Destroy(this.gameObject);
            Destroy(this.hp_Slider);
        }
    }
}
