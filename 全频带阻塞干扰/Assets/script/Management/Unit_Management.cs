using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Unit_Management : MonoBehaviour
{
    public static Unit_Management unit_Management;
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
        Config_Item.Config_item.Config_Item_Json();///
        if (unit_Management==false)
        {
            unit_Management = this;
        }
    }

    private void Start()
    {
        revocation_Button.transform.gameObject.SetActive(false);
        revocation_Button.onClick.AddListener(RevocationOnClick);
        Unit_Update();
    }
  
    void Update()
    {
        Unit_Selected();
    }
    public void Unit_Update()
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
        revocation_Button.transform.gameObject.SetActive(true);
    }

    private void RevocationOnClick()
    {
        revocation_Button.transform.gameObject.SetActive(false);
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
                Map_Pool.map_Pool.Recycle_Hex();
                Map_Pool.map_Pool.Recycle_Enemytag();
                if (hit.transform.tag== "Player")
                {
                    player_Script = hit.transform.gameObject.GetComponent<Unit_Control>();

                    player_Script.BFS(Search_setting.Moverange);
                    player_Index = player_Script.unit_Position_Index;
                    revocation_Button.transform.gameObject.SetActive(false);
                }
                if (hit.transform.tag == "Untagged")
                {
                    revocation_Button.transform.gameObject.SetActive(false);
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
                        revocation_Button.transform.gameObject.SetActive(false);
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
                Map_Pool.map_Pool.Recycle_Hex();
                Map_Pool.map_Pool.Recycle_Enemytag();
                if (hit.transform.tag == "Player")
                {
                    player_Script = hit.transform.gameObject.GetComponent<Unit_Control>();

                    player_Script.BFS(Search_setting.Moverange);
                    player_Index = player_Script.unit_Position_Index;
                    revocation_Button.transform.gameObject.SetActive(false);
                }
                if (hit.transform.tag == "Untagged")
                {
                    revocation_Button.transform.gameObject.SetActive(false);
                }
                if (hit.transform.tag == "Map")
                {
                    move_Target = hit.transform.GetComponent<Hex_Info>().index;
                    player_Script.Move(player_Index, move_Target);
                    Unit_Update();
                }
                if (hit.transform.tag == "Enemy")
                {
                    if (player_Script != null)
                    {
                        revocation_Button.transform.gameObject.SetActive(false);
                        enemy_Script = hit.transform.GetComponent<Unit_Control>();
                        player_Script.Attack(enemy_Script);
                    }
                    Unit_Update();
                    player_Script = null;
                }
            }
        }
#endif
    }
}
