﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 图鉴面板的单位详细信息显示
/// </summary>
public class Item_Detail : MonoBehaviour {

    public static Item_Detail instance;
    public MeshFilter model;
    public Text camp;
    public Text target;

    public Text ap;
    public Text attack;
    public Text defense;
    public Text range;
    public Text hp;

    public Text desc;
    private Item item;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SetData(Item item)
    {
        if (item == null)
        {
            return;
        }
        this.item = item;
        this.model.mesh = Item_Model.Instance.Model_display(this.item.item_Model);
        this.camp.text =this.item.Item_Camp.ToString();
        this.ap.text = this.item.item_AP.ToString();
        this.attack.text = this.item.item_Attack.ToString();
        this.defense.text = this.item.item_Defense.ToString();
        this.target.text = this.item.Item_Target.ToString();
        this.range.text = this.item.iten_Range.ToString();
        this.hp.text = this.item.item_HP.ToString();

        this.desc.text = "\u3000\u3000" + this.item.item_Desc;
    }
}
