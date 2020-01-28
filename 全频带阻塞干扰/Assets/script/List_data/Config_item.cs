using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using LitJson;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;

public class Config_Item
{
    public List<Item> item_List_All = new List<Item>();
    public List<Item> item_List_Building = new List<Item>();
    public List<Item> item_List_People = new List<Item>();
    public List<Item> item_List_Tank = new List<Item>();
    public List<Item> item_List_Plane = new List<Item>();

    public Dictionary<int, Item.Type> upgrade_type_item = new Dictionary<int, Item.Type>();
    public Dictionary<string, int> upgrade_item = new Dictionary<string, int>();

    public Dictionary<string, int> unit_info = new Dictionary<string, int>();


    JsonData item_Json_Data;

    bool update_Object=true;//更新对象层，只需要执行一次

    private static Config_Item config_Item;
    public static Config_Item Config_item
    {
        get
        {
            if (config_Item == null) config_Item = new Config_Item();
            return config_Item;
        }
    }

    public void Config_Item_Json()
    {
#if UNITY_EDITOR_WIN
        item_Json_Data = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + "/Item_Json.json", Encoding.GetEncoding("UTF-8")));
#endif

#if UNITY_ANDROID
        FileInfo file = new FileInfo(Application.persistentDataPath + "Item_Json.json");
        if (file.Exists)
        {
            string path =Application.persistentDataPath + "Item_Json.json";
            item_Json_Data = JsonMapper.ToObject(File.ReadAllText(path, Encoding.GetEncoding("UTF-8")));

        }
        else
        {

            string path = "jar:file://" + Application.dataPath + "!/assets/Item_Json.json";
            WWW www = new WWW(path);
            while (!www.isDone) { }

            item_Json_Data = JsonMapper.ToObject(www.text);


            StreamWriter write = file.CreateText();
            string json = JsonMapper.ToJson(item_Json_Data);
            Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
            json = reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
            write.Write(json);

            write.Close();
            write.Dispose();

            item_Json_Data = JsonMapper.ToObject(File.ReadAllText(Application.persistentDataPath + "Item_Json.json", Encoding.GetEncoding("UTF-8")));
        }
#endif
        Decode_Item_Json();
    }
  
    private void Decode_Item_Json()
    {
        item_List_All.Clear();//更新数据
        unit_info.Clear();

        for (int i = 0; i < item_Json_Data.Count; i++)
        {
            string item_Model = item_Json_Data[i]["Item_Model"].ToString();

            string item_Number = item_Json_Data[i]["Item_Number"].ToString();
            int item_Type = (int)item_Json_Data[i]["Item_Type"];
            int item_Target = (int)item_Json_Data[i]["Item_Target"];
            int item_Camp = (int)item_Json_Data[i]["Item_Camp"];

            string item_Name = item_Json_Data[i]["Item_Name"].ToString();
            int item_AP = (int)item_Json_Data[i]["Item_AP"];
            int item_Attack = (int)item_Json_Data[i]["Item_Attack"];
            int item_Defense = (int)item_Json_Data[i]["Item_Defense"];
            int iten_range = (int)item_Json_Data[i]["Item_range"];

            int item_Price = (int)item_Json_Data[i]["Item_Price"];
            int item_HP = (int)item_Json_Data[i]["Item_HP"];
            int item_Vision = (int)item_Json_Data[i]["Item_Vision"];

            string item_Desc = item_Json_Data[i]["Item_Desc"].ToString();

          
            Item item = new Item(item_Model, item_Number, item_Type,item_Target,item_Camp,item_Name,item_AP, item_Attack, item_Defense, iten_range, item_Price,
                                  item_HP, item_Vision, item_Desc);

            item_List_All.Add(item);//使ui图鉴获取单位信息
            unit_info.Add(item.Item_Model, i);//使游戏单位获得自己的各种信息

            if (update_Object==true)
            {
                if (item.Item_Type == Item.Type.建筑单位)
                {
                    item_List_Building.Add(item);
                }
                if (item.Item_Type == Item.Type.步兵单位)
                {
                    item_List_People.Add(item);
                }
                if (item.Item_Type == Item.Type.装甲单位)
                {
                    item_List_Tank.Add(item);
                }
                if (item.Item_Type == Item.Type.飞行单位)
                {
                    item_List_Plane.Add(item);
                }
                if (item.Item_Camp == Item.Camp.俄罗斯)
                {
                    upgrade_type_item.Add(i, item.Item_Type);
                    upgrade_item.Add(item.Item_Name, i);
                }
            }
        }
        update_Object = false;
    }

    public int Config_unity_info(string name )
    {
        if (unit_info.ContainsKey(name))
        {

            int index = unit_info[name];
            return index;
        }
        else
        {
            return -1;
        }
    }
  
    public void Upgrade_Attack(ref Item.Type type, ref int value)//批量升级攻击
    {
        FileInfo file;
        foreach (KeyValuePair<int, Item.Type> Upgrade_item in upgrade_type_item)
        {
            if (Upgrade_item.Value == type)
            {
                item_Json_Data[Upgrade_item.Key]["Item_Attack"] = (int)item_Json_Data[Upgrade_item.Key]["Item_Attack"] + value;
            }
        }
#if UNITY_EDITOR_WIN
        file = new FileInfo(Application.streamingAssetsPath + "/Item_Json.json");
#endif

#if UNITY_ANDROID
        file = new FileInfo(Application.persistentDataPath + "Item_Json.json");
#endif
        StreamWriter write = file.CreateText();

        string json = JsonMapper.ToJson(item_Json_Data);
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        json = reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
        write.Write(json);

        write.Close();
        write.Dispose();

        Config_Item_Json();
    }

    public void Upgrade_Attack(ref string name, ref int value)//升级指定单位攻击
    {
        FileInfo file;
        if (upgrade_item.ContainsKey(name))
        {
            int index = -1;
            upgrade_item.TryGetValue(name, out index);
            item_Json_Data[index]["Item_Attack_extra"] = (int)item_Json_Data[index]["Item_Attack_extra"] + value;
        }
#if UNITY_EDITOR_WIN
        file = new FileInfo(Application.streamingAssetsPath + "/Item_Json.json");
#endif

#if UNITY_ANDROID
        file = new FileInfo(Application.persistentDataPath + "Item_Json.json");
#endif
        StreamWriter write = file.CreateText();

        string json = JsonMapper.ToJson(item_Json_Data);
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        json = reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
        write.Write(json);

        write.Close();
        write.Dispose();

        Config_Item_Json();
    }

    public void Upgrade_Defense(ref Item.Type type, ref int value)//升级防御
    {
        FileInfo file;
        foreach (KeyValuePair<int, Item.Type> Upgrade_item in upgrade_type_item)
        {
            if (Upgrade_item.Value == type)
            {
                item_Json_Data[Upgrade_item.Key]["Item_Defense"] = (int)item_Json_Data[Upgrade_item.Key]["Item_Defense"] + value;
            }
        }
#if UNITY_EDITOR_WIN
        file = new FileInfo(Application.streamingAssetsPath + "/Item_Json.json");
#endif

#if UNITY_ANDROID
        file = new FileInfo(Application.persistentDataPath + "Item_Json.json");
#endif
        StreamWriter write = file.CreateText();

        string json = JsonMapper.ToJson(item_Json_Data);
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        json = reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
        write.Write(json);

        write.Close();
        write.Dispose();

        Config_Item_Json();
    }
    public void Upgrade_AP(ref Item.Type type, ref int value)//升级行动点数
    {
        FileInfo file;
        foreach (KeyValuePair<int, Item.Type> Upgrade_item in upgrade_type_item)
        {
            if (Upgrade_item.Value == type)
            {
                item_Json_Data[Upgrade_item.Key]["Item_AP"] = (int)item_Json_Data[Upgrade_item.Key]["Item_AP"] + value;
            }
        }
#if UNITY_EDITOR_WIN
        file = new FileInfo(Application.streamingAssetsPath + "/Item_Json.json");
#endif

#if UNITY_ANDROID
        file = new FileInfo(Application.persistentDataPath + "Item_Json.json");
#endif
        StreamWriter write = file.CreateText();

        string json = JsonMapper.ToJson(item_Json_Data);
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        json = reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
        write.Write(json);

        write.Close();
        write.Dispose();

        Config_Item_Json();
    }
    public void Upgrade_HP(ref Item.Type type, ref int value)//升级生命
    {
        FileInfo file;
        foreach (KeyValuePair<int, Item.Type> Upgrade_item in upgrade_type_item)
        {
            if (Upgrade_item.Value == type)
            {
                item_Json_Data[Upgrade_item.Key]["Item_HP"] = (int)item_Json_Data[Upgrade_item.Key]["Item_HP"] + value;
            }
        }
#if UNITY_EDITOR_WIN
        file = new FileInfo(Application.streamingAssetsPath + "/Item_Json.json");
#endif

#if UNITY_ANDROID
        file = new FileInfo(Application.persistentDataPath + "Item_Json.json");
#endif
        StreamWriter write = file.CreateText();

        string json = JsonMapper.ToJson(item_Json_Data);
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        json = reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
        write.Write(json);

        write.Close();
        write.Dispose();

        Config_Item_Json();
    }
}
