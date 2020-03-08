using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 存储对话信息
/// </summary>
public class Dialogue
{
    public string dialogue_Desc;
    public string speaker;
    public Dialogue(string dialogue_desc, string speaker)
    {
        this.dialogue_Desc = dialogue_desc;
        this.speaker = speaker;
    }
}
