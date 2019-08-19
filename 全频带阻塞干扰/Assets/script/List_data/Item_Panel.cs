using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Panel : MonoBehaviour {

    //有2个管理所有兵种的列表，一个管理数据层，一个管理对象层
    //Item item;
    List<GameObject> All_Item = new List<GameObject>();
    List<Toggle> Camp;

    List<Item> Config_List_Building;
    List<Item> Config_List_People;
    List<Item> Config_List_Tank;
    List<Item> Config_List_Plane;

    public GameObject Data_Canvas;//控制描述淡入淡出的画布

    public GameObject Item_temp;//模板

    public Toggle Camp_Rus;
    public Toggle Camp_NATO;
   
    public Transform Building_item_create;
    public Transform People_item_create;
    public Transform Tank_item_create;
    public Transform Plane_item_create;

    int Last_Item_index=0;
    int Last_Camp_index = 0;
    public enum Camp_choose
    {
        UnKown,
        RUS,
        NATO
    }

    public static Item_Panel Item_panel;
  
    void Awake()
    {
        Config_item.Config_Item.Config_Item_Json();
        Config_List_Building = Config_item.Config_Item.Item_List_Building;
        Config_List_People = Config_item.Config_Item.Item_List_People;
        Config_List_Tank = Config_item.Config_Item.Item_List_Tank;
        Config_List_Plane = Config_item.Config_Item.Item_List_Plane;

        Item_panel = this;

        Camp_Rus.isOn = true;
        Data_Canvas.SetActive(false);
        Camp = new List<Toggle>();
        Camp.Add(Camp_Rus);
        Camp.Add(Camp_NATO);
    }

    void Start()
    {
        CreateAllitem();
        for (int i = 0; i < Camp.Count; i++)
        {
            var index = Camp.IndexOf(Camp[i]);

            Camp[i].onValueChanged.AddListener((bool value) => { Toggle_Camp( ref value, ref index); });
        }
        for (int i = 0; i < All_Item.Count; i++)
        {
            
            var item = Config_item.Config_Item.Item_List_All[i];
            var index = All_Item.IndexOf(All_Item[i]);
            All_Item[i].GetComponent<Toggle>().onValueChanged.AddListener((bool value) =>  { Data_toggle(ref value,ref index); });
        }

        All_Item[0].GetComponent<Toggle>().isOn = true;
    }


    public GameObject Createitem(Item.Type type)
    {
        if (type==Item.Type.建筑单位)
        {
            GameObject Building_Item_UI = GameObject.Instantiate(Item_temp, Building_item_create);
            return Building_Item_UI;
        }
        if (type == Item.Type.步兵单位)
        {
            GameObject People_Item_UI = GameObject.Instantiate(Item_temp, People_item_create);
            return People_Item_UI;
        }
        if (type == Item.Type.装甲单位)
        {
            GameObject Tank_Item_UI = GameObject.Instantiate(Item_temp, Tank_item_create);
            return Tank_Item_UI;
        }
        if (type == Item.Type.飞行单位)
        {
            GameObject Plane_Item_UI = GameObject.Instantiate(Item_temp, Plane_item_create);
            return Plane_Item_UI;
        }
       
        return null;
    }
   
    public void CreateAllitem()
    {
        Item_temp.SetActive(false);
        for (int i = 0; i < Config_List_Building.Count; i++)
        {
            GameObject Building_list_ui =Createitem(Item.Type.建筑单位);
            All_Item.Add(Building_list_ui);

            Item_Info info = Building_list_ui.GetComponent<Item_Info>();//找到当前组件下的脚本，进行传参赋值
            info.Namedata(this.Config_List_Building[i]);
        }
        for (int i = 0; i < Config_List_People.Count; i++)
        {
            GameObject People_list_ui = Createitem(Item.Type.步兵单位);
            All_Item.Add(People_list_ui);

            Item_Info info = People_list_ui.GetComponent<Item_Info>();
            info.Namedata(this.Config_List_People[i]);
        }
        for (int i = 0; i < Config_List_Tank.Count; i++)
        {
            GameObject Tank_list_ui = Createitem(Item.Type.装甲单位);
            All_Item.Add(Tank_list_ui);

            Item_Info info = Tank_list_ui.GetComponent<Item_Info>();
            info.Namedata(this.Config_List_Tank[i]);
        }
        for (int i = 0; i < Config_List_Plane.Count; i++)
        {
            GameObject Plane_list_ui = Createitem(Item.Type.飞行单位);
            All_Item.Add(Plane_list_ui);

            Item_Info info = Plane_list_ui.GetComponent<Item_Info>();
            info.Namedata(this.Config_List_Plane[i]);
        }
    }

    public void Toggle_Camp(ref bool value , ref int index)//阵营切换
    {
        if (Last_Camp_index !=index && value==true)
        {
            if (gameObject.activeInHierarchy == true)
            {
                Audio_Management.Audio_management.SFXS_play("阵营切换");
            }
            if (Camp_Rus.isOn == true)
            {
                Camp_show(Camp_choose.RUS);
            }
            if (Camp_NATO.isOn == true)
            {
                Camp_show(Camp_choose.NATO);
            }
            Last_Camp_index = index;
        }
    }
    public void Data_toggle(ref bool value ,ref int index)//信息面板切换
    {
        //由于当toggle有变动时才会调用，所以不用担心由于切换阵营后ison关闭
        if (Last_Item_index != index && value==true)
        {
            if (All_Item[Last_Item_index].GetComponent<Toggle>().isOn == true && All_Item[Last_Item_index].activeInHierarchy == false)
            {
                All_Item[Last_Item_index].GetComponent<Toggle>().isOn = false;
            }
            if (gameObject.activeInHierarchy==true)
            {
                Audio_Management.Audio_management.SFXS_play("单位切换");
                StartCoroutine(Data_Toggle(index, Last_Item_index));
            }
            Last_Item_index = index;
        }
    }

    public void Camp_show( Camp_choose camp)//阵营显示
    {
        if (camp == Camp_choose.RUS)
        {
            for (int i = 0; i < All_Item.Count; i++)
            {
                if (All_Item[i].GetComponent<Item_Info>().Item_Camp == Item.Camp.俄罗斯.ToString())
                {
                   
                    All_Item[i].SetActive(true);
                }
                else
                {
                    All_Item[i].SetActive(false);
                }
            }
        }
        if (camp == Camp_choose.NATO)
        {
            for (int i = 0; i < All_Item.Count; i++)
            {
                if (All_Item[i].GetComponent<Item_Info>().Item_Camp == Item.Camp.北约.ToString())
                {
                    All_Item[i].SetActive(true);
                }
                else
                {
                    All_Item[i].SetActive(false);
                }
            }
        }
    }

    public void Open_list()//打开图鉴
    {
        Camp_show(Camp_choose.RUS);
        Item_Detail.Item_detail.SetData(Config_item.Config_Item.Item_List_All[0]);
        Item_Model.Item_model.Model_display(0, 0, "R_机场");
    }

    public void Close_list()//关闭图鉴
    {
        Camp_NATO.isOn = false;
        Camp_Rus.isOn = true;
        Camp[0].isOn = true;
        All_Item[0].GetComponent<Toggle>().isOn = true;
    }


    public IEnumerator Data_Toggle(int index, int last_index)//单位切换时的淡入淡出效果
    {
        float a = 0;
        float b = 255;
        while (b >= 0)
        {
            Data_Canvas.SetActive(true);
            a = a + 25;

            if (a < 225)
            {
                Data_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, a / 255);
            }
            else
            {
                b = b - 25;
                Data_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, b / 255);
            }
            if (a==250)
            {
                var item = Config_item.Config_Item.Item_List_All[index];
                Item_Detail.Item_detail.SetData(item);
                Item_Model.Item_model.Model_display(last_index, index, item.Item_Model);
            }
            if (b < 0)
            {
                Data_Canvas.SetActive(false);
            }
            yield return null;
        }
        yield return null;
    }
}
