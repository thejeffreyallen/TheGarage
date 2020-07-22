using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WadrobeRoomLoader : MonoBehaviour
{
    public static WadrobeRoomLoader instance;

    public GameObject roomPrefab;
    GameObject room;

    Light[] lights;

    void Start()
    {
        instance = this;
    }

    public void LoadRoom()
    {
        room = Instantiate(roomPrefab, new Vector3(10000, 0, 10000), Quaternion.identity);
    }

    public void UnloadRoom()
    {
        Destroy(room);
        room = null;

        HouseManager.instance.SetHouseOpen(true, true, false);
    }
}
