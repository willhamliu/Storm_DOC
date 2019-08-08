using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Info : MonoBehaviour {

    public Text Name;
    public string Item_Camp;
    private Item item;
    public void Namedata(Item item)
    {
        if (item == null)
        {
            return;
        }
        this.item = item;
        this.Item_Camp = this.item.Item_Camp.ToString();
        this.Name.text = this.item.Item_Name;
    }

}
