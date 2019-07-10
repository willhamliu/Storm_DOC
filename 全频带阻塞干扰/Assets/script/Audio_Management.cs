using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Management : MonoBehaviour
{
    public AudioClip[] BGM;//背景音
    public AudioClip[] SFXS;//音效

    public static Audio_Management Audio_management;
    void Awake()
    {
        Audio_management = this;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
