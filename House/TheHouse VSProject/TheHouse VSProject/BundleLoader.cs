using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Reflection;
using System.IO;

public class BundleLoader : MonoBehaviour
{
    string[] assetFiles;

    bool loadedBundles;

    void Start()
    {
        LoadContent();
    }

    void CheckFolder()
    {
        assetFiles = Directory.GetFiles(Path.Combine(Application.dataPath, "TheHouseContent"));
    }

    public void LoadContent()
    {
        CheckFolder();

        if (!loadedBundles)
        {
            for (int i = 0; i < assetFiles.Length; i++)
            {
                if (!Path.GetFileName(assetFiles[i]).Contains(".dll"))
                {
                    AssetBundle bundle = AssetBundle.LoadFromFile(assetFiles[i]);
                    Assembly.LoadFile(assetFiles[i] + ".dll");
                    Instantiate(bundle.LoadAllAssets()[0]);
                }
            }

            loadedBundles = true;
        }
    }
}
