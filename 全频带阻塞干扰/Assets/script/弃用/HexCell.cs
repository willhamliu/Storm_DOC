using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class HexCell : MonoBehaviour//绘制六边形网格(已弃用)
{
    List<Vector3> vertices = new List<Vector3>();//顶点位置
    List<int> triangles = new List<int>();
    Mesh mesh;

    void Awake()
    {
        mesh =transform.GetComponent<MeshFilter>().mesh ;
        mesh.name = "hexagon";
    }

    public void Triangulate(HexCell[] grid)
    {
        mesh.Clear();
        vertices.Clear();
        triangles.Clear();
        for (int i = 0; i < grid.Length; i++)
        {
            Triangulate(grid[i]);
        }
        mesh.vertices = vertices.ToArray();//ToArray()可以把列表数据转成数组形式
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    void Triangulate(HexCell cell)
    {
        for (int i = 0; i < 6; i++)
        {
            Vector3 center = cell.transform.localPosition;//每个六边形的中心点
            AddTriangle(center, center + Draw_Map.corners[i], center + Draw_Map.corners[i + 1]);//传递中心点与偏移量
        }
    }
    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }
}
