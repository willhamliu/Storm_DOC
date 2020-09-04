using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData:InstanceNull<MapData>
{
    public class Data
    {
        public List<Vector2Int> mapAdjacentHex;
        public Vector3 mapPoint;
        public enum terrain
        {
            Plain,
            mountain,
            lake,
            swamp,
        }
    }
    public Dictionary<Vector2Int, Data> mapDataDictonary = new Dictionary<Vector2Int, Data>();//地图的相邻节点信息


    /// <summary>
    /// 通过横纵坐标值获取位置
    /// </summary>
    /// <param name="unitIndex">横纵坐标</param>
    /// <returns></returns>
    public Vector3 GetPoint(Vector2Int unitIndex)
    {
        if (mapDataDictonary.ContainsKey(unitIndex))
        {
            return mapDataDictonary[unitIndex].mapPoint;
        }
        else
        {
            Debug.Log("无法找到该坐标");
            return Vector3.zero;
        }
    }
    /// <summary>
    /// 获取相邻地块
    /// </summary>
    /// <param name="mapIndex">指定地块坐标</param>
    /// <returns></returns>
    public List<Vector2Int> GetAdjacentMap(Vector2Int mapIndex)
    {
        if (mapDataDictonary.ContainsKey(mapIndex))
        {
            return mapDataDictonary[mapIndex].mapAdjacentHex;
        }
        else
        {
            Debug.Log("无相邻地块信息");
            return null;
        }
    }
}
