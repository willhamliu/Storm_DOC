using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 管理地图大小，每个六边形网格的显示位置...
/// </summary>
public class Map_Management : MonoBehaviour
{
    const float sidelength = 10;//六边形边长
    const float Innerradius = sidelength * 0.866f;//六边形内半径

    public int width;//横向地图块数
    public int height;//纵向地图块数

    public static Vector2 boundary;//地图边界
    bool obtain_Boundary = false;//检查摄像机是否获取了边界信息
    float offsetX;
    float offsetY;

    MapData.Data data;

    void Awake()
    {
        offsetX = -((width / 2f) - 0.5f) * (sidelength * 1.5f);
        offsetY = -((height / 2f) - 0.5f) * (Innerradius * 2f);
        if (!obtain_Boundary)
        {
            boundary = new Vector2(offsetX- (sidelength*2), offsetY- (Innerradius*2));
            obtain_Boundary = true;
        }

        for (int Y = 0; Y < height; Y++)
        {
            for (int X = 0; X < width; X++)
            {
                CreateCell(X, Y);
                Adjacent(X, Y);
            }
        }
    }
 
    void CreateCell(int x, int y)//创建六边形
    {
        data = new MapData.Data();
        Vector3 position;
        position.x = x * (sidelength * 1.5f) + offsetX;//左右距离为1.5倍边长,再加上偏移量
        position.y = y * (Innerradius * 2f) + Innerradius * (x % 2) + offsetY;//上下距离为2倍内半径,当x为奇数时为一倍内半径
        position.z = 0f;
        data.mapPoint = position;
    }

    void Adjacent(int x, int y)
    {
        List<Vector2Int> graph = new List<Vector2Int>();
        if (y + 1 < height) graph.Add(new Vector2Int(x, y + 1));//上

        if (x%2==0)
        {
            if (x + 1 < width) graph.Add(new Vector2Int(x + 1, y));//右上
            if (y - 1 >= 0 && x + 1 < width) graph.Add(new Vector2Int(x + 1, y - 1));//右下
        }
        else
        {
            if (x + 1 < width && y + 1 < height) graph.Add(new Vector2Int(x + 1, y + 1));
            if (x + 1 < width) graph.Add(new Vector2Int(x + 1, y));
        }
      
        if (y - 1 >= 0) graph.Add(new Vector2Int(x, y - 1));//下
      
        if (x % 2 == 0)
        {
            if (y - 1 >= 0 && x - 1 >= 0) graph.Add(new Vector2Int(x - 1, y - 1));//左下
            if (x - 1 >= 0) graph.Add(new Vector2Int(x - 1, y));//左上
        }
        else
        {
            if (x - 1 >= 0) graph.Add(new Vector2Int(x - 1, y));
            if (y + 1 < height && x - 1 >= 0) graph.Add(new Vector2Int(x - 1, y + 1));
        }
        data.mapAdjacentHex = graph;

        MapData.Instance.mapDataDictonary.Add(new Vector2Int(x,y), data);
    }
}
