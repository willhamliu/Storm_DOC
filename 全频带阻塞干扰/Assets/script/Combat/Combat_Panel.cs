using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public GameObject level_Temp;

    public GameObject option_Canvas;
    public GameObject level_canvas;
    public GameObject upgrade_Canvas;

    public Transform level_create;
    public Transform upgrade_create;

    int last_Option_index;
    int last_Level_index;
    int last_Upgrade_index;

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
        CreateAlllevel();
        for (int i = 0; i < option.Count; i++)
        {
            var index = option.IndexOf(option[i]);
            option[i].onValueChanged.AddListener((bool value) => { Toggle_OptionOClick(value, index); });
            UI_Management.instance.AddButtonEventTrigger(option[i].gameObject, audioName: "按钮点击", callBackAudio: Audio_Management.instance.SFXS_play);
        }
        for (int i = 0; i < level_All.Count; i++)
        {
            var level = Config_Level.Instance.level_DataAll[i];
            var index = level_All.IndexOf(level_All[i]);
            level_All[i].GetComponent<Toggle>().onValueChanged.AddListener((bool value) => { Toggle_levelOnClick(level, index, value); });
            UI_Management.instance.AddButtonEventTrigger(level_All[i], audioName: "单位切换", callBackAudio: Audio_Management.instance.SFXS_play);
        }
        for (int i = 0; i < upgrade_All.Count; i++)
        {
            var customize = Config_Customize.Instance.customize_DataAll[i];
            var index = upgrade_All.IndexOf(upgrade_All[i]);
            upgrade_All[i].onValueChanged.AddListener((bool value) => { Toggle_UpgradeOnClick(index, value); });
            UI_Management.instance.AddButtonEventTrigger(upgrade_All[i].gameObject, audioName: "单位切换", callBackAudio: Audio_Management.instance.SFXS_play);
        }
        level_All[last_Level_index].GetComponent<Toggle>().isOn = true;
        upgrade_All[0].isOn = true;
    }
    public void CreateAlllevel()//加载关卡数据
    {
        level_Temp.SetActive(false);
        for (int i = 0; i < level_Data_All.Count; i++)
        {
            GameObject Level_UI = GameObject.Instantiate(level_Temp, level_create);
            level_All.Add(Level_UI);
            Level_Info info = Level_UI.GetComponent<Level_Info>();
            info.Namedata(this.level_Data_All[i]);
            Level_UI.SetActive(true);
        }
    }
    public void Open_Combat()
    {
        var star_index=0;
        var customize = Config_Customize.Instance.customize_DataAll[0];

        Level_Detail.instance.SetData(Config_Level.Instance.level_DataAll[last_Level_index]);
        Customize_Data.instance.SetData (ref customize, ref star_index);

        mission_Content.SetActive(true);
        customize_Content.SetActive(false);
    }
    public void Close_Combat()
    {
        //当父物体未激活时虽然toggle可以控制，但toggle group会失效，所以要强行关闭已经打开的toggle
        option_Mission.isOn = true;
        option_Customize.isOn = false;
     
        upgrade_All[last_Upgrade_index].isOn = false;
        upgrade_All[0].isOn = true;
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
            if (option[index].gameObject.activeInHierarchy == true)
            {
                StartCoroutine(Toggle_option(index));
            }
            last_Option_index = index;
        }
    }

    public void Toggle_levelOnClick(Level level, int index, bool value)//切换关卡
    {
        if (last_Level_index!=index && value == true)
        {
            if (level_All[index].activeInHierarchy == true)
            {
                StartCoroutine(Toggle_level(level));
            }
            last_Level_index = index;
        }
    }

    public void Toggle_UpgradeOnClick(int index, bool value)//切换升级项目
    {
        if (last_Upgrade_index!=index && value == true)
        {
            if (upgrade_All[index].gameObject.activeInHierarchy == true)
            {
                StartCoroutine(Upgrade_level(index));
            }
            last_Upgrade_index = index;
        }
    }
   
    public IEnumerator Toggle_level(Level level)
    {
        float a = 0;
        float b = 255;
        while (b >= 0)
        {
            level_canvas.SetActive(true);
            a = a + 25;
            if (a < 225)
            {
                level_canvas.GetComponent<Image>().color = new Color(0, 0, 0, a / 255);
            }
            else
            {
                b = b - 25;
                level_canvas.GetComponent<Image>().color = new Color(0, 0, 0, b / 255);
            }
            if (a == 250)
            {
                Level_Detail.instance.SetData(level);
            }
            if (b < 0)
            {
                level_canvas.SetActive(false);
            }
            yield return null;
        }
    }
    public IEnumerator Upgrade_level(int index)
    {
        float a = 0;
        float b = 255;
        while (b >= 0)
        {
            upgrade_Canvas.SetActive(true);
            a = a + 25;
            if (a < 225)
            {
                upgrade_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, a / 255);
            }
            else
            {
                b = b - 25;
                upgrade_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, b / 255);
            }
            if (a == 250)
            {
                var customize = Config_Customize.Instance.customize_DataAll[index];
                Customize_Data.instance.SetData( ref customize,ref index);
            }
            if (b < 0)
            {
                upgrade_Canvas.SetActive(false);
            }
            yield return null;
        }
    }
    public IEnumerator Toggle_option(int index)
    {
        float a = 0;
        float b = 255;
        while (b >= 0)
        {
            option_Canvas.SetActive(true);
            a = a + 25;
            if (a < 225)
            {
                option_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, a / 255);
            }
            else
            {
                b = b - 25;
                option_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, b / 255);
            }
            if (a == 250)
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
            }
            if (b < 0)
            {
                option_Canvas.SetActive(false);
            }
            yield return null;
        }
    }
}
