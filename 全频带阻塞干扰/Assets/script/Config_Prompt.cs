using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;
using System.Text;
/// <summary>
/// 配置关卡加载时的操作提示
/// </summary>
public class Config_Prompt
{
    public List<string> prompts = new List<string>();
    JsonData prompt;

    private static Config_Prompt config_Prompt;
    public static Config_Prompt Config_prompt
    {
        get
        {
            if (config_Prompt == null) config_Prompt = new Config_Prompt();
            return config_Prompt;
        }
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
        prompts.Clear();//更新数据
        for (int i = 0; i < prompt.Count; i++)
        {
            string prompt_desc = prompt[i]["prompt"].ToString();
            prompts.Add(prompt_desc);
        }
    }
}
