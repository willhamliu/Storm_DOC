using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using LitJson;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
/// <summary>
/// 配置单位升级说明，执行单位升级命令
/// </summary>
public class Config_Customize:InstanceNull<Config_Customize>
{
    public List<Customize> customize_DataAll = new List<Customize>();
    private JsonData customize_JsonData;

    public void Confing_Customize_Json()
    {
#if UNITY_EDITOR_WIN
        customize_JsonData = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + "/Upgrade_Json.json", Encoding.GetEncoding("UTF-8")));
#endif

#if UNITY_ANDROID
        FileInfo file = new FileInfo(Application.persistentDataPath + "Upgrade_Json.json");
        if (file.Exists)//如果之前存过就不用再存了
        {
            string path = Application.persistentDataPath + "Upgrade_Json.json";
            customize_JsonData = JsonMapper.ToObject(File.ReadAllText(path, Encoding.GetEncoding("UTF-8")));

        }
        else
        {
            string path = "jar:file://" + Application.dataPath + "!/assets/Upgrade_Json.json";
            WWW www = new WWW(path);
            while (!www.isDone) { }

            customize_JsonData = JsonMapper.ToObject(www.text);


            StreamWriter write = file.CreateText();//创建一个文件夹
            string json = JsonMapper.ToJson(customize_JsonData);
            Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
            json = reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
            write.Write(json);//写入

            write.Close();
            write.Dispose();
            customize_JsonData = JsonMapper.ToObject(File.ReadAllText(Application.persistentDataPath + "Upgrade_Json.json", Encoding.GetEncoding("UTF-8")));
            //安卓端必须写入后再次赋值，否则升级时第一次无法写入(查了好久。。。)
        }

#endif
        Decode_Confing_Json();
    }

    private void Decode_Confing_Json()
    {
        customize_DataAll.Clear();//更新数据
        for (int i = 0; i < customize_JsonData.Count; i++)
        {
            string customize_Name = customize_JsonData[i]["Customize_Name"].ToString();
            string customize_Desc = customize_JsonData[i]["Customize_Desc"].ToString();
            int customize_Price = (int)customize_JsonData[i]["Customize_Price"];
            int customize_Unlockindex = (int)customize_JsonData[i]["Customize_Unlockindex"];
            bool customize_Purchase_Status = (bool)customize_JsonData[i]["Customize_Purchase_Status"];
            Customize customize = new Customize(customize_Name, customize_Desc, customize_Price, customize_Unlockindex,customize_Purchase_Status);

            customize_DataAll.Add(customize);
        }
    }

    public void Purchase_Status_Modify(int index)
    {
        FileInfo file;
        customize_JsonData[index]["Customize_Purchase_Status"] = true;
#if UNITY_EDITOR_WIN
        file = new FileInfo(Application.streamingAssetsPath + "/Upgrade_Json.json");
#endif

#if UNITY_ANDROID
        file = new FileInfo(Application.persistentDataPath + "Upgrade_Json.json");
#endif
        StreamWriter write = file.CreateText();

        string json = JsonMapper.ToJson(customize_JsonData);
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        json = reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
        write.Write(json);

        write.Close();
        write.Dispose();

        Confing_Customize_Json();

        string upgradeProject;
        string upgraDename;
        Item.Type upgradeType;
        int upgradeValue;

        switch (index)
        {
            case 0:
                upgradeProject = "Item_HP";
                upgradeType = Item.Type.步兵单位;
                upgradeValue = 5;
                Config_Item.Instance.Upgrade(upgradeProject, upgradeType, upgradeValue);
                break;
            case 1:
                upgradeProject = "Item_Attack_extra";
                upgraDename = "自行火炮";
                upgradeValue = 10;
                Config_Item.Instance.Upgrade(upgradeProject, upgraDename, upgradeValue);
                break;
            case 2:
                upgradeProject = "Item_Attack";
                upgradeType = Item.Type.步兵单位;
                upgradeValue = 3;
                Config_Item.Instance.Upgrade(upgradeProject, upgradeType, upgradeValue);
                break;
            case 3:
                upgradeProject = "Item_Attack";
                upgradeType = Item.Type.飞行单位;
                upgradeValue = 10;
                Config_Item.Instance.Upgrade(upgradeProject, upgradeType, upgradeValue);
                break;
            case 4:
                upgradeProject = "Item_HP";
                upgradeType = Item.Type.装甲单位;
                upgradeValue = 10;
                Config_Item.Instance.Upgrade(upgradeProject, upgradeType, upgradeValue);
                break;
            case 5:
                upgradeProject = "Item_Attack";
                upgradeType = Item.Type.步兵单位;
                upgradeValue = 7;
                Config_Item.Instance.Upgrade(upgradeProject, upgradeType, upgradeValue);
                break;
            case 6:
                upgradeProject = "Item_Attack";
                upgradeType = Item.Type.飞行单位;
                upgradeValue = 15;
                Config_Item.Instance.Upgrade(upgradeProject, upgradeType, upgradeValue);
                break;
            case 7:
                upgradeProject = "Item_HP";
                upgradeType = Item.Type.装甲单位;
                upgradeValue = 10;
                Config_Item.Instance.Upgrade(upgradeProject, upgradeType, upgradeValue);
                break;
            case 8:
                upgradeProject = "Item_AP";
                upgradeType = Item.Type.装甲单位;
                upgradeValue = 1;
                Config_Item.Instance.Upgrade(upgradeProject, upgradeType, upgradeValue);
                break;
            case 9:
                upgradeProject = "Item_HP";
                upgradeType = Item.Type.飞行单位;
                upgradeValue = 15;
                Config_Item.Instance.Upgrade(upgradeProject, upgradeType, upgradeValue);
                break;
        }
    }
}
