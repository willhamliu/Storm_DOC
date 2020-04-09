using System;
using System.Collections;
using System.Collections.Generic;
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
    List<GameObject> building_Item = new List<GameObject>();
    List<GameObject> people_Item = new List<GameObject>();
    List<GameObject> tank_Item = new List<GameObject>();
    List<GameObject> plane_Item = new List<GameObject>();
    List<Toggle> camp = new List<Toggle>();

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
        camp.Add(camp_Rus);
        camp.Add(camp_NATO);
    }

    void Start()
    {
        CreateAllitem();
        for (int i = 0; i < camp.Count; i++)
        {
            var index = camp.IndexOf(camp[i]);
            camp[i].group = camptoggleGroup.GetComponent<ToggleGroup>();
            camp[i].onValueChanged.AddListener((bool value) => { Toggle_CampOnClick(value, index); });
            UI_Management.instance.AddButtonEventTrigger<Toggle>(camp[i].gameObject, audioName: "阵营切换", callBackAudio: Audio_Management.instance.SFXS_play);
        }
        for (int i = 0; i < all_Item.Count; i++)
        {
            var index = all_Item.IndexOf(all_Item[i]);
            all_Item[i].GetComponent<Toggle>().group = unittoggleGroup.GetComponent<ToggleGroup>();
            all_Item[i].GetComponent<Toggle>().onValueChanged.AddListener((bool value) => { Data_toggleOnClick(value, index); });
            UI_Management.instance.AddButtonEventTrigger<Toggle>(all_Item[i], audioName: "单位切换", callBackAudio: Audio_Management.instance.SFXS_play);
        }
        camp[0].isOn = true;
        all_Item[0].GetComponent<Toggle>().isOn = true;
    }
 
    public void Createitem(Item.Type type,int index)
    {
        if (type == Item.Type.建筑单位)
        {
            Resources_Management.Instance.Load<GameObject>("UI/unit", (item) =>
            {
                all_Item.Add(item);
                building_Item.Add(item);

                Item_Info info = item.GetComponent<Item_Info>();//找到当前组件下的脚本，进行传参赋值
                info.NameData(this.config_List_Building[index]);
            },building_Item_Create);
        }
        if (type == Item.Type.步兵单位)
        {
            Resources_Management.Instance.Load<GameObject>("UI/unit", (item) =>
            {
                all_Item.Add(item);
                people_Item.Add(item);

                Item_Info info = item.GetComponent<Item_Info>();
                info.NameData(this.config_List_People[index]);
            },people_Item_Create);
        }
        if (type == Item.Type.装甲单位)
        {
            Resources_Management.Instance.Load<GameObject>("UI/unit", (item) =>
            {
                all_Item.Add(item);
                tank_Item.Add(item);

                Item_Info info = item.GetComponent<Item_Info>();
                info.NameData(this.config_List_Tank[index]);
            },tank_Item_Create);
        }
        if (type == Item.Type.飞行单位)
        {
            Resources_Management.Instance.Load<GameObject>("UI/unit", (item) =>
            {
                all_Item.Add(item);
                plane_Item.Add(item);     

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
            camp[last_Camp_Index].interactable = true;
            camp[index].interactable = false;

            if (camp[index].gameObject.activeInHierarchy == true)
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
            all_Item[last_Item_Index].GetComponent<Toggle>().interactable = true;
            all_Item[index].GetComponent<Toggle>().interactable = false;

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

    public void Open_list()//打开图鉴
    {
        Camp_show(Item.Camp.俄罗斯, false);
        Item_Detail.instance.SetData(Config_Item.Instance.item_List_All[0]);
        Item_Model_Management.instance.Model_display(0, 0, "R_机场");
        all_Item[0].GetComponent<Toggle>().interactable = false;
        camp[0].interactable = false;
    }

    public void Close_list()//关闭图鉴
    {
        camp[0].isOn = true;
        camp[1].isOn = false;
        all_Item[0].GetComponent<Toggle>().isOn = true;
    }

    public void Camp_show(Item.Camp camp, bool isFidein)
    {
        if (!isFidein)
        {
            for (int i = 0; i < all_Item.Count; i++)
            {
                if (all_Item[i].GetComponent<Item_Info>().item_Camp == camp.ToString())
                {
                    all_Item[i].SetActive(true);
                    all_Item[i].GetComponent<Item_Info>().blackImage.color = new Color(0, 0, 0, 0);
                }
                else
                {
                    all_Item[i].SetActive(false);
                    all_Item[i].GetComponent<Item_Info>().blackImage.color = new Color(0, 0, 0, 1);
                }
            }
        }
        else
        {
            int latencyTime_Building = 0;
            int latencyTime_People = 0;
            int latencyTime_Tank = 0;
            int latencyTime_plane = 0;
            for (int i = 0; i < all_Item.Count; i++)
            {
                if (all_Item[i].GetComponent<Item_Info>().item_Camp != camp.ToString())
                {
                    all_Item[i].SetActive(false);
                    all_Item[i].GetComponent<Item_Info>().blackImage.color = new Color(0, 0, 0, 1);
                }
            }
            for (int i = 0; i < all_Item.Count/2; i++)
            {
                if (i < building_Item.Count && building_Item[i].GetComponent<Item_Info>().item_Camp == camp.ToString())
                {
                    StartCoroutine(Fading(building_Item[i],latencyTime_Building++));
                }
                if (i < people_Item.Count && people_Item[i].GetComponent<Item_Info>().item_Camp == camp.ToString())
                {
                    StartCoroutine(Fading(people_Item[i],latencyTime_People++));
                }
                if (i < tank_Item.Count && tank_Item[i].GetComponent<Item_Info>().item_Camp == camp.ToString())
                {
                    StartCoroutine(Fading(tank_Item[i],latencyTime_Tank++));

                }
                if (i < plane_Item.Count && plane_Item[i].GetComponent<Item_Info>().item_Camp == camp.ToString())
                {
                    StartCoroutine(Fading(plane_Item[i],latencyTime_plane++));
                }
            }
        }
    }

    public IEnumerator Fading(GameObject obj,float latencyTime)
    {
        yield return new WaitForSeconds(latencyTime/15);
        obj.SetActive(true);
        float alpha = 1;
        while (alpha >= 0)
        {
            alpha -= Time.deltaTime * 5;
            obj.GetComponent<Item_Info>().blackImage.color = new Color(0, 0, 0, alpha);
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
