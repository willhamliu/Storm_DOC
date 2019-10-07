using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateAssetsBundle
{
   [MenuItem("AssetBundle/Buile AssetBundle")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "Assets/StreamingAssets/AssetBundles";
        if (Directory.Exists(assetBundleDirectory))
        {
            Directory.Delete(assetBundleDirectory, true);//为了保证资源为最新,所以每次加载需要删除旧的信息
        }

        Directory.CreateDirectory(assetBundleDirectory);
        AssetDatabase.Refresh();//刷新刚删除了旧资源的文件夹


#if UNITY_EDITOR_WIN
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
#endif
#if UNITY_ANDROID
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.Android);
#endif
    }
}
