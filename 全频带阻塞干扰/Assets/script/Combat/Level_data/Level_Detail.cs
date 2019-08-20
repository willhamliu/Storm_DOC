using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level_Detail : MonoBehaviour
{
    public Text desc;
    public Text point;
    public Button star;
    private Level level;
    public static Level_Detail Level_detail;
    void Awake()
    {
        Level_detail = this;
    }
    void Start()
    {
        star.onClick.AddListener(Load_level);
    }
    public void SetData(Level level)
    {
        if (level == null)
        {
            return;
        }
        this.level = level;
        desc.text ="\u3000\u3000"+this.level.Level_Desc;
        point.text = this.level.Level_Point.ToString();
    }
    public void Load_level()
    {
        SceneManager.LoadScene("Load_level");
        Level_Get.Level_get.Load_level(level);
    }
}
