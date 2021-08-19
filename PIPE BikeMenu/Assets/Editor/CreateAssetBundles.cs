﻿using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "G:/PIPE/3099592/PIPE_Data/";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.StandaloneWindows);
        string[] files = Directory.GetFiles(assetBundleDirectory, "*.manifest");

        foreach (string file in files)
        {
            try
            {
                File.Delete(file); 
            }
            catch{
                
            }
        }
        File.Delete("G:/PIPE/3099592/PIPE_Data/PIPE_Data");
    }
}
