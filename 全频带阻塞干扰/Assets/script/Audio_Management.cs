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


    public Dictionary<string, AudioClip> Audio_BGM = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> Audio_SFXS = new Dictionary<string, AudioClip>();

    AssetBundle load_BGM;
    AssetBundle load_SFXS;

    public static Audio_Management Audio_management;

    void Awake()
    {
        Audio_management = this;

#if UNITY_EDITOR_WIN
        load_BGM = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/StreamingAssets/AssetBundles/audio/bgm.audio"));
        load_SFXS = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/StreamingAssets/AssetBundles/audio/sfxs.audio"));
#endif

#if UNITY_ANDROID
        load_BGM = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "!assets/AssetBundles/audio/bgm.audio"));
        load_SFXS = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "!assets/AssetBundles/audio/sfxs.audio"));
#endif
        if (PlayerPrefs.HasKey("BGM_value"))
        {
            BGM_Slider.value = PlayerPrefs.GetFloat("BGM_value");
            BGM.volume = BGM_Slider.value;
        }
        if (PlayerPrefs.HasKey("SFXS_value"))
        {
            SFXS_Slider.value = PlayerPrefs.GetFloat("SFXS_value");
            SFXS.volume = SFXS_Slider.value;
        }
    }
    void Start()
    {
        sound_play_object = this.gameObject;
        DontDestroyOnLoad(sound_play_object);//切换场景后不销毁
        DontDestroyOnLoad(BGM);//切换场景后不销毁
        DontDestroyOnLoad(SFXS);//切换场景后不销毁

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
            SFXS.Play();//播放背景音乐
        }
        else
        {
            GameObject load = load_SFXS.LoadAsset<GameObject>(audio_name);

            AudioClip AC = load.GetComponent<AudioSource>().clip;

            SFXS.clip = AC;
            SFXS.Play();//播放背景音乐

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
}
