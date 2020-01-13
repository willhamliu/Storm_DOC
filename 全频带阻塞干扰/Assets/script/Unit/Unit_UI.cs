using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit_UI : MonoBehaviour
{
    public Transform HP_Slider_create;
    public Transform Sliderdisplay_Position;
    public GameObject HP_Slider_Prefabe;//血量条预制体
    public float HP;
    public float MAX_HP;

    GameObject HP_Slider;
    Slider Slider;
    Image Effect;
    float Hurtspeed = 0.01f;//缓冲速度


    public void Create_HP()
    {
        HP_Slider = Instantiate(HP_Slider_Prefabe, HP_Slider_create);
        HP_Slider.transform.position = Sliderdisplay_Position.position;
        Slider = HP_Slider.GetComponent<Slider>();
        foreach (Transform effect in HP_Slider.transform)
        {
            if (effect.name== "Effect")
            {
                this.Effect = effect.GetComponent<Image>();
            }
        }


        Slider.value = Effect.fillAmount = HP / MAX_HP;
    }

    public void Update_Slider_Position()//更新血量
    {
        HP_Slider.transform.position = Sliderdisplay_Position.position;
    }

    public void Update_HP()//更新血量
    {
        Slider.value = HP / MAX_HP;
        StartCoroutine(Hit());
    }

    IEnumerator Hit()
    {
        while (Effect.fillAmount > Slider.value)
        {
            Effect.fillAmount -= Hurtspeed;
            yield return null;
        }
        Effect.fillAmount = Slider.value;
        if (Slider.value == 0)
        {
            Destroy(this.gameObject);
            Destroy(this.HP_Slider);
        }
    }
}
