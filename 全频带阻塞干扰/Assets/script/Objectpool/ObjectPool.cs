using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 六边形网格与攻击图标的对象池
/// </summary>
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    public GameObject hex_Prefab;
    public GameObject enemy_Prefab;
    public Transform map_Create;
    public Transform enemytag_Create;
    private List<GameObject> map_Instance = new List<GameObject>();
    private List<GameObject> enemytag_Instance = new List<GameObject>();

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
            GameObject enemytag= Instantiate(enemy_Prefab, enemytag_Create);
            GameObject hex = Instantiate(hex_Prefab, map_Create);
            enemytag.SetActive(false);
            hex.SetActive(false);
            enemytag_Instance.Add(enemytag);
            map_Instance.Add(hex);
        }
    }

    public GameObject Get_Enemytag()
    {
        for (int i = 0; i < enemytag_Instance.Count; i++)
        {
            if (!enemytag_Instance[i].activeInHierarchy)
            {
                enemytag_Instance_End = i;
                enemytag_Instance[i].SetActive(true);
                return enemytag_Instance[i];
            }
        }
        return null;
    }
    public GameObject Get_Hex()
    {
        for (int i = 0; i < map_Instance.Count; i++)
        {
            if (!map_Instance[i].activeInHierarchy) 
            {
                map_Instance_End = i;
                map_Instance[i].SetActive(true);
                return map_Instance[i];             
            }
        }
        return null;
    }
    public void Recycle_Enemytag()
    {
        for (int i = 0; i <= enemytag_Instance_End; i++)
        {
            enemytag_Instance[i].SetActive(false);
        }
    }
    public void Recycle_Hex()
    {
        for (int i = 0; i <= map_Instance_End; i++)
        {
            map_Instance[i].SetActive(false);
        }
    }
}
