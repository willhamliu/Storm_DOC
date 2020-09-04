using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
/// <summary>
/// 图鉴面板UI逻辑
/// </summary>
public class Item_Panel : MonoBehaviour
{
    public static Item_Panel instance;
    //有2个管理所有兵种的列表，一个管理数据层，一个管理对象层
    List<GameObject> all_ItemObj = new List<GameObject>();
    List<Item> all_ItemData = new List<Item>();

    List<Item_Info> all_Item_Script = new List<Item_Info>();
    List<Toggle> camp = new List<Toggle>();

    public GameObject data_Canvas;//控制描述淡入淡出的画布

    public ToggleGroup unitItemGroup;

    public Toggle camp_Rus;
    public Toggle camp_NATO;
   
    public Transform building_Item_Create;
    public Transform people_Item_Create;
    public Transform tank_Item_Create;
    public Transform plane_Item_Create;

    int last_Item_Index=0;
    int last_Camp_Index = 0;
    bool isClose;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        Config_Item.Instance.Config_Item_Json();
        all_ItemData = Config_Item.Instance.item_List_All;
        data_Canvas.SetActive(false);
        camp.Add(camp_Rus);
        camp.Add(camp_NATO);
    }

    void Start()
    {
        CreateAllitem();
        for (int i = 0; i < camp.Count; i++)
        {
            var index = camp.IndexOf(camp[i]);
            camp[i].onValueChanged.AddListener((bool value) => { Toggle_CampOnClick(value, index); });
            UI_Management.instance.AddButtonEventTrigger<Toggle>(camp[i].gameObject, audioName: "阵营切换", callBackAudio: Audio_Management.instance.SFXS_play);
        }
        for (int i = 0; i < all_ItemObj.Count; i++)
        {
            var index = all_ItemObj.IndexOf(all_ItemObj[i]);
            all_ItemObj[i].GetComponent<Toggle>().group = unitItemGroup;
            all_ItemObj[i].GetComponent<Toggle>().onValueChanged.AddListener((bool value) => { Data_toggleOnClick(value, index); });
            UI_Management.instance.AddButtonEventTrigger<Toggle>(all_ItemObj[i], audioName: "单位切换", callBackAudio: Audio_Management.instance.SFXS_play);
        }
        camp[0].isOn = true;
        all_ItemObj[0].GetComponent<Toggle>().isOn = true;
    }
    public void CreateAllitem()
    {
        foreach (var item in all_ItemData)
        {
            Createitem(item.Item_Type, item);
        }
    }
    public void Createitem(Item.Type type,Item item)
    {
        Transform createParent = null;
        switch (type)
        {
            case Item.Type.建筑单位:
                createParent = building_Item_Create;
                break;
            case Item.Type.步兵单位:
                createParent = people_Item_Create;
                break;
            case Item.Type.装甲单位:
                createParent = tank_Item_Create;
                break;
            case Item.Type.飞行单位:
                createParent = plane_Item_Create;
                break;
        }

        Resources_Management.Instance.Load<GameObject>("UI/Unit", createParent, (itemToggle) =>
        {
            Item_Info info = itemToggle.GetComponent<Item_Info>();//找到当前组件下的脚本，进行传参赋值
            info.NameData(item);

            all_ItemObj.Add(itemToggle);
            all_Item_Script.Add(info);
        });
    }

    public void Open_list()//打开图鉴
    {
        isClose = false;
        Camp_show(Item.Camp.俄罗斯, false);
        Item_Detail.instance.SetData(Config_Item.Instance.item_List_All[0]);
        all_ItemObj[0].GetComponent<Toggle>().interactable = false;
        camp[0].interactable = false;
    }

    public void Close_list()//关闭图鉴
    {
        isClose = true;
        all_ItemObj[0].GetComponent<Toggle>().isOn = true;
        camp[0].isOn = true;
        last_Item_Index = 0;
    }

    public void Toggle_CampOnClick(bool value, int index)//阵营切换
    {
        if (last_Camp_Index !=index && value==true)
        {
            camp[last_Camp_Index].interactable = true;
            camp[index].interactable = false;

            if (!isClose)
            {
                if (camp_Rus.isOn == true)
                {
                    Camp_show(Item.Camp.俄罗斯, true);
                }
                if (camp_NATO.isOn == true)
                {
                    Camp_show(Item.Camp.北约, true);
                }
            }
            last_Camp_Index = index;
        }
    }
    public void Data_toggleOnClick(bool value, int index)//信息面板切换
    {
        if (last_Item_Index != index && value==true)
        {
            data_Canvas.SetActive(true);
            all_ItemObj[last_Item_Index].GetComponent<Toggle>().interactable = true;
            all_ItemObj[index].GetComponent<Toggle>().interactable = false;
            if (!all_ItemObj[last_Item_Index].activeInHierarchy || isClose)//强制关闭
            {
                all_ItemObj[last_Item_Index].GetComponent<Toggle>().isOn = false;
            }
            if (!isClose)
            {
                data_Canvas.GetComponent<Image>().DOColor(new Color(0, 0, 0, 1), 0.1f).OnComplete(() =>
                {
                    var item = Config_Item.Instance.item_List_All[index];
                    Item_Detail.instance.SetData(item);
                    last_Item_Index = index;
                    data_Canvas.GetComponent<Image>().DOColor(new Color(0, 0, 0, 0), 0.1f).OnComplete(() =>
                    {
                        data_Canvas.SetActive(false);
                    });
                });
            }
        }
    }

    public void Camp_show(Item.Camp camp, bool isFidein)
    {
        if (!isFidein)
        {
            for (int i = 0; i < all_ItemObj.Count; i++)
            {
                if (all_Item_Script[i].item_Camp == camp)
                {
                    all_ItemObj[i].SetActive(true);
                    all_Item_Script[i].blackImage.color = new Color(0, 0, 0, 0);
                }
                else
                {
                    all_ItemObj[i].SetActive(false);
                    all_Item_Script[i].blackImage.color = new Color(0, 0, 0, 1);
                }
            }
        }
        else
        {
            float waitTime_Building = 0f;
            float waitTime_People = 0f;
            float waitTime_Tank = 0f;
            float waitTime_plane = 0f;

            float increaseTime = 0.05f;
            for (int i = 0; i < all_ItemObj.Count; i++)
            {
                all_Item_Script[i].Hide();
            }
            for (int i = 0; i < all_ItemObj.Count; i++)
            {
                if (all_Item_Script[i].item_Camp == camp)
                {
                    switch (all_Item_Script[i].item_Type)
                    {
                        case Item.Type.建筑单位:
                            waitTime_Building += increaseTime;
                            StartCoroutine(all_Item_Script[i].DisPlay(waitTime_Building));
                            break;
                        case Item.Type.步兵单位:
                            waitTime_People += increaseTime;
                            StartCoroutine(all_Item_Script[i].DisPlay(waitTime_People));
                            break;
                        case Item.Type.装甲单位:
                            waitTime_Tank += increaseTime;
                            StartCoroutine(all_Item_Script[i].DisPlay(waitTime_Tank));
                            break;
                        case Item.Type.飞行单位:
                            waitTime_plane += increaseTime;
                            StartCoroutine(all_Item_Script[i].DisPlay(waitTime_plane));
                            break;
                    }
                }
            }
        }
    }
}
