using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HouseRoomLoader : MonoBehaviour
{
    public static HouseRoomLoader instance;

    public GameObject roomPrefab;
    [HideInInspector]
    public GameObject room;

    public GameObject roomUI;

    Light[] lights;

    void Awake()
    {
        instance = this;
        roomUI.SetActive(false);
    }

    public void LoadRoom(bool getLights)
    {
        if (getLights)
        {
            lights = FindObjectsOfType<Light>();
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].enabled = false;
            }
        }

        room = Instantiate(roomPrefab, new Vector3(10000, 0, 10000), Quaternion.identity);
        roomUI.SetActive(true);

        GetComponent<HouseCam>().FindCam();
    }

    public void UnloadRoom(bool enteringMod)
    {
        if (!enteringMod)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].enabled = true;
            }
            lights = null;
        }

        roomUI.SetActive(false);
        Destroy(room);
        room = null;
    }

    public void EnableLights()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].enabled = true;
        }
        lights = null;
    }
}
