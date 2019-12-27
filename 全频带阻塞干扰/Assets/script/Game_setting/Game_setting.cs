using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Game_setting : MonoBehaviour
{
    Tweener setting_Insert;
    Tweener setting_Raw;

    bool insert = false;

    public Transform setting_Panel;
    public Transform insert_point;//插入位置
    public Transform raw_point;//原始位置

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

    int last_setting_toggle=0;

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
            setting_Toggle[i].onValueChanged.AddListener((bool value) => { Setting_Toggle(ref value, ref index); });
        }

        setting_Open.onClick.AddListener(Setting_Open);
        setting_Close.onClick.AddListener(Setting_Close);
        level_Quit.onClick.AddListener(Level_Quit);
        level_Again.onClick.AddListener(Level_Again);

        confrim.onClick.AddListener(Confrim);
        cancel.onClick.AddListener(Cancel);
    }

    void Setting_Toggle(ref bool value, ref int index)
    {
        if (insert==true)
        {
            Audio_Management.Audio_management.SFXS_play("阵营切换");
        }

        if (last_setting_toggle != index && value ==true)
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
            last_setting_toggle = index;
        }
    }
    private void Level_Quit()//退出关卡
    {
        Level_State.Level_state.Lecel_quit = true;
        SceneManager.LoadScene("Home");
    }
    private void Level_Again()//重新开始
    {
        Audio_Management.Audio_management.SFXS_play("按钮点击");
        quit_Panel.SetActive(true);
        setting_Panel.gameObject.SetActive(false);
    }
    private void Confrim()
    {
        Debug.Log("重新开始");
    }
    private void Cancel()
    {
        Audio_Management.Audio_management.SFXS_play("按钮点击");
        quit_Panel.SetActive(false);
        setting_Panel.gameObject.SetActive(true);
    }


    public void Setting_Open()//打开设置
    {
        if (setting_Insert!= null && setting_Insert.IsPlaying()==true|| setting_Raw != null && setting_Raw.IsPlaying() == true)
        {
            return;
        }
        insert = true;
        //Audio_Management.Audio_management.SFXS_play("按钮点击");
        setting_Canvas.SetActive(true);
        setting_Insert = setting_Panel.DOLocalMoveX(insert_point.localPosition.x, 0.3f);
    }
    public void Setting_Close()//关闭设置
    {
        if (setting_Insert != null && setting_Insert.IsPlaying() == true || setting_Raw != null && setting_Raw.IsPlaying() == true)
        {
            return;
        }
        insert = false;
        Audio_Management.Audio_management.SFXS_play("返回");
        setting_Canvas.SetActive(false);
        setting_Raw = setting_Panel.DOLocalMoveX(raw_point.localPosition.x, 0.3f);
        Invoke("Reduction",0.3f);
    }
    void Reduction()
    {
        setting_Toggle[0].isOn = true;
    }
}
