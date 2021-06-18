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

    public void SetMenuInactive(GameObject menu) {

        menu.SetActive(false);
    }

    public void ReturnMainMenu()
    {
        SetMenuActive(mainMenu);
    }

    public void DisableAllMenus()
    {
        try
        {
            foreach(Transform t in transform)
            {
                t.gameObject.SetActive(false);
            }
        }
        catch (Exception e) {
            Debug.Log("Error in DisableAllMenus() " + e.Message + e.StackTrace);
        }
    }

    public bool IsMenuActive(GameObject menu)
    {
        return menu.activeSelf;
    }
}
