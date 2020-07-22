using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    public static HouseManager instance;

    bool isOpen;

    void Awake()
    {
        instance = this;

        GetComponent<BundleLoader>().LoadContent();

        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4) && !isOpen)
            SetHouseOpen(true, false, true);

      /*  if (Input.GetKeyDown(KeyCode.F5) && isOpen)
            transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);*/
    }

    public void SetHouseOpen(bool open, bool enteringMod, bool getLights)
    {
        isOpen = open;

        if (isOpen)
            HouseRoomLoader.instance.LoadRoom(getLights);
        else
        {
            HouseRoomLoader.instance.UnloadRoom(enteringMod);
        }
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}
