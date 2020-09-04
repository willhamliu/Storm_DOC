using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 关卡选择面板的，关卡名字信息显示
/// </summary>
public class Level_Info : MonoBehaviour
{
    public Text lecel_Name;
    private Level level;
    public void Namedata(Level level)
    {
        if (level == null)
        {
            return;
        }
        this.level = level;
        lecel_Name.text = this.level.level_Name;
    }
}
