using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MTBSetter : MonoBehaviour
{
    Transform bike;

    bool active = false;

    List<Mesh> defaultMeshes = new List<Mesh>();
    public List<Mesh> mtbMeshes;

    bool modEnabled;

    public Mesh newRim;
    public Mesh newTire;
    public Mesh newFrontHub;
    public Mesh newRearHub;
    private Transform[] stockFrontWheel;
    private Transform[] stockRearWheel;
    public Material MTBWheelMat;
    public Material[] tireMats;
    private Mesh oldRim;
    private Mesh oldTire;
    private Mesh oldHub;
    private Material oldRimMat;
    private Material oldHubMat;
    private Material[] oldTireMats;


    private void Start()
    {
        stockFrontWheel = GameObject.Find("BMX:Wheel").GetComponentsInChildren<Transform>();
        stockRearWheel = GameObject.Find("BMX:Wheel 1").GetComponentsInChildren<Transform>();
        //Store old parts
        oldRim = stockFrontWheel[1].gameObject.GetComponent<MeshFilter>().mesh;
        oldHub = stockFrontWheel[3].gameObject.GetComponent<MeshFilter>().mesh;
        oldTire = stockFrontWheel[6].gameObject.GetComponent<MeshFilter>().mesh;
        //Store old materials
        oldRimMat = stockFrontWheel[1].gameObject.GetComponent<Renderer>().material;
        oldHubMat = stockFrontWheel[3].gameObject.GetComponent<Renderer>().material;
        oldTireMats = stockFrontWheel[6].gameObject.GetComponent<Renderer>().materials;
    }

    public void SetMTB()
    {
        active = !active;

        if (active)
            EnableMTB();
        else
            DisableMTB();
    }

    void EnableMTB()
    {
        SwapMesh("Bars Mesh", mtbMeshes[0]);
        SwapMesh("Chain Mesh", mtbMeshes[1]);
        SwapMesh("Forks Mesh", mtbMeshes[2]);
        SwapMesh("Frame Mesh", mtbMeshes[3]);
        SwapMesh("Headset Mesh", mtbMeshes[4]);
        SetWheels();
    }

    void DisableMTB()
    {
        SwapMesh("Bars Mesh", defaultMeshes[0]);
        SwapMesh("Chain Mesh", defaultMeshes[1]);
        SwapMesh("Forks Mesh", defaultMeshes[2]);
        SwapMesh("Frame Mesh", defaultMeshes[3]);
        SwapMesh("Headset Mesh", defaultMeshes[4]);
        DefaultWheels();
    }

    void SwapMesh(string partName, Mesh customMesh)
    {
        Mesh part = GameObject.Find(partName).GetComponent<MeshFilter>().mesh;
        defaultMeshes.Add(part);

        GameObject.Find(partName).GetComponent<MeshFilter>().mesh = customMesh;
    }

    void SwapMultipleMesh(string partName, Mesh customMesh)
    {

        List<GameObject> parts = new List<GameObject>();
        foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
        {
            if (go.name == partName)
                parts.Add(go);
        }
        for (int i = 0; i < parts.Count; i++)
        {
            defaultMeshes.Add(parts[i].GetComponent<MeshFilter>().mesh);
            parts[i].GetComponent<MeshFilter>().mesh = customMesh;
        }  
    }

    public void SetWheels()
    {


        //Front wheel rim
        stockFrontWheel[1].gameObject.GetComponent<MeshFilter>().mesh = newRim;
        stockFrontWheel[1].gameObject.GetComponent<Renderer>().material = MTBWheelMat;
        //Front Wheel hub
        stockFrontWheel[3].gameObject.GetComponent<MeshFilter>().mesh = newFrontHub;
        stockFrontWheel[3].gameObject.GetComponent<Renderer>().material = MTBWheelMat;
        //Front wheel tire
        stockFrontWheel[6].gameObject.GetComponent<MeshFilter>().mesh = newTire;
        stockFrontWheel[6].gameObject.GetComponent<Renderer>().materials = tireMats;

        //Rear wheel rim
        stockRearWheel[1].gameObject.GetComponent<MeshFilter>().mesh = newRim;
        stockRearWheel[1].gameObject.GetComponent<Renderer>().material = MTBWheelMat;
        //Rear wheel tire
        stockRearWheel[3].gameObject.GetComponent<MeshFilter>().mesh = newTire;
        stockRearWheel[3].gameObject.GetComponent<Renderer>().materials = tireMats;
        //rear wheel hub
        stockRearWheel[4].gameObject.GetComponent<MeshFilter>().mesh = newRearHub;
        stockRearWheel[4].gameObject.GetComponent<Renderer>().material = MTBWheelMat;
        this.modEnabled = true;
        PartManager.instance.tiresCount = 4;

    }

    public void DefaultWheels()
    {
        //Front wheel rim
        stockFrontWheel[1].gameObject.GetComponent<MeshFilter>().mesh = oldRim;
        stockFrontWheel[1].gameObject.GetComponent<Renderer>().material = oldRimMat;
        //Front Wheel hub
        stockFrontWheel[3].gameObject.GetComponent<MeshFilter>().mesh = oldHub;
        stockFrontWheel[3].gameObject.GetComponent<Renderer>().material = oldHubMat;
        //Front wheel tire
        stockFrontWheel[6].gameObject.GetComponent<MeshFilter>().mesh = oldTire;
        stockFrontWheel[6].gameObject.GetComponent<Renderer>().materials = oldTireMats;

        //Rear wheel rim
        stockRearWheel[1].gameObject.GetComponent<MeshFilter>().mesh = oldRim;
        stockRearWheel[1].gameObject.GetComponent<Renderer>().material = oldRimMat;
        //Rear wheel tire
        stockRearWheel[3].gameObject.GetComponent<MeshFilter>().mesh = oldTire;
        stockRearWheel[3].gameObject.GetComponent<Renderer>().materials = oldTireMats;
        //rear wheel hub
        stockRearWheel[4].gameObject.GetComponent<MeshFilter>().mesh = oldHub;
        stockRearWheel[4].gameObject.GetComponent<Renderer>().material = oldHubMat;

        this.modEnabled = false;
    }
}
