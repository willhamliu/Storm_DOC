using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using LitJson;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;

public class Config_Customize
{
    public List<Customize> customize_Data_All = new List<Customize>();
    JsonData customize_Json_Data;

    private static Config_Customize config_Customize;
    public static Config_Customize Config_customize
    {
        get
        {
            if (config_Customize == null) config_Customize = new Config_Customize();
            return config_Customize;
        }
    }

    public void Confing_Customize_Json()
    {
#if UNITY_EDITOR_WIN
        customize_Json_Data = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + "/Upgrade_Json.json", Encoding.GetEncoding("UTF-8")));
#endif

#if UNITY_ANDROID
        FileInfo file = new FileInfo(Application.persistentDataPath + "Upgrade_Json.json");
        if (file.Exists)//如果之前存过就不用再存了
        {
            string path = Application.persistentDataPath + "Upgrade_Json.json";
            customize_Json_Data = JsonMapper.ToObject(File.ReadAllText(path, Encoding.GetEncoding("UTF-8")));

        }
        else
        {
            string path = "jar:file://" + Application.dataPath + "!/assets/Upgrade_Json.json";
            WWW www = new WWW(path);
            while (!www.isDone) { }

            customize_Json_Data = JsonMapper.ToObject(www.text);


            StreamWriter write = file.CreateText();//创建一个文件夹
            string json = JsonMapper.ToJson(customize_Json_Data);
            Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
            json = reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
            write.Write(json);//写入

            write.Close();
            write.Dispose();
            customize_Json_Data = JsonMapper.ToObject(File.ReadAllText(Application.persistentDataPath + "Upgrade_Json.json", Encoding.GetEncoding("UTF-8")));
            //安卓端必须写入后再次赋值，否则升级时第一次无法写入(查了好久。。。)
        }

#endif
        Decode_Confing_Json();
    }

    private void Decode_Confing_Json()
    {
        customize_Data_All.Clear();//更新数据
        for (int i = 0; i < customize_Json_Data.Count; i++)
        {
            string Customize_Name = customize_Json_Data[i]["Customize_Name"].ToString();
            string Customize_Desc = customize_Json_Data[i]["Customize_Desc"].ToString();
            int Customize_Price = (int)customize_Json_Data[i]["Customize_Price"];
            int Customize_Unlockindex = (int)customize_Json_Data[i]["Customize_Unlockindex"];
            bool Customize_Purchase_Status = (bool)customize_Json_Data[i]["Customize_Purchase_Status"];
            Customize customize = new Customize(Customize_Name, Customize_Desc, Customize_Price, Customize_Unlockindex,Customize_Purchase_Status);

            customize_Data_All.Add(customize);
        }
    }
    public void Purchase_Status_Modify(int index)
    {
        FileInfo file;
        customize_Json_Data[index]["Customize_Purchase_Status"] = true;
#if UNITY_EDITOR_WIN
        file = new FileInfo(Application.streamingAssetsPath + "/Upgrade_Json.json");
#endif

#if UNITY_ANDROID
        file = new FileInfo(Application.persistentDataPath + "Upgrade_Json.json");
#endif
        StreamWriter write = file.CreateText();

        string json = JsonMapper.ToJson(customize_Json_Data);
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        json = reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
        write.Write(json);

        write.Close();
        write.Dispose();

        Confing_Customize_Json();

        Item.Type type;
        string name;
        int value;

        switch (index)
        {
            case 0:
                type = Item.Type.步兵单位;
                value = 5;
                Config_Item.Config_item.Upgrade_HP(ref type, ref value);
                break;
            case 1:
                name = "自行火炮";
                value = 10;
                Config_Item.Config_item.Upgrade_Attack(ref name, ref value);
                break;
            case 2:
                type = Item.Type.步兵单位;
                value = 3;
                Config_Item.Config_item.Upgrade_Attack(ref type, ref value);
                break;
            case 3:
                type = Item.Type.飞行单位;
                value = 10;
                Config_Item.Config_item.Upgrade_Attack(ref type, ref value);
                break;
            case 4:
                type = Item.Type.装甲单位;
                value = 10;
                Config_Item.Config_item.Upgrade_HP(ref type, ref value);
                break;
            case 5:
                type = Item.Type.步兵单位;
                value = 7;
                Config_Item.Config_item.Upgrade_Attack(ref type, ref value);
                break;
            case 6:
                type = Item.Type.飞行单位;
                value = 15;
                Config_Item.Config_item.Upgrade_Attack(ref type, ref value);
                break;
            case 7:
                type = Item.Type.装甲单位;
                value = 10;
                Config_Item.Config_item.Upgrade_HP(ref type, ref value);
                break;
            case 8:
                type = Item.Type.装甲单位;
                value = 1;
                Config_Item.Config_item.Upgrade_AP(ref type, ref value);
                break;
            case 9:
                type = Item.Type.飞行单位;
                value = 15;
                Config_Item.Config_item.Upgrade_HP(ref type, ref value);
                break;
        }
    }
}
