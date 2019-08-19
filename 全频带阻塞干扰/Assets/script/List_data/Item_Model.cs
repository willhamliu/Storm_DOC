using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Item_Model : MonoBehaviour
{
    public Dictionary<int, GameObject> Models = new Dictionary<int, GameObject>();
    public Transform Model_create;
    string File_name;
    static AssetBundle load_model;

    public static Item_Model Item_model;
    private int index;

    public static bool Notload = true;

    void Awake()
    {
        Item_model = this;
        if (Notload == true)
        {
            AB_path();
            Notload = false;
        }
    }

    public void AB_path()
    {
#if UNITY_EDITOR_WIN
        load_model = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/StreamingAssets/AssetBundles/ui/model.unity3d"));
#endif
#if UNITY_ANDROID
        load_model = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "!assets/AssetBundles/ui/model.unity3d"));
#endif
    }

    public void Model_display(int lastindex, int index, string name)//显示指定模型
    {
        if (Models.ContainsKey(index))
        {
            Models[lastindex].SetActive(false);
            Models[index].SetActive(true);
        }
        else
        {
            GameObject model_prefab = load_model.LoadAsset<GameObject>(name);
            GameObject Model = Instantiate(model_prefab, Model_create);
            Models.Add(index, Model);

            Models[lastindex].SetActive(false);
            Model.SetActive(true);
        }
        this.index = index;
    }
    public void Close_list()
    {
        Models[index].SetActive(false);
    }
}
