using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
/// <summary>
/// 管理所有单位，对指定单位发出命令
/// </summary>
public class Unit_Management : MonoBehaviour
{
    public static Unit_Management instance;
    public Transform revocation;
    public Button home_Button;
    public Button revocation_Button;
    public bool Order_lock { get; set; } = false;
    int move_Target;//移动目标
    private int player_Index = -1;//选中单位索引
    private Unit_Control player_Script;
    private Unit_Control enemy_Script;
    public Dictionary<int, Unit_Control> Player_List { get; private set; } = new Dictionary<int, Unit_Control>();
    public Dictionary<int, Unit_Control> Enemy_List { get; private set; } = new Dictionary<int, Unit_Control>();

    public enum Search_setting
    {
        Enemy,
        Moverange,
        Morale
    }

    void Awake()
    {
        Config_Item.Instance.Config_Item_Json();///
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        revocation.transform.gameObject.SetActive(false);
        revocation_Button.onClick.AddListener(RevocationOnClick);
        home_Button.onClick.AddListener(ReturnHomeOnClick);
        Unit_Update(null);
    }

    void Update()
    {
        Unit_Selected();
    }
    public void Unit_Update(GameObject info)
    {
        GameObject[] Player_array_object = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] Enemy_array_object = GameObject.FindGameObjectsWithTag("Enemy");
        Player_List.Clear();
        for (int i = 0; i < Player_array_object.Length; i++)
        {
            Player_List.Add(Player_array_object[i].GetComponent<Unit_Control>().unit_Position_Index, Player_array_object[i].GetComponent<Unit_Control>());
        }
        Enemy_List.Clear();
        for (int i = 0; i < Enemy_array_object.Length; i++)
        {
            Enemy_List.Add(Enemy_array_object[i].GetComponent<Unit_Control>().unit_Position_Index, Enemy_array_object[i].GetComponent<Unit_Control>());
        }
        if (info!=null&&info.tag=="Player")
        {
            Player_List.Remove(info.GetComponent<Unit_Control>().unit_Position_Index);
        }
        else if(info != null && info.tag == "Enemy")
        {
            Enemy_List.Remove(info.GetComponent<Unit_Control>().unit_Position_Index);
        }
        Initial_Morale();
    }
    public void Initial_Morale()
    {
        foreach (var playerScript in Player_List)
        {
            playerScript.Value.BFS(Search_setting.Morale);
        }
        foreach (var enemyScript in Enemy_List)
        {
            enemyScript.Value.BFS(Search_setting.Morale);
        }
    }

    public void Revocation_Allow()//允许撤销
    {
        revocation.transform.gameObject.SetActive(true);
    }
    private void ReturnHomeOnClick()
    {
        EventManagement.Instance.CleanEvent();
        SceneManager.LoadScene("Home");
    }
    private void RevocationOnClick()
    {
        revocation.transform.gameObject.SetActive(false);
        player_Script.Revocation();
    }
   
    private void Unit_Selected()
    {
#if UNITY_EDITOR_WIN
        if (Input.GetMouseButtonUp(0)&& EventSystem.current.IsPointerOverGameObject() == false&& Order_lock==false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                ObjectPool.instance.Recycle_Hex();
                ObjectPool.instance.Recycle_Enemytag();
                if (hit.transform.tag== "Player")
                {
                    player_Script = hit.transform.gameObject.GetComponent<Unit_Control>();

                    player_Script.BFS(Search_setting.Moverange);
                    player_Index = player_Script.unit_Position_Index;
                    revocation.transform.gameObject.SetActive(false);
                }
                if (hit.transform.tag == "Untagged")
                {
                    revocation.transform.gameObject.SetActive(false);
                }
                if (hit.transform.tag == "Map")
                {
                    move_Target = hit.transform.GetComponent<Hex_Info>().index;
                    player_Script.Move(player_Index, move_Target);
                }
                if (hit.transform.tag == "Enemy")
                {
                    if (player_Script != null)
                    {
                        revocation.transform.gameObject.SetActive(false);
                        enemy_Script = hit.transform.GetComponent<Unit_Control>();
                        player_Script.Attack( enemy_Script);
                    }
                    player_Script = null;
                }
            }
        }
#endif

#if UNITY_ANDROID
        if (Input.touches[0].phase == TouchPhase.Began&& EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) == false)//在开始触摸时要更新固定点(首次触摸位置)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                ObjectPool.instance.Recycle_Hex();
                ObjectPool.instance.Recycle_Enemytag();
                if (hit.transform.tag == "Player")
                {
                    player_Script = hit.transform.gameObject.GetComponent<Unit_Control>();

                    player_Script.BFS(Search_setting.Moverange);
                    player_Index = player_Script.unit_Position_Index;
                    revocation.transform.gameObject.SetActive(false);
                }
                if (hit.transform.tag == "Untagged")
                {
                    revocation.transform.gameObject.SetActive(false);
                }
                if (hit.transform.tag == "Map")
                {
                    move_Target = hit.transform.GetComponent<Hex_Info>().index;
                    player_Script.Move(player_Index, move_Target);
                }
                if (hit.transform.tag == "Enemy")
                {
                    if (player_Script != null)
                    {
                        revocation.transform.gameObject.SetActive(false);
                        enemy_Script = hit.transform.GetComponent<Unit_Control>();
                        player_Script.Attack(enemy_Script);
                    }
                    player_Script = null;
                }
            }
        }
#endif
    }
}
