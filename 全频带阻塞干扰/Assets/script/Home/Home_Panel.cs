using DG.Tweening;
using System.Collections;
using UnityEngine;
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

            combat_Main_Canvas.SetActive(true);
            combat_Canvas.SetActive(true);
            combat_Content.SetActive(true);
            combat_Main_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, 255 / 255);
            combat_Canvas.GetComponent<Image>().DOColor(new Color(0, 0, 0, 0), 0.15f).OnComplete(() =>
            {
                Combat_Panel.instance.Open_Combat();
                combat_Canvas.SetActive(false);
            });

            Level_Radio.Instance.IsLevel_quit = false;
        }
    }
    void Start()
    {
        Audio_Management.instance.BGM_play("Home_BGM");
        UI_Management.instance.AddButtonEventTrigger<Button>("Combat", Combat_OpenOnClick,"按钮点击", Audio_Management.instance.SFXS_play);
        UI_Management.instance.AddButtonEventTrigger<Button>("Demo", DemoOnClick);
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
        combat_Main_Canvas.SetActive(true);
        combat_Main_Canvas.GetComponent<Image>().DOColor(new Color(0, 0, 0, 1), 0.15f).OnComplete(() =>
        {
            combat_Content.SetActive(true);
            Combat_Panel.instance.Open_Combat();
            combat_Canvas.GetComponent<Image>().DOColor(new Color(0, 0, 0, 0), 0.15f).OnComplete(()=> 
            {
                combat_Canvas.SetActive(false);
            });
        });
    }
    private void Combat_CloseOnClick()//关闭战斗面板
    {
        combat_Canvas.SetActive(true);
        combat_Canvas.GetComponent<Image>().DOColor(new Color(0, 0, 0, 1), 0.15f).OnComplete(() =>
        {
            Combat_Panel.instance.Close_Combat();
            combat_Canvas.SetActive(false);
            combat_Content.SetActive(false);
            fx.SetActive(true);
            combat_Main_Canvas.GetComponent<Image>().DOColor(new Color(0, 0, 0, 0), 0.15f).OnComplete(()=> 
            {
                combat_Main_Canvas.SetActive(false);
            });
        });
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
        list_Main_Canvas.SetActive(true);
        list_Main_Canvas.GetComponent<Image>().DOColor(new Color(0, 0, 0, 1), 0.15f).OnComplete(() =>
        {
            list_Canvas.SetActive(true);
            list_Content.SetActive(true);
            Item_Panel.instance.Open_list();
            list_Canvas.GetComponent<Image>().DOColor(new Color(0, 0, 0, 0), 0.15f).OnComplete(() =>
            {
                list_Canvas.SetActive(false);
            });
        });
    }
    private void List_CloseOnClick()//关闭图鉴
    {
        list_Canvas.SetActive(true);
        list_Canvas.GetComponent<Image>().DOColor(new Color(0, 0, 0, 1), 0.15f).OnComplete(() =>
        {
            Item_Panel.instance.Close_list();
            list_Canvas.SetActive(false);
            list_Content.SetActive(false);
            fx.SetActive(true);
            list_Main_Canvas.GetComponent<Image>().DOColor(new Color(0, 0, 0, 0), 0.15f).OnComplete(() =>
            {
                list_Main_Canvas.SetActive(false);
            });
        });
    }

    private void QuitOnClick()//退出
    {
        home_Canvas.SetActive(true);
        quit_Panel.SetActive(true);
    }
    private void Quit_CancelOnClick()//取消退出
    {
        home_Canvas.SetActive(false);
        quit_Panel.SetActive(false);
    }
    private void Quit_ConfrimOnClick()//确认退出
    {
        Audio_Management.instance.BGM_stop("Home_BGM");
        Application.Quit();
    }
}
