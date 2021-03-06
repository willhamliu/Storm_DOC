﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 六边形网格与攻击图标的对象池
/// </summary>
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    public GameObject hex_Prefab;
    public Transform map_Create;
    private List<GameObject> map_Instance = new List<GameObject>();

    private int map_Instance_End;//末端对象下标
    private int enemytag_Instance_End;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            GameObject hex = Instantiate(hex_Prefab, map_Create);
            hex.SetActive(false);
            map_Instance.Add(hex);
        }
    }

    public GameObject Get_Map()
    {
        for (int i = 0; i < map_Instance.Count; i++)
        {
            if (!map_Instance[i].activeInHierarchy) 
            {
                map_Instance_End = i;
                return map_Instance[i];             
            }
        }
        return null;
    }
  
    public void Recycle_Hex()
    {
        for (int i = 0; i <= map_Instance_End; i++)
        {
            map_Instance[i].SetActive(false);
        }
    }
}
