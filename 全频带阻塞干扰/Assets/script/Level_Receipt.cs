using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level_Receipt : MonoBehaviour
{
    public Slider Load_value;
    public Text value;
    public float Simulation_value=0;
    AsyncOperation async;

    public static Level_Receipt level_Receipt;
    void Awake()
    {
        level_Receipt = this;
    }
    void Update()
    {
        UI_display();
    }

    public void level_load(string level_name)
    {
        async = SceneManager.LoadSceneAsync(level_name);
        async.allowSceneActivation = false;
        StartCoroutine(Wait_load());
    }
    public void UI_display()
    {
        Simulation_value = Mathf.Lerp(Simulation_value, async.progress, Time.deltaTime);
       

        if (Simulation_value>= 0.85)
        {
            value.text = 100 +"%";
            Load_value.value = 1;
            async.allowSceneActivation = true;
        }
    }

    IEnumerator Wait_load()
    {
        while (true)
        {
            value.text = ((int)(Simulation_value / 9 * 10 * 100)).ToString() + "%";
            Load_value.value = (Simulation_value) / 9 * 10;
            yield return new WaitForSeconds(1f);
        }
    }
}
