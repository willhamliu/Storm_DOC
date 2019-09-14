using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw_Map : MonoBehaviour//绘制六边形网格(以弃用)
{
    public const float sidelength = 10;//六边形边长
    public const float innerRadius = sidelength * 0.8660254039f;//六边形内半径

    public static Vector3[] corners = {
        new Vector3(sidelength,0,0f),//正右
        new Vector3(sidelength*0.5f,0f,-innerRadius),//右下
        new Vector3(-(sidelength*0.5f),0f,-innerRadius),//左下
        new Vector3(-sidelength,0f,0f),//正左
        new Vector3(-(sidelength*0.5f),0f,innerRadius),//左上
        new Vector3(sidelength*0.5f,0f,innerRadius),//右上
        new Vector3(sidelength,0,0f),//最后一个渲染的坐标为最初渲染的坐标
    };
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
