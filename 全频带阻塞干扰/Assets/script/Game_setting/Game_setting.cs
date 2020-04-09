using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
/// <summary>
/// 游戏设置面板的UI逻辑
/// </summary>
public class Game_setting : MonoBehaviour
{
    Tweener setting_Insert;
    Tweener setting_Raw;

    bool insert = false;

    public Transform setting_Panel;
    public Transform insert_Point;//插入位置
    public Transform raw_Point;//原始位置

    public Toggle game_Toggle;
    public Toggle music_Toggle;

    public GameObject game_Setting;
    public GameObject music_Setting;
    public GameObject setting_Canvas;
    public GameObject quit_Panel;

    List<Toggle> setting_Toggle= new List<Toggle>();

    int last_SettingToggle_Index=0;

    void Awake()
    {
        setting_Canvas.SetActive(false);
        quit_Panel.SetActive(false);
        setting_Toggle.Add(game_Toggle);
        setting_Toggle.Add(music_Toggle);
        setting_Toggle[0].isOn = true;
    }
    void Start()
    {
        for (int i = 0; i < setting_Toggle.Count; i++)
        {
            int index = setting_Toggle.IndexOf(setting_Toggle[i]);
            setting_Toggle[i].onValueChanged.AddListener((bool value) => { Setting_ToggleOClick(value, index); });
            UI_Management.instance.AddButtonEventTrigger<Toggle>(setting_Toggle[i].gameObject, audioName: "阵营切换", callBackAudio: Audio_Management.instance.SFXS_play);
        }
        setting_Toggle[0].interactable = false;

        UI_Management.instance.AddButtonEventTrigger<Button>("Stop_Button", Setting_OpenOClick, "按钮点击", Audio_Management.instance.SFXS_play);
        UI_Management.instance.AddButtonEventTrigger<Button>("Setting_Close_Button", Setting_CloseOClick, "返回", Audio_Management.instance.SFXS_play);
        UI_Management.instance.AddButtonEventTrigger<Button>("Level_Quit_Button", Level_QuitOClick);
        UI_Management.instance.AddButtonEventTrigger<Button>("Level_Again_Button", Level_AgainOClick, "按钮点击", Audio_Management.instance.SFXS_play);
        UI_Management.instance.AddButtonEventTrigger<Button>("confrim", ConfrimOClick);
        UI_Management.instance.AddButtonEventTrigger<Button>("cancel", CancelOClick, "按钮点击", Audio_Management.instance.SFXS_play);
    }

    void Setting_ToggleOClick(bool value, int index)
    {
        if (last_SettingToggle_Index != index && value ==true)
        {
            setting_Toggle[last_SettingToggle_Index].interactable = true;
            setting_Toggle[index].interactable = false;
            if (game_Toggle.isOn == true)
            {
                game_Setting.SetActive(true);
                music_Setting.SetActive(false);
            }
            if (music_Toggle.isOn == true)
            {
                game_Setting.SetActive(false);
                music_Setting.SetActive(true);
            }
            last_SettingToggle_Index = index;
        }
    }
    private void Level_QuitOClick()//退出关卡
    {
        Level_Radio.Instance.IsLevel_quit = true;
        Level_Radio.Instance.IsLevel_again = false;
        SceneManager.LoadScene("Home");
    }
    private void Level_AgainOClick()//重新开始
    {
        quit_Panel.SetActive(true);
        setting_Panel.gameObject.SetActive(false);
    }
    private void ConfrimOClick()
    {
        Level_Radio.Instance.IsLevel_again = true;
        SceneManager.LoadScene(Level_Radio.Instance.Level_name);
    }
    private void CancelOClick()
    {
        quit_Panel.SetActive(false);
        setting_Panel.gameObject.SetActive(true);
    }


    public void Setting_OpenOClick()//打开设置
    {
        if (setting_Insert!= null && setting_Insert.IsPlaying()==true|| setting_Raw != null && setting_Raw.IsPlaying() == true)
        {
            return;
        }
        insert = true;
        setting_Canvas.SetActive(true);
        setting_Insert = setting_Panel.DOLocalMoveX(insert_Point.localPosition.x, 0.3f);
    }
    public void Setting_CloseOClick()//关闭设置
    {
        if (setting_Insert != null && setting_Insert.IsPlaying() == true || setting_Raw != null && setting_Raw.IsPlaying() == true)
        {
            return;
        }
        insert = false;
        setting_Canvas.SetActive(false);
        setting_Raw = setting_Panel.DOLocalMoveX(raw_Point.localPosition.x, 0.3f);
        Invoke("Reduction",0.3f);
    }
    void Reduction()
    {
        setting_Toggle[0].isOn = true;
    }
}
