using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;

public class Config_Dialogue
{
    public List<string> dialogues = new List<string>();
    JsonData dialogue;

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
        string file_Name = Level_Get.Level_get.Dialogue_name;

#if UNITY_EDITOR_WIN
        dialogue = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + "/Dialogue/" + file_Name+ ".json", Encoding.GetEncoding("UTF-8")));
#endif

#if UNITY_ANDROID
        string path = "jar:file://" + Application.dataPath + "!/assets/Dialogue/"+ file_Name+ ".json";
        WWW www = new WWW(path);
        while (!www.isDone) { }

        dialogue = JsonMapper.ToObject(www.text);
#endif
        Decode_Dialogue_Json();
    }
    public void Decode_Dialogue_Json()
    {
        for (int i = 0; i < dialogue.Count; i++)
        {
            string dialogue_desc = dialogue[i]["Dialogue"].ToString();
            dialogues.Add(dialogue_desc);
        }
    }
}
