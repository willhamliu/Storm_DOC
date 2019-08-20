using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using LitJson;
using UnityEngine.UI;

public class Config_Level
{
    public List<Level> level_Data_All = new List<Level>();
    JsonData level_Json_Data;

    private static Config_Level config_Level;
    public static Config_Level Config_level
    {
        get
        {
            if (config_Level == null) config_Level = new Config_Level();
            return config_Level;
        }
    }

   
    public void Config_Level_Json()
    {
#if UNITY_EDITOR_WIN
        level_Json_Data = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + "/Level_Json.json", Encoding.GetEncoding("UTF-8")));
#endif

#if UNITY_ANDROID
        string path = "jar:file://" + Application.dataPath + "!/assets/Level_Json.json";
        WWW www = new WWW(path);
        while (!www.isDone) { }

        level_Json_Data = JsonMapper.ToObject(www.text);
#endif

        Decode_Level_Json();
    }
    private void Decode_Level_Json()
    {
        for (int i = 0; i < level_Json_Data.Count; i++)
        {
            string level_Name = level_Json_Data[i]["Level_Name"].ToString();
            string level_Desc = level_Json_Data[i]["Level_Desc"].ToString();
            string load_Scene = level_Json_Data[i]["Load_Scene"].ToString();
            string load_Scene_Dialogue = level_Json_Data[i]["Load_Scene_Dialogue"].ToString();
            int level_Point =(int) level_Json_Data[i]["Level_Point"];
            Level level = new Level(level_Name, level_Desc, load_Scene, load_Scene_Dialogue, level_Point);

            level_Data_All.Add(level);
        }
    }
}
