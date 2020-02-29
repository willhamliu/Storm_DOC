using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customize
{
    public string customize_Name;
    public string customize_Desc;
    public int customize_Price;
    public int customize_Unlockindex;
    public bool ustomize_Purchase_Status;

    public Customize(string customize_Name, string customize_Desc, int customize_Price, int customize_Unlockindex,
        bool customize_Purchase_Status)
    {
        this.customize_Name = customize_Name;
        this.customize_Desc = customize_Desc;
        this.customize_Price = customize_Price;
        this.customize_Unlockindex = customize_Unlockindex;
        this.ustomize_Purchase_Status = customize_Purchase_Status;
    }
}
