using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 管理音频播放与音量大小调节
/// </summary>
public class Audio_Management : MonoBehaviour
{
    public static Audio_Management instance;

    public Text bgm_Value;
    public Slider bgm_Slider;
    public AudioSource bgm;

    public Text sfxs_Value;
    public Slider sfxs_Slider;
    public AudioSource sfxs;


    static Dictionary<string, AudioClip> audio_BGM = new Dictionary<string, AudioClip>();
    static Dictionary<string, AudioClip> audio_SFXS = new Dictionary<string, AudioClip>();

    static AssetBundle load_BGM;
    static AssetBundle load_SFXS;
    public static bool notload = true;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (notload)
        {
            notload = false;

#if UNITY_STANDALONE
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
            bgm.volume = PlayerPrefs.GetFloat("BGM_value");
            bgm_Slider.value = bgm.volume;
        }
        if (PlayerPrefs.HasKey("SFXS_value"))
        {
            sfxs.volume = PlayerPrefs.GetFloat("SFXS_value");
            sfxs_Slider.value = sfxs.volume;
        }
    }
    void Start()
    {

        bgm_Value.text = ((int)(bgm_Slider.value * 100)).ToString();
        bgm_Slider.onValueChanged.AddListener((float value) => BGM_AdjustOnClick(value));

        sfxs_Value.text = ((int)(sfxs_Slider.value * 100)).ToString();
        sfxs_Slider.onValueChanged.AddListener((float value) => SFXS_AdjustOnClick(value));
    }

    public void BGM_AdjustOnClick(float value)//背景音乐滑动条
    {
        bgm_Value.text = ((int)(value * 100)).ToString();

        bgm.volume = bgm_Slider.value;

        PlayerPrefs.SetFloat("BGM_value", value);
        PlayerPrefs.Save();
    }

    public void SFXS_AdjustOnClick(float value)//音效滑动条
    {
        sfxs_Value.text = ((int)(value * 100)).ToString();

        sfxs.volume = sfxs_Slider.value;

        PlayerPrefs.SetFloat("SFXS_value", value);
        PlayerPrefs.Save();
    }

    public void SFXS_play(string audioName)
    {
        if (audio_SFXS.ContainsKey(audioName))
        {
            sfxs.clip = audio_SFXS[audioName];
            sfxs.Play();
        }
        else
        {
            GameObject load = load_SFXS.LoadAsset<GameObject>(audioName);

            AudioClip AC = load.GetComponent<AudioSource>().clip;

            sfxs.clip = AC;
            sfxs.Play();

            audio_SFXS.Add(AC.name, AC);
        }
    }

    public void BGM_play(string audioName)
    {
        if (audio_BGM.ContainsKey(audioName))
        {
            bgm.clip = audio_BGM[audioName];
            bgm.Play();//播放背景音乐
        }
        else
        {
            GameObject load = load_BGM.LoadAsset<GameObject>(audioName);

            AudioClip AC = load.GetComponent<AudioSource>().clip;

            bgm.clip = AC;
            bgm.Play();//播放背景音乐

            audio_BGM.Add(AC.name, AC);
        }
    }
    public void BGM_stop(string audioName)
    {
        bgm.clip = audio_BGM[audioName];
        bgm.Stop();//停止播放背景音乐
    }
}
