using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using LitJson;
using UnityEngine.UI;
/// <summary>
/// 配置关卡信息
/// </summary>
public class Config_Level:InstanceNull<Config_Level>
{
    public List<Level> level_DataAll = new List<Level>();
    JsonData level_JsonData;

    private static Config_Level config_Level;
   
    public void Config_Level_Json()
    {
#if UNITY_EDITOR_WIN
        level_JsonData = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + "/Level_Json.json", Encoding.GetEncoding("UTF-8")));
#endif

#if UNITY_ANDROID
        string path = "jar:file://" + Application.dataPath + "!/assets/Level_Json.json";
        WWW www = new WWW(path);
        while (!www.isDone) { }

        level_JsonData = JsonMapper.ToObject(www.text);
#endif

        Decode_Level_Json();
    }
    private void Decode_Level_Json()
    {
        level_DataAll.Clear();
        for (int i = 0; i < level_JsonData.Count; i++)
        {
            string level_Name = level_JsonData[i]["Level_Name"].ToString();
            string level_Desc = level_JsonData[i]["Level_Desc"].ToString();
            string load_Scene = level_JsonData[i]["Load_Scene"].ToString();
            string load_Scene_Dialogue = level_JsonData[i]["Load_Scene_Dialogue"].ToString();
            int level_Point =(int) level_JsonData[i]["Level_Point"];
            Level level = new Level(level_Name, level_Desc, load_Scene, load_Scene_Dialogue, level_Point);

            level_DataAll.Add(level);
        }
    }
}
