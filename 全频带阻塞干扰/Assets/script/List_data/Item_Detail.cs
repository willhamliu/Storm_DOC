using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Detail : MonoBehaviour {

    public Text camp;
    public Text target;

    public Text ap;
    public Text attack;
    public Text defense;
    public Text range;
    public Text hp;

    public Text desc;
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
        this.camp.text =this.item.Item_Camp.ToString();
        this.ap.text = this.item.Item_AP;
        this.attack.text = this.item.Item_Attack;
        this.defense.text = this.item.Item_Defense;
        this.target.text = this.item.Item_Target.ToString();
        this.range.text = this.item.Iten_Range;
        this.hp.text = this.item.Item_HP;

        this.desc.text = "\u3000\u3000" + this.item.Item_Desc;
    }
}
