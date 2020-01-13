using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Info : Unit_UI
{
    public int coordinate_x;//该单位在x轴的编号
    public int coordinate_y;//该单位在y轴的编号
    public string unit_name;
    public int AP;//行动点数
    public int unity_position_index { get; set; }//该单位所在位置的下标
    public int unity_revocation_position_index;//执行撤销指令时的下标
    private Vector3 unit_position;//该单位所在的位置
    private Vector3 unity_targetposition;//目标位置
    public bool selected_State { get; set; } = false;//单位是否被选中
    public Transform MAP_create;
    private Vector3[] position_array;

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
        unity_position_index = Map_Management.map_Management.Hex_index[coordinate_x, coordinate_y];
        unit_position = position_array[unity_position_index];

        this.transform.position = unit_position;

        AP = Config_Item.Config_item.item_List_All[Config_Item.Config_item.Config_unity_info(unit_name)].Item_AP;
        MAX_HP = HP = Config_Item.Config_item.item_List_All[Config_Item.Config_item.Config_unity_info(unit_name)].Item_HP;
        Create_HP();
    }

    private void Update()
    {
        Be_hit();
    }

    public void BFS()//搜索附近的点
    {
        int search_count=0;//搜索次数
        
        List<int> seen = new List<int>();
        Queue<int> seen_queue = new Queue<int>();
        seen_queue.Enqueue(unity_position_index);//入队
        unity_revocation_position_index = unity_position_index;
        seen.Add(unity_position_index);
        int floor_count = 0;//每层网格相邻的非重叠的网格数量(作为下次需要遍历的网格数量)
        int hex_count = 1;//每一层需要遍历的网格数量

        while (search_count < AP)
        {
            for (int floor = 0; floor < hex_count; floor++)
            {
                for (int i = 0; i < graph[seen_queue.Peek()].Count; i++)
                {
                    if (seen.Contains(graph[seen_queue.Peek()][i]) == false)//遍历附近的节点，如果重复则跳过
                    {
                        seen_queue.Enqueue(graph[seen_queue.Peek()][i]);
                        seen.Add(graph[seen_queue.Peek()][i]);
                        GameObject hex = Map_Pool.Map_pool.Get_Hex();
                        if (Unit_Management.Unit_management.Unit_list.Contains(graph[seen_queue.Peek()][i]) == false)//如果为友方单位则不会被阻挡
                        {
                            hex.SetActive(true);
                        }
                        else
                        {
                            hex.SetActive(false);
                        }
                        hex.transform.position = position_array[graph[seen_queue.Peek()][i]];
                        hex.GetComponent<Hex_Info>().Hex_data(graph[seen_queue.Peek()][i]);
                        floor_count++;
                    }
                }
                seen_queue.Dequeue();
            }
            hex_count = floor_count;
            floor_count = 0;//赋值后要把当前层的网格数量归零
            search_count++;
        }
    }

    public void Revocation()//撤销命令
    {
        transform.position= position_array[unity_revocation_position_index];
        unity_position_index = unity_revocation_position_index;
        Unit_Management.Unit_management.Unit_update();
        Update_Slider_Position();
        this.BFS();
    }

    public void Move(int unit_targetposition_index)//前往目标位置
    {
        int move_count = 0;//移动次数
        int move_path = -1;//最短路径下标
      
        path_list.Clear();
        Queue<int> move_queue = new Queue<int>();
        move_queue.Enqueue(unity_position_index);

        while (move_count < AP)
        {
            move_count++;
            int shortest_hex = 0;//最近网格
            for (int i = 1; i < graph[move_queue.Peek()].Count; i++)
            {
                if (Vector3.Distance(position_array[graph[move_queue.Peek()][shortest_hex]], position_array[unit_targetposition_index]) <
                    Vector3.Distance(position_array[graph[move_queue.Peek()][i]], position_array[unit_targetposition_index]))
                {
                    move_path = graph[move_queue.Peek()][shortest_hex];
                }
                else
                {
                    shortest_hex = i;
                    move_path = graph[move_queue.Peek()][shortest_hex];
                }
            }

            move_queue.Enqueue(move_path);
            path_list.Add(move_path);
            move_queue.Dequeue();
            if (move_path == unit_targetposition_index)
            {
                unity_position_index = unit_targetposition_index;//更新单位位置的下标
                break;
            }
        }

        Map_Pool.Map_pool.Recycle();//回收对象
        StartCoroutine(Way());
    }

    public void Be_hit()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            HP -= (MAX_HP*0.2f);
            Update_HP();
        }
    }

    IEnumerator Way()
    {
        for (int move_count = 0; move_count < path_list.Count; move_count++)
        {
            Vector3 present_poision = transform.position;
            float move_T = 0;
            while (move_T < 1)
            {
                move_T = move_T + 0.1f;
                transform.position = Vector3.Lerp(present_poision, position_array[path_list[move_count]], move_T);
                Update_Slider_Position();
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        Unit_Management.Unit_management.Revocation_allow();
        Unit_Management.Unit_management.Unit_update();
    }
}
