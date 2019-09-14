using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{
    public string Dialogue_Desc;
    public string Speaker;
    public Dialogue(string dialogue_desc, string speaker)
    {
        this.Dialogue_Desc = dialogue_desc;
        this.Speaker = speaker;
    }
}
