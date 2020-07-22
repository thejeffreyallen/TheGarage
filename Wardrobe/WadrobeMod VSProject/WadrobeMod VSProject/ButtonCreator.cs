using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ButtonCreator : MonoBehaviour
{
    public static ButtonCreator instance;

    public Transform[] listParents;
    public GameObject buttonPrefab;

    Image lastImg;

    void Start()
    {
        instance = this;
    }

    public void CreateButton(string url)
    {
        GameObject button = Instantiate(buttonPrefab, listParents[TextureSwapManager.instance.clothingDropdown.value]);
        button.transform.localPosition = new Vector3(0, 320 - ((listParents[TextureSwapManager.instance.clothingDropdown.value].childCount - 1) * 310), 0);

        button.GetComponent<Button>().onClick.AddListener(delegate { TextureSwapManager.instance.SetTexture(url); });

        string texturePath = Path.GetFileName(url);
        button.transform.GetChild(1).GetComponent<Text>().text = texturePath;

        Davinci.get().load(url).into(button.transform.GetChild(0).GetComponent<Image>()).start();
    }

    public void CreateButton(string url, int list)
    {
        GameObject button = Instantiate(buttonPrefab, listParents[list]);
        button.transform.localPosition = new Vector3(0, 320 - ((listParents[list].childCount - 1) * 310), 0);

        button.GetComponent<Button>().onClick.AddListener(delegate { TextureSwapManager.instance.SetTexture(url); });

        string texturePath = Path.GetFileName(url);
        button.transform.GetChild(1).GetComponent<Text>().text = texturePath;

        Davinci.get().load(url).into(button.transform.GetChild(0).GetComponent<Image>()).start();
    }
}
