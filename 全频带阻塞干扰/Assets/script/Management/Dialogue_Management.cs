using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class Dialogue_Management : MonoBehaviour
{
    List<Dialogue> dialogues = new List<Dialogue>();
    string log_String;
    string Link = null;

    bool log_Open = false;//文本查看状态
    bool text_Ponit_Start = false;//逐字显示状态
    public bool dialogue_end { get; private set; } = false;

    int dialogues_Index=0;
    int index = 0;

    int onClick_Count=0;

    public RectTransform dialogue_Image;
    public RectTransform dialogue_Image_Background;
    public RectTransform log_Image;
    public RectTransform log_Image_Background;
    public GameObject log_Panel;

    public GameObject scene_Canvas;

    public Text dialogue_Text;
    public Text speaker_Text;
    public Text log_Text;

    public Button log_Button;//对话列表

    Tween log_Animation;
    Tween dialogue_Animation;

    void Awake()
    {
        scene_Canvas.SetActive(true);

        log_Panel.SetActive(false);
        log_Image.gameObject.SetActive(false);
        dialogue_Image.gameObject.SetActive(true);

        Config_Dialogue.Config_dialogue.Config_Dialogue_Json();
        this.dialogues = Config_Dialogue.Config_dialogue.dialogues;
    }
    void Start()
    {
        StartCoroutine(Scene_Initialization());

        dialogue_Animation = dialogue_Image.DOSizeDelta(new Vector2(dialogue_Image.sizeDelta.x, 350), 0.3f);
        dialogue_Animation = dialogue_Image_Background.DOSizeDelta(new Vector2(dialogue_Image_Background.sizeDelta.x, 330), 0.3f);

        log_Button.onClick.AddListener(Log_State);

        Invoke("Log_Start", 0.5f);//为了满足渐变效果延迟调用
    }

    void Update()
    {
#if UNITY_EDITOR_WIN
        
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            if (dialogue_end==true)
            {
                return;
            }
            if (log_Open == false && dialogues_Index+ 1 < dialogues.Count && text_Ponit_Start == false)//逐字显示
            {
                dialogues_Index++;
                speaker_Text.text = dialogues[dialogues_Index].Speaker;
                StartCoroutine(Show_Text(dialogues[dialogues_Index].Dialogue_Desc.Length));
            }
            else if (log_Open == false && text_Ponit_Start == true)//提前显示
            {
                text_Ponit_Start = false;
                dialogue_Text.text = (dialogues[dialogues_Index].Dialogue_Desc).ToString();
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
            if (log_Open == false && dialogues_Index + 1 < dialogues.Count && text_Ponit_Start == false)
            {
                dialogues_Index++;
                speaker_Text.text = dialogues[dialogues_Index].Speaker;
                StartCoroutine(Show_Text(dialogues[dialogues_Index].Dialogue_Desc.Length));
            }
            else if (text_Ponit_Start == true)
            {
                text_Ponit_Start = false;
                dialogue_Text.text = (dialogues[dialogues_Index].Dialogue_Desc).ToString();
            }
            else if (log_Open == false && dialogues_Index+1 == dialogues.Count)
            {
                End_Dialogue();
            }
        }
#endif
    }

    public void Log_State()
    {
        if ((log_Animation != null&& log_Animation.IsPlaying() == true) || text_Ponit_Start == true)
        {
            return;
        }
        Audio_Management.Audio_management.SFXS_play("阵营切换");

        Log_Add();
        onClick_Count++;
        if (onClick_Count % 2 == 0)
        {
            if(dialogue_end == false)
            {
                log_Panel.SetActive(false);
                log_Animation = log_Image.DOSizeDelta(new Vector2(log_Image.sizeDelta.x, 350), 0.3f);
                log_Animation = log_Image_Background.DOSizeDelta(new Vector2(log_Image_Background.sizeDelta.x, 330), 0.3f);
                Invoke("Log_Close", 0.4f);
            }
            else
            {
                log_Panel.SetActive(false);
                log_Animation = log_Image.DOSizeDelta(new Vector2(log_Image.sizeDelta.x, 20), 0.4f);
                log_Animation = log_Image_Background.DOSizeDelta(new Vector2(log_Image_Background.sizeDelta.x, 0), 0.3f);

                Invoke("Log_Close", 0.5f);
            }
        }
        if (onClick_Count % 2 != 0)
        {
            if (dialogue_end == false)
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
                log_Animation = log_Image_Background.DOSizeDelta(new Vector2(log_Image_Background.sizeDelta.x, 760), 0.4f);


                Invoke("Log_Open", 0.5f);
            }
        }
    }

    private void Log_Close()//关闭目录
    {
        if (dialogue_end==false&& log_Animation.IsPlaying() == false)
        {
            log_Image.gameObject.SetActive(false);
            dialogue_Image.gameObject.SetActive(true);
            speaker_Text.gameObject.SetActive(true);
            log_Open = false;
        }
        else if(dialogue_end == true&& log_Animation.IsPlaying() == false)
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
        speaker_Text.text = dialogues[0].Speaker;

        StartCoroutine(Show_Text(dialogues[0].Dialogue_Desc.Length));
    }

    public void Log_Add()
    {
        for (int i = index; i <= dialogues_Index ; i++)//防止每次打开调用时都会遍历造成字符串重复，因此作用域 (i) 不能是0
        {
            for (int j = index; j <= index; j++)//每次遍历1回，遍历完后则增加索引进行第二次添加
            {
                Link = dialogues[index].Speaker+"       "+ dialogues[index].Dialogue_Desc;
            }
            index++;
            log_String = log_String + Link+"\n";
        }
        log_Text.text = log_String;
    }

    private void End_Dialogue()//结束对话
    {
        dialogues_Index = dialogues.Count-1;
        speaker_Text.gameObject.SetActive(false);
        dialogue_Animation = dialogue_Image.DOSizeDelta(new Vector2(dialogue_Image.sizeDelta.x, 0), 0.3f);
        dialogue_Animation = dialogue_Image_Background.DOSizeDelta(new Vector2(dialogue_Image_Background.sizeDelta.x, 0), 0.3f);

        log_Image.sizeDelta = new Vector2(log_Image.sizeDelta.x, 20);
        log_Image_Background.sizeDelta = new Vector2(log_Image_Background.sizeDelta.x, 0);

        dialogue_end = true;
    }

    IEnumerator Show_Text(int Dialogue_Length)
    {
        int i = 0;
        string desc = null;
        text_Ponit_Start = true;

        while (i < Dialogue_Length)
        {
            if (text_Ponit_Start == true)
            {
                if ((dialogues[dialogues_Index].Dialogue_Desc)[i].ToString()=="/")
                {
                    desc = desc + "\r\n";
                    
                }
                else
                {
                    desc = desc + (dialogues[dialogues_Index].Dialogue_Desc)[i].ToString();
                }
                dialogue_Text.text = desc;
                i++;
            }
            else//提前显示时终止协程
            {
                i = Dialogue_Length;
            }
            yield return new WaitForSeconds(0.1f);
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
