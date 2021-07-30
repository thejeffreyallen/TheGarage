/**
* Used for managing custom bike part meshes. 
*/
using System;
using System.Collections.Generic;
using System.Collections;
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
    public int selectedFrame = 1;
    public Mesh[] frameMeshes;
    public Text selectedFrameText;

    [Header("Bars")]
    public int selectedBars = 1;
    public Mesh[] barMeshes;
    public Text selectedBarsText;

    [Header("SeatPosts")]
    public int selectedSeatPost = 1;
    public Mesh[] seatPostMeshes;
    public Text selectedSeatPostText;

    [Header("Pegs")]
    public int selectedFrontPegs = 1;
    public int selectedRearPegs = 1;
    public Mesh[] pegMeshes;
    public Text selectedFrontPegsText;
    public Text selectedRearPegsText;

    [Header("Spokes")]
    public int selectedFrontSpokes = 1;
    public int selectedRearSpokes = 1;
    public Mesh[] spokesMeshes;
    public Text selectedFrontSpokesText;
    public Text selectedRearSpokesText;

    [Header("Sprocket")]
    public int selectedSprocket = 1;
    public Mesh[] sprocketMeshes;
    public Text selectedSprocketText;

    [Header("Stem")]
    public int selectedStem;
    public Mesh[] stemMeshes;
    public Text selectedStemText;

    [Header("Rims")]
    public int selectedFrontRim = 1;
    public int selectedRearRim = 1;
    public Mesh[] rimMeshes;
    public Text selectedFrontRimText;
    public Text selectedRearRimText;

    [Header("Cranks")]
    public int selectedCranks = 1;
    public Mesh[] cranksMeshes;
    public Text selectedCranksText;

    [Header("Forks")]
    public int selectedForks = 1;
    public Mesh[] forksMeshes;
    public Text selectedForksText;

    [Header("Pedals")]
    public int selectedPedals = 1;
    public Mesh[] pedalMeshes;
    public Text selectedPedalsText;

    [Header("FrontHubGuards")]
    public int selectedFrontHubGuard = 1;
    public Mesh[] frontHubGuardMeshes;
    public Text selectedFrontHubGuardText;

    [Header("RearHubGuards")]
    public int selectedRearHubGuard = 1;
    public Mesh[] rearHubGuardMeshes;
    public Text selectedRearHubGuardText;

    [Header("Stem Bolts")]
    public Mesh[] stemBoltMeshes;

    [Header("Crank Bolts")]
    public Mesh[] crankBoltMeshes;

    [Header("Hubs")]
    public int selectedFrontHub = 1;
    public int selectedRearHub = 1;
    public Mesh[] hubMeshes;
    public Text selectedFrontHubText;
    public Text selectedRearHubText;

    [Header("Spoke Accessories")]
    public int selectedFrontAccessory = 1;
    public int selectedRearAccessory = 1;
    public Mesh[] accessoryMeshes;
    public Text selectedFrontAccessoryText;
    public Text selectedRearAccessoryText;

    public Material[] accMats;

    [Header("Bar Accessories")]
    public int selectedBarAccessory = 1;
    public Mesh[] barAccMeshes;
    public Text selectedBarAccessoryText;

    [Header("Frame Accessories")]
    public int selectedFrameAccessory = 1;
    public Mesh[] frameAccMeshes;
    public Text selectedFrameAccessoryText;

    

    [Header("Seat")]
    public int selectedSeat = 1;
    public Mesh[] seatMeshes;
    public Text selectedSeatText;

    private GameObject rightCrankBolts;
    private GameObject leftCrankBolts;
    private GameObject stemBolts;
    private String basePath;
    private bool frontLightsOn = false;
    private bool rearLightsOn = false;

    private ObjImporter objImporter;

    public bool isDoneLoading = false;

    List<GameObject> origSpokes = new List<GameObject>();
    List<GameObject> origHubs = new List<GameObject>();
    List<GameObject> origPegs = new List<GameObject>();
    public GameObject origSeatPost;

    public Mesh seatPostClamp;
    public Mesh longSeatPost;

    Dictionary<string, int> customMeshList;

    public List<MeshObject> frames;
    public List<MeshObject> bars;
    public List<MeshObject> forks;
    public List<MeshObject> stems;
    public List<MeshObject> rims;
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
    public List<MeshObject> barAccessories;
    public List<MeshObject> frameAccessories;
    public List<MeshObject> seatPosts;

    public Dictionary<string, List<MeshObject>> meshLists = new Dictionary<string, List<MeshObject>>();

    private float nextActionTime = 0.0f;
    public float period = 0.2f;

    void Awake()
    {
        instance = this;
        InitMeshObjectLists();
    }

    void Start()
    {
        objImporter = new ObjImporter();
        basePath = Application.dataPath + "/GarageContent/";
        customMeshList = new Dictionary<string, int>();
        rightCrankBolts = PartMaster.instance.GetPart(PartMaster.instance.rightCrankBolt);
        leftCrankBolts = PartMaster.instance.GetPart(PartMaster.instance.leftCrankBolt);
        stemBolts = PartMaster.instance.GetPart(PartMaster.instance.stemBolts);
        rightCrankBolts.GetComponent<MeshFilter>().mesh = crankBoltMeshes[1];
        leftCrankBolts.GetComponent<MeshFilter>().mesh = crankBoltMeshes[1];
        GameObject SeatClampBolt = PartMaster.instance.GetPart(PartMaster.instance.seatClampBolt);
        SeatClampBolt.SetActive(false);
        PartMaster.instance.SetMesh(PartMaster.instance.seatClamp, seatPostClamp);
        SwitchDefaultParts();
        //PartMaster.instance.SetMesh(PartMaster.instance.seatPost, longSeatPost);
        LoadFiles(); // Load meshes from file
        isDoneLoading = true;
    }

    public void SwitchDefaultParts()
    {
        SetFrameMesh(0);
        SetForksMesh(0);
        SetBarsMesh(0);
        SetPedalsMesh(0);
        SetFrontRimMesh(0);
        SetRearRimMesh(0);
    }

    void Update()
    {
        if (Time.time > nextActionTime && (frontLightsOn || rearLightsOn))
        {
            nextActionTime += period;
            float f = 10f;
            Color color = new Color(UnityEngine.Random.Range(0.2f, 1f), UnityEngine.Random.Range(0.2f, 1f), UnityEngine.Random.Range(0.2f, 1f), 1f) * f;
            if (frontLightsOn)
            {
                PartMaster.instance.GetMaterial(PartMaster.instance.frontAcc).color = color / f;
                PartMaster.instance.GetMaterial(PartMaster.instance.frontAcc).SetColor("_EmissionColor", color);
            }
            if (rearLightsOn)
            {
                PartMaster.instance.GetMaterial(PartMaster.instance.rearAcc).color = color / f;
                PartMaster.instance.GetMaterial(PartMaster.instance.rearAcc).SetColor("_EmissionColor", color);
            }
        }
        
    }

    public void InitMeshObjectLists()
    {
        try
        {
            frames = new List<MeshObject>();
            bars = new List<MeshObject>();
            forks = new List<MeshObject>();
            stems = new List<MeshObject>();
            rims = new List<MeshObject>();
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
            barAccessories = new List<MeshObject>();
            frameAccessories = new List<MeshObject>();
            seatPosts = new List<MeshObject>();

            foreach (Mesh m in frameMeshes)
                frames.Add(new MeshObject(m, false, ""));
            foreach (Mesh m in barMeshes)
                bars.Add(new MeshObject(m, false, ""));
            foreach (Mesh m in forksMeshes)
                forks.Add(new MeshObject(m, false, ""));
            foreach (Mesh m in stemMeshes)
                stems.Add(new MeshObject(m, false, ""));
            foreach (Mesh m in rimMeshes)
                rims.Add(new MeshObject(m, false, ""));
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
            foreach (Mesh m in barAccMeshes)
                barAccessories.Add(new MeshObject(m, false, ""));
            foreach (Mesh m in frameAccMeshes)
                frameAccessories.Add(new MeshObject(m, false, ""));
            foreach (Mesh m in seatPostMeshes)
                seatPosts.Add(new MeshObject(m, false, ""));

            meshLists.Add("frame", frames);
            meshLists.Add("bars", bars);
            meshLists.Add("sprocket", sprockets);
            meshLists.Add("stem", stems);
            meshLists.Add("cranks", cranks);
            meshLists.Add("frontSpokes", spokes);
            meshLists.Add("rearSpokes", spokes);
            meshLists.Add("pedals", pedals);
            meshLists.Add("forks", forks);
            meshLists.Add("frontPegs", pegs);
            meshLists.Add("rearPegs", pegs);
            meshLists.Add("frontHub", hubs);
            meshLists.Add("rearHub", hubs);
            meshLists.Add("seat", seats);
            meshLists.Add("frontRim", rims);
            meshLists.Add("rearRim", rims);
            meshLists.Add("frontSpokeAccessory", accessories);
            meshLists.Add("rearSpokeAccessory", accessories);
            meshLists.Add("barAccessory", barAccessories);
            meshLists.Add("frameAccessory", frameAccessories);
            meshLists.Add("frontHubGuard", frontHubGuards);
            meshLists.Add("rearHubGuard", rearHubGuards);
            meshLists.Add("seatPost", seatPosts);
            meshLists.Add("stemBolts", boltsStem);
            meshLists.Add("crankBolts", boltsCrank);
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
            pedals = LoadFromFile("Pedals/", pedals);
            rims = LoadFromFile("Rims/", rims);
            accessories = LoadFromFile("WheelAccessories/", accessories);
            barAccessories = LoadFromFile("BarAccessories/", barAccessories);
            frameAccessories = LoadFromFile("FrameAccessories/", frameAccessories);
            seatPosts = LoadFromFile("SeatPosts/", seatPosts);
            frontHubGuards = LoadFromFile("FrontHubGuards/", frontHubGuards);
            rearHubGuards = LoadFromFile("RearHubGuards/", rearHubGuards);
        }
        catch (Exception e)
        {
            Debug.Log("Error while loading meshes from file: " + e.Message + "\n" + e.StackTrace);
        }
        
    }

    public Mesh FindSpecific(string list, string fileName)
    {
        List<MeshObject> temp = meshLists[list];
        Mesh mesh = null;
        if (temp == null)
        {
            Debug.Log("Mesh not found in mesh lists");
            return null;
        }
        foreach (MeshObject mo in temp)
        {
            if (mo.fileName.Equals(fileName))
            {
                mesh = mo.mesh;
                break;
            }
        }
        return mesh;
    }

    public void SetFrontSpokeAccMesh(int i)
    {
        int index = i % accessories.Count;
        GameObject partObject = PartMaster.instance.GetPart(PartMaster.instance.frontAcc);
        partObject.GetComponent<MeshFilter>().mesh = accessories[index].mesh;
        if (index >= 0 && index < 4)
            partObject.GetComponent<MeshRenderer>().material = accMats[index];
        else
            partObject.GetComponent<MeshRenderer>().material = MaterialManager.instance.defaultMat;
        selectedFrontAccessoryText.text = accessories[index].mesh.name;
        frontLightsOn = index == 3 ? true : false;
        selectedFrontAccessory = (index) + 1;
    }

    public void SetRearSpokeAccMesh(int i)
    {
        int index = i % accessories.Count;
        GameObject partObject = PartMaster.instance.GetPart(PartMaster.instance.rearAcc);
        partObject.GetComponent<MeshFilter>().mesh = accessories[index].mesh;
        if (index >= 0 && index < 4)
            partObject.GetComponent<MeshRenderer>().material = accMats[index];
        else
            partObject.GetComponent<MeshRenderer>().material = MaterialManager.instance.defaultMat;
        selectedRearAccessoryText.text = accessories[index].mesh.name;
        rearLightsOn = index == 3 ? true : false;
        selectedRearAccessory = (index) + 1;
    }

    public void SetBarAccMesh(int i)
    {
        GameObject partObject = PartMaster.instance.GetPart(PartMaster.instance.barAcc);
        partObject.GetComponent<MeshFilter>().mesh = barAccessories[i % barAccessories.Count].mesh;
        selectedBarAccessoryText.text = barAccessories[i % barAccessories.Count].mesh.name;
        selectedBarAccessory = (i % barAccessories.Count) + 1;
    }

    public void SetFrameAccMesh(int i)
    {
        GameObject partObject = PartMaster.instance.GetPart(PartMaster.instance.frameAcc);
        partObject.GetComponent<MeshFilter>().mesh = frameAccessories[i % frameAccessories.Count].mesh;
        selectedFrameAccessoryText.text = frameAccessories[i % frameAccessories.Count].mesh.name;
        selectedFrameAccessory = (i % frameAccessories.Count) + 1;
    }

    /// <summary>
    /// LoadFromFile method - Loads .obj files from a folder at runtime and integrate into the included meshes.
    /// </summary>
    /// <param name="folder"> The folder to load meshes from </param>
    /// <param name="fileNameArray"> An array to hold any file names that are found in the folder </param>
    /// <param name="origMeshArray"> The original mesh array to merge into </param>
    public List<MeshObject> LoadFromFile(String folder, List<MeshObject> meshObjectList)
    {
        
            String[] fileNameArray = Directory.GetFiles(basePath + folder); // Get the file names
            for (int i = 0; i < fileNameArray.Length; i++)
            {
                if (customMeshList.ContainsKey(fileNameArray[i]) || !fileNameArray[i].Contains(".obj")) {
                    continue;
                }
            try
            {
                customMeshList.Add(fileNameArray[i], 0);
                Mesh temp = objImporter.ImportFile(fileNameArray[i]); // Import each file name as a mesh
                temp.name = Path.GetFileName(fileNameArray[i]).Replace(".obj", ""); // Remove the .obj extension for cleaner look when updating the button text
                meshObjectList.Add(new MeshObject(temp, true, fileNameArray[i]));
            }
            catch (Exception e)
            {
                Debug.Log("Error in loading " + fileNameArray[i] +": " + e.Message + e.StackTrace);
                continue;
            }
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

    public void SetSeatPostMesh(int i)
    {
        PartMaster.instance.SetMesh(PartMaster.instance.seatPost, seatPosts[i % seatPosts.Count].mesh);
        selectedSeatPostText.text = seatPosts[i % seatPosts.Count].mesh.name;
        selectedSeatPost = (i % seatPosts.Count) + 1;
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
    /// SetFrontRimMesh method - Change the bike rim mesh at runtime
    /// </summary>
    /// <param name="i"> the index of the mesh to change to </param>
    public void SetFrontRimMesh(int i)
    {
        PartMaster.instance.SetMesh(PartMaster.instance.frontRim, rims[i % rims.Count].mesh);
        selectedFrontRimText.text = rims[i % rims.Count].mesh.name;
        selectedFrontRim = (i % rims.Count) + 1;
    }

    /// <summary>
    /// SetRearRimMesh method - Change the bike rim mesh at runtime
    /// </summary>
    /// <param name="i"> the index of the mesh to change to </param>
    public void SetRearRimMesh(int i)
    {
        PartMaster.instance.SetMesh(PartMaster.instance.rearRim, rims[i % rims.Count].mesh);
        selectedRearRimText.text = rims[i % rims.Count].mesh.name;
        selectedRearRim = (i % rims.Count) + 1;
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
        PartMaster.instance.SetMesh(PartMaster.instance.frontHub, hubs[j % hubs.Count].mesh);
        selectedFrontHubText.text = hubs[j % hubs.Count].mesh.name;
        selectedFrontHub = (j % hubs.Count) + 1;
    }

    /// <summary>
    /// Set rear Hub at runtime
    /// </summary>
    /// <param name="j"> the index of the mesh to change to </param>
    public void SetRearHubMesh(int j)
    {
        PartMaster.instance.SetMesh(PartMaster.instance.rearHub, hubs[j % hubs.Count].mesh);
        selectedRearHubText.text = hubs[j % hubs.Count].mesh.name;
        selectedRearHub = (j % hubs.Count) + 1;
    }

    public void SetSeatMesh(int i)
    {
        PartMaster.instance.SetMesh(PartMaster.instance.seat, seats[i % seats.Count].mesh);
        selectedSeatText.text = seats[i % seats.Count].mesh.name;
        selectedSeat = (i % seats.Count) + 1;
    }

    /// <summary>
    /// Set front hub guard at runtime
    /// </summary>
    /// <param name="j"> the index of the mesh to change to </param>
    public void SetFrontHubGuardMesh(int j)
    {
        PartMaster.instance.SetMesh(PartMaster.instance.frontHubG, frontHubGuards[j % frontHubGuards.Count].mesh);
        selectedFrontHubGuardText.text = frontHubGuards[j % frontHubGuards.Count].mesh.name;
        selectedFrontHubGuard = (j % frontHubGuards.Count) + 1;
    }

    /// <summary>
    /// Set rear hub guard at runtime
    /// </summary>
    /// <param name="j"> the index of the mesh to change to </param>
    public void SetRearHubGuardMesh(int j)
    {
        PartMaster.instance.SetMesh(PartMaster.instance.rearHubG, rearHubGuards[j % rearHubGuards.Count].mesh);
        selectedRearHubGuardText.text = rearHubGuards[j % rearHubGuards.Count].mesh.name;
        selectedRearHubGuard = (j % rearHubGuards.Count) + 1;
    }

}