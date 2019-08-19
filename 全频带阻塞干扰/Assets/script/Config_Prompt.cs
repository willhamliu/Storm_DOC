using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;
using System.Text;

public class Config_Prompt : MonoBehaviour
{
    public Text prompt_Text;
    List<string> prompts = new List<string>();
    JsonData prompt;

    void Awake()
    {
        Config_Prompt_Json();
    }
    private void Start()
    {
        int i = Random.Range(0, prompts.Count);
        prompt_Text.text = prompts[i];
    }
    public void Config_Prompt_Json()
    {
#if UNITY_EDITOR_WIN
        prompt = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + "/Level_load_prompt_json.json", Encoding.GetEncoding("UTF-8")));
#endif

#if UNITY_ANDROID
        string path = "jar:file://" + Application.dataPath + "!/assets/Level_load_prompt_json.json";
        WWW www = new WWW(path);
        while (!www.isDone) { }

        prompt = JsonMapper.ToObject(www.text);
#endif
        Decode_Prompt_Json();
    }
    public void Decode_Prompt_Json()
    {
        for (int i = 0; i < prompt.Count; i++)
        {
            string prompt_desc = prompt[i]["prompt"].ToString();
            prompts.Add(prompt_desc);
        }
    }
}
