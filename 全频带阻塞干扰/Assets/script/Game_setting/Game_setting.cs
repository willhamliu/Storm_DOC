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

    public Button setting_Open;
    public Button setting_Close;
    public Button level_Quit;
    public Button level_Again;

    public Button confrim;
    public Button cancel;


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
        }

        setting_Open.onClick.AddListener(Setting_OpenOClick);
        setting_Close.onClick.AddListener(Setting_CloseOClick);
        level_Quit.onClick.AddListener(Level_QuitOClick);
        level_Again.onClick.AddListener(Level_AgainOClick);

        confrim.onClick.AddListener(ConfrimOClick);
        cancel.onClick.AddListener(CancelOClick);
    }

    void Setting_ToggleOClick(bool value, int index)
    {
        if (insert==true)
        {
            Audio_Management.instance.SFXS_play("阵营切换");
        }

        if (last_SettingToggle_Index != index && value ==true)
        {
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
        Audio_Management.instance.SFXS_play("按钮点击");
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
        Audio_Management.instance.SFXS_play("按钮点击");
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
        Audio_Management.instance.SFXS_play("返回");
        setting_Canvas.SetActive(false);
        setting_Raw = setting_Panel.DOLocalMoveX(raw_Point.localPosition.x, 0.3f);
        Invoke("Reduction",0.3f);
    }
    void Reduction()
    {
        setting_Toggle[0].isOn = true;
    }
}
