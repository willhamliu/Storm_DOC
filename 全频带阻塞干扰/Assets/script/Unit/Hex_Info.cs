using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 六边形网格的位置索引
/// </summary>
public class Hex_Info : MonoBehaviour
{
    public Vector2Int coordinate;
   
    public void SetMapData(Vector2Int coordinate)
    {
        this.coordinate = coordinate;
    }

    void OnMouseDown()
    {
        Unit_Management.instance.Unit_Move(coordinate);
    }
}
