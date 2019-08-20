using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public string Level_Name;
    public string Level_Desc;
    public string Load_Scene;
    public string Load_Scene_Dialogue;
    public int Level_Point;


    public Level(string level_Name,string level_Desc,string load_Scene,string load_Scene_Dialogue,int level_Point)
    {
        this.Level_Name = level_Name;
        this.Level_Desc = level_Desc;
        this.Load_Scene = load_Scene;
        this.Load_Scene_Dialogue = load_Scene_Dialogue;
        this.Level_Point = level_Point;
    }
}
