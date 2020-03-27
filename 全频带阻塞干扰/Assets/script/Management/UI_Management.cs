using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 管理场景中的所有已生成的UI
/// </summary>
public class UI_Management : MonoBehaviour
{
    public static UI_Management instance;
    private Dictionary<string, List<UIBehaviour>> UIdict = new Dictionary<string, List<UIBehaviour>>();
    void Awake()
    {
        if (instance==null)
        {
            instance = this;
        }
        FindChildrenControl<Button>();
    }
    public T GetControl<T>(string controlName) where T : UIBehaviour//获取UI对象
    {
        if (UIdict.ContainsKey(controlName))
        {
            for (int i = 0; i < UIdict[controlName].Count; i++)
            {
                if (UIdict[controlName][i] is T)
                {
                    return UIdict[controlName][i] as T;
                }
            }
        }
        return null;
    }
   
    private void FindChildrenControl<T>() where T : UIBehaviour
    {
        T[] button = this.GetComponentsInChildren<T>(true);
        string objname;
        for (int i = 0; i < button.Length; i++)
        {
            objname = button[i].gameObject.name;
            if (UIdict.ContainsKey(objname))
            {
                UIdict[objname].Add(button[i]);

            }
            else
            {
                UIdict.Add(objname, new List<UIBehaviour>() { (button[i]) });
            }
        }
    }
}
