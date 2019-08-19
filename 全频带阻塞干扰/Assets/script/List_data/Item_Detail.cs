using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Detail : MonoBehaviour {

    public Text Camp;
    public Text AP;
    public Text Attack;
    public Text Defense;
    public Text Target;
    public Text range;
    public Text HP;

    public Text Desc;
    private Item item;

    public static Item_Detail Item_detail;

    void Awake()
    {
        Item_detail = this;
    }

    public void SetData(Item item)
    {
        if (item == null)
        {
            return;
        }
        this.item = item;
        this.Camp.text =this.item.Item_Camp.ToString();
        this.AP.text = this.item.Item_AP;
        this.Attack.text = this.item.Item_Attack;
        this.Defense.text = this.item.Item_Defense;
        this.Target.text = this.item.Item_Target.ToString();
        this.range.text = this.item.Iten_Range;
        this.HP.text = this.item.Item_HP;

        this.Desc.text = "\u3000\u3000" + this.item.Item_Desc;
    }
}
