using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// 管理Home场景的粒子效果以及按钮的功能实现
/// </summary>
public class Home_Panel : MonoBehaviour
{
    public static Home_Panel instance;

    Tweener setting_Insert;
    public GameObject list_Main_Canvas;//遮挡主页
    public GameObject list_Canvas;//控制图鉴淡入淡出画布
    public GameObject combat_Main_Canvas;//遮挡主页
    public GameObject combat_Canvas;//控制战斗面板淡入淡出画布

    public GameObject home_Canvas;

    public GameObject quit_Panel;

    public GameObject list_Content;
    public GameObject combat_Content;

    public GameObject fx;


    public Transform setting_Panel;
    public Transform insert_point;//插入位置
    public Transform raw_point;//原始位置



    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (Level_Radio.Instance.IsLevel_quit == false)
        {
            combat_Content.SetActive(false);
            list_Content.SetActive(false);
            quit_Panel.SetActive(false);
            combat_Main_Canvas.SetActive(false);
            combat_Canvas.SetActive(false);
            list_Main_Canvas.SetActive(false);
            list_Canvas.SetActive(false);
            home_Canvas.SetActive(false);
            fx.SetActive(true);
        }
        else
        {
            fx.SetActive(false);
            list_Main_Canvas.SetActive(false);

            StartCoroutine(Level_Quit());
            Level_Radio.Instance.IsLevel_quit = false;
        }
    }
    void Start()
    {
        Audio_Management.instance.BGM_play("Home_BGM");
        UI_Management.instance.AddButtonEventTrigger<Button>("Combat", Combat_OpenOnClick,"按钮点击", Audio_Management.instance.SFXS_play);
        UI_Management.instance.AddButtonEventTrigger<Button>("Demo", DemoOnClick, "按钮点击", Audio_Management.instance.SFXS_play);
        UI_Management.instance.AddButtonEventTrigger<Button>("Setting", Setting_OpenOnClick, "按钮点击", Audio_Management.instance.SFXS_play);
        UI_Management.instance.AddButtonEventTrigger<Button>("List", List_OpenOnClick, "按钮点击", Audio_Management.instance.SFXS_play);
        UI_Management.instance.AddButtonEventTrigger<Button>("Quit", QuitOnClick, "按钮点击", Audio_Management.instance.SFXS_play);
        UI_Management.instance.AddButtonEventTrigger<Button>("Combat_Close_Button", Combat_CloseOnClick, "返回", Audio_Management.instance.SFXS_play);
        UI_Management.instance.AddButtonEventTrigger<Button>("List_Close_Button", List_CloseOnClick, "返回", Audio_Management.instance.SFXS_play);
        UI_Management.instance.AddButtonEventTrigger<Button>("Setting_Close_Button", Setting_CloseOnClick, "返回", Audio_Management.instance.SFXS_play);
        UI_Management.instance.AddButtonEventTrigger<Button>("Quit_confrim", Quit_ConfrimOnClick, "按钮点击", Audio_Management.instance.SFXS_play);
        UI_Management.instance.AddButtonEventTrigger<Button>("Quit_cancel", Quit_CancelOnClick, "返回", Audio_Management.instance.SFXS_play);
    }

    private void Combat_OpenOnClick()//开启战斗面板
    {
        fx.SetActive(false);
        StartCoroutine(Combat_ON());
    }
    private void Combat_CloseOnClick()//关闭战斗面板
    {
        StartCoroutine(Combat_OFF());
    }

    private void DemoOnClick()//打开Demo场景
    {
        SceneManager.LoadScene("Demo");
    }

    private void Setting_OpenOnClick()//打开设置
    {
        home_Canvas.SetActive(true);
        setting_Insert = setting_Panel.DOLocalMoveX(insert_point.localPosition.x, 0.3f);
    }
    private void Setting_CloseOnClick()//关闭设置
    {
        home_Canvas.SetActive(false);
        setting_Insert = setting_Panel.DOLocalMoveX(raw_point.localPosition.x, 0.3f);
    }

    private void List_OpenOnClick()//打开图鉴
    {
        fx.SetActive(false);
        StartCoroutine(List_ON());
    }
    private void List_CloseOnClick()//关闭图鉴
    {
        StartCoroutine(List_OFF());
    }

    private void QuitOnClick()//退出
    {
        home_Canvas.SetActive(true);
        quit_Panel.SetActive(true);
    }
    private void Quit_CancelOnClick()//取消退出
    {
        Audio_Management.instance.SFXS_play("按钮点击");
        home_Canvas.SetActive(false);
        quit_Panel.SetActive(false);
    }
    private void Quit_ConfrimOnClick()//确认退出
    {
        Audio_Management.instance.BGM_stop("Home_BGM");
        Application.Quit();
    }

    public IEnumerator Combat_ON()
    {
        combat_Main_Canvas.SetActive(true);
        float a = 0;
        float b = 255;
        while (b >= 0)
        {
            a = a + 25;
            combat_Main_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, a / 255);
            combat_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, b / 255);

            if (a > 225)
            {
                b = b - 25;
                if (a == 250)
                {
                    combat_Canvas.SetActive(true);
                    combat_Content.SetActive(true);
                    Combat_Panel.instance.Open_Combat();
                }
            }
            if (b < 0)
            {
                combat_Canvas.SetActive(false);
            }
            yield return null;
        }
    }
    public IEnumerator Combat_OFF()
    {
        combat_Canvas.SetActive(true);
        float a = 255;
        float b = 0;
        while (a >= 0)
        {
            b = b + 25;
            combat_Main_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, a / 255);
            combat_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, b / 255);
           
            if (b > 225)
            {
                a = a - 25;
                if (b == 250)
                {
                    combat_Canvas.SetActive(false);
                    combat_Content.SetActive(false);
                    fx.SetActive(true);
                }
            }

            if (a < 0)
            {
                combat_Main_Canvas.SetActive(false);
                Combat_Panel.instance.Close_Combat();
            }
            yield return null;
        }
    }

    public IEnumerator List_ON()
    {
        list_Main_Canvas.SetActive(true);
        float a = 0;
        float b = 255;
        while (b >= 0)
        {
            a = a + 25;
            list_Main_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, a / 255);
            list_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, b / 255);
           
            if (a > 225)
            {
                b = b - 25;
                if (a == 250)
                {
                    list_Canvas.SetActive(true);
                    list_Content.SetActive(true);
                    Item_Panel.instance.Open_list();
                }
            }
            if (b < 0)
            {
                list_Canvas.SetActive(false);
            }
            yield return null;
        }
    }
    public IEnumerator List_OFF()
    {
        list_Canvas.SetActive(true);
        float a = 255;
        float b = 0;
        while (a >= 0)
        {
            b = b + 25;
            list_Main_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, a / 255);
            list_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, b / 255);
           
            if (b>225)
            {
                a = a - 25;
                if (b == 250)
                {
                    list_Canvas.SetActive(false);
                    list_Content.SetActive(false);
                    fx.SetActive(true);
                }
            }

            if (a<0)
            {
                list_Main_Canvas.SetActive(false);
                Item_Panel.instance.Close_list();
                Item_Model_Management.instance.Close_list();
            }
            yield return null;
        }
    }

    public IEnumerator Level_Quit()
    {
        combat_Main_Canvas.SetActive(true);
        combat_Canvas.SetActive(true);
        combat_Content.SetActive(true);
        combat_Main_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, 255 / 255);

        float b = 255;

        while (b >= 0)
        {
            combat_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, b / 255);

            b = b - 25;
            if (b == 205)
            {
                Combat_Panel.instance.Open_Combat();
            }
            if (b < 0)
            {
                combat_Canvas.SetActive(false);
            }
            yield return null;
        }
    }
}
