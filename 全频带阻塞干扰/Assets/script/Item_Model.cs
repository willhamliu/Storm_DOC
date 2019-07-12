using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Item_Model : MonoBehaviour
{
    private List<GameObject> Models;
    public Transform Model_create;
    string File_name;
    AssetBundle load_model;

    public static Item_Model item_Model;
    public int index;

    void Awake()
    {
        item_Model = this;
        Models = new List<GameObject>();
        AB_path();
    }

    public void AB_path()
    {
        load_model = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/AssetBundles/ui/model.unity3d"));
    }


    public void Load_AB(Item item)
    {
        this.File_name = item.Item_Model;
        GameObject model_prefab = load_model.LoadAsset<GameObject>(File_name);
        GameObject Model = Instantiate(model_prefab, Model_create);
        Models.Add(Model);
    }
    public void Model_display(int lastindex ,int index)//显示指定模型
    {
        Models[lastindex].SetActive(false);
        Models[index].SetActive(true);
        this.index = index;
    }
    public void Close_list()
    {
        Models[index].SetActive(false);
    }
}
