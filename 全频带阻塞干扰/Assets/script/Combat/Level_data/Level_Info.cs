using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level_Info : MonoBehaviour
{
    public Text Name;
    private Level level;
    public void Namedata(Level level)
    {
        if (level == null)
        {
            return;
        }
        this.level = level;
        Name.text = this.level.Level_Name;
    }
}
