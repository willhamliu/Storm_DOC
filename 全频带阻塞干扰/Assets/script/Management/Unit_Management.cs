using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Unit_Management : MonoBehaviour
{
    public static Unit_Management Unit_management;
    public Button revocation_Button;
    public bool order_lock { get; set; } = false;
    int move_target;//移动目标
    int attack_target;//攻击目标
    private int player_index = -1;//选中单位索引
    private Unit_Control player_script;
    private Unit_Control enemy_script;
    public List<int> Player_list { get; private set; } = new List<int>();
    public List<int> Enemy_list { get; private set; } = new List<int>();
    public enum Search_setting
    {
        Enemy,
        Moverange,
    }




    void Awake()
    {
        Config_Item.Config_item.Config_Item_Json();//
        if (Unit_management==false)
        {
            Unit_management = this;
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

        Player_list.Clear();
        for (int i = 0; i < Player_array_object.Length; i++)
        {
            Player_list.Add(Player_array_object[i].transform.GetComponent<Unit_Control>().unit_position_index);
        }
        Enemy_list.Clear();
        for (int i = 0; i < Enemy_array_object.Length; i++)
        {
            Enemy_list.Add(Enemy_array_object[i].transform.GetComponent<Unit_Control>().unit_position_index);
        }
    }

    public void Revocation_Allow()//允许撤销
    {
        revocation_Button.transform.gameObject.SetActive(true);
    }

    private void RevocationOnClick()
    {
        revocation_Button.transform.gameObject.SetActive(false);
        player_script.Revocation();
    }
   
    private void Unit_Selected()
    {
#if UNITY_EDITOR_WIN
        if (Input.GetMouseButtonUp(0)&& EventSystem.current.IsPointerOverGameObject() == false&& order_lock==false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Map_Pool.Map_pool.Recycle_Hex();
                Map_Pool.Map_pool.Recycle_Enemytag();
                if (hit.transform.tag== "Player")
                {
                    Unit_Update();
                    player_script = hit.transform.gameObject.GetComponent<Unit_Control>();
                    player_script.BFS(Search_setting.Moverange);
                    player_index = player_script.unit_position_index;
                    revocation_Button.transform.gameObject.SetActive(false);
                }
                if (hit.transform.tag == "Untagged")
                {
                    revocation_Button.transform.gameObject.SetActive(false);
                }
                if (hit.transform.tag == "Map")
                {
                    move_target = hit.transform.GetComponent<Hex_Info>().index;
                    player_script.Move(player_index, move_target);
                }
                if (hit.transform.tag == "Enemy")
                {
                    if (player_script != null)
                    {
                        revocation_Button.transform.gameObject.SetActive(false);
                        enemy_script = hit.transform.GetComponent<Unit_Control>();
                        player_script.Attack(player_script.Attack_power, enemy_script);
                    }
                    player_script = null;
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
                Map_Pool.Map_pool.Recycle_Hex();
                Map_Pool.Map_pool.Recycle_Enemytag();
                if (hit.transform.tag == "Player")
                {
                    player_script = hit.transform.gameObject.GetComponent<Unit_Control>();
                    player_script.BFS(Search_setting.Moverange);
                    player_index = player_script.unit_position_index;
                    revocation_Button.transform.gameObject.SetActive(false);
                }
                if (hit.transform.tag == "Untagged")
                {
                    revocation_Button.transform.gameObject.SetActive(false);
                }
                if (hit.transform.tag == "Map")
                {
                    move_target = hit.transform.GetComponent<Hex_Info>().index;
                    player_script.Move(player_index, move_target);
                    Unit_Update();
                }
                if (hit.transform.tag == "Enemy")
                {
                    if (player_script != null)
                    {
                        revocation_Button.transform.gameObject.SetActive(false);
                        enemy_script = hit.transform.GetComponent<Unit_Control>();
                        player_script.Attack(player_script.Attack_power, enemy_script);
                    }
                    Unit_Update();
                    player_script = null;
                }
            }
        }
#endif
    }
}
