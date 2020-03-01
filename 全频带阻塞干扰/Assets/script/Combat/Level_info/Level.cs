using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public string level_Name;
    public string level_Desc;
    public string load_Scene;
    public string load_Scene_Dialogue;
    public int level_Point;


    public Level(string level_Name,string level_Desc,string load_Scene,string load_Scene_Dialogue,int level_Point)
    {
        this.level_Name = level_Name;
        this.level_Desc = level_Desc;
        this.load_Scene = load_Scene;
        this.load_Scene_Dialogue = load_Scene_Dialogue;
        this.level_Point = level_Point;
    }
}
