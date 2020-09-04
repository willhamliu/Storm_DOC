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
public class Config_Customize: BaseConfigJson<Config_Customize>
{
    public List<Customize> customize_DataAll = new List<Customize>();
    private JsonData customize_JsonData;

    public void Confing_Customize_Json()
    {
        customize_JsonData = ReadJson(customize_JsonData, "/Upgrade_Json.json", true);
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
        customize_JsonData[index]["Customize_Purchase_Status"] = true;

        WriteJson(customize_JsonData, saveFile+ "/Upgrade_Json.json");
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
