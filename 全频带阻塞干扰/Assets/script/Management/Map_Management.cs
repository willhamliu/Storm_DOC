using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map_Management : MonoBehaviour
{
    public static Map_Management map_Management;

    public const float sidelength = 10;//六边形边长
    public  float innerRadius { get; private set; } = sidelength * 0.866f;//六边形内半径

    public int width;//x轴长度
    public int height;//z轴长度

    public GameObject hex_Prefab;//六边形预制体
    public Vector3 boundary { get; private set; }//地图边界

    public Vector3 [] Hex_position { get; private set; }//六边形的二维数组
    public int[,] Hex_index { get; private set; }
    public Dictionary<int, List<int>> Hex_graph { get; private set; } = new Dictionary<int, List<int>>();//所都点的相邻节点信息

    private bool Obtain_boundary = false;//检查摄像机是否获取了边界信息

    public Transform MAP_create;

    public void Awake()
    {
        Config_Item.Config_item.Config_Item_Json();//测试后记得删掉！！！！！！！！！！！！！！！！！！！

        if (map_Management==null)
        {
            map_Management = this;
        }
        Hex_position = new Vector3[width * height];

        Hex_index = new int[width,height];

        for (int y = 0, index=0; y < height; y++)
        {
            for (int X = 0; X < width; X++, index++)
            {
                CreateCell(index, X,y);
            }
        }
        for (int y = 0 ,index=0; y < height; y++)
        {
            for (int X = 0; X < width; X++, index++)
            {
                Graph(index,X, y);
            }
        }
    }

    void CreateCell(int index, int x, int y)//创建六边形
    {
        Vector3 position;
        position.x = x * (sidelength * 1.5f) + ((-width / 2) * (sidelength * 1.5f));//左右距离为1.5倍边长,再加上偏移量
        position.y = (y + ((x * 0.5f) - (x / 2))) * (innerRadius * 2f) + ((-height / 2) * (innerRadius * 2f));//上下距离为2倍内半径
        position.z = 0f;
        //(x / 2为小数时会直接移除小数，因此当x为偶数时(y+ ((x*0.5f)-(x/2)) )为   (2+ ((4*0.5f)- (4/2)) ) =2

        Hex_position[index] = position;
        Hex_index[x, y] = index;

        //GameObject hex = Instantiate(hex_Prefab, MAP_create);
        //Hex_Info hex_Info = hex.GetComponent<Hex_Info>();
        //hex_Info.Hex_data(x, y, index);
        //hex.transform.localPosition = position;

        if (Obtain_boundary==false)
        {
            boundary = new Vector3(position.x- (2*sidelength), position.y - (2*innerRadius), position.z);
            Obtain_boundary = true;
        }
    }

    void Graph(int index, int x, int y)
    {
        List<int> graph = new List<int>();
        if (y + 1 < height)//上
        {
            graph.Add(Hex_index[x, y + 1]);
        }

        if (x%2==0)
        {
            if (x + 1 < width)//右上
            {
                graph.Add(Hex_index[x + 1, y]);
            }
            if (y - 1 >= 0 && x + 1 < width)//右下
            {
                graph.Add(Hex_index[x + 1, y - 1]);
            }
        }
        else
        {
            if (x + 1 < width && y + 1 < height)
            {
                graph.Add(Hex_index[x + 1, y + 1]);
            }
            if (x + 1 < width)
            {
                graph.Add(Hex_index[x + 1, y]);
            }
        }
      

        if (y - 1 >= 0)//下
        {
            graph.Add(Hex_index[x, y - 1]);
        }

        if (x % 2 == 0)
        {
            if (y - 1 >= 0 && x - 1 >= 0)//左下
            {
                graph.Add(Hex_index[x - 1, y - 1]);
            }
            if (x - 1 >= 0)//左上
            {
                graph.Add(Hex_index[x - 1, y]);
            }
        }
        else
        {
            if (x - 1 >= 0)
            {
                graph.Add(Hex_index[x - 1, y]);
            }
            if (y + 1 < height && x - 1 >= 0)
            {
                graph.Add(Hex_index[x - 1, y + 1]);
            }
        }
        Hex_graph.Add(index, graph);
    }
}
