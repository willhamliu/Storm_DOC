using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Management : MonoBehaviour
{
    void Awake()
    {
        Config_Dialogue.Config_dialogue.Config_Dialogue_Json();
        for (int i = 0; i < Config_Dialogue.Config_dialogue.dialogues.Count; i++)
        {
            Debug.Log(Config_Dialogue.Config_dialogue.dialogues[i]);
        }
    }
}
