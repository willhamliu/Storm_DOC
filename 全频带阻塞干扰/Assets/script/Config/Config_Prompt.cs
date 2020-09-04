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
public class Config_Prompt: BaseConfigJson<Config_Prompt>
{
    public List<string> prompts = new List<string>();
    JsonData prompt;
   
    public void Config_Prompt_Json()
    {
        prompt = ReadJson(prompt, "/Level_load_prompt_json.json");
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
