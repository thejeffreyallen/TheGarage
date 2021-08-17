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
    string path;
    string[] presets;
    string empty;

    public GameObject buttonPrefab;
    public Transform buttonParent;
    public RectTransform rt;

    void OnEnable()
    {
        CheckFolder();
    }

    public void CheckFolder()
    {
        path = Application.dataPath + "//GarageContent/GarageSaves/";
        presets = Directory.GetFiles(path);
        Debug.Log("Checking save folder for button creation..");
        //Remove Missing Buttons
        if (buttonParent.childCount > 0)
        {
            for (int i = 0; i < buttonParent.childCount; i++)
            {
                Destroy(buttonParent.GetChild(i).gameObject);
            }
        }
        //rt.sizeDelta = new Vector2(rt.sizeDelta.x, 50 * presets.Length);
        //Create buttons
        for (int i = 0; i < presets.Length; i++)
        {
            if (presets[i].Contains(".preset"))
            {
                GameObject button = Instantiate(buttonPrefab, buttonParent);
               // button.transform.position = new Vector2(buttonParent.position.x, buttonParent.position.y + (580 - (50 * i)));

                string fileName = Path.GetFileName(presets[i]).Replace(".preset", "");
                button.transform.GetChild(0).GetComponent<Text>().text = fileName;
                button.GetComponent<Button>().onClick.AddListener(delegate { SavingManager.instance.Load(fileName); });
            }
        }
        
    }
}
