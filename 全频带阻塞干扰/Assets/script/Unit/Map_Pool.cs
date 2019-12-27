using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Pool : MonoBehaviour
{
    public static Map_Pool Map_pool;
    public GameObject hex_Prefab;
    public Transform MAP_create;
    private List<GameObject> MAP_instance = new List<GameObject>();

    private int MAP_instance_initally;//开头对象下标
    private int MAP_instance_end;//末端对象下标

    private void Awake()
    {
        if (Map_pool==null)
        {
            Map_pool = this;
        }
    }

    void Start()
    {
        for (int i = 0; i < 48; i++)
        {
            GameObject hex = Instantiate(hex_Prefab, MAP_create);
            hex.SetActive(false);
            MAP_instance.Add(hex);
        }
    }

    public GameObject Get_Hex()
    {
        for (int i = 0; i < MAP_instance.Count; i++)
        {
            if (!MAP_instance[i].activeInHierarchy) //判断该子弹是否在场景中激活。
            {
                MAP_instance_end = i;
                return MAP_instance[i];             //找到没有被激活的子弹并返回
            }
        }
        return null;
    }
    public void Recycle()
    {
        for (int i = 0; i <= MAP_instance_end; i++)
        {
            MAP_instance[i].SetActive(false);
        }
    }
}
