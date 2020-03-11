using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// 关卡选择面板的，关卡加载，关卡介绍信息显示
/// </summary>
public class Level_Detail : MonoBehaviour
{
    public static Level_Detail instance;
    public Text desc;
    public Text point;
    public Button star;
    private Level level;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
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
        Level_Radio.Instance.Load_level(level);
    }
}
