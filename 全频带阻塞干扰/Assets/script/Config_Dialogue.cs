using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;

public class Config_Dialogue
{
    public List<Dialogue> dialogues = new List<Dialogue>();

    public string Main_TaskGoal;
    public string Secondary_TaskGoal;

    JsonData dialogue_Json_Data;

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
        dialogue_Json_Data = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + "/Dialogue/" + file_Name+ ".json", Encoding.GetEncoding("UTF-8")));
#endif

#if UNITY_ANDROID
        string path = "jar:file://" + Application.dataPath + "!/assets/Dialogue/"+ file_Name+ ".json";
        WWW www = new WWW(path);
        while (!www.isDone) { }

        dialogue_Json_Data = JsonMapper.ToObject(www.text);
#endif
        Decode_Dialogue_Json();
    }
    public void Decode_Dialogue_Json()
    {
        dialogues.Clear();
        for (int i = 0; i < dialogue_Json_Data.Count-1; i++)//任务信息在最后
        {
            string dialogue_desc = this.dialogue_Json_Data[i]["Dialogue"].ToString();
            string speaker = this.dialogue_Json_Data[i]["Speaker"].ToString();

            Dialogue Dialogue = new Dialogue(dialogue_desc, speaker);

            dialogues.Add(Dialogue);
        }
        Main_TaskGoal = this.dialogue_Json_Data[dialogue_Json_Data.Count-1]["Main_TaskGoal"].ToString();
        Secondary_TaskGoal = this.dialogue_Json_Data[dialogue_Json_Data.Count-1]["Secondary_TaskGoal"].ToString();
    }
}
