using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UI_Management : MonoBehaviour {

    Tweener Setting_Insert;
    public GameObject Main_Canvas;//图鉴
    public GameObject List_Canvas;//控制图鉴淡入淡出画布
    public GameObject Setting_Canvas;

    public Transform Setting_Panel;
    public Transform Insert_point;//插入位置
    public Transform Raw_point;//原始位置

    public Button List_close;
    public Button Setting_close;


    public Transform Main_Button;
    private Button[] buttons;

    public Transform Content;
    private GameObject[] List_Content;

    void Awake()
    {
        Main_Canvas.SetActive(false);
        List_Canvas.SetActive(false);
        Setting_Canvas.SetActive(false);
    }
    void Start()
    {
        Audio_Management.Audio_management.BGM_play("Home_BGM");
        buttons = new Button[Main_Button.childCount];
        List_Content = new GameObject[Content.childCount];
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i] = Main_Button.GetChild(i).GetComponent<Button>();
            buttons[i].onClick.AddListener(Audo_Button);
        }
        for (int i = 0; i < List_Content.Length; i++)
        {
            List_Content[i] = Content.transform.GetChild(i).gameObject;
            List_Content[i].SetActive(false);
        }


        buttons[0].onClick.AddListener(Combat);
        buttons[1].onClick.AddListener(Demo);
        buttons[2].onClick.AddListener(Setting_Open);
        buttons[3].onClick.AddListener(List_Open);
        buttons[4].onClick.AddListener(Exit);

        List_close.onClick.AddListener(List_Close);
        Setting_close.onClick.AddListener(Setting_Close);
    }


    public void Combat()//开始游戏
    {
        Debug.Log("开始游戏");
    }
    public void Demo()//demo
    {
        Debug.Log("demo");
    }

    public void Setting_Open()//打开设置
    {
        Setting_Canvas.SetActive(true);
        Setting_Insert = Setting_Panel.DOLocalMoveX(Insert_point.localPosition.x, 0.3f);
        Setting_Insert.SetAutoKill(false);
    }
    public void Setting_Close()//关闭设置
    {
        Audio_Management.Audio_management.SFXS_play("返回");
        Setting_Canvas.SetActive(false);
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


    public void Exit()//退出
    {
        Application.Quit();
    }

    public void Audo_Button()//播放点击音效
    {
        Audio_Management.Audio_management.SFXS_play("按钮点击");
    }

    public IEnumerator List_ON()
    {
        Main_Canvas.SetActive(true);
        float a = 0;
        float b = 255;
        while (b >= 0)
        {
            a = a + 25;
            Main_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, a / 255);
            List_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, b / 255);

            if (a > 255)
            {
                Item_Panel.item_Panel.Open_list();
                List_Canvas.SetActive(true);
                for (int i = 0; i < List_Content.Length; i++)
                {
                    List_Content[i].SetActive(true);
                }
                b = b - 25;
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
            Main_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, a / 255);
            List_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, b / 255);
            if (b>255)
            {
                List_Canvas.SetActive(false);
                for (int i = 0; i < List_Content.Length; i++)
                {
                    List_Content[i].SetActive(false);
                }
                a = a - 25;
            }
            
            if (a<0)
            {
                Main_Canvas.SetActive(false);
                Item_Panel.item_Panel.Close_list();
                Item_Model.item_Model.Close_list();
            }
            yield return null;
        }
        yield return null;
    }
}
