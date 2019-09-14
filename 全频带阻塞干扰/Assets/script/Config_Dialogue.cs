using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;

public class Config_Dialogue
{
    public List<Dialogue> dialogues = new List<Dialogue>();
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
        string file_Name = Level_State.Level_state.Dialogue_name;

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
        for (int i = 0; i < dialogue_Json_Data.Count; i++)
        {
            string dialogue_desc = this.dialogue_Json_Data[i]["Dialogue"].ToString();
            string speaker = this.dialogue_Json_Data[i]["Speaker"].ToString();

            Dialogue Dialogue = new Dialogue(dialogue_desc, speaker);

            dialogues.Add(Dialogue);
        }
    }
}
