using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using LitJson;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;

public class Config_item
{

    public List<Item> Item_List_All = new List<Item>();
    public List<Item> Item_List_Building = new List<Item>();
    public List<Item> Item_List_People = new List<Item>();
    public List<Item> Item_List_Tank = new List<Item>();
    public List<Item> Item_List_Plane = new List<Item>();

    public Dictionary<int, Item.Type> Upgrade_type_item = new Dictionary<int, Item.Type>();
    public Dictionary<string, int> Upgrade_item = new Dictionary<string, int>();


    JsonData Item_Json_Data;

    bool update_object=true;//更新对象层，只需要执行一次

    private static Config_item config_Item;
    public static Config_item Config_Item
    {
        get
        {
            if (config_Item == null) config_Item = new Config_item();
            return config_Item;
        }
    }

    public void Config_Item_Json()
    {
#if UNITY_EDITOR_WIN
        Item_Json_Data = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + "/Item_Json.json", Encoding.GetEncoding("UTF-8")));
#endif

#if UNITY_ANDROID
        FileInfo file = new FileInfo(Application.persistentDataPath + "Item_Json.json");
        if (file.Exists)
        {
            string path =Application.persistentDataPath + "Item_Json.json";
            Item_Json_Data = JsonMapper.ToObject(File.ReadAllText(path, Encoding.GetEncoding("UTF-8")));

        }
        else
        {

            string path = "jar:file://" + Application.dataPath + "!/assets/Item_Json.json";
            WWW www = new WWW(path);
            while (!www.isDone) { }

            Item_Json_Data = JsonMapper.ToObject(www.text);


            StreamWriter write = file.CreateText();
            string json = JsonMapper.ToJson(Item_Json_Data);
            Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
            json = reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
            write.Write(json);

            write.Close();
            write.Dispose();

            Item_Json_Data = JsonMapper.ToObject(File.ReadAllText(Application.persistentDataPath + "Item_Json.json", Encoding.GetEncoding("UTF-8")));
        }
#endif
        Decode_Item_Json();
    }
  
    private void Decode_Item_Json()
    {
        for (int i = 0; i < Item_Json_Data.Count; i++)
        {
            string item_Model = Item_Json_Data[i]["Item_Model"].ToString();

            string item_Number = Item_Json_Data[i]["Item_Number"].ToString();
            int item_Type = (int)Item_Json_Data[i]["Item_Type"];
            int item_Target = (int)Item_Json_Data[i]["Item_Target"];
            int item_Camp = (int)Item_Json_Data[i]["Item_Camp"];

            string item_Name = Item_Json_Data[i]["Item_Name"].ToString();
            string item_AP = Item_Json_Data[i]["Item_AP"].ToString();
            string item_Attack = Item_Json_Data[i]["Item_Attack"].ToString();
            string item_Defense = Item_Json_Data[i]["Item_Defense"].ToString();
            string iten_range = Item_Json_Data[i]["Item_range"].ToString();

            string item_Price = Item_Json_Data[i]["Item_Price"].ToString();
            string item_HP = Item_Json_Data[i]["Item_HP"].ToString();
            string item_Vision = Item_Json_Data[i]["Item_Vision"].ToString();

            string item_Desc = Item_Json_Data[i]["Item_Desc"].ToString();

          
            Item item = new Item(item_Model, item_Number, item_Type,item_Target,item_Camp,item_Name,item_AP, item_Attack, item_Defense, iten_range, item_Price,
                                  item_HP, item_Vision, item_Desc);

            Item_List_All.Add(item);
            if (update_object==true)
            {
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
                if (item.Item_Camp == Item.Camp.俄罗斯)
                {
                    Upgrade_type_item.Add(i, item.Item_Type);
                    Upgrade_item.Add(item.Item_Name, i);
                }
            }
        }
        update_object = false;
    }
  
    public void Upgrade_Attack(ref Item.Type type, ref int value)//批量升级攻击
    {
        FileInfo file;
        foreach (KeyValuePair<int, Item.Type> Upgrade_item in Upgrade_type_item)
        {
            if (Upgrade_item.Value == type)
            {
                Item_Json_Data[Upgrade_item.Key]["Item_Attack"] = (int)Item_Json_Data[Upgrade_item.Key]["Item_Attack"] + value;
            }
        }
#if UNITY_EDITOR_WIN
        file = new FileInfo(Application.streamingAssetsPath + "/Item_Json.json");
#endif

#if UNITY_ANDROID
        file = new FileInfo(Application.persistentDataPath + "Item_Json.json");
#endif
        StreamWriter write = file.CreateText();

        string json = JsonMapper.ToJson(Item_Json_Data);
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        json = reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
        write.Write(json);

        write.Close();
        write.Dispose();

        Item_List_All.Clear();//更新数据
        Config_Item_Json();
    }

    public void Upgrade_Attack(ref string name, ref int value)//升级指定单位攻击
    {
        FileInfo file;
        if (Upgrade_item.ContainsKey(name))
        {
            int index = -1;
            Upgrade_item.TryGetValue(name, out index);
            Item_Json_Data[index]["Item_Attack_extra"] = (int)Item_Json_Data[index]["Item_Attack_extra"] + value;
        }
#if UNITY_EDITOR_WIN
        file = new FileInfo(Application.streamingAssetsPath + "/Item_Json.json");
#endif

#if UNITY_ANDROID
        file = new FileInfo(Application.persistentDataPath + "Item_Json.json");
#endif
        StreamWriter write = file.CreateText();

        string json = JsonMapper.ToJson(Item_Json_Data);
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        json = reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
        write.Write(json);

        write.Close();
        write.Dispose();

        Item_List_All.Clear();//更新数据
        Config_Item_Json();
    }

    public void Upgrade_Defense(ref Item.Type type, ref int value)//升级防御
    {
        FileInfo file;
        foreach (KeyValuePair<int, Item.Type> Upgrade_item in Upgrade_type_item)
        {
            if (Upgrade_item.Value == type)
            {
                Item_Json_Data[Upgrade_item.Key]["Item_Defense"] = (int)Item_Json_Data[Upgrade_item.Key]["Item_Defense"] + value;
            }
        }
#if UNITY_EDITOR_WIN
        file = new FileInfo(Application.streamingAssetsPath + "/Item_Json.json");
#endif

#if UNITY_ANDROID
        file = new FileInfo(Application.persistentDataPath + "Item_Json.json");
#endif
        StreamWriter write = file.CreateText();

        string json = JsonMapper.ToJson(Item_Json_Data);
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        json = reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
        write.Write(json);

        write.Close();
        write.Dispose();

        Item_List_All.Clear();//更新数据
        Config_Item_Json();
    }
    public void Upgrade_AP(ref Item.Type type, ref int value)//升级行动点数
    {
        FileInfo file;
        foreach (KeyValuePair<int, Item.Type> Upgrade_item in Upgrade_type_item)
        {
            if (Upgrade_item.Value == type)
            {
                Item_Json_Data[Upgrade_item.Key]["Item_AP"] = (int)Item_Json_Data[Upgrade_item.Key]["Item_AP"] + value;
            }
        }
#if UNITY_EDITOR_WIN
        file = new FileInfo(Application.streamingAssetsPath + "/Item_Json.json");
#endif

#if UNITY_ANDROID
        file = new FileInfo(Application.persistentDataPath + "Item_Json.json");
#endif
        StreamWriter write = file.CreateText();

        string json = JsonMapper.ToJson(Item_Json_Data);
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        json = reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
        write.Write(json);

        write.Close();
        write.Dispose();

        Item_List_All.Clear();//更新数据
        Config_Item_Json();
    }
    public void Upgrade_HP(ref Item.Type type, ref int value)//升级生命
    {
        FileInfo file;
        foreach (KeyValuePair<int, Item.Type> Upgrade_item in Upgrade_type_item)
        {
            if (Upgrade_item.Value == type)
            {
                Item_Json_Data[Upgrade_item.Key]["Item_HP"] = (int)Item_Json_Data[Upgrade_item.Key]["Item_HP"] + value;
            }
        }
#if UNITY_EDITOR_WIN
        file = new FileInfo(Application.streamingAssetsPath + "/Item_Json.json");
#endif

#if UNITY_ANDROID
        file = new FileInfo(Application.persistentDataPath + "Item_Json.json");
#endif
        StreamWriter write = file.CreateText();

        string json = JsonMapper.ToJson(Item_Json_Data);
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        json = reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
        write.Write(json);

        write.Close();
        write.Dispose();

        Item_List_All.Clear();//更新数据
        Config_Item_Json();
    }
}
