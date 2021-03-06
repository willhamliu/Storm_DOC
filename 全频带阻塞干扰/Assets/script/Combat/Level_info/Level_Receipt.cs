﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// 异步加载指定关卡
/// </summary>
public class Level_Receipt : MonoBehaviour
{
    public Slider load_Value;
    public Text value;
    public Text prompt_Text;
    public float simulation_Value=0;
    AsyncOperation async;

    void Awake()
    {
        Config_Prompt.Instance.Config_Prompt_Json();
    }
    void Start()
    {
        Level_load();
        int i = Random.Range(0, Config_Prompt.Instance.prompts.Count);
        prompt_Text.text = Config_Prompt.Instance.prompts[i];
    }
    void Update()
    {
        UI_display();
    }
    
    public void Level_load()
    {
        async = SceneManager.LoadSceneAsync(Level_Radio.Instance.Level_name);
        async.allowSceneActivation = false;
        StartCoroutine(Wait_load());
    }
    public void UI_display()
    {
        simulation_Value = Mathf.Lerp(simulation_Value, async.progress, Time.deltaTime);
       

        if (simulation_Value>= 0.85)
        {
            value.text = 100 +"%";
            load_Value.value = 1;
            async.allowSceneActivation = true;
        }
    }

    IEnumerator Wait_load()
    {
        while (true)
        {
            value.text = ((int)(simulation_Value / 9 * 10 * 100)).ToString() + "%";
            load_Value.value = (simulation_Value) / 9 * 10;
            yield return new WaitForSeconds(1f);
        }
    }
}
