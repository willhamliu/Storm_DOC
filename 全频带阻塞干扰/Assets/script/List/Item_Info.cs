using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 图鉴面板的单位基本信息显示
/// </summary>
public class Item_Info : MonoBehaviour {

    public Text item_Name;
    public Image blackImage;
    public string item_Camp;
    private Item item;
    public void NameData(Item item)
    {
        
        if (item == null)
        {
            return;
        }
        this.item = item;
        this.item_Camp = this.item.Item_Camp.ToString();
        this.item_Name.text = this.item.item_Name;
    }
}
