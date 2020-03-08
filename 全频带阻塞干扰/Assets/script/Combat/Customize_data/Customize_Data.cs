using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 单位升级面板的UI逻辑
/// </summary>
public class Customize_Data : MonoBehaviour
{
    public Text data;
    public Text price;
    public Text point_Count;
    int point;
    public int point_Buy;
    public int purchase_Index;

    public Text buy_Panel_Name;
    public Button buy_Bonfrim;
    public Button buy_Cancel;

    public GameObject buy;
    public GameObject item_Price;
    public GameObject attention_Buy;
    public GameObject buy_Panel;

    public Customize customize;
    public static Customize_Data customize_Data;
    private void Awake()
    {
        customize_Data = this;
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
        buy.GetComponent<Button>().onClick.AddListener(PurchaseOnClick);
        buy_Cancel.onClick.AddListener(Purchase_CanelOnClick);
        buy_Bonfrim.onClick.AddListener(Purchase_ComfrimOnClick);
    }
    public void PurchaseOnClick()
    {
        Audio_Management.audio_Management.SFXS_play("按钮点击");
        buy_Panel_Name.text = "是否研发\u3000" + this.customize.customize_Name.ToString();
        buy_Panel.SetActive(true);
    }
    public void Purchase_CanelOnClick()
    {
        Audio_Management.audio_Management.SFXS_play("返回");
        buy_Panel.SetActive(false);
    }
   
    public void Purchase_ComfrimOnClick()
    {
        Audio_Management.audio_Management.SFXS_play("确认研发");

        Config_Customize.Config_customize.Purchase_Status_Modify(purchase_Index);
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
            if (Config_Customize.Config_customize.customize_DataAll[customize.customize_Unlockindex].ustomize_Purchase_Status == true)
            {
                if (Config_Customize.Config_customize.customize_DataAll[index].ustomize_Purchase_Status == true)
                {
                    item_Price.SetActive(false);
                    buy.SetActive(false);
                    attention_Buy.SetActive(false);
                }
                if (Config_Customize.Config_customize.customize_DataAll[index].ustomize_Purchase_Status == false)
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
            if (Config_Customize.Config_customize.customize_DataAll[index].ustomize_Purchase_Status == true)
            {
                item_Price.SetActive(false);
                buy.SetActive(false);
                attention_Buy.SetActive(false);
            }
            if (Config_Customize.Config_customize.customize_DataAll[index].ustomize_Purchase_Status == false)
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
