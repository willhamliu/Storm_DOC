using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
/// <summary>
/// 管理所有单位模型的加载与显示
/// </summary>
public class Item_Model_Management : MonoBehaviour
{
    public static Item_Model_Management instance;

    public Dictionary<int, GameObject> models = new Dictionary<int, GameObject>();
    public Transform model_Create;
    static AssetBundle load_Model;

    private int index;
    public static bool notload = true;

    void Awake()
    {
        if (instance==null)
        {
            instance = this;
        }
        if (notload == true)
        {
            AB_path();
            notload = false;
        }
    }

    public void AB_path()
    {
#if UNITY_STANDALONE
        load_Model = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/StreamingAssets/AssetBundles/ui/model.unity3d"));
#endif
#if UNITY_ANDROID
        load_Model = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "!assets/AssetBundles/ui/model.unity3d"));
#endif
    }

    public void Model_display(int lastindex, int index, string name)//显示指定模型
    {
        if (models.ContainsKey(index))
        {
            models[lastindex].SetActive(false);
            models[index].SetActive(true);
        }
        else
        {
            GameObject model_prefab = load_Model.LoadAsset<GameObject>(name);
            GameObject Model = Instantiate(model_prefab, model_Create);
            models.Add(index, Model);

            models[lastindex].SetActive(false);
            Model.SetActive(true);
        }
        this.index = index;
    }
    public void Close_list()
    {
        models[index].SetActive(false);
    }
}
