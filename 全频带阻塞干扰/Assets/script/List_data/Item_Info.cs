using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Info : MonoBehaviour {

    public Text item_Name;
    public string item_Camp;
    private Item item;
    public void Namedata(Item item)
    {
        if (item == null)
        {
            return;
        }
        this.item = item;
        this.item_Camp = this.item.Item_Camp.ToString();
        this.item_Name.text = this.item.Item_Name;
    }

}
