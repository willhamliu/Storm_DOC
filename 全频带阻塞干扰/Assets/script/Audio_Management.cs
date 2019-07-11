using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Audio_Management : MonoBehaviour {

    public Text BGM_Value;
    public Slider BGM_Slider;
    public AudioSource BGM;

    public Text SFXS_Value;
    public Slider SFXS_Slider;
    public AudioSource SFXS;

    GameObject sound_play_object;


    public List<string> BGM_Path;
    public List<string> SFXS_Path;

    public Dictionary<string, AudioSource> Audio_BGM=new Dictionary<string, AudioSource>();
    public Dictionary<string, AudioSource> Audio_SFXS=new Dictionary<string, AudioSource>();


    public static Audio_Management Audio_management;

    void Awake()
    {
        Audio_management = this;

        BGM_Path = new List<string>();
        BGM_Path.Add("/Audio/BGM/Home_BGM");


        SFXS_Path = new List<string>();
        SFXS_Path.Add("/Audio/SFXS/单位切换");
        SFXS_Path.Add("/Audio/SFXS/阵营切换");
        SFXS_Path.Add("/Audio/SFXS/按钮点击");
        SFXS_Path.Add("/Audio/SFXS/返回");


        for (int i = 0; i < BGM_Path.Count; i++)
        {
            AudioSource AS = Resources.Load(BGM_Path[i]) as AudioSource;
            Audio_BGM.Add(i.ToString(), AS);
            Debug.Log(Audio_BGM);
        }

        for (int i = 0; i < SFXS_Path.Count; i++)
        {
            AudioSource AS = Resources.Load(SFXS_Path[i]) as AudioSource;
            Audio_SFXS.Add(i.ToString(), AS);
            Debug.Log(Audio_SFXS);
        }

        if (PlayerPrefs.HasKey("BGM_value"))
        {
            BGM_Slider.value = PlayerPrefs.GetFloat("BGM_value");
            //foreach (KeyValuePair<string, AudioSource> Audio_bgm in Audio_BGM)
            //{
            //    Audio_bgm.Value.volume = BGM_Slider.value;
            //}
        }
        if (PlayerPrefs.HasKey("SFXS_value"))
        {
            SFXS_Slider.value = PlayerPrefs.GetFloat("SFXS_value");
            //foreach (KeyValuePair<string, AudioSource> Audio_sfxs in Audio_SFXS)
            //{
            //    Audio_sfxs.Value.volume = SFXS_Slider.value;
            //}
        }
    }
    void Start()
    {
        sound_play_object = this.gameObject;
        DontDestroyOnLoad(sound_play_object);//切换场景后不销毁

        BGM_Value.text = ((int)(BGM_Slider.value*100)).ToString();
        BGM_Slider.onValueChanged.AddListener((float value) =>BGM_Adjust(value));

        SFXS_Value.text = ((int)(SFXS_Slider.value * 100)).ToString();
        SFXS_Slider.onValueChanged.AddListener((float value) => SFXS_Adjust(value));
    }

    public void BGM_Adjust( float value)//背景音乐滑动条
    {

        BGM_Value.text = ( (int)(value*100) ).ToString();

        foreach (KeyValuePair<string, AudioSource> Audio_bgm in Audio_BGM)
        {
            Audio_bgm.Value.volume = BGM_Slider.value;
        }
        PlayerPrefs.SetFloat("BGM_value",value);
        PlayerPrefs.Save();
    }

    public void SFXS_Adjust(float value)//音效滑动条
    {
        SFXS_Value.text = ((int)(value*100)).ToString();
        foreach (KeyValuePair<string, AudioSource> Audio_sfxs in Audio_SFXS)
        {
            Audio_sfxs.Value.volume = SFXS_Slider.value;
        }
        PlayerPrefs.SetFloat("SFXS_value", value);
        PlayerPrefs.Save();
    }

    public void SFXS_play( string audio_name)
    {
        if (Audio_SFXS.ContainsKey(audio_name))
        {
            SFXS.clip = Audio_SFXS[audio_name].clip;
            SFXS.Play();//播放背景音乐
        }
        else
        {
            Debug.Log("检测不到这个key");
        }
    }

    public void BGM_play( string audio_name)
    {
        if (Audio_SFXS.ContainsKey(audio_name))
        {
            SFXS.clip = Audio_SFXS[audio_name].clip;
            SFXS.Play();//播放背景音乐
        }
        else
        {
            Debug.Log("检测不到这个key");
        }
    }
}
