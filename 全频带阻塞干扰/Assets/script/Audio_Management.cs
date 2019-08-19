using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    static Dictionary<string, AudioClip> Audio_BGM = new Dictionary<string, AudioClip>();
    static Dictionary<string, AudioClip> Audio_SFXS = new Dictionary<string, AudioClip>();

    static AssetBundle load_BGM;
    static AssetBundle load_SFXS;

    public static Audio_Management Audio_management;

    public static bool isNoDestroyHandler = true;//是否没有DontDestroyOnLoad处理

    void Awake()
    {
        Audio_management = this;
        sound_play_object = this.gameObject;
        if (isNoDestroyHandler)
        {
            isNoDestroyHandler = false;

#if UNITY_EDITOR_WIN
            load_BGM = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/StreamingAssets/AssetBundles/audio/bgm.audio"));
            load_SFXS = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/StreamingAssets/AssetBundles/audio/sfxs.audio"));
#endif

#if UNITY_ANDROID
        load_BGM = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "!assets/AssetBundles/audio/bgm.audio"));
        load_SFXS = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "!assets/AssetBundles/audio/sfxs.audio"));
#endif
        }


        if (PlayerPrefs.HasKey("BGM_value"))
        {
            BGM.volume = PlayerPrefs.GetFloat("BGM_value");
            BGM_Slider.value = BGM.volume;
        }
        if (PlayerPrefs.HasKey("SFXS_value"))
        {
            SFXS.volume = PlayerPrefs.GetFloat("SFXS_value");
            SFXS_Slider.value = SFXS.volume;
        }
    }
    void Start()
    {

        BGM_Value.text = ((int)(BGM_Slider.value * 100)).ToString();
        BGM_Slider.onValueChanged.AddListener((float value) => BGM_Adjust(value));

        SFXS_Value.text = ((int)(SFXS_Slider.value * 100)).ToString();
        SFXS_Slider.onValueChanged.AddListener((float value) => SFXS_Adjust(value));
    }

    public void BGM_Adjust(float value)//背景音乐滑动条
    {
        BGM_Value.text = ((int)(value * 100)).ToString();

        BGM.volume = BGM_Slider.value;

        PlayerPrefs.SetFloat("BGM_value", value);
        PlayerPrefs.Save();
    }

    public void SFXS_Adjust(float value)//音效滑动条
    {
        SFXS_Value.text = ((int)(value * 100)).ToString();

        SFXS.volume = SFXS_Slider.value;

        PlayerPrefs.SetFloat("SFXS_value", value);
        PlayerPrefs.Save();
    }

    public void SFXS_play(string audio_name)
    {
        if (Audio_SFXS.ContainsKey(audio_name))
        {
            SFXS.clip = Audio_SFXS[audio_name];
            SFXS.Play();
        }
        else
        {
            GameObject load = load_SFXS.LoadAsset<GameObject>(audio_name);

            AudioClip AC = load.GetComponent<AudioSource>().clip;

            SFXS.clip = AC;
            SFXS.Play();

            Audio_SFXS.Add(AC.name, AC);
        }
    }

    public void BGM_play(string audio_name)
    {
        if (Audio_BGM.ContainsKey(audio_name))
        {
            BGM.clip = Audio_BGM[audio_name];
            BGM.Play();//播放背景音乐
        }
        else
        {
            GameObject load = load_BGM.LoadAsset<GameObject>(audio_name);

            AudioClip AC = load.GetComponent<AudioSource>().clip;

            BGM.clip = AC;
            BGM.Play();//播放背景音乐

            Audio_BGM.Add(AC.name, AC);
        }
    }
    public void BGM_stop(string audio_name)
    {
        if (Audio_BGM.ContainsKey(audio_name))
        {
            BGM.clip = Audio_BGM[audio_name];
            BGM.Play();//播放背景音乐
        }
        else
        {
            GameObject load = load_BGM.LoadAsset<GameObject>(audio_name);

            AudioClip AC = load.GetComponent<AudioSource>().clip;

            BGM.clip = AC;
            BGM.Stop();//播放背景音乐
        }
    }
}
