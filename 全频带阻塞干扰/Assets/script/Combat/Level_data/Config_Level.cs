using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using LitJson;
using UnityEngine.UI;

public class Config_Level : MonoBehaviour
{
    public List<Level> Level_Data_All = new List<Level>();
    public JsonData Level_Json_Data = new JsonData();
    public static Config_Level Config_level;

    private void Awake()
    {
        Config_level = this;
    }

    public void Config_Level_Json()
    {
#if UNITY_EDITOR_WIN
        Level_Json_Data = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + "/Level_Json.json", Encoding.GetEncoding("UTF-8")));
#endif

#if UNITY_ANDROID
        string path = "jar:file://" + Application.dataPath + "!/assets/Level_Json.json";
        WWW www = new WWW(path);
        while (!www.isDone) { }

        Level_Json_Data = JsonMapper.ToObject(www.text);
#endif

        Decode_Level_Json();
    }
    private void Decode_Level_Json()
    {
        for (int i = 0; i < Level_Json_Data.Count; i++)
        {
            string level_Name = Level_Json_Data[i]["Level_Name"].ToString();
            string level_Desc = Level_Json_Data[i]["Level_Desc"].ToString();
            Level level = new Level(level_Name, level_Desc);

            Level_Data_All.Add(level);
        }
    }
}
