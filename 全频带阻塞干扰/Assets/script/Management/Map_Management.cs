using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map_Management : MonoBehaviour
{
    public const float sidelength = 10;//六边形边长
    public const float innerRadius = sidelength * 0.866f;//六边形内半径

    public int width;//x轴长度
    public int height;//z轴长度

    public GameObject hex_Prefab;//六边形预制体

    public List<Vector3> Hex_point { get; private set; } = new List<Vector3>();//所有六边形的列表

    public void Awake()
    {
        int index=0;
        for (int y = 0; y < height; y++)
        {
            for (int X = 0; X < width; X++)
            {
                CreateCell(X,y, index ++);
            }
        }
    }
    
    void CreateCell(int x, int y, int index)//创建六边形
    {
        Vector3 position;
        position.x = x * (sidelength * 1.5f) + ((-width / 2) * (sidelength * 1.5f));//左右距离为1.5倍边长,再加上偏移量
        position.y = (y + ((x * 0.5f) - (x / 2))) * (innerRadius * 2f) + ((-height / 2) * (innerRadius * 2f));//上下距离为2倍内半径
        position.z = 0f;
        //(x / 2为小数时会直接移除小数，因此当x为偶数时(y+ ((x*0.5f)-(x/2)) )为   (2+ ((4*0.5f)- (4/2)) ) =2
        Hex_point.Add(position);
        GameObject hex = Instantiate(hex_Prefab, transform);
        hex.transform.localPosition = position;

    }
}
