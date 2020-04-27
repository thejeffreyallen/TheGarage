using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    bool isOpen;
    void Awake()
    {
        
        
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
            SetOpen();*/
    }

    public void SetOpen()
    {
        isOpen = !isOpen;        

        if (isOpen)
        {
            MenuManager.instance.SetMenuActive(MenuManager.instance.mainMenu);
            RoomLoader.instance.LoadRoom();
                
        }
        else
        {
            MenuManager.instance.DisableAllMenus();
            RoomLoader.instance.DestroyRoom();
        }
    }

    public void SetOpen(bool _isOpen)
    {
        isOpen = _isOpen;

        if (isOpen)
        {
            MenuManager.instance.SetMenuActive(MenuManager.instance.mainMenu);
            RoomLoader.instance.LoadRoom();
        }
        else
        {
            MenuManager.instance.DisableAllMenus();
            RoomLoader.instance.DestroyRoom();
        }
    }

    private IEnumerator LoadDefault()
    {
        yield return new WaitForSeconds(0.5f);
        SavingManager.instance.Load("default");
        yield break;
    }
}
