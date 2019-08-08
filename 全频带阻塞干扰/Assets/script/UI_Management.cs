using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Management : MonoBehaviour {

    Tweener Setting_Insert;
    public GameObject List_Main_Canvas;//遮挡主页
    public GameObject List_Canvas;//控制图鉴淡入淡出画布
    public GameObject Combat_Main_Canvas;//遮挡主页
    public GameObject Combat_Canvas;//控制战斗面板淡入淡出画布

    public GameObject Button_Canvas;

    public GameObject Quit_panel;

    public GameObject List_Content;
    public GameObject Combat_Content;


    public Transform Setting_Panel;
    public Transform Insert_point;//插入位置
    public Transform Raw_point;//原始位置

    public Button Combat_close;
    public Button List_close;
    public Button Setting_close;

    public Button Quit_confrim;
    public Button Quit_cancel;


    public Transform Main_Button;
    private Button[] buttons;
  
    void Awake()
    {
        Combat_Content.SetActive(false);
        List_Content.SetActive(false);
        Quit_panel.SetActive(false);
        Combat_Main_Canvas.SetActive(false);
        Combat_Canvas.SetActive(false);
        List_Main_Canvas.SetActive(false);
        List_Canvas.SetActive(false);
        Button_Canvas.SetActive(false);
    }
    void Start()
    {
        Audio_Management.Audio_management.BGM_play("Home_BGM");
        buttons = new Button[Main_Button.childCount];
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i] = Main_Button.GetChild(i).GetComponent<Button>();
            buttons[i].onClick.AddListener(Audo_Button);
        }
    
        buttons[0].onClick.AddListener(Combat_Open);
        buttons[1].onClick.AddListener(Demo);
        buttons[2].onClick.AddListener(Setting_Open);
        buttons[3].onClick.AddListener(List_Open);
        buttons[4].onClick.AddListener(Quit);

        Combat_close.onClick.AddListener(Combat_Close);
        List_close.onClick.AddListener(List_Close);
        Setting_close.onClick.AddListener(Setting_Close);

        Quit_cancel.onClick.AddListener(Quit_Cancel);
        Quit_confrim.onClick.AddListener(Quit_Confrim);
    }


    public void Demo()//demo
    {
        Debug.Log("demo");
    }

    public void Setting_Open()//打开设置
    {
        Button_Canvas.SetActive(true);
        Setting_Insert = Setting_Panel.DOLocalMoveX(Insert_point.localPosition.x, 0.3f);
        Setting_Insert.SetAutoKill(false);
    }
    public void Setting_Close()//关闭设置
    {
        Audio_Management.Audio_management.SFXS_play("返回");
        Button_Canvas.SetActive(false);
        Setting_Insert = Setting_Panel.DOLocalMoveX(Raw_point.localPosition.x, 0.3f);
        Setting_Insert.SetAutoKill(false);
    }


    public void List_Open()//打开图鉴
    {
        StartCoroutine(List_ON());
    }
    public void List_Close()//关闭图鉴
    {
        Audio_Management.Audio_management.SFXS_play("返回");
        StartCoroutine(List_OFF());
    }


    public void Combat_Open()//开启战斗面板
    {
        StartCoroutine(Combat_ON());
    }
    public void Combat_Close()//关闭战斗面板
    {
        Audio_Management.Audio_management.SFXS_play("返回");
        StartCoroutine(Combat_OFF());
    }


    public void Quit()//退出
    {
        Button_Canvas.SetActive(true);
        Quit_panel.SetActive(true);
    }
    public void Quit_Cancel()
    {
        Audio_Management.Audio_management.SFXS_play("按钮点击");
        Button_Canvas.SetActive(false);
        Quit_panel.SetActive(false);
    }
    public void Quit_Confrim()
    {
        Audio_Management.Audio_management.BGM_stop("Home_BGM");
        Application.Quit();
    }


    public void Audo_Button()//播放点击音效
    {
        Audio_Management.Audio_management.SFXS_play("按钮点击");
    }

    public IEnumerator Combat_ON()
    {
        Combat_Main_Canvas.SetActive(true);
        float a = 0;
        float b = 255;
        while (b >= 0)
        {
            a = a + 25;
            Combat_Main_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, a / 255);
            Combat_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, b / 255);

            if (a > 225)
            {
                b = b - 25;
                if (a == 250)
                {
                    Combat_Canvas.SetActive(true);
                    Combat_Content.SetActive(true);
                    Combat_Panel.Combat_panel.Open_Combat();
                }
            }
            if (b < 0)
            {
                Combat_Canvas.SetActive(false);
            }
            yield return null;
        }
        yield return null;
    }
    public IEnumerator Combat_OFF()
    {
        Combat_Canvas.SetActive(true);
        float a = 255;
        float b = 0;
        while (a >= 0)
        {
            b = b + 25;
            Combat_Main_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, a / 255);
            Combat_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, b / 255);
           
            if (b > 225)
            {
                a = a - 25;
                if (b == 250)
                {
                    Combat_Canvas.SetActive(false);
                    Combat_Content.SetActive(false);
                }
            }

            if (a < 0)
            {
                Combat_Main_Canvas.SetActive(false);
                Combat_Panel.Combat_panel.Close_Combat();
            }
            yield return null;
        }
        yield return null;
    }

    public IEnumerator List_ON()
    {
        List_Main_Canvas.SetActive(true);
        float a = 0;
        float b = 255;
        while (b >= 0)
        {
            a = a + 25;
            List_Main_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, a / 255);
            List_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, b / 255);
           
            if (a > 225)
            {
                b = b - 25;
                if (a == 250)
                {
                    List_Canvas.SetActive(true);
                    List_Content.SetActive(true);
                    Item_Panel.Item_panel.Open_list();
                }
            }
            if (b < 0)
            {
                List_Canvas.SetActive(false);
            }
            yield return null;
        }
        yield return null;
    }
    public IEnumerator List_OFF()
    {
        List_Canvas.SetActive(true);
        float a = 255;
        float b = 0;
        while (a >= 0)
        {
            b = b + 25;
            List_Main_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, a / 255);
            List_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, b / 255);
           
            if (b>225)
            {
                a = a - 25;
                if (b == 250)
                {
                    List_Canvas.SetActive(false);
                    List_Content.SetActive(false);
                }
            }

            if (a<0)
            {
                List_Main_Canvas.SetActive(false);
                Item_Panel.Item_panel.Close_list();
                Item_Model.Item_model.Close_list();
            }
            yield return null;
        }
        yield return null;
    }
}
