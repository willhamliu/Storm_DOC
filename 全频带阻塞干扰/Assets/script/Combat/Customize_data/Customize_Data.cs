using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Customize_Data : MonoBehaviour
{
    public Text Data;
    public Text Price;
    public Text Point;
    public int point;
    public int point_buy;
    public int Purchase_index;

    public Text buy_panel_name;
    public Button buy_confrim;
    public Button buy_cancel;

    public GameObject buy;
    public GameObject attention_buy;
    public GameObject buy_panel;

    public Customize customize;
    public static Customize_Data Customize_data;
    private void Awake()
    {
       
        Customize_data = this;
        if (PlayerPrefs.HasKey("point"))
        {
            Point.text = PlayerPrefs.GetInt("point").ToString();
        }
        else
        {
            point = 200;
        }
        Point.text = point.ToString();

    }
    private void Start()
    {
        buy.GetComponent<Button>().onClick.AddListener(Purchase);
        buy_cancel.onClick.AddListener(Purchase_Canel);
        buy_confrim.onClick.AddListener(Purchase_Comfrim);
    }
    public void Purchase()
    {
        Audio_Management.Audio_management.SFXS_play("按钮点击");
        buy_panel_name.text = "是否研发\u3000" + this.customize.Customize_Name.ToString();
        buy_panel.SetActive(true);
        Config_item.Config_Item.Config_Item_Json();
    }
    public void Purchase_Canel()
    {
        Audio_Management.Audio_management.SFXS_play("返回");
        buy_panel.SetActive(false);
    }
   
    public void Purchase_Comfrim()
    {
        Audio_Management.Audio_management.SFXS_play("确认研发");

        Config_Customize.Config_customize.Purchase_Status_Modify(Purchase_index);
        buy_panel.SetActive(false);
        buy.SetActive(false);
        point_buy = point - customize.Customize_Price;
        StartCoroutine(use_point());

        PlayerPrefs.SetInt("point", point);
        PlayerPrefs.Save();
    }
    IEnumerator use_point()
    {
        while (point > point_buy)
        {
            point = point - 25;
            Point.text = point.ToString();
            yield return null;
        }
        yield return null;
    }
    public void SetData(ref Customize customize, ref int index)
    {
        Purchase_index = index;
        if (customize == null)
        {
            return;
        }
        this.customize = customize;
        Data.text = this.customize.Customize_Desc;
        Price.text = this.customize.Customize_Price.ToString();
       

        if (customize.Customize_Unlockindex !=-1)//如果上一级索引的装备未购买则无法购买当前装备
        {
            if (Config_Customize.Config_customize.Customize_Data_All[customize.Customize_Unlockindex].Customize_Purchase_Status == true)
            {
                if (Config_Customize.Config_customize.Customize_Data_All[index].Customize_Purchase_Status == true)
                {
                    buy.SetActive(false);
                    attention_buy.SetActive(false);
                }
                if (Config_Customize.Config_customize.Customize_Data_All[index].Customize_Purchase_Status == false)
                {
                    if (point < customize.Customize_Price)
                    {
                        buy.SetActive(false);
                        attention_buy.SetActive(false);
                    }
                    else
                    {
                        buy.SetActive(true);
                        attention_buy.SetActive(false);
                    }
                }
            }
            else
            {
                buy.SetActive(false);
                attention_buy.SetActive(true);
            }
        }
        else
        {
            if (Config_Customize.Config_customize.Customize_Data_All[index].Customize_Purchase_Status == true)
            {
                buy.SetActive(false);
                attention_buy.SetActive(false);
            }
            if (Config_Customize.Config_customize.Customize_Data_All[index].Customize_Purchase_Status == false)
            {
                if (point < customize.Customize_Price)
                {
                    buy.SetActive(false);
                    attention_buy.SetActive(false);
                }
                else
                {
                    buy.SetActive(true);
                    attention_buy.SetActive(false);
                }
            }
        }
    }
}
