﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;
/// <summary>
/// 配置每个关卡的对话信息与任务信息
/// </summary>
public class Config_Dialogue: BaseConfigJson<Config_Dialogue>
{
    public List<Dialogue> dialogues = new List<Dialogue>();

    public string main_TaskGoal;
    public string secondary_TaskGoal;

    JsonData dialogue_JsonData;

    public void Config_Dialogue_Json()
    {
        string file_Name = Level_Radio.Instance.Dialogue_name;

        dialogue_JsonData = ReadJson(dialogue_JsonData, "/Dialogue/" + file_Name + ".json");

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
