using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PresetLister : MonoBehaviour
{
    string[] presets;

    public GameObject buttonPrefab;
    public Transform buttonParent;

    void OnEnable()
    {
        CheckFolder();
    }

    void CheckFolder()
    {
        string path = Path.Combine(Application.dataPath, "GarageSaves");
        presets = Directory.GetFiles(path);

        //Remove Missing Buttons
        if (buttonParent.childCount > 0)
        {
            for (int i = 0; i < buttonParent.childCount; i++)
            {
                Destroy(buttonParent.GetChild(i).gameObject);
            }
        }

        //Create buttons
        for (int i = 0; i < presets.Length; i++)
        {
            if (presets[i].Contains(".preset"))
            {
                GameObject button = Instantiate(buttonPrefab, buttonParent);
                button.transform.position = new Vector2(buttonParent.position.x, buttonParent.position.y + (350 - (35 * i)));


                string fileName = Path.GetFileName(presets[i]).Replace(".preset", "");
                button.transform.GetChild(0).GetComponent<Text>().text = fileName;
                button.GetComponent<Button>().onClick.AddListener(delegate { SavingManager.instance.Load(fileName); });
            }
        }
    }
}
