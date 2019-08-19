using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Management : MonoBehaviour
{
    public static Level_Management level_management;
    private static string level_name;
    void Awake()
    {
        level_management = this;
    }
    void Start()
    {
        Load();
    }

    public void Load_level(Level level)
    {
        level_name = level.Load_Scene;
    }
    public void Load()
    {
        if (SceneManager.GetActiveScene().name == "Load_level")
        {
            Level_Receipt.level_Receipt.level_load(level_name);
        }
    }
}
