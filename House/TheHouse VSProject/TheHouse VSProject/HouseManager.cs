using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

public class HouseManager : MonoBehaviour
{
    public static HouseManager instance;
    public string saveErrors;

    bool isOpen;

    void Awake()
    {
        instance = this;

        GetComponent<BundleLoader>().LoadContent();

        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        try
        {

            if (Input.GetKeyDown(KeyCode.F4) && !isOpen)
            {
                SetHouseOpen(true, false, true);

                /*  if (Input.GetKeyDown(KeyCode.F5) && isOpen)
                      transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);*/
            }
        }
        catch (Exception ex)
        {
            this.saveErrors += this.saveErrors + ex.Message + "\n " + ex.StackTrace + "\n ";
        }
        File.AppendAllText(Application.dataPath + "//TheHouseErrorLog.txt", "\n" + DateTime.Now + "\nERRORS: " + this.saveErrors);
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
