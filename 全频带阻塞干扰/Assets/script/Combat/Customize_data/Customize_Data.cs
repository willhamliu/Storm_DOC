using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 单位升级面板的UI逻辑
/// </summary>
public class Customize_Data : MonoBehaviour
{
    public static Customize_Data instance;
    public Text data;
    public Text price;
    public Text point_Count;
    int point;
    public int point_Buy;
    public int purchase_Index;

    public Text buy_Panel_Name;

    public GameObject buy;
    public GameObject item_Price;
    public GameObject attention_Buy;
    public GameObject buy_Panel;

    public Customize customize;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (PlayerPrefs.HasKey("point"))
        {
            point = PlayerPrefs.GetInt("point");
            point_Count.text = PlayerPrefs.GetInt("point").ToString();
        }
        else
        {
            point = 5000;
            PlayerPrefs.SetInt("point", point);
            PlayerPrefs.Save();
        }
        point_Count.text = PlayerPrefs.GetInt("point").ToString();
    }
    private void Start()
    {
        UI_Management.instance.AddButtonEventTrigger<Button>("Buy_Button", PurchaseOnClick, "按钮点击", Audio_Management.instance.SFXS_play);
        UI_Management.instance.AddButtonEventTrigger<Button>("cancel", Purchase_CanelOnClick, "返回", Audio_Management.instance.SFXS_play);
        UI_Management.instance.AddButtonEventTrigger<Button>("confrim", Purchase_ComfrimOnClick, "确认研发", Audio_Management.instance.SFXS_play);
    }
    public void PurchaseOnClick()
    {
        buy_Panel_Name.text = "是否研发\u3000" + this.customize.customize_Name.ToString();
        buy_Panel.SetActive(true);
    }
    public void Purchase_CanelOnClick()
    {
        buy_Panel.SetActive(false);
    }
   
    public void Purchase_ComfrimOnClick()
    {
        Config_Customize.Instance.Purchase_Status_Modify(purchase_Index);
        buy_Panel.SetActive(false);
        buy.SetActive(false);
        item_Price.SetActive(false);
        point_Buy = point - customize.customize_Price;
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
        purchase_Index = index;
        if (customize == null)
        {
            return;
        }
        this.customize = customize;
        data.text = this.customize.customize_Desc;
        price.text = this.customize.customize_Price.ToString();
       

        if (customize.customize_Unlockindex !=-1)//如果上一级索引的装备未购买则无法购买当前装备
        {
            if (Config_Customize.Instance.customize_DataAll[customize.customize_Unlockindex].ustomize_Purchase_Status == true)
            {
                if (Config_Customize.Instance.customize_DataAll[index].ustomize_Purchase_Status == true)
                {
                    item_Price.SetActive(false);
                    buy.SetActive(false);
                    attention_Buy.SetActive(false);
                }
                if (Config_Customize.Instance.customize_DataAll[index].ustomize_Purchase_Status == false)
                {
                    if (point < customize.customize_Price)
                    {
                        item_Price.SetActive(true);
                        buy.SetActive(false);
                        attention_Buy.SetActive(false);
                    }
                    else
                    {
                        item_Price.SetActive(true);
                        buy.SetActive(true);
                        attention_Buy.SetActive(false);
                    }
                }
            }
            else
            {
                buy.SetActive(false);
                item_Price.SetActive(true);
                attention_Buy.SetActive(true);
            }
        }
        else
        {
            if (Config_Customize.Instance.customize_DataAll[index].ustomize_Purchase_Status == true)
            {
                item_Price.SetActive(false);
                buy.SetActive(false);
                attention_Buy.SetActive(false);
            }
            if (Config_Customize.Instance.customize_DataAll[index].ustomize_Purchase_Status == false)
            {
                if (point < customize.customize_Price)
                {
                    Debug.Log(point+" "+customize.customize_Price);
                    item_Price.SetActive(true);
                    buy.SetActive(false);
                    attention_Buy.SetActive(false);
                }
                else
                {
                    item_Price.SetActive(true);
                    buy.SetActive(true);
                    attention_Buy.SetActive(false);
                }
            }
        }
    }
}
