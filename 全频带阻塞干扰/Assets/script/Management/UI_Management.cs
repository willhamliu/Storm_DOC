﻿using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Management : MonoBehaviour
{
    public static UI_Management ui_Management;

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

    public Button combat_Close;
    public Button list_Close;
    public Button setting_Close;

    public Button quit_Confrim;
    public Button quit_Cancel;


    void Awake()
    {
        if (ui_Management==null)
        {
            ui_Management = this;
        }
        if (Level_Radio.Level_radio.Level_quit == false)
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
            Level_Radio.Level_radio.Level_quit = false;
        }
        Config_Item.Config_item.Config_Item_Json();
    }
    void Start()
    {
        Audio_Management.audio_Management.BGM_play("Home_BGM");
    }


    public void DemoOnClick()//打开Demo场景
    {
        SceneManager.LoadScene("Demo");
    }

    public void Setting_OpenOnClick()//打开设置
    {
        home_Canvas.SetActive(true);
        setting_Insert = setting_Panel.DOLocalMoveX(insert_point.localPosition.x, 0.3f);
    }
    public void Setting_CloseOnClick()//关闭设置
    {
        Audio_Management.audio_Management.SFXS_play("返回");
        home_Canvas.SetActive(false);
        setting_Insert = setting_Panel.DOLocalMoveX(raw_point.localPosition.x, 0.3f);
    }


    public void List_OpenOnClick()//打开图鉴
    {
        fx.SetActive(false);
        StartCoroutine(List_ON());
    }
    public void List_CloseOnClick()//关闭图鉴
    {
        Audio_Management.audio_Management.SFXS_play("返回");
        StartCoroutine(List_OFF());
    }


    public void Combat_OpenOnClick()//开启战斗面板
    {
        fx.SetActive(false);
        StartCoroutine(Combat_ON());
    }
    public void Combat_CloseOnClick()//关闭战斗面板
    {
        Audio_Management.audio_Management.SFXS_play("返回");
        StartCoroutine(Combat_OFF());
    }


    public void QuitOnClick()//退出
    {
        home_Canvas.SetActive(true);
        quit_Panel.SetActive(true);
    }
    public void Quit_CancelOnClick()//取消退出
    {
        Audio_Management.audio_Management.SFXS_play("按钮点击");
        home_Canvas.SetActive(false);
        quit_Panel.SetActive(false);
    }
    public void Quit_ConfrimOnClick()//确认退出
    {
        Audio_Management.audio_Management.BGM_stop("Home_BGM");
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
                    Combat_Panel.Combat_panel.Open_Combat();
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
                Combat_Panel.Combat_panel.Close_Combat();
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
                    Item_Panel.item_Panel.Open_list();
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
                Item_Panel.item_Panel.Close_list();
                Item_Model_Management.item_Model_Management.Close_list();
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
                Combat_Panel.Combat_panel.Open_Combat();
            }
            if (b < 0)
            {
                combat_Canvas.SetActive(false);
            }
            yield return null;
        }
    }
}
