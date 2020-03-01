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
    public static Level_Detail level_Detail;
    void Awake()
    {
        level_Detail = this;
    }
    void Start()
    {
        star.onClick.AddListener(Send_levelOnClick);
    }
    public void SetData(Level level)
    {
        if (level == null)
        {
            return;
        }
        this.level = level;
        desc.text ="\u3000\u3000"+this.level.level_Desc;
        point.text = this.level.level_Point.ToString();
    }
    public void Send_levelOnClick()
    {
        SceneManager.LoadScene("Load_level");
        Level_Radio.Level_radio.Load_level(level);
    }
}
