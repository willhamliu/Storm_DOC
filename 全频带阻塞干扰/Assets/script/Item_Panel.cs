using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Panel : MonoBehaviour {

    //有2个管理所有兵种的列表，一个管理数据层，一个管理对象层
    List<GameObject> All_Item ;

    List<Item> Config_List_Building;
    List<Item> Config_List_People;
    List<Item> Config_List_Tank;
    List<Item> Config_List_Plane;

    public AudioClip Toggle_AUD;
    public AudioClip Camp_AUD;

    public Item_Detail item_Detail;
    public Item_Model item_Model;
    public GameObject Data_Canvas;//控制描述淡入淡出的画布

    public GameObject Item_temp;//模板

    public Toggle Camp_Rus;
    public Toggle Camp_NATO;
   
    public Transform Building_item_create;
    public Transform People_item_create;
    public Transform Tank_item_create;
    public Transform Plane_item_create;

    int Last_index=0;
    public enum Camp_choose
    {
        UnKown,
        RUS,
        NATO
    }

    public static Item_Panel item_Panel;
  
    void Awake()
    {
        item_Panel = this;
        Data_Canvas.SetActive(false);
        All_Item = new List<GameObject>();

        Config_item.Config_Item.Configjson();
        Config_List_Building = Config_item.Config_Item.Item_List_Building;
        Config_List_People = Config_item.Config_Item.Item_List_People;
        Config_List_Tank = Config_item.Config_Item.Item_List_Tank;
        Config_List_Plane = Config_item.Config_Item.Item_List_Plane;
    }

    void Start()
    {
       
        CreateAllitem();

        Camp_NATO.onValueChanged.AddListener( delegate{ Toggle_Camp(); });
        for (int i = 0; i < All_Item.Count; i++)
        {
            
            var item = Config_item.Config_Item.Item_List_All[i];
            var index = All_Item.IndexOf(All_Item[i]);
            item_Model.Load_AB(item);
            All_Item[i].GetComponent<Toggle>().onValueChanged.AddListener((bool value) =>  { Data_toggle(ref item ,ref value,ref index); });
        }
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

    public void Toggle_Camp()//阵营切换
    {
        if (gameObject.activeInHierarchy == true)
        {
            AudioSource.PlayClipAtPoint(Camp_AUD, transform.position);
        }
        if (Camp_Rus.isOn == true)
        {
            Camp_show(Camp_choose.RUS);
        }
        else
        {
            Camp_show(Camp_choose.NATO);
        }
    }
    public void Data_toggle(ref Item item ,ref bool value ,ref int index)//信息面板切换
    {
        
        
        //由于当toggle有变动时才会调用，所以不用担心由于切换阵营后ison关闭
        if (Last_index != index && value==true)
        {
            if (All_Item[Last_index].GetComponent<Toggle>().isOn == true && All_Item[Last_index].activeInHierarchy == false)
            {
                All_Item[Last_index].GetComponent<Toggle>().isOn = false;
            }
            if (gameObject.activeInHierarchy==true)
            {
                AudioSource.PlayClipAtPoint(Toggle_AUD, transform.position);
                StartCoroutine(Data_Toggle(item, index, Last_index));
            }
            Last_index = index;
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
        Camp_Rus.isOn = true;
        Camp_show(Camp_choose.RUS);
        All_Item[0].GetComponent<Toggle>().isOn = true;
        item_Detail.SetData(Config_item.Config_Item.Item_List_All[0]);

        item_Model.Model_display(0, 0);
    }

    public void Close_list()//关闭图鉴
    {
        Camp_NATO.isOn = false;
        All_Item[0].GetComponent<Toggle>().isOn = true;
    }


    public IEnumerator Data_Toggle( Item item, int index, int last_index)
    {
        float a = 0;
        float b = 255;
        while (b >= 0)
        {
            Data_Canvas.SetActive(true);
            a = a + 15;

            if (a < 225)
            {
                Data_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, a / 255);
            }
            else
            {
                b = b - 15;
                Data_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, b / 255);
            }
            if (a==255)
            {
                item_Detail.SetData(item);
                item_Model.Model_display(last_index, index);
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
