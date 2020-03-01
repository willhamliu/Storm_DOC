using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;

public class Dialogue_Management : MonoBehaviour
{
    List<Dialogue> dialogues = new List<Dialogue>();
    string log_String;
    string link = null;

    bool log_Open = false;//文本查看状态
    bool text_Ponit_Start = false;//逐字显示状态
    bool dialogue_End  = false;

    int dialogues_Index=0;
    int text_Index = 0;
    int index = 0;

    int onClick_Count=0;

    public RectTransform dialogue_Image;
    public RectTransform dialogue_Image_Background;
    public RectTransform log_Image;
    public RectTransform log_Image_Background;
    public GameObject log_Panel;
    public GameObject task_Panel;

    public GameObject interface_Canvas;
    public GameObject scene_Canvas;

    public Text dialogue_Text;//对话内容
    public Text speaker_Text;
    public Text log_Text;//对话列表

    public Text main_TaskGoal_Text_SettingPanel;//首要目标(设置面板显示)
    public Text secondary_TaskGoal_Text_SettingPanel;//次要目标(设置面板显示)  

    public Text main_TaskGoal_Text_TaskPanel;//首要目标(任务面板显示)
    public Text secondary_TaskGoal_Text_TaskPanel;//次要目标(任务面板显示)



    public Button log_Button;//对话列表
    public Button close_TaskPanel_Button;//关闭任务面板

    Tween log_Animation;
    Tween dialogue_Animation;

    public bool on;

    void Awake()
    {
        scene_Canvas.SetActive(true);

        log_Panel.SetActive(false);
        task_Panel.SetActive(false);
        log_Image.gameObject.SetActive(false);
        dialogue_Image.gameObject.SetActive(true);
        if (on==true)
        {
            StartCoroutine(Scene_Initialization());

            End_Dialogue();
            return;
        }

        Config_Dialogue.Config_dialogue.Config_Dialogue_Json();
        this.dialogues = Config_Dialogue.Config_dialogue.dialogues;
    }
    void Start()
    {
        StartCoroutine(Scene_Initialization());

        dialogue_Animation = dialogue_Image.DOSizeDelta(new Vector2(dialogue_Image.sizeDelta.x, 350), 0.3f);
        dialogue_Animation = dialogue_Image_Background.DOSizeDelta(new Vector2(dialogue_Image_Background.sizeDelta.x, 330), 0.3f);

        main_TaskGoal_Text_SettingPanel.text =main_TaskGoal_Text_TaskPanel.text =Config_Dialogue.Config_dialogue.main_TaskGoal.ToString();
        secondary_TaskGoal_Text_SettingPanel.text = secondary_TaskGoal_Text_TaskPanel.text =Config_Dialogue.Config_dialogue.secondary_TaskGoal.ToString();


        log_Button.onClick.AddListener(Log_StateOnClick);
        close_TaskPanel_Button.onClick.AddListener(Close_TaskPanelOnClick);

        Invoke("Log_Start", 0.5f);//为了满足渐变效果延迟调用
    }

    void Update()
    {
#if UNITY_EDITOR_WIN
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            if (dialogue_End==true|| text_Index == 0)//必须显示第一个字之后才可提前显示
            {
                return;
            }
            if (log_Open == false && dialogues_Index+ 1 < dialogues.Count && text_Ponit_Start == false)//逐字显示
            {
                dialogues_Index++;
                speaker_Text.text = dialogues[dialogues_Index].speaker;
                StartCoroutine(Show_Text(dialogues[dialogues_Index].dialogue_Desc.Length));
            }
            else if (log_Open == false && text_Ponit_Start == true)//提前显示
            {
                text_Ponit_Start = false;
                string desc = null;
                for (int i = 0; i < dialogues[dialogues_Index].dialogue_Desc.Length; i++)
                {
                    if ((dialogues[dialogues_Index].dialogue_Desc)[i].ToString() == "/")
                    {
                        desc = desc + "\r\n";
                    }
                    else
                    {
                        desc = desc + (dialogues[dialogues_Index].dialogue_Desc)[i].ToString();
                    }
                }
                if (dialogues_Index==0)
                {
                    dialogue_Text.text = desc;
                }
                else
                {
                    dialogue_Text.text = "\u3000\u3000" + desc;
                }
            }
            else if (log_Open == false && dialogues_Index+1 == dialogues.Count)//播放完成
            {
                End_Dialogue();
            }
        }

#endif

#if UNITY_ANDROID
        if (Input.touches[0].phase== TouchPhase.Began && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)==false)
        {
            if (dialogue_End==true|| text_Index == 0)
            {
                return;
            }
            if (log_Open == false && dialogues_Index + 1 < dialogues.Count && text_Ponit_Start == false)
            {
                dialogues_Index++;
                speaker_Text.text = dialogues[dialogues_Index].speaker;
                StartCoroutine(Show_Text(dialogues[dialogues_Index].dialogue_Desc.Length));
            }
            else if (text_Ponit_Start == true)
            {
                text_Ponit_Start = false;
                string desc = null;
                for (int i = 0; i < dialogues[dialogues_Index].dialogue_Desc.Length; i++)
                {
                    if ((dialogues[dialogues_Index].dialogue_Desc)[i].ToString() == "/")
                    {
                        desc = desc + "\r\n";
                    }
                    else
                    {
                        desc = desc + (dialogues[dialogues_Index].dialogue_Desc)[i].ToString();
                    }
                }
                if (dialogues_Index==0)
                {
                    dialogue_Text.text = desc;
                }
                else
                {
                    dialogue_Text.text = "\u3000\u3000" + desc;
                }
            }
            else if (log_Open == false && dialogues_Index + 1 == dialogues.Count)
            {
                End_Dialogue();
            }
        }
#endif
    }
    private void Close_TaskPanelOnClick()
    {
        task_Panel.SetActive(false);
        interface_Canvas.SetActive(false);
    }
    private void Open_TaskPanel()
    {
        task_Panel.SetActive(true);
        interface_Canvas.SetActive(true);
    }


    public void Log_StateOnClick()
    {
        if ((log_Animation != null&& log_Animation.IsPlaying() == true) || text_Ponit_Start == true)
        {
            return;
        }
        Audio_Management.audio_Management.SFXS_play("阵营切换");

        Log_Add();
        onClick_Count++;
        if (onClick_Count % 2 == 0)
        {
            if(dialogue_End == false)
            {
                log_Panel.SetActive(false);
                log_Animation = log_Image.DOSizeDelta(new Vector2(log_Image.sizeDelta.x, 350), 0.3f);
                log_Animation = log_Image_Background.DOSizeDelta(new Vector2(log_Image_Background.sizeDelta.x, 330), 0.3f);
                Invoke("Log_Close", 0.4f);
            }
            else
            {
                log_Panel.SetActive(false);
                log_Animation = log_Image.DOSizeDelta(new Vector2(log_Image.sizeDelta.x, 0), 0.4f);
                log_Animation = log_Image_Background.DOSizeDelta(new Vector2(log_Image_Background.sizeDelta.x, 0), 0.3f);

                Invoke("Log_Close", 0.5f);
            }
        }
        if (onClick_Count % 2 != 0)
        {
            if (dialogue_End == false)
            {
                log_Open = true;
                speaker_Text.gameObject.SetActive(false);
                dialogue_Image.gameObject.SetActive(false);
                log_Image.gameObject.SetActive(true);

                log_Animation = log_Image.DOSizeDelta(new Vector2(log_Image.sizeDelta.x, 780), 0.3f);
                log_Animation = log_Image_Background.DOSizeDelta(new Vector2(log_Image_Background.sizeDelta.x, 760), 0.3f);


                Invoke("Log_Open", 0.4f);
            }
            else
            {
                log_Image.gameObject.SetActive(true);
                log_Animation = log_Image.DOSizeDelta(new Vector2(log_Image.sizeDelta.x, 780), 0.4f);
                log_Animation = log_Image_Background.DOSizeDelta(new Vector2(log_Image_Background.sizeDelta.x, 760), 0.44f);


                Invoke("Log_Open", 0.5f);
            }
        }
    }

    private void Log_Close()//关闭目录
    {
        if (dialogue_End==false&& log_Animation.IsPlaying() == false)
        {
            log_Image.gameObject.SetActive(false);
            dialogue_Image.gameObject.SetActive(true);
            speaker_Text.gameObject.SetActive(true);
            log_Open = false;
        }
        else if(dialogue_End == true&& log_Animation.IsPlaying() == false)
        {
            log_Image.gameObject.SetActive(false);
            log_Open = false;
        }
    }

    private void Log_Open()//打开目录
    {
        if (log_Animation.IsPlaying() == false)
        {
            log_Panel.SetActive(true);
        }
    }
   

    public void Log_Start()
    {
        speaker_Text.text = dialogues[0].speaker;

        StartCoroutine(Show_Text(dialogues[0].dialogue_Desc.Length));
    }

    public void Log_Add()
    {
        for (int i = index; i <= dialogues_Index ; i++)//防止每次打开调用时都会遍历造成字符串重复，因此作用域 (i) 不能是0
        {
            for (int j = index; j <= index; j++)//每次遍历1回，遍历完后则增加索引进行第二次添加
            {
                if (index==0)
                {
                    link = dialogues[index].speaker  + dialogues[index].dialogue_Desc;
                }
                else
                {
                    link = dialogues[index].speaker + ":\u3000" + dialogues[index].dialogue_Desc;
                }
            }
            index++;
            log_String = log_String + link+"\n\n";
        }
        log_Text.text = log_String;
    }

    private void End_Dialogue()//结束对话
    {
        dialogues_Index = dialogues.Count-1;
        speaker_Text.gameObject.SetActive(false);
        dialogue_Animation = dialogue_Image.DOSizeDelta(new Vector2(dialogue_Image.sizeDelta.x, 0), 0.3f);
        dialogue_Animation = dialogue_Image_Background.DOSizeDelta(new Vector2(dialogue_Image_Background.sizeDelta.x, 0), 0.26f);

        log_Image.sizeDelta = new Vector2(log_Image.sizeDelta.x, 0);
        log_Image_Background.sizeDelta = new Vector2(log_Image_Background.sizeDelta.x, 0);

        dialogue_End = true;
        Invoke("Open_TaskPanel",0.4f);//打开任务目标面板
    }

    IEnumerator Show_Text(int Dialogue_Length)
    {
        text_Index = 0;
        string desc = null;
        text_Ponit_Start = true;

        while (text_Index < Dialogue_Length)
        {
            if (text_Ponit_Start == true)
            {
                if ((dialogues[dialogues_Index].dialogue_Desc)[text_Index].ToString()=="/")
                {
                    desc = desc + "\r\n";                    
                }
                else
                {
                    desc = desc + (dialogues[dialogues_Index].dialogue_Desc)[text_Index].ToString();
                }
                if (dialogues_Index == 0)
                {
                    dialogue_Text.text = desc;
                }
                else
                {
                    dialogue_Text.text = "\u3000\u3000" + desc;
                }
                text_Index++;
            }
            else//提前显示时终止协程
            {
                text_Index = Dialogue_Length;
            }
            yield return new WaitForSeconds(0.05f);
        }
        text_Ponit_Start = false;
    }

    IEnumerator Scene_Initialization()
    {
        float a = 255;
        while (a >= 0)
        {
            a = a - 17;
            scene_Canvas.GetComponent<Image>().color = new Color(0, 0, 0, a / 255);
            yield return null;
        }
        scene_Canvas.SetActive(false);
    }
}
