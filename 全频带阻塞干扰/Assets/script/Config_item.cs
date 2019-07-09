using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using LitJson;

public class Config_item    {

    public List<Item> Item_List_All = new List<Item>();
    public List<Item> Item_List_Building = new List<Item>();
    public List<Item> Item_List_People = new List<Item>();
    public List<Item> Item_List_Tank = new List<Item>();
    public List<Item> Item_List_Plane = new List<Item>();


    private JsonData list_Data;
    private string write_Data;

    private static Config_item config_Item;

    public static Config_item Config_Item
    {
        get
        {
            if (config_Item==null)
            {
                config_Item = new Config_item();
            }
            return config_Item;
        }
    }
    


    public void Configjson()
    {
#if UNITY_EDITOR_WIN
        list_Data = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + "/Item_Json.json", Encoding.GetEncoding("GB2312")));
#endif

#if UNITY_ANDROID
        //WWW wWW = new WWW("file://" + File.ReadAllText(Application.dataPath + "!assets/Item_Json.json", Encoding.GetEncoding("GB2312")));

        //list_Data = JsonMapper.ToObject(wWW); 

#endif
        Decodejson();
    }

    public void Decodejson()
    {
        for (int i = 0; i < list_Data.Count; i++)
        {
            string item_Model = list_Data[i]["Item_Model"].ToString();

            string item_Number = (string)list_Data[i]["Item_Number"];
            int item_Type = (int)list_Data[i]["Item_Type"];
            int item_Target = (int)list_Data[i]["Item_Target"];
            int item_Camp = (int)list_Data[i]["Item_Camp"];

            string item_Name = (string)list_Data[i]["Item_Name"];
            int item_AP = (int)list_Data[i]["Item_AP"];
            int item_Attack = (int)list_Data[i]["Item_Attack"];
            int item_Defense = (int)list_Data[i]["Item_Defense"];
            int iten_range = (int)list_Data[i]["Item_range"];

            int item_Price = (int)list_Data[i]["Item_Price"];
            int item_HP = (int)list_Data[i]["Item_HP"];
            int item_Vision = (int)list_Data[i]["Item_Vision"];

            string item_Desc = list_Data[i]["Item_Desc"].ToString();

          
            Item item = new Item(item_Model, item_Number, item_Type,item_Target,item_Camp,item_Name,item_AP, item_Attack, item_Defense, iten_range, item_Price,
                                  item_HP, item_Vision, item_Desc);

            Item_List_All.Add(item);

            if (item.Item_Type == Item.Type.建筑单位)
            {
                Item_List_Building.Add(item);
            }
            if (item.Item_Type == Item.Type.步兵单位)
            {
                Item_List_People.Add(item);
            }
            if (item.Item_Type == Item.Type.装甲单位)
            {
                Item_List_Tank.Add(item);
            }
            if (item.Item_Type == Item.Type.飞行单位)
            {
                Item_List_Plane.Add(item);
            }
        }
    }
}
