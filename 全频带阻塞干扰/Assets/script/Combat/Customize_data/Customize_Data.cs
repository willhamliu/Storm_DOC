using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Customize_Data : MonoBehaviour
{
    public Text data;
    public Text price;
    public Text point_Count;
    int point;
    public int point_Buy;
    public int Purchase_Index;

    public Text buy_Panel_Name;
    public Button buy_Bonfrim;
    public Button buy_Cancel;

    public GameObject buy;
    public GameObject item_price;
    public GameObject attention_Buy;
    public GameObject buy_Panel;

    public Customize customize;
    public static Customize_Data Customize_data;
    private void Awake()
    {
       
        Customize_data = this;

        if (PlayerPrefs.HasKey("point"))
        {
            point = PlayerPrefs.GetInt("point");
            point_Count.text = PlayerPrefs.GetInt("point").ToString();
        }
        else
        {
            point = 3000;
            PlayerPrefs.SetInt("point", point);
            PlayerPrefs.Save();
        }
        point_Count.text = PlayerPrefs.GetInt("point").ToString();
    }
    private void Start()
    {
        buy.GetComponent<Button>().onClick.AddListener(Purchase);
        buy_Cancel.onClick.AddListener(Purchase_Canel);
        buy_Bonfrim.onClick.AddListener(Purchase_Comfrim);
    }
    public void Purchase()
    {
        Audio_Management.Audio_management.SFXS_play("按钮点击");
        buy_Panel_Name.text = "是否研发\u3000" + this.customize.Customize_Name.ToString();
        buy_Panel.SetActive(true);
        Config_Item.Config_item.Config_Item_Json();
    }
    public void Purchase_Canel()
    {
        Audio_Management.Audio_management.SFXS_play("返回");
        buy_Panel.SetActive(false);
    }
   
    public void Purchase_Comfrim()
    {
        Audio_Management.Audio_management.SFXS_play("确认研发");

        Config_Customize.Config_customize.Purchase_Status_Modify(Purchase_Index);
        buy_Panel.SetActive(false);
        buy.SetActive(false);
        item_price.SetActive(false);
        point_Buy = point - customize.Customize_Price;
        StartCoroutine(use_point());

        PlayerPrefs.SetInt("point", point);
        PlayerPrefs.Save();
    }
    IEnumerator use_point()
    {
        while (point > point_Buy)
        {
            point = point - 25;
            point_Count.text = point.ToString();
            yield return null;
        }
    }
  
    public void SetData(ref Customize customize, ref int index)
    {
        Purchase_Index = index;
        if (customize == null)
        {
            return;
        }
        this.customize = customize;
        data.text = this.customize.Customize_Desc;
        price.text = this.customize.Customize_Price.ToString();
       

        if (customize.Customize_Unlockindex !=-1)//如果上一级索引的装备未购买则无法购买当前装备
        {
            if (Config_Customize.Config_customize.customize_Data_All[customize.Customize_Unlockindex].Customize_Purchase_Status == true)
            {
                if (Config_Customize.Config_customize.customize_Data_All[index].Customize_Purchase_Status == true)
                {
                    item_price.SetActive(false);
                    buy.SetActive(false);
                    attention_Buy.SetActive(false);
                }
                if (Config_Customize.Config_customize.customize_Data_All[index].Customize_Purchase_Status == false)
                {
                    if (point < customize.Customize_Price)
                    {
                        item_price.SetActive(true);
                        buy.SetActive(false);
                        attention_Buy.SetActive(false);
                    }
                    else
                    {
                        item_price.SetActive(true);
                        buy.SetActive(true);
                        attention_Buy.SetActive(false);
                    }
                }
            }
            else
            {
                buy.SetActive(false);
                item_price.SetActive(true);
                attention_Buy.SetActive(true);
            }
        }
        else
        {
            if (Config_Customize.Config_customize.customize_Data_All[index].Customize_Purchase_Status == true)
            {
                item_price.SetActive(false);
                buy.SetActive(false);
                attention_Buy.SetActive(false);
            }
            if (Config_Customize.Config_customize.customize_Data_All[index].Customize_Purchase_Status == false)
            {
                if (point < customize.Customize_Price)
                {
                    Debug.Log(point+" "+customize.Customize_Price);
                    item_price.SetActive(true);
                    buy.SetActive(false);
                    attention_Buy.SetActive(false);
                }
                else
                {
                    item_price.SetActive(true);
                    buy.SetActive(true);
                    attention_Buy.SetActive(false);
                }
            }
        }
    }
}
