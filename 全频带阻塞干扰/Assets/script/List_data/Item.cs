using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {
   
    public enum Type
    {
        UnKown=-1,
        建筑单位,
        步兵单位,
        装甲单位,
        飞行单位,
    }

    public enum Target
    {
        UnKown = -1,
        所有目标,
        地面目标,
        空中目标,
        侦察单位,
        无法攻击
    }

    public enum Camp
    {
        UnKown = -1,
        北约,
        俄罗斯,
    }

    public string Item_Model;//模型路径
    public string Item_Number;
    public Type Item_Type =Type.UnKown;
    public Target Item_Target=Target.UnKown;
    public Camp Item_Camp=Camp.UnKown;

    public string Item_Name;//名字
    public int Item_AP;//行动点数
    public int Item_Attack;//攻击力

    public int Item_Defense;//防御力

    public int Iten_Range;//射程

    public int Item_Price;//价格
    public int Item_HP;//血量
    public int Item_Vision;//视野范围
    public string Item_Desc;//描述信息

    public Item(string item_Model, string item_Number, int item_Type, int item_Target, int item_Camp, string item_Name, int item_AP, int item_Attack, int item_Defense,
                int iten_Range, int item_Price, int item_HP, int item_Vision, string item_Desc)
    {
        this.Item_Model = item_Model;
        this.Item_Number = item_Number;
        switch (item_Type)
        {
            case -1:
                this.Item_Type = Type.UnKown;
                break;
            case 0:
                this.Item_Type = Type.建筑单位;
                break;
            case 1:
                this.Item_Type = Type.步兵单位;
                break;
            case 2:
                this.Item_Type = Type.装甲单位;
                break;
            case 3:
                this.Item_Type = Type.飞行单位;
                break;
            default:
                break;
        }

        switch (item_Target)
        {
            case -1:
                this.Item_Type = Type.UnKown;
                break;
            case 0:
                this.Item_Target = Target.所有目标;
                break;
            case 1:
                this.Item_Target = Target.地面目标;
                break;
            case 2:
                this.Item_Target = Target.空中目标;
                break;
            case 3:
                this.Item_Target = Target.侦察单位;
                break;
            case 4:
                this.Item_Target = Target.无法攻击;
                break;
            default:
                break;
        }

        switch (item_Camp)
        {
            case -1:
                this.Item_Type = Type.UnKown;
                break;
            case 0:
                this.Item_Camp = Camp.北约;
                break;
            case 1:
                this.Item_Camp = Camp.俄罗斯;
                break;
            default:
                break;
        }
        this.Item_Name = item_Name;
        this.Item_AP = item_AP;
        this.Item_Attack = item_Attack;
        this.Item_Defense = item_Defense;
        this.Iten_Range = iten_Range;
        this.Item_Price = item_Price;
        this.Item_HP = item_HP;
        this.Item_Vision = item_Vision;
        this.Item_Desc = item_Desc;
    }
}
