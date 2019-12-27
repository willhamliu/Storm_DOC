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


    void Awake()
    {
        if (Unit_management==false)
        {
            Unit_management = this;
        }
    }

    private void Start()
    {
        revocation_Button.onClick.AddListener(Revocation);

    }
    private void Revocation()
    {
        choose_unit.GetComponent<Unit_Info>().Revocation();
    }

    void Update()
    {
        Unit_selected();
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
                if (hit.transform.tag=="Unit")
                {
                    hit.transform.GetComponent<Unit_Info>().BFS();
                    choose_unit = hit.transform.gameObject;
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
               
            }
        }
#endif
    }
}
