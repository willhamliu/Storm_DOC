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

    public Text cell_Label_Prefab;
    public GameObject hex_Prefab;//六边形预制体

    public Transform gridCanvas;//ui坐标父物体

    public void Awake()
    {
        int index=0;
        for (int Z = 0; Z < height; Z++)
        {
            for (int X = 0; X < width; X++)
            {
                CreateCell(X,Z, index ++);
            }
        }
    }
    
    void CreateCell(int x, int z, int index)//创建六边形
    {
        Vector3 position;
        position.x = x * (sidelength* 1.5f);//左右距离为1.5倍边长
        position.y = 0f;
        position.z = (z+ ((x*0.5f)-(x/2)) ) * (innerRadius* 2f);//上下距离为2倍内半径
        //(x / 2为小数时会直接移除小数，因此当x为偶数时(z+ ((x*0.5f)-(x/2)) )为z   (2+ ((4*0.5f)- (4/2)) ) =2
        GameObject hex = Instantiate(hex_Prefab, transform);
        hex.transform.localPosition = position;

        Text coordinate = Instantiate(cell_Label_Prefab, gridCanvas.transform);
        coordinate.rectTransform.anchoredPosition = new Vector3(position.x, position.z);
        coordinate.text = x.ToString() + "\n" + z.ToString();
    }
}
