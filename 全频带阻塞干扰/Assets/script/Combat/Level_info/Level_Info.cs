using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
