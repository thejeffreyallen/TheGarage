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
    public Mesh newSpokes;
    public Mesh newNipples;
    private Transform[] stockFrontWheel;
    private Transform[] stockRearWheel;
    public Material MTBWheelMat;
    public Material[] tireMats;
    private Mesh oldRim;
    private Mesh oldTire;
    private Mesh oldTireThin;
    private Mesh oldTireFat;
    private Mesh oldHub;
    private Mesh oldSpokes;
    private Mesh oldNipples;
    private Material oldRimMat;
    private Material oldHubMat;
    private Material[] oldTireMats;

    private Transform frameJoint;
    private Transform barsJoint;
    private Transform driveTrain;
    private float initFramePos;
    private float initBarsPos;
    private float initDrivePos;

    private Transform frontWheel;
    private Transform rearWheel;

    private GameObject spacers;

    private GameObject frontWheelCol;
    private GameObject rearWheelCol;

    private Mesh originalWheelCol;


    private void Start()
    {

        stockFrontWheel = GameObject.Find("BMX:Wheel").GetComponentsInChildren<Transform>();
        stockRearWheel = GameObject.Find("BMX:Wheel 1").GetComponentsInChildren<Transform>();
        //Store old parts
        oldRim = stockFrontWheel[1].gameObject.GetComponent<MeshFilter>().mesh;
        oldHub = stockFrontWheel[3].gameObject.GetComponent<MeshFilter>().mesh;
        oldTire = stockFrontWheel[6].gameObject.GetComponent<MeshBlendShape>().referenceMesh;
        oldTireThin = stockFrontWheel[6].gameObject.GetComponent<MeshBlendShape>().shape01;
        oldTireFat = stockFrontWheel[6].gameObject.GetComponent<MeshBlendShape>().shape02;
        oldSpokes = stockFrontWheel[4].gameObject.GetComponent<MeshFilter>().mesh;
        oldNipples = stockFrontWheel[2].gameObject.GetComponent<MeshFilter>().mesh;
        //Store old materials
        oldRimMat = stockFrontWheel[1].gameObject.GetComponent<Renderer>().material;
        oldHubMat = stockFrontWheel[3].gameObject.GetComponent<Renderer>().material;
        oldTireMats = stockFrontWheel[6].gameObject.GetComponent<Renderer>().materials;

        frameJoint = GameObject.Find("BMX:Frame_Joint").transform;
        barsJoint = GameObject.Find("BMX:Bars_Joint").GetComponent<Transform>();
        driveTrain = GameObject.Find("BMX:DriveTrain_Joint").transform;
        initFramePos = frameJoint.localPosition.y;
        initBarsPos = barsJoint.localPosition.y;
        initDrivePos = driveTrain.localPosition.y;

        frontWheel = GameObject.Find("BMX:Wheel").GetComponent<Transform>();
        rearWheel = GameObject.Find("BMX:Wheel 1").GetComponent<Transform>();

        spacers = GameObject.Find("Headset Spacers Mesh");

        frontWheelCol = GameObject.Find("FrontWheelMeshCollider");
        rearWheelCol = GameObject.Find("BackWheelMeshCollider");

        originalWheelCol = GameObject.Find("FrontWheelMeshCollider").GetComponent<MeshFilter>().mesh;

       

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
        //frameJoint.localPosition = new Vector3(frameJoint.localPosition.x, initFramePos+0.124556f, frameJoint.localPosition.z);
        //barsJoint.localPosition = new Vector3(barsJoint.localPosition.x, initBarsPos+0.124556f, barsJoint.localPosition.z);
        //driveTrain.localPosition = new Vector3(driveTrain.localPosition.x, initDrivePos - 0.124556f, driveTrain.localPosition.z);

        barsJoint.Rotate(-7.3f, 0, 0);

        List<GameObject> partObjects = new List<GameObject>();

        foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
        {
            if (go.name == "Pegs Mesh")
                partObjects.Add(go);
        }

        partObjects[0].transform.SetParent(frontWheel);
        partObjects[1].transform.SetParent(rearWheel);

        frontWheel.localPosition = new Vector3(frontWheel.localPosition.x, frontWheel.localPosition.y + 0.028f, frontWheel.localPosition.z + 0.1f);
        rearWheel.localPosition = new Vector3(rearWheel.localPosition.x, rearWheel.localPosition.y + 0.028f, rearWheel.localPosition.z - 0.019f);

        spacers.SetActive(false);


        SwapMesh("Bars Mesh", mtbMeshes[0]);
        SwapMesh("Chain Mesh", mtbMeshes[1]);
        SwapMesh("Forks Mesh", mtbMeshes[2]);
        SwapMesh("Frame Mesh", mtbMeshes[3]);
        SwapMesh("Headset Mesh", mtbMeshes[4]);
        SwapMesh("Right Crank Arm Mesh", mtbMeshes[5]);
        SwapMesh("Left Crank Arm Mesh", mtbMeshes[5]);
        SwapMesh("Sprocket Mesh", mtbMeshes[6]);

        SwapMesh("Stem Bolts Mesh", mtbMeshes[8]);
        SwapMesh("Stem Mesh", mtbMeshes[9]);
        SwapMesh("BB Mesh", mtbMeshes[10]);
        SetWheels();
        frontWheelCol.GetComponent<MeshFilter>().mesh = newTire;
        rearWheelCol.GetComponent<MeshFilter>().mesh = newTire;
    }

    void DisableMTB()
    {
        //frameJoint.localPosition = new Vector3(frameJoint.localPosition.x, initFramePos, frameJoint.localPosition.z);
        //barsJoint.localPosition = new Vector3(barsJoint.localPosition.x, initBarsPos, barsJoint.localPosition.z);
        //driveTrain.localPosition = new Vector3(driveTrain.localPosition.x, initDrivePos, driveTrain.localPosition.z);

        barsJoint.Rotate(7.3f, 0, 0);

        frontWheel.localPosition = new Vector3(frontWheel.localPosition.x, frontWheel.localPosition.y - 0.028f, frontWheel.localPosition.z - 0.1f);
        rearWheel.localPosition = new Vector3(rearWheel.localPosition.x, rearWheel.localPosition.y - 0.028f, rearWheel.localPosition.z + 0.019f);

        spacers.SetActive(true);

        SwapMesh("Bars Mesh", defaultMeshes[0]);
        SwapMesh("Chain Mesh", defaultMeshes[1]);
        SwapMesh("Forks Mesh", defaultMeshes[2]);
        SwapMesh("Frame Mesh", defaultMeshes[3]);
        SwapMesh("Headset Mesh", defaultMeshes[4]);
        SwapMesh("Right Crank Arm Mesh", defaultMeshes[5]);
        SwapMesh("Left Crank Arm Mesh", defaultMeshes[6]);
        SwapMesh("Sprocket Mesh", defaultMeshes[7]);

        SwapMesh("Stem Bolts Mesh", defaultMeshes[9]);
        SwapMesh("Stem Mesh", defaultMeshes[10]);
        SwapMesh("BB Mesh", defaultMeshes[11]);
        DefaultWheels();
        frontWheelCol.GetComponent<MeshFilter>().mesh = originalWheelCol;
        rearWheelCol.GetComponent<MeshFilter>().mesh = originalWheelCol;
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
        //stockFrontWheel[1].gameObject.GetComponent<Renderer>().material = MTBWheelMat;

        //Front Wheel hub
        stockFrontWheel[3].gameObject.GetComponent<MeshFilter>().mesh = newFrontHub;
        //stockFrontWheel[3].gameObject.GetComponent<Renderer>().material = MTBWheelMat;

        //Front wheel tire
        stockFrontWheel[6].gameObject.GetComponent<MeshFilter>().mesh = newTire;
        //stockFrontWheel[6].gameObject.GetComponent<MeshBlendShape>().shape01 = newTire;
        //stockFrontWheel[6].gameObject.GetComponent<MeshBlendShape>().shape02 = newTire;
        //stockFrontWheel[6].gameObject.GetComponent<Renderer>().materials = tireMats;

        //Front wheel spokes
        stockFrontWheel[4].gameObject.GetComponent<MeshFilter>().mesh = newSpokes;

        //Front wheel nipples
        stockFrontWheel[2].gameObject.GetComponent<MeshFilter>().mesh = newNipples;

        //Rear wheel rim
        stockRearWheel[1].gameObject.GetComponent<MeshFilter>().mesh = newRim;
        //stockRearWheel[1].gameObject.GetComponent<Renderer>().material = MTBWheelMat;

        //Rear wheel tire
        stockRearWheel[3].gameObject.GetComponent<MeshFilter>().mesh = newTire;
        //stockRearWheel[3].gameObject.GetComponent<MeshBlendShape>().shape01 = newTire;
        //stockRearWheel[3].gameObject.GetComponent<MeshBlendShape>().shape02 = newTire;
        //stockRearWheel[3].gameObject.GetComponent<Renderer>().materials = tireMats;

        //rear wheel hub
        stockRearWheel[4].gameObject.GetComponent<MeshFilter>().mesh = newRearHub;
        //stockRearWheel[4].gameObject.GetComponent<Renderer>().material = MTBWheelMat;

        //rear wheel spokes
        stockRearWheel[6].gameObject.GetComponent<MeshFilter>().mesh = newSpokes;

        //rear wheel nipples
        stockRearWheel[2].gameObject.GetComponent<MeshFilter>().mesh = newNipples;

        this.modEnabled = true;
        PartManager.instance.tiresCount = 4;

    }

    public void DefaultWheels()
    {
        

        //Front wheel rim
        stockFrontWheel[1].gameObject.GetComponent<MeshFilter>().mesh = oldRim;
        //stockFrontWheel[1].gameObject.GetComponent<Renderer>().material = oldRimMat;
        //Front Wheel hub
        stockFrontWheel[3].gameObject.GetComponent<MeshFilter>().mesh = oldHub;
        //stockFrontWheel[3].gameObject.GetComponent<Renderer>().material = oldHubMat;
        //Front wheel tire
        stockFrontWheel[6].gameObject.GetComponent<MeshFilter>().mesh = oldTire;
        //stockFrontWheel[6].gameObject.GetComponent<MeshBlendShape>().shape01 = oldTireThin;
        //stockFrontWheel[6].gameObject.GetComponent<MeshBlendShape>().shape02 = oldTireFat;
        //stockFrontWheel[6].gameObject.GetComponent<Renderer>().materials = oldTireMats;

        //Front wheel spokes
        stockFrontWheel[4].gameObject.GetComponent<MeshFilter>().mesh = oldSpokes;

        //Front wheel nipples
        stockFrontWheel[2].gameObject.GetComponent<MeshFilter>().mesh = oldNipples;

        //Rear wheel rim
        stockRearWheel[1].gameObject.GetComponent<MeshFilter>().mesh = oldRim;
        //stockRearWheel[1].gameObject.GetComponent<Renderer>().material = oldRimMat;
        //Rear wheel tire
        stockRearWheel[3].gameObject.GetComponent<MeshFilter>().mesh = oldTire;
        //stockRearWheel[3].gameObject.GetComponent<MeshBlendShape>().shape01 = oldTireThin;
        //stockRearWheel[3].gameObject.GetComponent<MeshBlendShape>().shape02 = oldTireFat;
        //stockRearWheel[3].gameObject.GetComponent<Renderer>().materials = oldTireMats;
        //rear wheel hub
        stockRearWheel[4].gameObject.GetComponent<MeshFilter>().mesh = oldHub;
        //stockRearWheel[4].gameObject.GetComponent<Renderer>().material = oldHubMat;

        //rear wheel spokes
        stockRearWheel[6].gameObject.GetComponent<MeshFilter>().mesh = oldSpokes;

        //rear wheel nipples
        stockRearWheel[2].gameObject.GetComponent<MeshFilter>().mesh = oldNipples;

        this.modEnabled = false;
    }
}
