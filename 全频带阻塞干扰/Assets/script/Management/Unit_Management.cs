using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Unit_Management : MonoBehaviour
{
    public static Unit_Management Unit_management;
    public Button revocation_Button;
    public bool order_lock { get; set; } = false;
    GameObject choose_unit;
    int target_index;//目标索引
    private GameObject[] Unit_array_object;
    public List<int> Unit_list { get; private set; } = new List<int>();
    private int Unit_index=-1;//选中单位索引


    void Awake()
    {
        if (Unit_management==false)
        {
            Unit_management = this;
        }
    }

    private void Start()
    {
        revocation_Button.transform.gameObject.SetActive(false);
        revocation_Button.onClick.AddListener(Revocation);
        Invoke("Unit_update", 1);
    }
  
    void Update()
    {
        Unit_selected();
    }
    public void Unit_update()
    {
        Unit_array_object = GameObject.FindGameObjectsWithTag("Unit");
        Unit_list.Clear();
        for (int i = 0; i < Unit_array_object.Length; i++)
        {
            Unit_list.Add(Unit_array_object[i].transform.GetComponent<Unit_Info>().unity_position_index);
        }
    }

    public void Revocation_allow()//允许撤销
    {
        revocation_Button.transform.gameObject.SetActive(true);
    }

    private void Revocation()
    {
        revocation_Button.transform.gameObject.SetActive(false);
        choose_unit.GetComponent<Unit_Info>().Revocation();
    }

    private void Unit_selected()
    {
#if UNITY_EDITOR_WIN
        if (Input.GetMouseButtonUp(0)&& EventSystem.current.IsPointerOverGameObject() == false&& order_lock==false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag=="Unit"&& Unit_index != hit.transform.GetComponent<Unit_Info>().unity_position_index)
                {
                    Map_Pool.Map_pool.Recycle();//回收对象
                    choose_unit = hit.transform.gameObject;
                    choose_unit.GetComponent<Unit_Info>().BFS();
                    Unit_index = choose_unit.GetComponent<Unit_Info>().unity_position_index;
                    revocation_Button.transform.gameObject.SetActive(false);
                }
                if (hit.transform.tag == "Untagged")
                {
                    revocation_Button.transform.gameObject.SetActive(false);
                }
                if (hit.transform.tag == "Map")
                {
                    target_index = hit.transform.GetComponent<Hex_Info>().index;
                    choose_unit.GetComponent<Unit_Info>().Move(target_index);
                }
            }
        }
#endif

#if UNITY_ANDROID
        if (Input.touches[0].phase == TouchPhase.Began&& EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) == false)//在开始触摸时要更新固定点(首次触摸位置)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Unit" && Unit_index != hit.transform.GetComponent<Unit_Info>().unity_position_index)
                {
                    Map_Pool.Map_pool.Recycle();//回收对象
                    choose_unit = hit.transform.gameObject;
                    choose_unit.GetComponent<Unit_Info>().BFS();
                    Unit_index = choose_unit.GetComponent<Unit_Info>().unity_position_index;
                    revocation_Button.transform.gameObject.SetActive(false);
                }
                if (hit.transform.tag == "Untagged")
                {
                    revocation_Button.transform.gameObject.SetActive(false);
                }
                if (hit.transform.tag == "Map")
                {
                    target_index = hit.transform.GetComponent<Hex_Info>().index;
                    choose_unit.GetComponent<Unit_Info>().Move(target_index);
                }
            }
        }
#endif
    }
}
