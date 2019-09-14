using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_State
{
    private static Level_State level_State;
    public static Level_State Level_state
    {
        get
        {
            if (level_State == null) level_State = new Level_State();
            return level_State;
        }
    }
    private static string level_Name;
    public string Level_Name//关卡名字
    {
        get
        {
            return level_Name;
        }
    }
    private static string dialogue_Name;
    public string Dialogue_name//对话文件名字
    {
        get
        {
            return dialogue_Name;
        }
    }

    private static bool level_Quit = false;
    public bool Lecel_quit//退出关卡
    {
        get
        {
            return level_Quit;
        }
        set
        {
            level_Quit = value;
        }
    }


    
    public void Load_level(Level level)
    {
        level_Name = level.Load_Scene;
        dialogue_Name = level.Load_Scene_Dialogue;
    }
}
