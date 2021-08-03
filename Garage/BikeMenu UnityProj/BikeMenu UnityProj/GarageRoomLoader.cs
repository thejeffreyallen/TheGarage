using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class GarageRoomLoader : MonoBehaviour
{
    public static GarageRoomLoader instance;

    public GameObject roomPrefab;
    GameObject room;

    GameObject player;
    Vector3 previousMarkerPos;
    Quaternion previousMarkerRot;

    Camera mainCam;
    Camera dummyCam = new Camera();
    GameObject camScripts;

    Light[] lightsGo;

    Transform marker;
    Renderer[] markerRend;
    Renderer[] playerRends;
    public Material skybox;
    Material currentMapSky;
    private Vector3 bikePlacement;


    void Start()
    {
        instance = this;

        if(CustomMeshManager.instance.selectedFrame == 0)
            CustomMeshManager.instance.SetFrameMesh(0);
        if (CustomMeshManager.instance.selectedForks == 0)
            CustomMeshManager.instance.SetForksMesh(0);
        if (CustomMeshManager.instance.selectedBars == 0)
            CustomMeshManager.instance.SetBarsMesh(0);
        if (CustomMeshManager.instance.selectedPedals == 0)
            CustomMeshManager.instance.SetPedalsMesh(0);
        if (CustomMeshManager.instance.selectedFrontRim == 0)
            CustomMeshManager.instance.SetFrontRimMesh(0);
        if (CustomMeshManager.instance.selectedRearRim == 0)
            CustomMeshManager.instance.SetRearRimMesh(0);
    }

    public void LoadRoom()
    {
        try
        {
            mainCam = Camera.main;
            room = Instantiate(roomPrefab, new Vector3(UnityEngine.Random.Range(10000, 20000), UnityEngine.Random.Range(10000, 20000), UnityEngine.Random.Range(10000, 20000)), Quaternion.identity);
            bikePlacement = room.transform.position;

            player = GameObject.Find("Daryien").transform.parent.gameObject;

            playerRends = player.GetComponentsInChildren<Renderer>();
            
            foreach (Renderer r in playerRends)
            {
                r.enabled = false; // Fixes weird player twisting behaviour
            }

            marker = FindObjectOfType<SessionMarker>().marker;
            markerRend = marker.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in markerRend)
            {
                r.enabled = false;
            }
            previousMarkerPos = marker.position;
            previousMarkerRot = marker.rotation;

            FindObjectOfType<SessionMarker>().SetMarker(bikePlacement, Quaternion.identity);
            FindObjectOfType<SessionMarker>().ResetPlayerAtMarker();

            

            camScripts = GameObject.Find("BMX Camera");

            mainCam.gameObject.AddComponent<CamController>();
            camScripts.GetComponent<camFollow>().CenterCam();
            camScripts.GetComponent<camFollow>().StopAllCoroutines();

            

            FindObjectOfType<DrivableVehicle>().vehiclePhysicsBody.GetComponent<Rigidbody>().isKinematic = true;
            lightsGo = FindObjectsOfType<Light>() as Light[];
            foreach (Light thisLight in lightsGo)
            {
                if (!thisLight.gameObject.name.Equals("GarageLight")) {
                    thisLight.gameObject.SetActive(false);
                }
            }
            currentMapSky = RenderSettings.skybox;
            RenderSettings.skybox = skybox;
            RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Skybox;
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
            DynamicGI.UpdateEnvironment();
        }
        catch(System.ArgumentOutOfRangeException e) {
            Debug.Log(e.Message +" : "+ e.StackTrace + " : " + e.TargetSite);
        }
    }

    public void DestroyRoom()
    {

        FindObjectOfType<DrivableVehicle>().vehiclePhysicsBody.GetComponent<Rigidbody>().isKinematic = false;

        foreach (Renderer r in markerRend)
        {
            r.enabled = true;
        }
        FindObjectOfType<SessionMarker>().SetMarker(previousMarkerPos, previousMarkerRot);
        foreach (Renderer r in playerRends)
        {
            r.enabled = true;
        }
        FindObjectOfType<SessionMarker>().ResetPlayerAtMarker();
        foreach (Light thisLight in lightsGo)
        {
            thisLight.gameObject.SetActive(true);
        }
        Destroy(mainCam.GetComponent<CamController>());
        //camScripts.GetComponent<CamMaster>().cam = mainCam;
        //camScripts.GetComponent<camFollow>().cam = mainCam;
        Destroy(room);
        RenderSettings.skybox = currentMapSky;
        RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Skybox;
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
        DynamicGI.UpdateEnvironment();

    }
}
