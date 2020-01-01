using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex_Info : MonoBehaviour
{
    public int Hex_x;//获取当前六边形的位置
    public int Hex_y;//获取当前六边形的位置
    public int index;
    public void Hex_data(int index,int x,int y)
    {
        Hex_x = x;
        Hex_y = y;
        this.index = index;
    }
    public void Hex_data(int index)
    {
        this.index = index;
    }
}
