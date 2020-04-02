﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 图鉴面板UI逻辑
/// </summary>
public class Item_Panel : MonoBehaviour
{
    public static Item_Panel instance;
    //有2个管理所有兵种的列表，一个管理数据层，一个管理对象层
    List<GameObject> all_Item = new List<GameObject>();
    List<GameObject> camp = new List<GameObject>();

    List<Item> config_List_Building;
    List<Item> config_List_People;
    List<Item> config_List_Tank;
    List<Item> config_List_Plane;

    public GameObject data_Canvas;//控制描述淡入淡出的画布

    public GameObject unittoggleGroup;
    public GameObject camptoggleGroup;

    public Toggle camp_Rus;
    public Toggle camp_NATO;
   
    public Transform building_Item_Create;
    public Transform people_Item_Create;
    public Transform tank_Item_Create;
    public Transform plane_Item_Create;

    int last_Item_Index=0;
    int last_Camp_Index = 0;
    public enum Camp_choose
    {
        UnKown,
        RUS,
        NATO
    }
  
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        Config_Item.Instance.Config_Item_Json();

        config_List_Building = Config_Item.Instance.item_List_Building;
        config_List_People = Config_Item.Instance.item_List_People;
        config_List_Tank = Config_Item.Instance.item_List_Tank;
        config_List_Plane = Config_Item.Instance.item_List_Plane;


        data_Canvas.SetActive(false);
        camp.Add(camp_Rus.gameObject);
        camp.Add(camp_NATO.gameObject);
    }

    void Start()
    {
        CreateAllitem();
        for (int i = 0; i < camp.Count; i++)
        {
            var index = camp.IndexOf(camp[i]);
            camp[i].GetComponent<Toggle>().group = camptoggleGroup.GetComponent<ToggleGroup>();
            camp[i].GetComponent<Toggle>().onValueChanged.AddListener((bool value) => { Toggle_CampOnClick(value, index); });
            UI_Management.instance.AddButtonEventTrigger(camp[i], audioName: "阵营切换", callBackAudio: Audio_Management.instance.SFXS_play);
        }
        for (int i = 0; i < all_Item.Count; i++)
        {
            var index = all_Item.IndexOf(all_Item[i]);
            all_Item[i].GetComponent<Toggle>().group = unittoggleGroup.GetComponent<ToggleGroup>();
            all_Item[i].GetComponent<Toggle>().onValueChanged.AddListener((bool value) => { Data_toggleOnClick(value, index); });
            UI_Management.instance.AddButtonEventTrigger(all_Item[i], audioName: "单位切换", callBackAudio: Audio_Management.instance.SFXS_play);
        }
        camp[0].GetComponent<Toggle>().isOn = true;
        all_Item[0].GetComponent<Toggle>().isOn = true;
    }
 
    public void Createitem(Item.Type type,int index)
    {
        if (type == Item.Type.建筑单位)
        {
            Resources_Management.Instance.Load<GameObject>("UI/unit", (item) =>
            {
                all_Item.Add(item);

                Item_Info info = item.GetComponent<Item_Info>();//找到当前组件下的脚本，进行传参赋值
                info.NameData(this.config_List_Building[index]);
            },building_Item_Create);
        }
        if (type == Item.Type.步兵单位)
        {
            Resources_Management.Instance.Load<GameObject>("UI/unit", (item) =>
            {
                all_Item.Add(item);

                Item_Info info = item.GetComponent<Item_Info>();
                info.NameData(this.config_List_People[index]);
            },people_Item_Create);
        }
        if (type == Item.Type.装甲单位)
        {
            Resources_Management.Instance.Load<GameObject>("UI/unit", (item) =>
            {
                all_Item.Add(item);

                Item_Info info = item.GetComponent<Item_Info>();
                info.NameData(this.config_List_Tank[index]);
            },tank_Item_Create);
        }
        if (type == Item.Type.飞行单位)
        {
            Resources_Management.Instance.Load<GameObject>("UI/unit", (item) =>
            {
                 all_Item.Add(item);

                 Item_Info info = item.GetComponent<Item_Info>();
                 info.NameData(this.config_List_Plane[index]);
            },plane_Item_Create);
        }
    }

    public void CreateAllitem()
    {
        for (int i = 0; i < config_List_Building.Count; i++)
        {
            Createitem(Item.Type.建筑单位,i);
        }
        for (int i = 0; i < config_List_People.Count; i++)
        {
            Createitem(Item.Type.步兵单位, i);
        }
        for (int i = 0; i < config_List_Tank.Count; i++)
        {
            Createitem(Item.Type.装甲单位, i);
        }
        for (int i = 0; i < config_List_Plane.Count; i++)
        {
            Createitem(Item.Type.飞行单位, i);
        }
    }

    public void Toggle_CampOnClick(bool value, int index)//阵营切换
    {
        if (last_Camp_Index !=index && value==true)
        {
            if (camp[index].gameObject.activeInHierarchy == true)
            {
                if (camp_Rus.isOn == true)
                {
                    Camp_show(Camp_choose.RUS,true);
                }
                if (camp_NATO.isOn == true)
                {
                    Camp_show(Camp_choose.NATO,true);
                }
            }
            last_Camp_Index = index;
        }
    }
    public void Data_toggleOnClick(bool value, int index)//信息面板切换
    {
        //由于当toggle有变动时才会调用，所以不用担心由于切换阵营后ison关闭
        if (last_Item_Index != index && value==true)
        {
            if (all_Item[last_Item_Index].GetComponent<Toggle>().isOn == true && all_Item[last_Item_Index].activeInHierarchy == false)
            {
                all_Item[last_Item_Index].GetComponent<Toggle>().isOn = false;
            }
          
            if (last_Item_Index != index && all_Item[index].activeInHierarchy==true)
            {
                StartCoroutine(Data_Toggle(index, last_Item_Index));
            }
            last_Item_Index = index;
        }
    }

    public void Camp_show( Camp_choose camp, bool isFidein)//阵营显示
    {
        if (camp == Camp_choose.RUS)
        {
            for (int i = 0; i < all_Item.Count; i++)
            {
                if (all_Item[i].GetComponent<Item_Info>().item_Camp == Item.Camp.俄罗斯.ToString())
                {
                    all_Item[i].SetActive(true);
                    if (isFidein)
                    {
                        StartCoroutine(Fidein(all_Item[i]));
                    }
                    else
                    {
                        all_Item[i].GetComponent<Item_Info>().blackImage.color = new Color(0, 0, 0, 0);
                    }
                }
                else
                {
                    all_Item[i].SetActive(false);
                    all_Item[i].GetComponent<Item_Info>().blackImage.color = new Color(0, 0, 0, 1);
                }
            }
        }
        else if (camp == Camp_choose.NATO)
        {
            for (int i = 0; i < all_Item.Count; i++)
            {
                if (all_Item[i].GetComponent<Item_Info>().item_Camp == Item.Camp.北约.ToString())
                {
                    all_Item[i].SetActive(true);
                    if (isFidein)
                    {
                        StartCoroutine(Fidein(all_Item[i]));
                    }
                    else
                    {
                        all_Item[i].GetComponent<Item_Info>().blackImage.color = new Color(0, 0, 0, 0);
                    }
                }
                else
                {
                    all_Item[i].SetActive(false);
                    all_Item[i].GetComponent<Item_Info>().blackImage.color = new Color(0, 0, 0, 1);
                }
            }
        }
    }

    public void Open_list()//打开图鉴
    {
        Camp_show(Camp_choose.RUS,false);
        Item_Detail.instance.SetData(Config_Item.Instance.item_List_All[0]);
        Item_Model_Management.instance.Model_display(0, 0, "R_机场");
    }

    public void Close_list()//关闭图鉴
    {
        camp[0].GetComponent<Toggle>().isOn = true;
        camp[1].GetComponent<Toggle>().isOn = false;
        all_Item[0].GetComponent<Toggle>().isOn = true;
    }

    public IEnumerator Fidein(GameObject obj)
    {
        float alpha = 1;
        while (alpha>=0)
        {
            alpha -= Time.deltaTime*2;
            obj.GetComponent<Item_Info>().blackImage.color= new Color(0,0,0, alpha);
            yield return null;
        }
    }

    public IEnumerator Data_Toggle(int index, int last_index)//单位切换时的淡入淡出效果
    {
        float a = 0;
        float b = 255;
        while (b >= 0)
        {
            data_Canvas.SetActive(true);
            a = a + 25;

            if (a < 225)
            {
                data_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, a / 255);
            }
            else
            {
                b = b - 25;
                data_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, b / 255);
            }
            if (a==250)
            {
                var item = Config_Item.Instance.item_List_All[index];
                Item_Detail.instance.SetData(item);
                Item_Model_Management.instance.Model_display(last_index, index, item.item_Model);
            }
            if (b < 0)
            {
                data_Canvas.SetActive(false);
            }
            yield return null;
        }
    }
}
