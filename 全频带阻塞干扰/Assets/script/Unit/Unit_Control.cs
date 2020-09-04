using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 对指定单位实现搜索，攻击，移动，扣血，死亡...
/// </summary>
public class Unit_Control : Unit_Info
{
    enum SearchModel
    {
        Attack,
        Move,
    }
    public GameObject targetedIcon;//被瞄准提示图标

    public bool isCanAttack=true;//是否可以攻击
    public bool isCanMove=true;//是否可以移动

    bool isMorale;//是否被包围
    int AP;//行动点数
    int Attack_Range;//射程
    int Attack_Power;//攻击力
    Vector2Int lastCoordinate;//上一个坐标
    List<Vector2Int> canMoveList = new List<Vector2Int>();//可移动的坐标
    List<Vector2Int> canAttackTargetList = new List<Vector2Int>();//可攻击的坐标
    List<Vector2Int> path_List = new List<Vector2Int>();//移动路径
    Dictionary<Vector2Int, Vector2Int> parentNodeDic = new Dictionary<Vector2Int, Vector2Int>();//可移动点父节点

    void Start()
    {
        SetPostition();
        SetUnitData();
        SetHP();
        targetedIcon.SetActive(false);
        morale_Panel.SetActive(false);
        GetComponent<MeshRenderer>().sortingOrder = 10;
        AP = Config_Item.Instance.GetItemInfo(unit_Name).item_AP;
        Attack_Range = Config_Item.Instance.GetItemInfo(unit_Name).iten_Range;
        Attack_Power = Config_Item.Instance.GetItemInfo(unit_Name).item_Attack;
    }

    void OnMouseDown()
    {
        Unit_Management.instance.Unit_Selected(this);
    }

    /// <summary>
    /// 更改单位坐标信息
    /// </summary>
    void SetUnitData()
    {
        Dictionary<Vector2Int, GameObject> setDictonary =new Dictionary<Vector2Int, GameObject>();
        if (this.transform.tag == "Friendly")
        {
            setDictonary = Unit_Management.instance.friendlyCoordinate;
        }
        else if (this.transform.tag == "Enemy")
        {
            setDictonary = Unit_Management.instance.enemyCoordinate;
        }

        if (setDictonary.ContainsKey(this.lastCoordinate))
        {
            setDictonary.Remove(lastCoordinate);
            setDictonary.Add(coordinate, this.gameObject);
        }
        else
            setDictonary.Add(coordinate, this.gameObject);
    }

    /// <summary>
    /// 撤消
    /// </summary>
    public void Revocation()
    {
        var lastCoordinate = coordinate;
        coordinate = this.lastCoordinate;
        this.lastCoordinate = lastCoordinate;
        SetPostition();
        SetUnitData();
        MovePointDisplay();
        AttackTargetDisplay();
        Morale_Inspection();
    }

    /// <summary>
    /// 显示可移动的格子
    /// </summary>
    public void MovePointDisplay()
    {
        BFS(SearchModel.Move, AP);
        for (int i = 1; i < canMoveList.Count; i++)
        {
            if (Unit_Management.instance.friendlyCoordinate.ContainsKey(canMoveList[i])) continue;
            var mapPrefab = ObjectPool.instance.Get_Map();
            mapPrefab.GetComponent<Hex_Info>().SetMapData(canMoveList[i]) ;
            mapPrefab.transform.position = MapData.Instance.GetPoint(canMoveList[i]);
            mapPrefab.SetActive(true);
        }
    }
    /// <summary>
    /// 显示可以攻击的目标
    /// </summary>
    public void AttackTargetDisplay()
    {
        BFS(SearchModel.Attack, Attack_Range);
        for (int i = 0; i < canAttackTargetList.Count; i++)
        {
            if (Unit_Management.instance.enemyCoordinate.ContainsKey(canAttackTargetList[i]))
            {
                Unit_Management.instance.enemyCoordinate[canAttackTargetList[i]].GetComponent<Unit_Control>().targetedIcon.SetActive(true);
            }
        }
    }

    /// <summary>
    /// 单位移动
    /// </summary>
    /// <param name="target"></param>
    public void Unit_Move(Vector2Int target)
    {
        lastCoordinate = coordinate;
        Vector2Int parentNode = parentNodeDic[target];
        path_List.Clear();
        path_List.Add(target);

        while (parentNode != coordinate)
        {
            path_List.Add(parentNode);
            parentNode = parentNodeDic[parentNode];
        }
        ObjectPool.instance.Recycle_Hex();

        StartCoroutine(Move());
        IEnumerator Move()
        {
            for (int i = path_List.Count - 1; i >= 0; i--)
            {
                Vector3 present_poision = transform.position;
                float move_T = 0;
                while (move_T < 1)
                {
                    move_T += Time.deltaTime * 5;
                    transform.position = Vector3.Lerp(present_poision, MapData.Instance.GetPoint(path_List[i]), move_T);
                    yield return null;
                }
            }
            coordinate = target;
            Morale_Inspection();
            AttackTargetDisplay();
            SetUnitData();
            Unit_Management.instance.RevocationPanelDisPlay();
        }
    }
    /// <summary>
    /// 检查自身是否被包围
    /// </summary>
    public void Morale_Inspection()
    {
        List<Vector2Int> adjacentList = MapData.Instance.GetAdjacentMap(coordinate);
        List<int> bearing = new List<int>();
        int moraleCount = 0;
        for (int i = 0; i < adjacentList.Count; i++)
        {
            if (Unit_Management.instance.enemyCoordinate.ContainsKey(adjacentList[i]))
            {
                bearing.Add(i);
            }
        }
        if(adjacentList.Count == bearing.Count)
        {
            morale_Panel.SetActive(true);
            moraleCount = -2;
            isMorale = true;
            return;
        }
        for (int i = 0; i < bearing.Count/2; i++)
        {
            if (bearing.Contains(bearing[i]+3))
            {
                morale_Panel.SetActive(true);
                moraleCount--;
                isMorale = true;
            }
        }
    }

    void BFS(SearchModel searchModel,int searchRange) 
    {
        int needSearchMapCount =1;//每层搜索次数
        int adjacentMapCount =0;//每层的相邻地块数量
        int searchCount =0;
        Queue<Vector2Int> searchQueue = new Queue<Vector2Int>();
        searchQueue.Enqueue(coordinate);
        if (searchModel == SearchModel.Move)
        {
            parentNodeDic.Clear();
            canMoveList.Clear();
            canMoveList.Add(coordinate);
        }
        else
        {
            canAttackTargetList.Clear();
            canAttackTargetList.Add(coordinate);
        }
        List<Vector2Int> adjacentList;

        while (searchCount < searchRange)
        {
            searchCount++;

            for (int i = 0; i < needSearchMapCount; i++)
            {
                var parentNode = searchQueue.Peek();
                adjacentList = MapData.Instance.GetAdjacentMap(searchQueue.Dequeue());
                for (int j = 0; j < adjacentList.Count; j++)
                {
                    if (searchModel == SearchModel.Move)
                    {
                        if (canMoveList.Contains(adjacentList[j]) || Unit_Management.instance.enemyCoordinate.ContainsKey(adjacentList[j])) continue;
                        canMoveList.Add(adjacentList[j]);
                        parentNodeDic.Add(adjacentList[j], parentNode);
                    }
                    else
                    {
                        if (canAttackTargetList.Contains(adjacentList[j])) continue;
                        canAttackTargetList.Add(adjacentList[j]);
                    }
                    searchQueue.Enqueue(adjacentList[j]);
                    adjacentMapCount++;
                }
            }
            needSearchMapCount = adjacentMapCount;
            adjacentMapCount = 0;
        }
    }

    public void Attack(Unit_Control target)
    {
        int hit=0;
        if (isMorale)
        {
            hit = Attack_Power/2;
        }
        else
        {
            hit = Attack_Power;
        }
        target.Be_Hit(hit);
    }
}
