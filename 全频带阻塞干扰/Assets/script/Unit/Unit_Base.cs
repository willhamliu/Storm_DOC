using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 对指定单位共有的属性信息进行管理
/// </summary>
public class Unit_Base : MonoBehaviour
{
    protected string unit_Name;
    public Vector2Int coordinate;
    protected Vector3 unit_Position;//该单位所在的位置

    public float hp;//单位当前血量
    protected float max_HP;//单位最大血量

    private void Awake()
    {
        unit_Name = this.gameObject.name;
    }
    protected void SetPostition()
    {
        unit_Position = MapData.Instance.GetPoint(coordinate);
        this.transform.position = unit_Position;
    }

    protected void SetHP()
    {
        max_HP = hp = Config_Item.Instance.GetItemInfo(unit_Name).item_HP;
    }
}
