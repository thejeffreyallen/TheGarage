/**
* Used for managing custom bike part meshes. 
*/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MeshObject
{
    public Mesh mesh;
    public bool isCustom;
    public string fileName;

    public MeshObject(Mesh mesh, bool isCustom, string fileName)
    {
        this.mesh = mesh;
        this.isCustom = isCustom;
        this.fileName = fileName;
    }
}

public class CustomMeshManager : MonoBehaviour
{
    public static CustomMeshManager instance;

    [Header("Frame")]
    public int selectedFrame;
    public Mesh[] frameMeshes;
    public Text selectedFrameText;

    [Header("Bars")]
    public int selectedBars;
    public Mesh[] barMeshes;
    public Text selectedBarsText;

    [Header("Pegs")]
    public int selectedFrontPegs;
    public int selectedRearPegs;
    public Mesh[] pegMeshes;
    public Text selectedFrontPegsText;
    public Text selectedRearPegsText;

    [Header("Spokes")]
    public int selectedFrontSpokes;
    public int selectedRearSpokes;
    public Mesh[] spokesMeshes;
    public Text selectedFrontSpokesText;
    public Text selectedRearSpokesText;

    [Header("Sprocket")]
    public int selectedSprocket;
    public Mesh[] sprocketMeshes;
    public Text selectedSprocketText;

    [Header("Stem")]
    public int selectedStem;
    public Mesh[] stemMeshes;
    public Text selectedStemText;

    [Header("Cranks")]
    public int selectedCranks;
    public Mesh[] cranksMeshes;
    public Text selectedCranksText;

    [Header("Forks")]
    public int selectedForks;
    public Mesh[] forksMeshes;
    public Text selectedForksText;

    [Header("Pedals")]
    public int selectedPedals;
    public Mesh[] pedalMeshes;
    public Text selectedPedalsText;

    [Header("FrontHubGuards")]
    public int selectedFrontHubGuards;
    public Mesh[] frontHubGuardMeshes;
    public Text selectedFrontHubGuardsText;

    [Header("RearHubGuards")]
    public int selectedRearHubGuards;
    public Mesh[] rearHubGuardMeshes;
    public Text selectedRearHubGuardsText;

    [Header("Stem Bolts")]
    public Mesh[] stemBoltMeshes;

    [Header("Crank Bolts")]
    public Mesh[] crankBoltMeshes;

    [Header("Hubs")]
    public int selectedFrontHub;
    public int selectedRearHub;
    public Mesh[] hubMeshes;
    public Text selectedFrontHubText;
    public Text selectedRearHubText;

    [Header("Spoke Accessories")]
    public int selectedFrontAccessory;
    public int selectedRearAccessory;
    public Mesh[] accessoryMeshes;
    public Text selectedFrontAccessoryText;
    public Text selectedRearAccessoryText;

    GameObject accFront;
    GameObject accRear;
    public Material tennis;
    GameObject backPegcollider;
    GameObject frontPegcollider;

    [Header("Seat")]
    public int selectedSeat;
    public Mesh[] seatMeshes;
    public Text selectedSeatText;

    private GameObject rightCrankBolts;
    private GameObject leftCrankBolts;
    private GameObject stemBolts;
    private String basePath;

    private ObjImporter objImporter;

    List<GameObject> origSpokes = new List<GameObject>();
    List<GameObject> origHubs = new List<GameObject>();
    List<GameObject> origPegs = new List<GameObject>();
    public GameObject origSeatPost;

    public Mesh seatPostClamp;
    public Mesh longSeatPost;

    public List<MeshObject> frames;
    public List<MeshObject> bars;
    public List<MeshObject> forks;
    public List<MeshObject> stems;
    public List<MeshObject> cranks;
    public List<MeshObject> sprockets;
    public List<MeshObject> spokes;
    public List<MeshObject> pegs;
    public List<MeshObject> pedals;
    public List<MeshObject> seats;
    public List<MeshObject> accessories;
    public List<MeshObject> hubs;
    public List<MeshObject> frontHubGuards;
    public List<MeshObject> rearHubGuards;
    public List<MeshObject> boltsCrank;
    public List<MeshObject> boltsStem;

    void Awake()
    {
        instance = this;
        InitMeshObjectLists();
    }

    void Start()
    {
        objImporter = new ObjImporter();
        basePath = Application.dataPath + "/GarageContent/";

        rightCrankBolts = PartMaster.instance.GetPart(PartMaster.instance.rightCrankBolt);
        leftCrankBolts = PartMaster.instance.GetPart(PartMaster.instance.leftCrankBolt);
        stemBolts = PartMaster.instance.GetPart(PartMaster.instance.stemBolts);
        rightCrankBolts.GetComponent<MeshFilter>().mesh = crankBoltMeshes[1];
        leftCrankBolts.GetComponent<MeshFilter>().mesh = crankBoltMeshes[1];
        GameObject SeatClampBolt = PartMaster.instance.GetPart(PartMaster.instance.seatClampBolt);
        SeatClampBolt.SetActive(false);
        PartMaster.instance.SetMesh(PartMaster.instance.seatClamp, seatPostClamp);
        PartMaster.instance.SetMesh(PartMaster.instance.seatPost, longSeatPost);
        LoadFiles(); // Load meshes from file
        

        
        accFront = new GameObject("FrontAccessory");
        accFront.AddComponent<MeshRenderer>();
        accFront.GetComponent<MeshRenderer>().material = tennis;
        accFront.AddComponent<MeshFilter>();

        accRear = new GameObject("RearAccessory");
        accRear.AddComponent<MeshRenderer>();
        accRear.GetComponent<MeshRenderer>().material = tennis;
        accRear.AddComponent<MeshFilter>();

        Instantiate(accFront, PartMaster.instance.GetPart(PartMaster.instance.frontSpokes).transform);
        Instantiate(accRear, PartMaster.instance.GetPart(PartMaster.instance.rearSpokes).transform);
        
    }

    public void InitMeshObjectLists()
    {
        try
        {
            frames = new List<MeshObject>();
            bars = new List<MeshObject>();
            forks = new List<MeshObject>();
            stems = new List<MeshObject>();
            cranks = new List<MeshObject>();
            sprockets = new List<MeshObject>();
            spokes = new List<MeshObject>();
            pegs = new List<MeshObject>();
            pedals = new List<MeshObject>();
            seats = new List<MeshObject>();
            accessories = new List<MeshObject>();
            hubs = new List<MeshObject>();
            frontHubGuards = new List<MeshObject>();
            rearHubGuards = new List<MeshObject>();
            boltsCrank = new List<MeshObject>();
            boltsStem = new List<MeshObject>();

            foreach (Mesh m in frameMeshes)
                frames.Add(new MeshObject(m, false, ""));
            foreach (Mesh m in barMeshes)
                bars.Add(new MeshObject(m, false, ""));
            foreach (Mesh m in forksMeshes)
                forks.Add(new MeshObject(m, false, ""));
            foreach (Mesh m in stemMeshes)
                stems.Add(new MeshObject(m, false, ""));
            foreach (Mesh m in cranksMeshes)
                cranks.Add(new MeshObject(m, false, ""));
            foreach (Mesh m in sprocketMeshes)
                sprockets.Add(new MeshObject(m, false, ""));
            foreach (Mesh m in spokesMeshes)
                spokes.Add(new MeshObject(m, false, ""));
            foreach (Mesh m in pegMeshes)
                pegs.Add(new MeshObject(m, false, ""));
            foreach (Mesh m in pedalMeshes)
                pedals.Add(new MeshObject(m, false, ""));
            foreach (Mesh m in accessoryMeshes)
                accessories.Add(new MeshObject(m, false, ""));
            foreach (Mesh m in hubMeshes)
                hubs.Add(new MeshObject(m, false, ""));
            foreach (Mesh m in frontHubGuardMeshes)
                frontHubGuards.Add(new MeshObject(m, false, ""));
            foreach (Mesh m in rearHubGuardMeshes)
                rearHubGuards.Add(new MeshObject(m, false, ""));
            foreach (Mesh m in crankBoltMeshes)
                boltsCrank.Add(new MeshObject(m, false, ""));
            foreach (Mesh m in stemBoltMeshes)
                boltsStem.Add(new MeshObject(m, false, ""));
            foreach (Mesh m in seatMeshes)
                seats.Add(new MeshObject(m, false, ""));
        }
        catch (Exception e)
        {
            Debug.Log("Error on initialization of CustomMeshManager: " + e.Message + e.StackTrace);
        }
    }

    public void LoadFiles()
    {
        try
        {
            frames = LoadFromFile("Frames/", frames);
            bars = LoadFromFile("Bars/", bars);
            pegs = LoadFromFile("Pegs/", pegs);
            spokes = LoadFromFile("Spokes/", spokes);
            sprockets = LoadFromFile("Sprockets/", sprockets);
            stems = LoadFromFile("Stems/", stems);
            cranks = LoadFromFile("Cranks/", cranks);
            forks = LoadFromFile("Forks/", forks);
            boltsStem = LoadFromFile("StemBolts/", boltsStem);
            boltsCrank = LoadFromFile("CrankBolts/", boltsCrank);
            hubs = LoadFromFile("Hubs/", hubs);
            seats = LoadFromFile("Seats/", seats);
        }
        catch (Exception e)
        {
            Debug.Log("Error while loading meshes from file: " + e.Message + "\n" + e.StackTrace);
        }
    }

    public void SetFrontSpokeAccMesh(int i)
    {    
        accFront.GetComponent<MeshFilter>().mesh = accessoryMeshes[i % accessoryMeshes.Length];
        selectedFrontAccessoryText.text = accessoryMeshes[i % accessoryMeshes.Length].name;
        selectedFrontAccessory = (i % accessoryMeshes.Length) + 1;
        
    }

    public void SetRearSpokeAccMesh(int i)
    {
        accRear.GetComponent<MeshFilter>().mesh = accessoryMeshes[i % accessoryMeshes.Length];
        selectedRearAccessoryText.text = accessoryMeshes[i % accessoryMeshes.Length].name;
        selectedRearAccessory = (i % accessoryMeshes.Length) + 1;
    }

    /// <summary>
    /// LoadFromFile method - Loads .obj files from a folder at runtime and integrate into the included meshes.
    /// </summary>
    /// <param name="folder"> The folder to load meshes from </param>
    /// <param name="fileNameArray"> An array to hold any file names that are found in the folder </param>
    /// <param name="origMeshArray"> The original mesh array to merge into </param>
    public List<MeshObject> LoadFromFile(String folder, List<MeshObject> meshObjectList)
    {
        try
        {
            String[] fileNameArray = Directory.GetFiles(basePath + folder); // Get the file names
            for (int i = 0; i < fileNameArray.Length; i++)
            {
                Mesh temp = objImporter.ImportFile(fileNameArray[i]); // Import each file name as a mesh
                temp.name = Path.GetFileName(fileNameArray[i]).Replace(".obj", ""); // Remove the .obj extension for cleaner look when updating the button text
                meshObjectList.Add(new MeshObject(temp, true, fileNameArray[i]));
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error in LoadFromFile method: " + e.Message + e.StackTrace);
        }
        return meshObjectList;

    }

    /// <summary>
    /// SetFrameMesh method - Change the bike frame mesh at runtime
    /// </summary>
    /// <param name="i"> the index of the mesh to change to </param>
    public void SetFrameMesh(int i)
    {
        GameObject partObject = PartMaster.instance.GetPart(PartMaster.instance.frame);
        if (i == frameMeshes.Length) {
            partObject.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        if (i % frames.Count == 0) {
            partObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        partObject.GetComponent<MeshFilter>().mesh = frames[i % frames.Count].mesh;
        selectedFrameText.text = frames[i % frames.Count].mesh.name;
        selectedFrame = (i % frames.Count) + 1;
    }

    /// <summary>
    /// SetForksMesh method - Change the bike forks mesh at runtime
    /// </summary>
    /// <param name="i"> the index of the mesh to change to </param>
    public void SetForksMesh(int i)
    {
        PartMaster.instance.SetMesh(PartMaster.instance.forks, forks[i % forks.Count].mesh);
        selectedForksText.text = forks[i % forks.Count].mesh.name;
        selectedForks = (i % forks.Count) + 1;
    }

    /// <summary>
    /// SetBarsMesh method - Change the bike bars mesh at runtime
    /// </summary>
    /// <param name="i"> the index of the mesh to change to </param>
    public void SetBarsMesh(int i)
    {
        PartMaster.instance.SetMesh(PartMaster.instance.bars, bars[i % bars.Count].mesh);
        selectedBarsText.text = bars[i % bars.Count].mesh.name;
        selectedBars = (i % bars.Count) + 1;
    }

    /// <summary>
    /// SetPedalsMesh method - Change the bike pedals mesh at runtime
    /// </summary>
    /// <param name="i"> the index of the mesh to change to </param>
    public void SetPedalsMesh(int i)
    {
        PartMaster.instance.SetMesh(PartMaster.instance.leftPedal, pedals[i % pedals.Count].mesh);
        PartMaster.instance.SetMesh(PartMaster.instance.rightPedal, pedals[i % pedals.Count].mesh);
        selectedPedalsText.text = pedals[i % pedals.Count].mesh.name;
        selectedPedals = (i % pedals.Count) + 1;
    }

    /// <summary>
    /// SetSprocketMesh method - Change the bike sprocket (Chainwheel) mesh at runtime
    /// </summary>
    /// <param name="i"> the index of the mesh to change to </param>
    public void SetSprocketMesh(int i)
    {

        PartMaster.instance.SetMesh(PartMaster.instance.sprocket, sprockets[i % sprockets.Count].mesh);
        selectedSprocketText.text = sprockets[i % sprockets.Count].mesh.name;
        selectedSprocket = (i % sprockets.Count) + 1;
    }

    /// <summary>
    /// SetStemMesh method - Change the bike stem mesh at runtime
    /// </summary>
    /// <param name="i"> the index of the mesh to change to </param>
    public void SetStemMesh(int i)
    {
        PartMaster.instance.SetMesh(PartMaster.instance.stem, stems[i % stems.Count].mesh);
        PartMaster.instance.SetMesh(PartMaster.instance.stemBolts, boltsStem[i % boltsStem.Count].mesh);
        selectedStemText.text = stems[i % stems.Count].mesh.name;
        selectedStem = (i % stems.Count) +1;
    }

    /// <summary>
    /// SetCranksMesh method - Change the bike cranks mesh at runtime
    /// </summary>
    /// <param name="i"> the index of the mesh to change to </param>
    public void SetCranksMesh(int i)
    {
        PartMaster.instance.SetMesh(PartMaster.instance.rightCrank, cranks[i % cranks.Count].mesh);
        PartMaster.instance.SetMesh(PartMaster.instance.leftCrank, cranks[i % cranks.Count].mesh);
        PartMaster.instance.SetMesh(PartMaster.instance.rightCrankBolt, boltsCrank[i % boltsCrank.Count].mesh);
        PartMaster.instance.SetMesh(PartMaster.instance.leftCrankBolt, boltsCrank[i % boltsCrank.Count].mesh);
        selectedCranksText.text = cranks[i % cranks.Count].mesh.name;
        selectedCranks = (i % cranks.Count) +1;
    }

    /// <summary>
    /// SetPegsMesh method - Change the bike pegs mesh at runtime
    /// </summary>
    /// <param name="j"> the index of the mesh to change to </param>
    public void SetFrontPegsMesh(int j)
    {
        //frontPegcollider.GetComponent<CapsuleCollider>().center = new Vector3(0.1f, 0f, 0f);
        //frontPegcollider.GetComponent<CapsuleCollider>().height = 0.25f;
        PartMaster.instance.SetMesh(PartMaster.instance.frontPegs, pegs[j % pegs.Count].mesh);
        selectedFrontPegsText.text = pegs[j % pegs.Count].mesh.name;
        selectedFrontPegs = (j % pegs.Count) +1;

    }

    /// <summary>
    /// SetPegsMesh method - Change the bike pegs mesh at runtime
    /// </summary>
    /// <param name="j"> the index of the mesh to change to </param>
    public void SetRearPegsMesh(int j)
    {

        //backPegcollider.GetComponent<CapsuleCollider>().center = new Vector3(0.1f, 0f, 0f);
        //backPegcollider.GetComponent<CapsuleCollider>().height = 0.25f;
        PartMaster.instance.SetMesh(PartMaster.instance.rearPegs, pegs[j % pegs.Count].mesh);
        selectedRearPegsText.text = pegs[j % pegs.Count].mesh.name;
        selectedRearPegs = (j % pegs.Count) + 1;
    }

    /// <summary>
    /// SetSpokesMesh method - Change the bike spokes mesh at runtime
    /// </summary>
    /// <param name="j"> the index of the mesh to change to </param>
    public void SetFrontSpokesMesh(int j)
    {
        PartMaster.instance.SetMesh(PartMaster.instance.frontSpokes, spokes[j % spokes.Count].mesh);
        selectedFrontSpokesText.text = spokes[j % spokes.Count].mesh.name;
        selectedFrontSpokes = (j % spokes.Count) +1;
    }

    /// <summary>
    /// SetSpokesMesh method - Change the bike spokes mesh at runtime
    /// </summary>
    /// <param name="j"> the index of the mesh to change to </param>
    public void SetRearSpokesMesh(int j)
    {
        PartMaster.instance.SetMesh(PartMaster.instance.rearSpokes, spokes[j % spokes.Count].mesh);
        selectedRearSpokesText.text = spokes[j % spokes.Count].mesh.name;
        selectedRearSpokes = (j % spokes.Count) + 1;
    }

    /// <summary>
    /// Set front hub at runtime
    /// </summary>
    /// <param name="j"> the index of the mesh to change to </param>
    public void SetFrontHubMesh(int j)
    {
        BetterWheelsMod.instance.OnChangeHubFront();
        PartMaster.instance.SetMesh(PartMaster.instance.frontHub, hubs[j % hubs.Count].mesh);
        selectedFrontHubText.text = hubs[j % hubs.Count].mesh.name;
        selectedFrontHub = (j % hubs.Count) + 1;
        if(BetterWheelsMod.instance.GetBetterWheels())
            BetterWheelsMod.instance.ChangeFrontHub();
    }

    /// <summary>
    /// Set rear Hub at runtime
    /// </summary>
    /// <param name="j"> the index of the mesh to change to </param>
    public void SetRearHubMesh(int j)
    {
        BetterWheelsMod.instance.OnChangeHubRear();
        PartMaster.instance.SetMesh(PartMaster.instance.rearHub, hubs[j % hubs.Count].mesh);
        selectedRearHubText.text = hubs[j % hubs.Count].mesh.name;
        selectedRearHub = (j % hubs.Count) + 1;
        if (BetterWheelsMod.instance.GetBetterWheels())
            BetterWheelsMod.instance.ChangeRearHub();
    }

    public void SetSeatMesh(int i)
    {
        PartMaster.instance.SetMesh(PartMaster.instance.seat, seats[i % seats.Count].mesh);
        selectedSeatText.text = seats[i % seats.Count].mesh.name;
        selectedSeat = (i % seats.Count) + 1;
    }

}