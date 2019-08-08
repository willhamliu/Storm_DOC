using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combat_Panel : MonoBehaviour
{
    List<Toggle> Option;

    List<Toggle> Upgrade_All = new List<Toggle>();
    List<Level> Level_Data_All;
    List<GameObject> Level_All =new List<GameObject>();
    public Toggle Option_Mission;
    public Toggle Option_Customize;

    public GameObject Mission_Content;
    public GameObject Customize_Content;
    public GameObject Level_temp;

    public GameObject Option_Canvas;
    public GameObject Level_canvas;
    public GameObject Upgrade_Canvas;

    public Transform level_create;
    public Transform Upgrade_create;

    int Last_Option_index;
    public int Last_Level_index;
    int Last_Upgrade_index;

    public static Combat_Panel Combat_panel;
    private void Awake()
    {
        Combat_panel = this;

        Config_Level.Config_level.Config_Level_Json();
        Config_Customize.Config_customize.Confing_Customize_Json();

        Level_Data_All = Config_Level.Config_level.Level_Data_All;

       
        if (PlayerPrefs.HasKey("level_index"))
        {
            Last_Level_index = PlayerPrefs.GetInt("level_index");
        }

        for (int i = 0; i < Upgrade_create.childCount; i++)
        {
            Upgrade_All.Add(Upgrade_create.GetChild(i).GetComponent<Toggle>());
        }

        Option = new List<Toggle>();
        Option.Add(Option_Mission);
        Option.Add(Option_Customize);

        Option_Mission.isOn = true;
        Option_Canvas.SetActive(false);
        Level_canvas.SetActive(false);
        Upgrade_Canvas.SetActive(false);
        Mission_Content.SetActive(true);
        Customize_Content.SetActive(false);
    }
    void Start()
    {
        CreateAlllevel();
        for (int i = 0; i < Option.Count; i++)
        {
            var index = Option.IndexOf(Option[i]);
            Option[i].onValueChanged.AddListener((bool value) => { Toggle_Option(ref value, ref index); });
        }
        for (int i = 0; i < Level_All.Count; i++)
        {
            var level = Config_Level.Config_level.Level_Data_All[i];
            var index = Level_All.IndexOf(Level_All[i]);
            Level_All[i].GetComponent<Toggle>().onValueChanged.AddListener((bool value) => { Toggle_level(ref level, ref index, ref value); });
        }
        for (int i = 0; i < Upgrade_All.Count; i++)
        {
            var customize = Config_Customize.Config_customize.Customize_Data_All[i];
            var index = Upgrade_All.IndexOf(Upgrade_All[i]);
            Upgrade_All[i].onValueChanged.AddListener((bool value) => { Toggle_Upgrade(ref index, ref value); });
        }

        Level_All[Last_Level_index].GetComponent<Toggle>().isOn = true;
        Upgrade_All[0].isOn = true;
    }
    public void CreateAlllevel()//加载关卡数据
    {
        Level_temp.SetActive(false);
        for (int i = 0; i < Level_Data_All.Count; i++)
        {
            GameObject Level_UI = GameObject.Instantiate(Level_temp, level_create);
            Level_All.Add(Level_UI);
            Level_Info info = Level_UI.GetComponent<Level_Info>();
            info.Namedata(this.Level_Data_All[i]);
            Level_UI.SetActive(true);
        }
    }
    public void Open_Combat()
    {
        var star_index=0;
        var customize = Config_Customize.Config_customize.Customize_Data_All[0];

        Level_Detail.Level_detail.SetData(Config_Level.Config_level.Level_Data_All[Last_Level_index]);
        Customize_Data.Customize_data.SetData (ref customize, ref star_index);

        Mission_Content.SetActive(true);
        Customize_Content.SetActive(false);
    }
    public void Close_Combat()
    {
        //当父物体未激活时虽然toggle可以控制，但toggle group会失效，所以要强行关闭已经打开的toggle
        Option_Mission.isOn = true;
        Option_Customize.isOn = false;
     
        Upgrade_All[Last_Upgrade_index].isOn = false;
        Upgrade_All[0].isOn = true;
        PlayerPrefs.SetInt("level_index", Last_Level_index);
        PlayerPrefs.Save();
    }

    public void Buy(int coount)
    {
        PlayerPrefs.SetInt("point", coount);
        PlayerPrefs.Save();
    }

    public void Toggle_Option( ref bool value,ref int index)//切换选项
    {

        if (Last_Option_index != index && value == true)
        {
            if (gameObject.activeInHierarchy == true)
            {
                Audio_Management.Audio_management.SFXS_play("按钮点击");
                StartCoroutine(Toggle_option(index));
            }
            Last_Option_index = index;
        }
    }

    public void Toggle_level(ref Level level, ref int index, ref bool value)//切换关卡
    {
        if (Last_Level_index!=index && value == true)
        {
            if (gameObject.activeInHierarchy == true)
            {
                Audio_Management.Audio_management.SFXS_play("单位切换");
                StartCoroutine(Toggle_level(level));
            }
            Last_Level_index = index;
        }
    }

    public void Toggle_Upgrade(ref int index, ref bool value)//切换升级项目
    {
        if (Last_Upgrade_index!=index && value == true)
        {
            if (gameObject.activeInHierarchy == true)
            {

                Audio_Management.Audio_management.SFXS_play("单位切换");
                StartCoroutine(Upgrade_level(index));
            }
            Last_Upgrade_index = index;
        }
    }
   
    public IEnumerator Toggle_level(Level level)
    {
        float a = 0;
        float b = 255;
        while (b >= 0)
        {
            Level_canvas.SetActive(true);
            a = a + 25;
            if (a < 225)
            {
                Level_canvas.GetComponent<Image>().color = new Color(0, 0, 0, a / 255);
            }
            else
            {
                b = b - 25;
                Level_canvas.GetComponent<Image>().color = new Color(0, 0, 0, b / 255);
            }
            if (a == 250)
            {
                Level_Detail.Level_detail.SetData(level);
            }
            if (b < 0)
            {
                Level_canvas.SetActive(false);
            }
            yield return null;
        }
        yield return null;
    }
    public IEnumerator Upgrade_level(int index)
    {
        float a = 0;
        float b = 255;
        while (b >= 0)
        {
            Upgrade_Canvas.SetActive(true);
            a = a + 25;
            if (a < 225)
            {
                Upgrade_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, a / 255);
            }
            else
            {
                b = b - 25;
                Upgrade_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, b / 255);
            }
            if (a == 250)
            {
                var customize = Config_Customize.Config_customize.Customize_Data_All[index];
                Customize_Data.Customize_data.SetData( ref customize,ref index);
            }
            if (b < 0)
            {
                Upgrade_Canvas.SetActive(false);
            }
            yield return null;
        }
        yield return null;
    }
    public IEnumerator Toggle_option(int index)
    {
        float a = 0;
        float b = 255;
        while (b >= 0)
        {
            Option_Canvas.SetActive(true);
            a = a + 25;
            if (a < 225)
            {
                Option_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, a / 255);
            }
            else
            {
                b = b - 25;
                Option_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, b / 255);
            }
            if (a == 250)
            {
                if (index == 0)
                {
                    Mission_Content.SetActive(true);
                    Customize_Content.SetActive(false);
                }
                if (index == 1)
                {
                    Mission_Content.SetActive(false);
                    Customize_Content.SetActive(true);
                }
            }
            if (b < 0)
            {
                Option_Canvas.SetActive(false);
            }
            yield return null;
        }
        yield return null;
    }
}
