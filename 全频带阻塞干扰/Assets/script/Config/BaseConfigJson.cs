using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public class BaseConfigJson<T>: InstanceNull<T>  where T : new()
{
    protected const string saveFile = "/DynamicData";

    public JsonData ReadJson(JsonData jsonData, string path, bool isDynamicData = false)
    {
        if (jsonData != null&& !isDynamicData)
        {
            return jsonData;
        }
        if (isDynamicData)
        {
            FileInfo file;
#if UNITY_STANDALONE
            file = new FileInfo(Application.streamingAssetsPath + saveFile+ path);
#elif UNITY_ANDROID
            file = new FileInfo(Application.persistentDataPath + savePath+ path);
#endif
            if (file.Exists)
            {
#if UNITY_STANDALONE
                jsonData = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + saveFile+ path, Encoding.GetEncoding("UTF-8")));
#elif UNITY_ANDROID
                jsonData = JsonMapper.ToObject(File.ReadAllText(Application.persistentDataPath + savePath+ path, Encoding.GetEncoding("UTF-8")));
#endif
            }
            else
            {
                ReadJsonData();
                WriteJson(jsonData, saveFile + path);
            }
        }
        else
        {
            ReadJsonData();
        }
        return jsonData;

        void ReadJsonData()
        {
#if UNITY_STANDALONE
            jsonData = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + path, Encoding.GetEncoding("UTF-8")));
#elif UNITY_ANDROID
            WWW www = new WWW(Application.streamingAssetsPath + path);
            while (!www.isDone) { }
            jsonData = JsonMapper.ToObject(www.text);
#endif
        }
    }

    public void WriteJson(JsonData jsonData, string path)
    {
        FileInfo file;
#if UNITY_STANDALONE
        file = new FileInfo(Application.streamingAssetsPath + path);
#elif UNITY_ANDROID
        file = new FileInfo(Application.persistentDataPath + path);
#endif
        string json = Regex.Unescape(JsonMapper.ToJson(jsonData));
        StreamWriter write = file.CreateText();

        write.Write(json);
        write.Close();
        write.Dispose();
    }
}
