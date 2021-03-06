﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;
/// <summary>
/// 管理对话与任务目标的UI显示
/// </summary>
public class Dialogue_Management : MonoBehaviour
{
    List<Dialogue> dialogues = new List<Dialogue>();
    string log_String;
    string link = null;

    bool islog_AnimPlayEnd = true;
    bool islog_Open = false;//文本查看状态
    bool istext_Ponit_Start = false;//逐字显示状态
    bool isdialogue_End  = false;

    int dialogues_Index=0;
    int text_Index = 0;
    int index = 0;

    int onClick_Count=0;

    public RectTransform dialogue_Image;
    public RectTransform log_Image;
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

    void Awake()
    {
        scene_Canvas.SetActive(true);

        log_Panel.SetActive(false);
        task_Panel.SetActive(false);
        log_Image.gameObject.SetActive(false);
        dialogue_Image.gameObject.SetActive(true);

        Config_Dialogue.Instance.Config_Dialogue_Json();
        this.dialogues = Config_Dialogue.Instance.dialogues;
    }
    void Start()
    {
        scene_Canvas.GetComponent<Image>().DOColor(new Color(0, 0, 0, 0), 0.6f).OnComplete(()=> 
        {
            scene_Canvas.SetActive(false);
            if (!Level_Radio.Instance.IsLevel_again) Log_Start();
        });
        if (Level_Radio.Instance.IsLevel_again == false)
        {
            dialogue_Image.DOSizeDelta(new Vector2(dialogue_Image.sizeDelta.x, 350), 0.3f);
        }
        else
        {
            isdialogue_End = true;
            dialogue_Image.gameObject.SetActive(false);
        }
        main_TaskGoal_Text_SettingPanel.text =main_TaskGoal_Text_TaskPanel.text =Config_Dialogue.Instance.secondary_TaskGoal.ToString();
        secondary_TaskGoal_Text_SettingPanel.text = secondary_TaskGoal_Text_TaskPanel.text =Config_Dialogue.Instance.secondary_TaskGoal.ToString();

        UI_Management.instance.AddButtonEventTrigger<Button>("Log_Button", Log_StateOnClick, "阵营切换", Audio_Management.instance.SFXS_play);
        UI_Management.instance.AddButtonEventTrigger<Button>("close_TaskGoal_Panel", Close_TaskPanelOnClick);
    }

    void Update()
    {
#if UNITY_STANDALONE
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)))
        {
            if (isdialogue_End==true|| text_Index == 0)//必须显示第一个字之后才可提前显示
            {
                return;
            }
            if (islog_Open == false && dialogues_Index+ 1 < dialogues.Count && istext_Ponit_Start == false)//逐字显示
            {
                dialogues_Index++;
                speaker_Text.text = dialogues[dialogues_Index].speaker;
                StartCoroutine(Show_Text(dialogues[dialogues_Index].dialogue_Desc.Length));
            }
            else if (islog_Open == false && istext_Ponit_Start == true)//提前显示
            {
                istext_Ponit_Start = false;
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
            else if (islog_Open == false && dialogues_Index+1 == dialogues.Count)//播放完成
            {
                End_Dialogue();
            }
        }

#endif

#if UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Began && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) == false)
            {
                if (isdialogue_End == true || text_Index == 0)
                {
                    return;
                }
                if (islog_Open == false && dialogues_Index + 1 < dialogues.Count && istext_Ponit_Start == false)
                {
                    dialogues_Index++;
                    speaker_Text.text = dialogues[dialogues_Index].speaker;
                    StartCoroutine(Show_Text(dialogues[dialogues_Index].dialogue_Desc.Length));
                }
                else if (istext_Ponit_Start == true)
                {
                    istext_Ponit_Start = false;
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
                    if (dialogues_Index == 0)
                    {
                        dialogue_Text.text = desc;
                    }
                    else
                    {
                        dialogue_Text.text = "\u3000\u3000" + desc;
                    }
                }
                else if (islog_Open == false && dialogues_Index + 1 == dialogues.Count)
                {
                    End_Dialogue();
                }
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

    public void Log_Start()
    {
        speaker_Text.text = dialogues[0].speaker;

        StartCoroutine(Show_Text(dialogues[0].dialogue_Desc.Length));
    }

    public void Log_StateOnClick()
    {
        if (!islog_AnimPlayEnd || istext_Ponit_Start == true)
        {
            return;
        }

        islog_AnimPlayEnd = false;
        onClick_Count++;
        if (onClick_Count % 2 == 0)
        {
            if(isdialogue_End == false)
            {
                log_Panel.SetActive(false);
                log_Image.DOSizeDelta(new Vector2(log_Image.sizeDelta.x, 350), 0.3f).OnComplete(() =>
                {
                    islog_AnimPlayEnd = true;
                    Log_Close();
                });
            }
            else
            {
                log_Panel.SetActive(false);
                log_Image.DOSizeDelta(new Vector2(log_Image.sizeDelta.x, 0), 0.4f).OnComplete(()=> 
                {
                    islog_AnimPlayEnd = true;
                    Log_Close();
                });
            }
        }
        if (onClick_Count % 2 != 0)
        {

            islog_Open = true;
            speaker_Text.gameObject.SetActive(false);
            dialogue_Image.gameObject.SetActive(false);
            log_Image.gameObject.SetActive(true);

            log_Image.DOSizeDelta(new Vector2(log_Image.sizeDelta.x, 780), 0.3f).OnComplete(() =>
            {
                islog_AnimPlayEnd = true;
                Log_Open();
            });
        }
    }

    private void Log_Close()//关闭目录
    {
        if (isdialogue_End==false)
        {
            log_Image.gameObject.SetActive(false);
            dialogue_Image.gameObject.SetActive(true);
            speaker_Text.gameObject.SetActive(true);
        }
        else if(isdialogue_End == true)
        {
            log_Image.gameObject.SetActive(false);
        }
        islog_Open = false;
    }

    public void Log_Open()//打开目录
    {
        log_Panel.SetActive(true);
        int updateCount;
        if (Level_Radio.Instance.IsLevel_again == false)
        {
            updateCount = dialogues_Index+1;
        }
        else
        {
            updateCount = dialogues.Count;//重新开始时直接更新所有对话
        }
        for (int i = index; i < updateCount; i++)//防止每次打开对话列表时都会遍历造成字符串重复，因此作用域 (i) 为上次显示的对话下标
        {
            if (index == 0)
            {
                link = dialogues[index].speaker + dialogues[index].dialogue_Desc;
            }
            else
            {
                link = dialogues[index].speaker + ":\u3000" + dialogues[index].dialogue_Desc;
            }
            index++;
            log_String = log_String + link+"\n\n";
        }
        log_Text.text = log_String;
    }

    private void End_Dialogue()//结束对话
    {
        speaker_Text.gameObject.SetActive(false);
        log_Image.sizeDelta = new Vector2(log_Image.sizeDelta.x, 0);
        dialogue_Image.DOSizeDelta(new Vector2(dialogue_Image.sizeDelta.x, 0), 0.3f).OnComplete(()=> 
        {
            Open_TaskPanel();
            isdialogue_End = true;
        });
    }

    IEnumerator Show_Text(int Dialogue_Length)
    {
        text_Index = 0;
        string desc = null;
        istext_Ponit_Start = true;

        while (text_Index < Dialogue_Length)
        {
            if (istext_Ponit_Start == true)
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
        istext_Ponit_Start = false;
    }
}
