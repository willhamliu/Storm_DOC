using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 对指定单位共有的属性信息进行管理
/// </summary>
public class Unit_Prototype : MonoBehaviour
{
    protected string unit_Name;
    public int coordinate_X;//该单位在x轴的编号
    public int coordinate_Y;//该单位在y轴的编号
    public int unit_Position_Index;//该单位所在位置的下标

    protected Vector3[] position_Array;//索引位置数组
    protected Vector3 unit_Position;//该单位所在的位置

    public float hp;//单位当前血量
    protected float max_HP;//单位最大血量

    private void Awake()
    {
        unit_Name = this.gameObject.name;
    }
    protected void Postition_Start()
    {
        position_Array = Map_Management.instance.hex_Position;
        unit_Position_Index = Map_Management.instance.hex_Idex[coordinate_X, coordinate_Y];
        unit_Position = position_Array[unit_Position_Index];

        this.transform.position = unit_Position;
    }

    protected void HP_Start()
    {
        max_HP = hp = Config_Item.Instance.item_List_All[Config_Item.Instance.Config_unity_info(unit_Name)].item_HP;
    }
}
