using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level_Detail : MonoBehaviour
{
    public Text Desc;
    private Level level;
    public static Level_Detail Level_detail;
    private void Awake()
    {
        Level_detail = this;
    }
    public void SetData(Level level)
    {
        if (level == null)
        {
            return;
        }
        this.level = level;
        Desc.text ="\u3000\u3000"+this.level.Level_Desc;
    }
}
