using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level_Receipt : MonoBehaviour
{
    public Slider load_Value;
    public Text value;
    public Text prompt_Text;
    public float simulation_Value=0;
    AsyncOperation async;

    public static Level_Receipt Level_receipt;
    void Awake()
    {
        Level_receipt = this;
        Config_Prompt.Config_prompt.Config_Prompt_Json();
    }
    void Start()
    {
        Level_Get.Level_get.Load();

        int i = Random.Range(0, Config_Prompt.Config_prompt.prompts.Count);
        prompt_Text.text = Config_Prompt.Config_prompt.prompts[i];
    }
    void Update()
    {
        UI_display();
    }

    public void Level_load(string level_name)
    {
        async = SceneManager.LoadSceneAsync(level_name);
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
