using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Pool : MonoBehaviour
{
    public static Map_Pool Map_pool;
    public GameObject hex_Prefab;
    public GameObject enemy_Prefab;
    public Transform MAP_create;
    public Transform Enemytag_create;
    private List<GameObject> MAP_instance = new List<GameObject>();
    private List<GameObject> Enemytag_instance = new List<GameObject>();

    private int MAP_instance_end;//末端对象下标
    private int Enemytag_instance_end;

    private void Awake()
    {
        if (Map_pool==null)
        {
            Map_pool = this;
        }
    }

    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            GameObject enemytag= Instantiate(enemy_Prefab, Enemytag_create);
            GameObject hex = Instantiate(hex_Prefab, MAP_create);
            enemytag.SetActive(false);
            hex.SetActive(false);
            Enemytag_instance.Add(enemytag);
            MAP_instance.Add(hex);
        }
    }
    public GameObject Get_Enemytag()
    {
        for (int i = 0; i < Enemytag_instance.Count; i++)
        {
            if (!Enemytag_instance[i].activeInHierarchy)
            {
                Enemytag_instance_end = i;
                Enemytag_instance[i].SetActive(true);
                return Enemytag_instance[i];
            }
        }
        return null;
    }
    public GameObject Get_Hex()
    {
        for (int i = 0; i < MAP_instance.Count; i++)
        {
            if (!MAP_instance[i].activeInHierarchy) 
            {
                MAP_instance_end = i;
                MAP_instance[i].SetActive(true);
                return MAP_instance[i];             
            }
        }
        return null;
    }
    public void Recycle_Enemytag()
    {
        for (int i = 0; i <= Enemytag_instance_end; i++)
        {
            Enemytag_instance[i].SetActive(false);
        }
    }
    public void Recycle_Hex()
    {
        for (int i = 0; i <= MAP_instance_end; i++)
        {
            MAP_instance[i].SetActive(false);
        }
    }
}
