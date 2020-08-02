using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;
    public bool isOpen;
    void Awake()
    {

        instance = this;
    }
    void Start()
    {
        MenuManager.instance.DisableAllMenus();

        DontDestroyOnLoad(this.gameObject);
        
    }

    void OnEnable()
    {
        StartCoroutine(LoadDefault());
    }

    void Update()
    {
        /*  if (MenuManager.instance == null)
               SetOpen();
           else if (!MenuManager.instance.IsMenuActive(MenuManager.instance.saveMenu))
               SetOpen();

        if (Input.GetKeyDown(KeyCode.F8))
            transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);*/
    }

    public void SetOpen()
    {
        isOpen = !isOpen;        

        if (isOpen)
        {
            MenuManager.instance.SetMenuActive(MenuManager.instance.mainMenu);
            GarageRoomLoader.instance.LoadRoom();
                
        }
        else
        {
            MenuManager.instance.DisableAllMenus();
            GarageRoomLoader.instance.DestroyRoom();
        }
    }

    public void SetOpen(bool _isOpen)
    {
        isOpen = _isOpen;

        if (isOpen)
        {
            MenuManager.instance.SetMenuActive(MenuManager.instance.mainMenu);
            GarageRoomLoader.instance.LoadRoom();
        }
        else
        {
            MenuManager.instance.DisableAllMenus();
            GarageRoomLoader.instance.DestroyRoom();
        }
    }

    private IEnumerator LoadDefault()
    {
        yield return new WaitForSeconds(0.5f);
        SavingManager.instance.Load("default");
        yield break;
    }
}
