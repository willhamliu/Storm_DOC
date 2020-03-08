using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using LitJson;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
/// <summary>
/// 配置每个单位的属性，通过特定的升级项目修改更新指定单位的属性
/// </summary>
public class Config_Item
{
    public List<Item> item_List_All = new List<Item>();
    public List<Item> item_List_Building = new List<Item>();
    public List<Item> item_List_People = new List<Item>();
    public List<Item> item_List_Tank = new List<Item>();
    public List<Item> item_List_Plane = new List<Item>();

    public Dictionary<int, Item.Type> upgrade_Type_Item = new Dictionary<int, Item.Type>();
    public Dictionary<string, int> upgrade_Item = new Dictionary<string, int>();

    public Dictionary<string, int> unit_Info = new Dictionary<string, int>();


    JsonData item_JsonData;

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
        item_JsonData = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + "/Item_Json.json", Encoding.GetEncoding("UTF-8")));
#endif

#if UNITY_ANDROID
        FileInfo file = new FileInfo(Application.persistentDataPath + "Item_Json.json");
        if (file.Exists)
        {
            string path =Application.persistentDataPath + "Item_Json.json";
            item_JsonData = JsonMapper.ToObject(File.ReadAllText(path, Encoding.GetEncoding("UTF-8")));
        }
        else
        {
            string path = "jar:file://" + Application.dataPath + "!/assets/Item_Json.json";
            WWW www = new WWW(path);
            while (!www.isDone) { }

            item_JsonData = JsonMapper.ToObject(www.text);

            StreamWriter write = file.CreateText();
            string json = JsonMapper.ToJson(item_JsonData);
            Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
            json = reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
            write.Write(json);

            write.Close();
            write.Dispose();

            item_JsonData = JsonMapper.ToObject(File.ReadAllText(Application.persistentDataPath + "Item_Json.json", Encoding.GetEncoding("UTF-8")));
        }
#endif
        Decode_Item_Json();
    }
  
    private void Decode_Item_Json()
    {
        item_List_All.Clear();//更新数据
        unit_Info.Clear();

        for (int i = 0; i < item_JsonData.Count; i++)
        {
            string item_Model = item_JsonData[i]["Item_Model"].ToString();

            string item_Number = item_JsonData[i]["Item_Number"].ToString();
            int item_Type = (int)item_JsonData[i]["Item_Type"];
            int item_Target = (int)item_JsonData[i]["Item_Target"];
            int item_Camp = (int)item_JsonData[i]["Item_Camp"];

            string item_Name = item_JsonData[i]["Item_Name"].ToString();
            int item_AP = (int)item_JsonData[i]["Item_AP"];
            int item_Attack = (int)item_JsonData[i]["Item_Attack"];
            int item_Defense = (int)item_JsonData[i]["Item_Defense"];
            int iten_range = (int)item_JsonData[i]["Item_range"];

            int item_Price = (int)item_JsonData[i]["Item_Price"];
            int item_HP = (int)item_JsonData[i]["Item_HP"];
            int item_Vision = (int)item_JsonData[i]["Item_Vision"];

            string item_Desc = item_JsonData[i]["Item_Desc"].ToString();

          
            Item item = new Item(item_Model, item_Number, item_Type,item_Target,item_Camp,item_Name,item_AP, item_Attack, item_Defense, iten_range, item_Price,
                                  item_HP, item_Vision, item_Desc);

            item_List_All.Add(item);//使ui图鉴获取单位信息
            unit_Info.Add(item.item_Model, i);//使游戏单位获得自己的各种信息

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
                    upgrade_Type_Item.Add(i, item.Item_Type);
                    upgrade_Item.Add(item.item_Name, i);
                }
            }
        }
        update_Object = false;
    }

    public int Config_unity_info(string name )
    {
        if (unit_Info.ContainsKey(name))
        {

            int index = unit_Info[name];
            return index;
        }
        else
        {
            return -1;
        }
    }
  
    public void Upgrade(string upgradeProject, Item.Type upgradeType, int upgradeValue)//批量升级
    {
        FileInfo file;
        foreach (KeyValuePair<int, Item.Type> Upgrade_item in upgrade_Type_Item)
        {
            if (Upgrade_item.Value == upgradeType)
            {
                item_JsonData[Upgrade_item.Key][upgradeProject] = (int)item_JsonData[Upgrade_item.Key][upgradeProject] + upgradeValue;
            }
        }
#if UNITY_EDITOR_WIN
        file = new FileInfo(Application.streamingAssetsPath + "/Item_Json.json");
#endif

#if UNITY_ANDROID
        file = new FileInfo(Application.persistentDataPath + "Item_Json.json");
#endif
        StreamWriter write = file.CreateText();

        string json = JsonMapper.ToJson(item_JsonData);
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        json = reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
        write.Write(json);

        write.Close();
        write.Dispose();

        Config_Item_Json();
    }

    public void Upgrade(string upgradeProject, string upgradeName, int upgradeValue)//升级指定单位
    {
        FileInfo file;
        if (upgrade_Item.ContainsKey(upgradeName))
        {
            int index = -1;
            upgrade_Item.TryGetValue(upgradeName, out index);
            item_JsonData[index][upgradeProject] = (int)item_JsonData[index][upgradeProject] + upgradeValue;
        }
#if UNITY_EDITOR_WIN
        file = new FileInfo(Application.streamingAssetsPath + "/Item_Json.json");
#endif

#if UNITY_ANDROID
        file = new FileInfo(Application.persistentDataPath + "Item_Json.json");
#endif
        StreamWriter write = file.CreateText();

        string json = JsonMapper.ToJson(item_JsonData);
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        json = reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
        write.Write(json);

        write.Close();
        write.Dispose();

        Config_Item_Json();
    }
}
