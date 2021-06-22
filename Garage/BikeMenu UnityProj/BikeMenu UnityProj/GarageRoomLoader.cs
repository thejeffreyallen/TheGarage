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
    Camera newCam;

    Transform marker;
    Renderer[] markerRend;
    Renderer[] playerRends;
    private Vector3 bikePlacement;

    void Start()
    {
        instance = this;
    }

    public void LoadRoom()
    {
        try
        {
            mainCam = Camera.main;

            room = Instantiate(roomPrefab, new Vector3(10000, 10000, 10000), Quaternion.identity);
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

            if (CustomMeshManager.instance.selectedFrame == 0)
                CustomMeshManager.instance.SetFrameMesh(0);
            if (CustomMeshManager.instance.selectedBars == 0)
                CustomMeshManager.instance.SetBarsMesh(0);
            if (CustomMeshManager.instance.selectedForks == 0)
                CustomMeshManager.instance.SetForksMesh(0);



            mainCam.enabled = false;

            if (newCam == null)
            {
                newCam = FindObjectOfType<Camera>();
            }
            else {
                newCam.gameObject.SetActive(true);
            }


           FindObjectOfType<DrivableVehicle>().vehiclePhysicsBody.GetComponent<Rigidbody>().isKinematic = true;
           
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

        mainCam.enabled = true;
        newCam.gameObject.SetActive(false);
        FindObjectOfType<SessionMarker>().SetMarker(previousMarkerPos, previousMarkerRot);
        foreach (Renderer r in playerRends)
        {
            r.enabled = true;
        }
        FindObjectOfType<SessionMarker>().ResetPlayerAtMarker();
        Destroy(room);
    }
}
