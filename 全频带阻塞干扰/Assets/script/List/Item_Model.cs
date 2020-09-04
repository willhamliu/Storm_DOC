using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
/// <summary>
/// 管理所有单位模型的加载与显示
/// </summary>
public class Item_Model:InstanceNull<Item_Model>
{
    Dictionary<string, Mesh> models = new Dictionary<string, Mesh>();
    AssetBundle load_Model;
    
    public void LoadModel()//加载模型
    {
#if UNITY_STANDALONE
        load_Model = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/StreamingAssets/AssetBundles/ui/model.unity3d"));
#endif
#if UNITY_ANDROID
        load_Model = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "!assets/AssetBundles/ui/model.unity3d"));
#endif
    }

    /// <summary>
    /// 显示指定模型
    /// </summary>
    public Mesh Model_display(string name)
    {
        if (models.ContainsKey(name))
        {
            return models[name];
        }
        else
        {
            GameObject model = load_Model.LoadAsset<GameObject>(name);
            var model_mesh = model.GetComponent<MeshFilter>().sharedMesh;
            models.Add(name, model_mesh);
            return model_mesh;
        }
    }
}
