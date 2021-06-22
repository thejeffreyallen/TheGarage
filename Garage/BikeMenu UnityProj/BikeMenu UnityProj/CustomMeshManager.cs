/**
* Used for managing custom bike part meshes. 
*/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

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
    GameObject origSeatPost;

    public Mesh seatPostClamp;
    public Mesh longSeatPost;

    void Awake()
    {
        instance = this;
    }

    void Start()
    { 
        GameObject junk = GameObject.Find("Stem Mesh");
        junk.SetActive(false);
        basePath = Application.dataPath + "/GarageContent/";
        rightCrankBolts = GameObject.Find("Right_Crankarm_Cap");
        leftCrankBolts = GameObject.Find("Left_Crankarm_Cap");
        stemBolts = GameObject.Find("Stem Bolts Mesh");
        rightCrankBolts.GetComponent<MeshFilter>().mesh = crankBoltMeshes[1];
        leftCrankBolts.GetComponent<MeshFilter>().mesh = crankBoltMeshes[1];
        objImporter = new ObjImporter();

        GameObject.Find("Seat_Clamp_Bolt").SetActive(false);
        GameObject.Find("Seat Clamp Mesh").GetComponent<MeshFilter>().mesh = seatPostClamp;

        

        frameMeshes = LoadFromFile("Frames/", frameMeshes);
        barMeshes = LoadFromFile("Bars/", barMeshes);
        pegMeshes = LoadFromFile("Pegs/", pegMeshes);
        spokesMeshes = LoadFromFile("Spokes/", spokesMeshes);
        sprocketMeshes = LoadFromFile("Sprockets/", sprocketMeshes);
        stemMeshes = LoadFromFile("Stems/", stemMeshes);
        cranksMeshes = LoadFromFile("Cranks/", cranksMeshes);
        forksMeshes = LoadFromFile("Forks/", forksMeshes);
        stemBoltMeshes = LoadFromFile("StemBolts/", stemBoltMeshes);
        crankBoltMeshes = LoadFromFile("CrankBolts/", crankBoltMeshes);
        hubMeshes = LoadFromFile("Hubs/", hubMeshes);
        seatMeshes = LoadFromFile("Seats/", seatMeshes);
        //pedalMeshes = LoadFromFile("Frames/", customFrames, customFrameMeshes, pedalMeshes);
        //frontHubGuardMeshes = LoadFromFile("Bars/", customBars, customBarsMeshes, frontHubGuardMeshes);

        Transform[] barsJoint = GameObject.Find("BMX:Bars_Joint").GetComponentsInChildren<Transform>(true);
        Transform[] frameJoint = GameObject.Find("BMX:Frame_Joint").GetComponentsInChildren<Transform>(true);

        foreach (Transform t in barsJoint)
        {
            if (t.gameObject.name.Equals("Pegs Mesh"))
                origPegs.Add(t.gameObject);
            if (t.gameObject.name.Equals("Hub Mesh"))
                origHubs.Add(t.gameObject);
            if (t.gameObject.name.Equals("Spokes Mesh"))
                origSpokes.Add(t.gameObject);
            
        }

        foreach (Transform t in frameJoint)
        {
            if (t.gameObject.name.Equals("Pegs Mesh"))
                origPegs.Add(t.gameObject);
            if (t.gameObject.name.Equals("Hub Mesh"))
                origHubs.Add(t.gameObject);
            if (t.gameObject.name.Equals("Spokes Mesh"))
                origSpokes.Add(t.gameObject);
            if (t.gameObject.name.Equals("Seat Post"))
                origSeatPost = t.gameObject;
        }

        origSeatPost.GetComponent<MeshFilter>().mesh = longSeatPost;
    }

    /// <summary>
    /// LoadFromFile method - Loads .obj files from a folder at runtime and integrate into the included meshes.
    /// </summary>
    /// <param name="folder"> The folder to load meshes from </param>
    /// <param name="fileNameArray"> An array to hold any file names that are found in the folder </param>
    /// <param name="origMeshArray"> The original mesh array to merge into </param>
    public Mesh[] LoadFromFile(String folder, Mesh[] origMeshArray)
    {
        try
        {
            String[] fileNameArray = Directory.GetFiles(basePath + folder); // Get the file names
            Mesh[] customMeshArray = new Mesh[fileNameArray.Length]; // instantiate a new Mesh array
            
            
            for (int i = 0; i < fileNameArray.Length; i++)
            {
                Mesh temp = objImporter.ImportFile(fileNameArray[i]); // Import each file name as a mesh
                customMeshArray[i] = temp; // Add the mesh to the custom mesh array
                customMeshArray[i].name = Path.GetFileName(fileNameArray[i]).Replace(".obj", ""); // Remove the .obj extension for cleaner look when updating the button text
            }
            if (fileNameArray.Length > 0) // If any meshes were found in the folder
            { 
                // Merge the custom mesh array with the built-in mesh array
                Mesh[] newArray = new Mesh[origMeshArray.Length + customMeshArray.Length];
                Array.Copy(origMeshArray, newArray, origMeshArray.Length);
                Array.Copy(customMeshArray, 0, newArray, origMeshArray.Length, customMeshArray.Length);
                return newArray;
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error in LoadFromFile method: " + e.Message + e.StackTrace);
        }
        return origMeshArray;

    }

    /// <summary>
    /// SetMultipleMesh method - Used for setting two or more meshes that have the same name, i.e. crank arms
    /// </summary>
    /// <param name="objectNames"> string that is the shared name of the objects </param>
    /// <param name="meshArray"> the mesh array to select meshes from </param>
    /// <param name="selectedMesh"> which mesh to select from the array </param>
    /// <param name="buttonText"> button text to update on mesh change </param>
    public void SetMultipleMesh(string objectNames, Mesh[] meshArray, int selectedMesh, Text buttonText)
    {
        List<GameObject> partObjects = new List<GameObject>();

        foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
        {
            if (go.name == objectNames)
                partObjects.Add(go);
        }

        for (int i = 0; i < partObjects.Count; i++)
        {
            partObjects[i].GetComponent<MeshFilter>().mesh = meshArray[selectedMesh % meshArray.Length];
        }

        buttonText.text = meshArray[selectedMesh % meshArray.Length].name;
    }

    /// <summary>
    /// SetPegsMesh method - Similar to the SetMultipleMesh method, but specific to the pegs
    /// </summary>
    /// <param name="objectNames"> string that is the shared name of the objects </param>
    /// <param name="meshArray"> the mesh array to select meshes from </param>
    /// <param name="selectedMesh"> which mesh to select from the array </param>
    /// <param name="buttonText"> button text to update on mesh change </param>
    /// <param name="partNum"> part num indicates which of the two peg meshes to change. This allows for front and back pegs to be changed independently </param>
    public void SetPegsMesh(string objectNames, Mesh[] meshArray, int selectedMesh, Text buttonText, int partNum)
    {
        List<GameObject> partObjects = new List<GameObject>();

        foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
        {
            if (go.name == objectNames)
                partObjects.Add(go);
        }

        partObjects[partNum].GetComponent<MeshFilter>().mesh = meshArray[selectedMesh % meshArray.Length];
        buttonText.text = meshArray[selectedMesh % meshArray.Length].name;
    }

    /// <summary>
    /// SetMesh method - General use method for changing a mesh on the bike at runtime
    /// </summary>
    /// <param name="objectName"> string that is the name of the object </param>
    /// <param name="meshArray"> the mesh array to select meshes from </param>
    /// <param name="selectedMesh"> which mesh to select from the array </param>
    /// <param name="buttonText"> button text to update on mesh change </param>
    public void SetMesh(string objectName, Mesh[] meshArray, int selectedMesh, Text buttonText)
    {
        // Update additional bolt meshes if changing the crank or stem meshes
        if (objectName.Contains("Crank"))
        {
            rightCrankBolts.GetComponent<MeshFilter>().mesh = crankBoltMeshes[selectedMesh % meshArray.Length];
            leftCrankBolts.GetComponent<MeshFilter>().mesh = crankBoltMeshes[selectedMesh % meshArray.Length];
        }
        if(objectName.Contains("Stem"))
        {
            stemBolts.GetComponent<MeshFilter>().mesh = stemBoltMeshes[selectedMesh % meshArray.Length];
        }

        buttonText.text = meshArray[selectedMesh % meshArray.Length].name;
        GameObject partObject = GameObject.Find(objectName);
        partObject.GetComponent<MeshFilter>().mesh = meshArray[selectedMesh % meshArray.Length];
    }

    /// <summary>
    /// SetFrameMesh method - Change the bike frame mesh at runtime
    /// </summary>
    /// <param name="i"> the index of the mesh to change to </param>
    public void SetFrameMesh(int i)
    {
        GameObject partObject = GameObject.Find("Frame Mesh");
        partObject.GetComponent<MeshFilter>().mesh = frameMeshes[i % frameMeshes.Length];
        selectedFrameText.text = frameMeshes[i % frameMeshes.Length].name;
        selectedFrame = (i % frameMeshes.Length) + 1;
    }

    /// <summary>
    /// SetForksMesh method - Change the bike forks mesh at runtime
    /// </summary>
    /// <param name="i"> the index of the mesh to change to </param>
    public void SetForksMesh(int i)
    {
        GameObject partObject = GameObject.Find("Forks Mesh");
        partObject.GetComponent<MeshFilter>().mesh = forksMeshes[i % forksMeshes.Length];
        selectedForksText.text = forksMeshes[i % forksMeshes.Length].name;
        selectedForks = (i % forksMeshes.Length) + 1;
    }

    /// <summary>
    /// SetBarsMesh method - Change the bike bars mesh at runtime
    /// </summary>
    /// <param name="i"> the index of the mesh to change to </param>
    public void SetBarsMesh(int i)
    {
        GameObject partObject = GameObject.Find("Bars Mesh");
        partObject.GetComponent<MeshFilter>().mesh = barMeshes[i % barMeshes.Length];
        selectedBarsText.text = barMeshes[i % barMeshes.Length].name;
        selectedBars = (i % barMeshes.Length) + 1;
    }

    /// <summary>
    /// SetPedalsMesh method - Change the bike pedals mesh at runtime
    /// </summary>
    /// <param name="i"> the index of the mesh to change to </param>
    public void SetPedalsMesh(int i)
    {
        SetMultipleMesh("Pedal Mesh", pedalMeshes, i, selectedPedalsText);
    }

    /// <summary>
    /// SetSprocketMesh method - Change the bike sprocket (Chainwheel) mesh at runtime
    /// </summary>
    /// <param name="i"> the index of the mesh to change to </param>
    public void SetSprocketMesh(int i)
    {

        GameObject partObject = GameObject.Find("Sprocket Mesh");
        partObject.GetComponent<MeshFilter>().mesh = sprocketMeshes[i % sprocketMeshes.Length];
        selectedSprocketText.text = sprocketMeshes[i % sprocketMeshes.Length].name;
        selectedSprocket = (i % sprocketMeshes.Length) + 1;
    }

    /// <summary>
    /// SetStemMesh method - Change the bike stem mesh at runtime
    /// </summary>
    /// <param name="i"> the index of the mesh to change to </param>
    public void SetStemMesh(int i)
    {
        GameObject partObject = GameObject.Find("Stem Mesh");
        partObject.GetComponent<MeshFilter>().mesh = stemMeshes[i % stemMeshes.Length];
        stemBolts.GetComponent<MeshFilter>().mesh = stemBoltMeshes[i % stemMeshes.Length];
        selectedStemText.text = stemMeshes[i % stemMeshes.Length].name;
        selectedStem = (i % stemMeshes.Length) +1;
    }

    /// <summary>
    /// SetCranksMesh method - Change the bike cranks mesh at runtime
    /// </summary>
    /// <param name="i"> the index of the mesh to change to </param>
    public void SetCranksMesh(int i)
    {
        GameObject partObject = GameObject.Find("Right Crank Arm Mesh");
        GameObject partObject2 = GameObject.Find("Left Crank Arm Mesh");
        partObject.GetComponent<MeshFilter>().mesh = cranksMeshes[i % cranksMeshes.Length];
        partObject2.GetComponent<MeshFilter>().mesh = cranksMeshes[i % cranksMeshes.Length];


        rightCrankBolts.GetComponent<MeshFilter>().mesh = crankBoltMeshes[i % cranksMeshes.Length];
        leftCrankBolts.GetComponent<MeshFilter>().mesh = crankBoltMeshes[i % cranksMeshes.Length];

        selectedCranksText.text = cranksMeshes[i % cranksMeshes.Length].name;
        selectedCranks = (i % cranksMeshes.Length) +1;
    }

    /// <summary>
    /// SetPegsMesh method - Change the bike pegs mesh at runtime
    /// </summary>
    /// <param name="j"> the index of the mesh to change to </param>
    public void SetFrontPegsMesh(int j)
    {
        origPegs[0].GetComponent<MeshFilter>().mesh = pegMeshes[j % pegMeshes.Length];

        selectedFrontPegsText.text = pegMeshes[j % pegMeshes.Length].name;

        selectedFrontPegs = (j % pegMeshes.Length) +1;

    }

    /// <summary>
    /// SetPegsMesh method - Change the bike pegs mesh at runtime
    /// </summary>
    /// <param name="k"> the index of the mesh to change to </param>
    public void SetRearPegsMesh(int k)
    {
        origPegs[1].GetComponent<MeshFilter>().mesh = pegMeshes[k % pegMeshes.Length];

        selectedRearPegsText.text = pegMeshes[k % pegMeshes.Length].name;

        selectedRearPegs = (k % pegMeshes.Length) + 1;
    }

    /// <summary>
    /// SetSpokesMesh method - Change the bike spokes mesh at runtime
    /// </summary>
    /// <param name="j"> the index of the mesh to change to </param>
    public void SetFrontSpokesMesh(int j)
    { 
        origSpokes[0].GetComponent<MeshFilter>().mesh = spokesMeshes[j % spokesMeshes.Length];
        selectedFrontSpokesText.text = spokesMeshes[j % spokesMeshes.Length].name;
        selectedFrontSpokes = (j % spokesMeshes.Length) +1;
    }

    /// <summary>
    /// SetSpokesMesh method - Change the bike spokes mesh at runtime
    /// </summary>
    /// <param name="j"> the index of the mesh to change to </param>
    public void SetRearSpokesMesh(int j)
    { 
        origSpokes[1].GetComponent<MeshFilter>().mesh = spokesMeshes[j % spokesMeshes.Length];
        selectedRearSpokesText.text = spokesMeshes[j % spokesMeshes.Length].name;
        selectedRearSpokes = (j % spokesMeshes.Length) + 1;
    }

    /// <summary>
    /// Set front hub at runtime
    /// </summary>
    /// <param name="j"> the index of the mesh to change to </param>
    public void SetFrontHubMesh(int j)
    {
        origHubs[0].GetComponent<MeshFilter>().mesh = hubMeshes[j % hubMeshes.Length];
        selectedFrontHubText.text = hubMeshes[j % hubMeshes.Length].name;
        selectedFrontHub = (j % hubMeshes.Length) + 1;
    }

    /// <summary>
    /// Set rear Hub at runtime
    /// </summary>
    /// <param name="j"> the index of the mesh to change to </param>
    public void SetRearHubMesh(int j)
    {
        origHubs[1].GetComponent<MeshFilter>().mesh = hubMeshes[j % hubMeshes.Length];
        selectedRearHubText.text = hubMeshes[j % hubMeshes.Length].name;
        selectedRearHub = (j % hubMeshes.Length) + 1;
    }

    public void SetSeatMesh(int i)
    {
        GameObject partObject = GameObject.Find("Seat Mesh");
        partObject.GetComponent<MeshFilter>().mesh = seatMeshes[i % seatMeshes.Length];
        selectedSeatText.text = seatMeshes[i % seatMeshes.Length].name;
        selectedSeat = (i % seatMeshes.Length) + 1;
    }

}