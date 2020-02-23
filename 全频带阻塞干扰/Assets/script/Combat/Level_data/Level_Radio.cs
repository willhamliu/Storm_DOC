using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Radio
{
    private static Level_Radio level_Radio;
    public static Level_Radio Level_radio
    {
        get
        {
            if (level_Radio == null) level_Radio = new Level_Radio();
            return level_Radio;
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
    public bool Level_quit//退出关卡
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
