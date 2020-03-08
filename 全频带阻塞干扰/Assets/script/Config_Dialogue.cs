using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;
/// <summary>
/// 配置每个关卡的对话信息与任务信息
/// </summary>
public class Config_Dialogue
{
    public List<Dialogue> dialogues = new List<Dialogue>();

    public string main_TaskGoal;
    public string secondary_TaskGoal;

    JsonData dialogue_JsonData;

    private static Config_Dialogue config_Dialogue;
    public static Config_Dialogue Config_dialogue
    {
        get
        {
            if (config_Dialogue == null) config_Dialogue = new Config_Dialogue();
            return config_Dialogue;
        }
    }

    public void Config_Dialogue_Json()
    {
        string file_Name = Level_Radio.Level_radio.Dialogue_name;

#if UNITY_EDITOR_WIN
        dialogue_JsonData = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + "/Dialogue/" + file_Name+ ".json", Encoding.GetEncoding("UTF-8")));
#endif

#if UNITY_ANDROID
        string path = "jar:file://" + Application.dataPath + "!/assets/Dialogue/"+ file_Name+ ".json";
        WWW www = new WWW(path);
        while (!www.isDone) { }

        dialogue_JsonData = JsonMapper.ToObject(www.text);
#endif
        Decode_Dialogue_Json();
    }
    public void Decode_Dialogue_Json()
    {
        dialogues.Clear();
        for (int i = 0; i < dialogue_JsonData.Count-1; i++)//任务信息在最后
        {
            string dialogue_Desc = this.dialogue_JsonData[i]["Dialogue"].ToString();
            string speaker = this.dialogue_JsonData[i]["Speaker"].ToString();

            Dialogue Dialogue = new Dialogue(dialogue_Desc, speaker);

            dialogues.Add(Dialogue);
        }
        main_TaskGoal = this.dialogue_JsonData[dialogue_JsonData.Count-1]["Main_TaskGoal"].ToString();
        secondary_TaskGoal = this.dialogue_JsonData[dialogue_JsonData.Count-1]["Secondary_TaskGoal"].ToString();
    }
}
