using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System.Windows.Forms;

public class GarageRoomLoader : MonoBehaviour
{
    public static GarageRoomLoader instance;

    public GameObject roomPrefab;
    GameObject room;

    GameObject player;
    Vector3 previousMarkerPos;
    Quaternion previousMarkerRot;

    Camera mainCam;

    Transform marker;
    Renderer markerRend;
    private Vector3 bikePlacement;

    Light[] mapLights;

    void Start()
    {
        instance = this;
        
      
    }

    public void LoadRoom()
    {
        mainCam = Camera.main;

        mapLights = FindObjectsOfType<Light>();
        for (int i = 0; i < mapLights.Length; i++)
        {
            mapLights[i].enabled = false;
        }

        room = Instantiate(roomPrefab, new Vector3(10000, 10000, 10000), Quaternion.identity);
        bikePlacement = room.transform.position;
       // bikePlacement.y -= 0.3f;

        player = GameObject.Find("Daryien").transform.parent.gameObject;

        marker = FindObjectOfType<SessionMarker>().marker;
        markerRend = marker.GetChild(0).GetChild(0).GetComponent<Renderer>();
        markerRend.enabled = false;
        previousMarkerPos = marker.position;
        previousMarkerRot = marker.rotation;
        FindObjectOfType<SessionMarker>().SetMarker(bikePlacement, Quaternion.identity);
        FindObjectOfType<SessionMarker>().ResetPlayerAtMarker();

        player.SetActive(false);

        mainCam.gameObject.SetActive(false);
        Camera newCam = FindObjectOfType<Camera>();
        newCam.gameObject.AddComponent<PostProcessVolume>();
        newCam.gameObject.GetComponent<PostProcessVolume>().sharedProfile = mainCam.GetComponent<PostProcessVolume>().sharedProfile;

        FindObjectOfType<DrivableVehicle>().vehiclePhysicsBody.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void DestroyRoom()
    {
       /* if (mapLights != null)
        {
            for (int i = 0; i < mapLights.Length; i++)
            {
                mapLights[i].enabled = true;
            }
        }*/

        FindObjectOfType<DrivableVehicle>().vehiclePhysicsBody.GetComponent<Rigidbody>().isKinematic = false;

        markerRend.enabled = true;

        mainCam.gameObject.SetActive(true);

        FindObjectOfType<SessionMarker>().SetMarker(previousMarkerPos, previousMarkerRot);
        player.SetActive(true);
        FindObjectOfType<SessionMarker>().ResetPlayerAtMarker();
        Destroy(room);
    }
}
