using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 对指定单位实现搜索，攻击，移动，扣血，死亡...
/// </summary>
public class Unit_Control : Unit_UI
{
    private int unit_Revocation_Position_Index;//执行撤销指令时的下标
    private int AP;//行动点数
    private int Attack_Range;//射程
    private int Attack_Power;//攻击力
    private bool reverse_Search = false;

    private Dictionary<int, List<int>> graph = new Dictionary<int, List<int>>();
    private List<int> path_List = new List<int>();
    private List<bool> enemy_Exist = new List<bool>();
    private bool ismorale;

    void Start()
    {
        Postition_Start();
        HP_Start();
        Create_UnityUI();
        graph = Map_Management.instance.hex_Graph;

        AP = Config_Item.Instance.item_List_All[Config_Item.Instance.Config_unity_info(unit_Name)].item_AP;
        Attack_Range = Config_Item.Instance.item_List_All[Config_Item.Instance.Config_unity_info(unit_Name)].iten_Range;
        Attack_Power = Config_Item.Instance.item_List_All[Config_Item.Instance.Config_unity_info(unit_Name)].item_Attack;
    }
  
    public void BFS(Unit_Management.Search_setting search_Setting)//显示移动和攻击范围范围
    {
        int search_Count=0;//搜索次数
        int search_Range=0;
        List<int> open_List = new List<int>();
        Queue<int> open_Queue = new Queue<int>();
        open_Queue.Enqueue(unit_Position_Index);//入队
        open_List.Add(unit_Position_Index);
        int adjacent_Count = 0;//每层网格相邻的非重叠的网格数量(作为下次需要遍历的网格数量)
        int hex_Count = 1;//每一层需要遍历的网格数量
        if (search_Setting == Unit_Management.Search_setting.Moverange)
        {
            unit_Revocation_Position_Index = unit_Position_Index;
            search_Range = AP;
        }
        else if(search_Setting == Unit_Management.Search_setting.Enemy)
        {
            search_Range = Attack_Range;
        }
        else if (search_Setting == Unit_Management.Search_setting.Morale)
        {
            search_Range = 1;
            ismorale = false;
        }

        while (search_Count < search_Range)
        {
            for (int floor = 0; floor < hex_Count; floor++)
            {
                for (int i = 0; i < graph[open_Queue.Peek()].Count; i++)
                {
                    if (open_List.Contains(graph[open_Queue.Peek()][i]) == false && search_Setting== Unit_Management.Search_setting.Moverange)//遍历附近的节点，如果重复则跳过
                    {
                        if (Unit_Management.instance.Enemy_List.ContainsKey(graph[open_Queue.Peek()][i]) == true)
                        {
                            continue;
                        }
                        else if (Unit_Management.instance.Player_List.ContainsKey(graph[open_Queue.Peek()][i]) == false)
                        {
                            GameObject hex = ObjectPool.instance.Get_Hex();
                            hex.transform.position = position_Array[graph[open_Queue.Peek()][i]];
                            hex.GetComponent<Hex_Info>().Hex_data(graph[open_Queue.Peek()][i]);
                        }

                        open_Queue.Enqueue(graph[open_Queue.Peek()][i]);
                        open_List.Add(graph[open_Queue.Peek()][i]);
                        adjacent_Count++;
                    }

                    else if (open_List.Contains(graph[open_Queue.Peek()][i]) == false && search_Setting == Unit_Management.Search_setting.Enemy)
                    {
                        if (Unit_Management.instance.Enemy_List.ContainsKey(graph[open_Queue.Peek()][i]) == true)
                        {
                            GameObject enemytag = ObjectPool.instance.Get_Enemytag();
                            enemytag.transform.position = position_Array[graph[open_Queue.Peek()][i]];
                        }
                        open_Queue.Enqueue(graph[open_Queue.Peek()][i]);
                        open_List.Add(graph[open_Queue.Peek()][i]);
                        adjacent_Count++;
                    }
                    else if (open_List.Contains(graph[open_Queue.Peek()][i]) == false && search_Setting == Unit_Management.Search_setting.Morale)
                    {
                        if (this.tag == "Player")
                        {
                            if (Unit_Management.instance.Enemy_List.ContainsKey(graph[open_Queue.Peek()][i]) == true)
                            {
                                enemy_Exist.Add(true);
                            }
                            else
                            {
                                enemy_Exist.Add(false);
                            }
                        }
                        else if (this.tag == "Enemy")
                        {
                            if (Unit_Management.instance.Player_List.ContainsKey(graph[open_Queue.Peek()][i]) == true)
                            {
                                enemy_Exist.Add(true);
                            }
                            else
                            {
                                enemy_Exist.Add(false);
                            }
                        }
                        open_Queue.Enqueue(graph[open_Queue.Peek()][i]);
                        open_List.Add(graph[open_Queue.Peek()][i]);
                        adjacent_Count++;
                    }
                }
                open_Queue.Dequeue();
            }
            hex_Count = adjacent_Count;
            adjacent_Count = 0;//赋值后要把当前层的网格数量归零
            search_Count++;
        }
        if (search_Setting == Unit_Management.Search_setting.Moverange)
        {
            BFS(Unit_Management.Search_setting.Enemy);
        }
        else if (search_Setting == Unit_Management.Search_setting.Morale)
        {
            Morale_Inspection();
        }
    }

    public void Morale_Inspection()//检查自身是否被包围
    {
        for (int i = 0; i < 3; i++)
        {
            if (enemy_Exist[i]==true&& enemy_Exist[i+3]==true)
            {
                morale.SetActive(true);
                ismorale = true;
            }
            if (ismorale==false)
            {
                morale.SetActive(false);
            }
        }
        enemy_Exist.Clear();
    }
   

    public void Revocation()//撤销命令
    {
        transform.position= position_Array[unit_Revocation_Position_Index];
        unit_Position_Index = unit_Revocation_Position_Index;
        Unit_Management.instance.Unit_Update(null);
        ObjectPool.instance.Recycle_Enemytag();
        Update_UnityUIPosition();
        this.BFS(Unit_Management.Search_setting.Moverange);
    }

    public void Attack(Unit_Control target)
    {
        int hit=0;
        if (ismorale)
        {
            hit = Attack_Power/2;
        }
        else
        {
            hit = Attack_Power;
        }
        target.Be_Hit(hit);
    }

    public void Be_Hit(int damage)
    {
        hp -= damage;
        Update_HP();
        StartCoroutine(Hit());
    }

    public void Move(int startposition_index, int targetposition_index)//前往目标位置
    {
        ismorale = false;
        morale.SetActive(false);

        path_List.Clear();

        int move_count = 0;//移动次数
        int move_path = -1;//最短路径下标
      
        Queue<int> move_queue = new Queue<int>();
        move_queue.Enqueue(startposition_index);
        while (move_count < AP)
        {
            move_count++;
            int shortest_hex = 0;//最近网格(模拟量)
            for (int i = 1; i < graph[move_queue.Peek()].Count; i++)
            {
                if (Vector3.Distance(position_Array[graph[move_queue.Peek()][shortest_hex]], position_Array[targetposition_index]) <
                    Vector3.Distance(position_Array[graph[move_queue.Peek()][     i      ]], position_Array[targetposition_index])&&
                    Unit_Management.instance.Enemy_List.ContainsKey(graph[move_queue.Peek()][shortest_hex]) == false)
                //当指定网格与终点的距离小于当前遍历网格与终点距离并且指定网格上无敌方单位时，这个网格的下标就会成为这次遍历中的最短路径
                {
                    move_path = graph[move_queue.Peek()][shortest_hex];
                }
                else if (Unit_Management.instance.Enemy_List.ContainsKey(graph[move_queue.Peek()][i]) == false)
                {
                    shortest_hex = i;
                    move_path = graph[move_queue.Peek()][shortest_hex];
                }
            }
            move_queue.Enqueue(move_path);
            path_List.Add(move_path);
            move_queue.Dequeue();
            if (move_path == targetposition_index)
            {

                if (reverse_Search == false)
                {
                    unit_Position_Index = targetposition_index;
                    StartCoroutine(Way());
                    break;
                }
                else
                {
                    unit_Position_Index = startposition_index;//当切换为倒序搜索时，找到终点后，单位下标为起点下标
                    path_List.Reverse();
                    path_List.RemoveAt(0);
                    path_List.Add(startposition_index);
                    StartCoroutine(Way());
                }
            }
        }

        if (move_count == AP && move_path != targetposition_index)//当正序搜索无法找到终点时使用倒序搜索
        {
            reverse_Search = true;
            Move(targetposition_index, startposition_index);
            reverse_Search = false;//当切换为倒序搜索时，找到终点后，需要重置搜索设置，以保证下次搜索时能使用正确的搜索设置
        }
    }
    private void Death()
    {
        Unit_Management.instance.Unit_Update(this.gameObject);
        Destroy(this.gameObject);
        Destroy(this.hp_Slider);
        Destroy(this.morale);
    }

    IEnumerator Hit()
    {
        while (effect.fillAmount > slider.value)
        {
            effect.fillAmount -= Hurtspeed;
            yield return null;
        }
        if (slider.value == 0)
        {
            Death();
        }
        effect.fillAmount = slider.value;
    }

    IEnumerator Way()
    {
        for (int i = 0; i < path_List.Count; i++)
        {
            Vector3 present_poision = transform.position;
            float move_T = 0;
            while (move_T < 1)
            {
                move_T = move_T + 0.1f;
                transform.position = Vector3.Lerp(present_poision, position_Array[path_List[i]], move_T);
                Update_UnityUIPosition();
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        Unit_Management.instance.Unit_Update(null);
        Unit_Management.instance.Revocation_Allow();

        BFS(Unit_Management.Search_setting.Enemy);
    }
}
