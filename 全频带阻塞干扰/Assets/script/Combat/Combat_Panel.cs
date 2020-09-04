using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
/// <summary>
/// 开始面板的UI逻辑
/// </summary>
public class Combat_Panel : MonoBehaviour
{
    public static Combat_Panel instance;

    List<Toggle> option = new List<Toggle>();
    List<Level> level_Data_All = new List<Level>();

    List<Toggle> upgrade_All = new List<Toggle>();
    List<GameObject> level_All =new List<GameObject>();
    public Toggle option_Mission;
    public Toggle option_Customize;

    public GameObject mission_Content;
    public GameObject customize_Content;

    public GameObject option_Canvas;
    public GameObject level_canvas;
    public GameObject upgrade_Canvas;

    public Transform level_create;
    public Transform upgrade_create;

    public ToggleGroup levelItemGroup;

    int last_Option_index;
    int last_Level_index;
    int last_Upgrade_index;
    bool isClose;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        Config_Level.Instance.Config_Level_Json();
        Config_Customize.Instance.Confing_Customize_Json();
        

        level_Data_All = Config_Level.Instance.level_DataAll;


        if (PlayerPrefs.HasKey("level_index"))
        {
            last_Level_index = PlayerPrefs.GetInt("level_index");
        }

        for (int i = 0; i < upgrade_create.childCount; i++)
        {
            upgrade_All.Add(upgrade_create.GetChild(i).GetComponent<Toggle>());
        }

        option.Add(option_Mission);
        option.Add(option_Customize);

        option_Mission.isOn = true;
        option_Canvas.SetActive(false);
        level_canvas.SetActive(false);
        upgrade_Canvas.SetActive(false);
        mission_Content.SetActive(true);
        customize_Content.SetActive(false);
    }
    void Start()
    {
        Item_Model.Instance.LoadModel();
        CreateAlllevel();
        for (int i = 0; i < option.Count; i++)
        {
            var index = option.IndexOf(option[i]);
            option[i].onValueChanged.AddListener((bool value) => { Toggle_OptionOClick(value, index); });
            UI_Management.instance.AddButtonEventTrigger<Toggle>(option[i].gameObject, audioName: "按钮点击", callBackAudio: Audio_Management.instance.SFXS_play);
        }
        for (int i = 0; i < level_All.Count; i++)
        {
            var level = Config_Level.Instance.level_DataAll[i];
            var index = level_All.IndexOf(level_All[i]);
            level_All[i].GetComponent<Toggle>().onValueChanged.AddListener((bool value) => { Toggle_levelOnClick(level, index, value); });
            UI_Management.instance.AddButtonEventTrigger<Toggle>(level_All[i], audioName: "单位切换", callBackAudio: Audio_Management.instance.SFXS_play);
        }
        for (int i = 0; i < upgrade_All.Count; i++)
        {
            var customize = Config_Customize.Instance.customize_DataAll[i];
            var index = upgrade_All.IndexOf(upgrade_All[i]);
            upgrade_All[i].onValueChanged.AddListener((bool value) => { Toggle_UpgradeOnClick(index, value); });
            UI_Management.instance.AddButtonEventTrigger<Toggle>(upgrade_All[i].gameObject, audioName: "单位切换", callBackAudio: Audio_Management.instance.SFXS_play);
        }
        level_All[last_Level_index].GetComponent<Toggle>().isOn = true;
        upgrade_All[0].isOn = true;
    }
    public void CreateAlllevel()//加载关卡数据
    {
        for (int i = 0; i < level_Data_All.Count; i++)
        {
            Resources_Management.Instance.Load<GameObject>("UI/Level", level_create, (LevelToggle) =>
            {
                level_All.Add(LevelToggle);
                LevelToggle.GetComponent<Toggle>().group = levelItemGroup;
                Level_Info info = LevelToggle.GetComponent<Level_Info>();
                info.Namedata(this.level_Data_All[i]);
            });
        }
    }
    public void Open_Combat()
    {
        isClose = false;
        var star_index=0;
        var customize = Config_Customize.Instance.customize_DataAll[0];

        Level_Detail.instance.SetData(Config_Level.Instance.level_DataAll[last_Level_index]);
        Customize_Data.instance.SetData (ref customize, ref star_index);
        level_All[last_Level_index].GetComponent<Toggle>().interactable = false;
        upgrade_All[0].interactable = false;
        option[0].interactable = false;

        mission_Content.SetActive(true);
        customize_Content.SetActive(false);
    }
    public void Close_Combat()
    {
        isClose = true;
        upgrade_All[0].isOn = true;
        option[0].isOn = true;
        PlayerPrefs.SetInt("level_index", last_Level_index);
        PlayerPrefs.Save();
    }

    public void Buy(int coount)
    {
        PlayerPrefs.SetInt("point", coount);
        PlayerPrefs.Save();
    }

    public void Toggle_OptionOClick(bool value, int index)//切换选项
    {
        if (last_Option_index != index && value == true)
        {
            option[last_Option_index].interactable = true;
            option[index].interactable = false;
            if (!isClose)
            {
                option_Canvas.SetActive(true);
                option_Canvas.GetComponent<Image>().DOColor(new Color(0, 0, 0, 1), 0.12f).OnComplete(() =>
                {
                    if (index == 0)
                    {
                        mission_Content.SetActive(true);
                        customize_Content.SetActive(false);
                    }
                    if (index == 1)
                    {
                        mission_Content.SetActive(false);
                        customize_Content.SetActive(true);
                    }
                    option_Canvas.GetComponent<Image>().DOColor(new Color(0, 0, 0, 0), 0.12f).OnComplete(() =>
                    {
                        option_Canvas.SetActive(false);
                    });
                });
            }
            last_Option_index = index;
        }
    }

    public void Toggle_levelOnClick(Level level, int index, bool value)//切换关卡
    {
        if (last_Level_index!=index && value == true)
        {
            level_All[last_Level_index].GetComponent<Toggle>().interactable = true;
            level_All[index].GetComponent<Toggle>().interactable = false;
            if (level_All[index].activeInHierarchy == true)
            {
                level_canvas.SetActive(true);
                level_canvas.GetComponent<Image>().DOColor(new Color(0, 0, 0, 1), 0.12f).OnComplete(()=> 
                {
                    Level_Detail.instance.SetData(level);
                    level_canvas.GetComponent<Image>().DOColor(new Color(0, 0, 0, 0), 0.12f).OnComplete(()=> 
                    {
                        level_canvas.SetActive(false);
                    });
                });
            }
            last_Level_index = index;
        }
    }

    public void Toggle_UpgradeOnClick(int index, bool value)//切换升级项目
    {
        if (last_Upgrade_index!=index && value == true)
        {
            upgrade_All[last_Upgrade_index].interactable = true;
            upgrade_All[index].interactable = false;
            if (!isClose)
            {
                upgrade_Canvas.SetActive(true);
                upgrade_Canvas.GetComponent<Image>().DOColor(new Color(0, 0, 0, 1), 0.12f).OnComplete(() =>
                {
                    var customize = Config_Customize.Instance.customize_DataAll[index];
                    Customize_Data.instance.SetData(ref customize, ref index);
                    upgrade_Canvas.GetComponent<Image>().DOColor(new Color(0, 0, 0, 0), 0.12f).OnComplete(() =>
                    {
                        upgrade_Canvas.SetActive(false);
                    });
                });
            }
            last_Upgrade_index = index;
        }
    }
}
