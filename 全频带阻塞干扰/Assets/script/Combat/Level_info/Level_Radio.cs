using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 储存需要加载的关卡，与关卡状态
/// </summary>
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
    public string Level_name//关卡名字
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

    private static bool islevel_Quit = false;
    public bool IsLevel_quit//是否退出了关卡
    {
        get
        {
            return islevel_Quit;
        }
        set
        {
            islevel_Quit = value;
        }
    }

    private static bool islevel_Again = false;
    public bool IsLevel_again//是否重新开始关卡
    {
        get
        {
            return islevel_Again;
        }
        set
        {
            islevel_Again = value;
        }
    }


    public void Load_level(Level level)
    {
        level_Name = level.load_Scene;
        dialogue_Name = level.load_Scene_Dialogue;
    }
}
