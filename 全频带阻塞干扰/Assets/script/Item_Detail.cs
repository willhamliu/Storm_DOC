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

    public static Item_Detail item_Detail;

    void Awake()
    {
        item_Detail = this;
    }

    public void SetData(Item item)
    {
        this.Camp.text = item.Item_Camp.ToString();
        this.AP.text = item.Item_AP.ToString();
        this.Attack.text = item.Item_Attack.ToString();
        this.Defense.text = item.Item_Defense.ToString();
        this.Target.text = item.Item_Target.ToString();
        this.range.text = item.Iten_Range.ToString();
        this.HP.text = item.Item_HP.ToString();

        this.Desc.text = item.Item_Desc;
    }
}
