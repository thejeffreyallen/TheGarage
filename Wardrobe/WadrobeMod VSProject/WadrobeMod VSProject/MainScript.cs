using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WadrobeMainScript : MonoBehaviour
{
    public GameObject child;

    bool isOpen;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        child.SetActive(isOpen);
    }

    void Update()
    {
       /* if (Input.GetKeyDown(KeyCode.F6))
            transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);*/
    }

    public void SetOpen()
    {
        isOpen = !isOpen;

        child.SetActive(isOpen);

        if (isOpen)
        {
            WadrobeRoomLoader.instance.LoadRoom();
        }
        else
        {
            TextureSaving.instance.SaveSelectedTextures();
            WadrobeRoomLoader.instance.UnloadRoom();
        }
    }
}
