using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public GameObject mainMenu;
    public GameObject colourMenu;
    public GameObject partMenu;
    public GameObject saveMenu;
    public GameObject helpMenu;

    void Awake()
    {
        instance = this;
    }

    public void SetMenuActive(GameObject menu)
    {
        DisableAllMenus();

        menu.SetActive(true);
    }

    public void ReturnMainMenu()
    {
        SetMenuActive(mainMenu);
    }

    public void DisableAllMenus()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public bool IsMenuActive(GameObject menu)
    {
        return menu.activeSelf;
    }
}
