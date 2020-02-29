using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Control : Unit_UI
{
    public int coordinate_x;//该单位在x轴的编号
    public int coordinate_y;//该单位在y轴的编号
    public string unit_name;
    public int unit_position_index;//该单位所在位置的下标
    public int unit_revocation_position_index;//执行撤销指令时的下标
    public int AP;//行动点数
    public int Attack_Range;//射程
    public int Attack_power;//攻击力
    private Vector3 unit_position;//该单位所在的位置
    private bool reverse_search = false;

    private Vector3[] position_array;//索引位置数组
    private Dictionary<int, List<int>> graph = new Dictionary<int, List<int>>();
    private List<int> path_list = new List<int>();

    private void Awake()
    {
        unit_name = this.gameObject.name;
    }
    void Start()
    {
        position_array = Map_Management.map_Management.Hex_position;
        graph = Map_Management.map_Management.Hex_graph;
        unit_position_index = Map_Management.map_Management.Hex_index[coordinate_x, coordinate_y];
        unit_position = position_array[unit_position_index];

        this.transform.position = unit_position;

        AP = Config_Item.Config_item.item_List_All[Config_Item.Config_item.Config_unity_info(unit_name)].Item_AP;
        Attack_Range = Config_Item.Config_item.item_List_All[Config_Item.Config_item.Config_unity_info(unit_name)].Iten_Range;
        Attack_power = Config_Item.Config_item.item_List_All[Config_Item.Config_item.Config_unity_info(unit_name)].Item_Attack;
        MAX_HP = HP = Config_Item.Config_item.item_List_All[Config_Item.Config_item.Config_unity_info(unit_name)].Item_HP;
        Create_HP();
    }

    public void BFS(Unit_Management.Search_setting search_setting)//显示移动和攻击范围范围
    {
        int search_count=0;//搜索次数
        int search_range=0;
        List<int> open_list = new List<int>();
        Queue<int> open_queue = new Queue<int>();
        open_queue.Enqueue(unit_position_index);//入队
        open_list.Add(unit_position_index);
        int floor_count = 0;//每层网格相邻的非重叠的网格数量(作为下次需要遍历的网格数量)
        int hex_count = 1;//每一层需要遍历的网格数量
        if (search_setting == Unit_Management.Search_setting.Moverange)
        {
            unit_revocation_position_index = unit_position_index;
            search_range = AP;
        }
        else
        {
            search_range = Attack_Range;
        }

        while (search_count < search_range)
        {
            for (int floor = 0; floor < hex_count; floor++)
            {
                for (int i = 0; i < graph[open_queue.Peek()].Count; i++)
                {
                    if (open_list.Contains(graph[open_queue.Peek()][i]) == false && search_setting== Unit_Management.Search_setting.Moverange)//遍历附近的节点，如果重复则跳过
                    {
                        if (Unit_Management.Unit_management.Enemy_list.Contains(graph[open_queue.Peek()][i]) == true)
                        {
                            continue;
                        }
                        else if (Unit_Management.Unit_management.Player_list.Contains(graph[open_queue.Peek()][i]) == false)
                        {
                            GameObject hex = Map_Pool.Map_pool.Get_Hex();
                            hex.transform.position = position_array[graph[open_queue.Peek()][i]];
                            hex.GetComponent<Hex_Info>().Hex_data(graph[open_queue.Peek()][i]);
                        }

                        open_queue.Enqueue(graph[open_queue.Peek()][i]);
                        open_list.Add(graph[open_queue.Peek()][i]);                        
                        floor_count++;
                    }
                    else if (open_list.Contains(graph[open_queue.Peek()][i]) == false && search_setting == Unit_Management.Search_setting.Enemy)
                    {
                        if (Unit_Management.Unit_management.Enemy_list.Contains(graph[open_queue.Peek()][i]) == true)
                        {
                            GameObject enemytag = Map_Pool.Map_pool.Get_Enemytag();
                            enemytag.transform.position = position_array[graph[open_queue.Peek()][i]];
                        }

                        open_queue.Enqueue(graph[open_queue.Peek()][i]);
                        open_list.Add(graph[open_queue.Peek()][i]);
                        floor_count++;
                    }
                }
                open_queue.Dequeue();
            }
            hex_count = floor_count;
            floor_count = 0;//赋值后要把当前层的网格数量归零
            search_count++;
        }
        if (search_setting == Unit_Management.Search_setting.Moverange)
        {
            BFS(Unit_Management.Search_setting.Enemy);
        }
    }
   

    public void Revocation()//撤销命令
    {
        transform.position= position_array[unit_revocation_position_index];
        unit_position_index = unit_revocation_position_index;
        Unit_Management.Unit_management.Unit_Update();
        Update_Slider_Position();
        this.BFS(Unit_Management.Search_setting.Moverange);
    }

    public void Attack(int atk ,Unit_Control target)
    {
        target.Be_hit(atk);
    }

    public void Be_hit(int damage)
    {
        HP -= damage;
        Update_HP();
    }
    public void Move(int startposition_index, int targetposition_index)//前往目标位置
    {
        int move_count = 0;//移动次数
        int move_path = -1;//最短路径下标
      
        path_list.Clear();
        Queue<int> move_queue = new Queue<int>();
        move_queue.Enqueue(startposition_index);
        while (move_count < AP)
        {
            move_count++;
            int shortest_hex = 0;//最近网格(模拟量)
            for (int i = 1; i < graph[move_queue.Peek()].Count; i++)
            {
                if (Vector3.Distance(position_array[graph[move_queue.Peek()][shortest_hex]], position_array[targetposition_index]) <
                    Vector3.Distance(position_array[graph[move_queue.Peek()][     i      ]], position_array[targetposition_index])&&
                    Unit_Management.Unit_management.Enemy_list.Contains(graph[move_queue.Peek()][shortest_hex]) == false)
                //当指定网格与终点的距离小于当前遍历网格与终点距离并且指定网格上无敌方单位时，这个网格的下标就会成为这次遍历中的最短路径
                {
                    move_path = graph[move_queue.Peek()][shortest_hex];
                }
                else if (Unit_Management.Unit_management.Enemy_list.Contains(graph[move_queue.Peek()][i]) == false)
                {
                    shortest_hex = i;
                    move_path = graph[move_queue.Peek()][shortest_hex];
                }
            }
            move_queue.Enqueue(move_path);
            path_list.Add(move_path);
            move_queue.Dequeue();
            if (move_path == targetposition_index)
            {

                if (reverse_search == false)
                {
                    unit_position_index = targetposition_index;
                    StartCoroutine(Way());
                    break;
                }
                else
                {
                    reverse_search = false;//当切换为倒序搜索时，找到终点后，需要重置搜索设置，以保证下次搜索时能使用正确的搜索设置
                    unit_position_index = startposition_index;//当切换为倒序搜索时，找到终点后，单位下标为起点下标
                    path_list.Reverse();
                    path_list.RemoveAt(0);
                    path_list.Add(startposition_index);
                    StartCoroutine(Way());
                }
            }
        }

        if (move_count == AP && move_path != targetposition_index)//当正序搜索无法找到终点时使用倒序搜索
        {
            reverse_search = true;
            Move(targetposition_index, startposition_index);
        }
    }
   
    IEnumerator Way()
    {       
        for (int i = 0; i < path_list.Count; i++)
        {
            Vector3 present_poision = transform.position;
            float move_T = 0;
            while (move_T < 1)
            {
                move_T = move_T + 0.1f;
                transform.position = Vector3.Lerp(present_poision, position_array[path_list[i]], move_T);
                Update_Slider_Position();
                yield return new WaitForSeconds(Time.deltaTime);
            }
            if (i == path_list.Count-1)
            {
                BFS(Unit_Management.Search_setting.Enemy);
            }
        }
        Unit_Management.Unit_management.Revocation_Allow();
    }
}
