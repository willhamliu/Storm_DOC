using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customize
{
    public string Customize_Name;
    public string Customize_Desc;
    public int Customize_Price;
    public int Customize_Unlockindex;
    public bool Customize_Purchase_Status;

    public Customize(string customize_Name, string customize_Desc, int customize_Price, int customize_Unlockindex,
        bool customize_Purchase_Status)
    {
        this.Customize_Name = customize_Name;
        this.Customize_Desc = customize_Desc;
        this.Customize_Price = customize_Price;
        this.Customize_Unlockindex = customize_Unlockindex;
        this.Customize_Purchase_Status = customize_Purchase_Status;
    }
}
