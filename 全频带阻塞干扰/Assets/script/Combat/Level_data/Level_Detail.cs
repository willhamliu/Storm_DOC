using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level_Detail : MonoBehaviour
{
    public Text Desc;
    public Text point;
    public Button Star;
    private Level level;
    public static Level_Detail Level_detail;
    void Awake()
    {
        Level_detail = this;
    }
    void Start()
    {
        Star.onClick.AddListener(Load_level);
    }
    public void SetData(Level level)
    {
        if (level == null)
        {
            return;
        }
        this.level = level;
        Desc.text ="\u3000\u3000"+this.level.Level_Desc;
        point.text = this.level.Level_Point.ToString();
    }
    public void Load_level()
    {
        SceneManager.LoadScene("Load_level");
        Level_Management.level_management.Load_level(level);
    }
}
