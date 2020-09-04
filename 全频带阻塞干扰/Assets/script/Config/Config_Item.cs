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
public class Config_Item: BaseConfigJson<Config_Item>
{
    public List<Item> item_List_All = new List<Item>();

    public Dictionary<string, Item> itemDictonary = new Dictionary<string, Item>();

    JsonData item_JsonData;

    public void Config_Item_Json()
    {
        item_JsonData = ReadJson(item_JsonData, "/Item_Json.json", true);
        Decode_Item_Json();
    }
    /// <summary>
    /// 通过名字获取指定单位信息
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
    public Item GetItemInfo(string itemName)
    {
        if (itemDictonary.ContainsKey(itemName))
        {
            return itemDictonary[itemName];
        }
        else
        {
            Debug.Log("找不到这个单位信息，单位名字"+ itemName);
            return null;
        }

    }

    private void Decode_Item_Json()
    {
        item_List_All.Clear();//更新数据
        itemDictonary.Clear();

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

          
            Item item = new Item(item_Model, item_Number, item_Type,item_Target,item_Camp,item_Name,i,item_AP, item_Attack, item_Defense, iten_range, item_Price,
                                  item_HP, item_Vision, item_Desc);

            itemDictonary.Add(item_Model, item);
            item_List_All.Add(item);//使ui图鉴获取单位信息
        }
    }

    public void Upgrade(string upgradeProject, Item.Type upgradeType, int upgradeValue)//批量升级
    {
        foreach (var values in itemDictonary.Values)
        {
            if (values.Item_Type.Equals(upgradeType)&& values.Item_Camp == Item.Camp.俄罗斯)
            {
                item_JsonData[values.item_Index][upgradeProject] = (int)item_JsonData[values.item_Index][upgradeProject] + upgradeValue;
            }
        }
        WriteJson(item_JsonData, saveFile+ "/Item_Json.json");
        Config_Item_Json();
    }

    public void Upgrade(string upgradeProject, string upgradeName, int upgradeValue)//升级指定单位
    {
        if (itemDictonary.ContainsKey(upgradeName))
        {
            item_JsonData[itemDictonary[upgradeName].item_Index][upgradeProject] = (int)item_JsonData[itemDictonary[upgradeName].item_Index][upgradeProject] + upgradeValue;
        }
        WriteJson(item_JsonData, saveFile+ "/Item_Json.json");
        Config_Item_Json();
    }
}
