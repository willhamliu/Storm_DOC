using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Get
{
    private static Level_Get level_Get;
    public static Level_Get Level_get
    {
        get
        {
            if (level_Get == null) level_Get = new Level_Get();
            return level_Get;
        }
    }
    private static string level_Name;
    private static string dialogue_Name;
    public string Dialogue_name
    {
        get
        {
            return dialogue_Name;
        }
    }
    
    public void Load_level(Level level)
    {
        level_Name = level.Load_Scene;
        dialogue_Name = level.Load_Scene_Dialogue;
    }
    public void Load()
    {
        Level_Receipt.Level_receipt.Level_load(level_Name);
    }
}
