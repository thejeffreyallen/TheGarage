/**
*To be used as a data structure for managing custom bike part models. 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    public int selectedPegs;
    public Mesh[] pegMeshes;
    public Text selectedPegsText;

    [Header("Spokes")]
    public int selectedSpokes;
    public Mesh[] spokesMeshes;
    public Text selectedSpokesText;

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

    [Header("Stem Bolts")]
    public Mesh[] stemBoltMeshes;

    [Header("crank Bolts")]
    public Mesh[] crankBoltMeshes;

    private GameObject rightCrankBolts;
    private GameObject leftCrankBolts;
    private GameObject stemBolts;

    void Awake()
    {
        instance = this;
    }

    void Start()
    { 
        GameObject junk = GameObject.Find("Stem Mesh");
        junk.SetActive(false);

        rightCrankBolts = GameObject.Find("Right_Crankarm_Cap");
        leftCrankBolts = GameObject.Find("Left_Crankarm_Cap");
        stemBolts = GameObject.Find("Stem Bolts Mesh");
        rightCrankBolts.GetComponent<MeshFilter>().mesh = crankBoltMeshes[1];
        leftCrankBolts.GetComponent<MeshFilter>().mesh = crankBoltMeshes[1];

    }

    public void SetMultipleMesh(string objectNames, Mesh[] meshArray, int selectedMesh, Text buttonText)
    {
        if (selectedMesh >= meshArray.Length - 1)
        {
            if (objectNames.Contains("Peg"))
                selectedPegs = 0;

            if (objectNames.Contains("Spoke"))
                selectedSpokes = 0;
        }

        List<GameObject> partObjects = new List<GameObject>();

        foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
        {
            if (go.name == objectNames)
                partObjects.Add(go);
        }

        for (int i = 0; i < partObjects.Count; i++)
        {
            partObjects[i].GetComponent<MeshFilter>().mesh = meshArray[selectedMesh];
        }

        buttonText.text = meshArray[selectedMesh].name;
    }

    public void SetMesh(string objectName, Mesh[] meshArray, int selectedMesh, Text buttonText)
    {
        if (selectedMesh >= meshArray.Length - 1)
        {
            if (objectName.Contains("Frame"))
                selectedFrame = 0;

            if (objectName.Contains("Bars"))
                selectedBars = 0;

            if (objectName.Contains("Sprocket"))
                selectedSprocket = 0;

            if (objectName.Contains("Stem"))
                selectedStem = 0;

            if (objectName.Contains("Crank"))
                selectedCranks = 0;
        }
        if (objectName.Contains("Crank"))
        {
            rightCrankBolts.GetComponent<MeshFilter>().mesh = crankBoltMeshes[selectedMesh];
            leftCrankBolts.GetComponent<MeshFilter>().mesh = crankBoltMeshes[selectedMesh];
        }
        if(objectName.Contains("Stem"))
        {
            stemBolts.GetComponent<MeshFilter>().mesh = stemBoltMeshes[selectedMesh];
        }

        buttonText.text = meshArray[selectedMesh].name;
        GameObject partObject = GameObject.Find(objectName);
        partObject.GetComponent<MeshFilter>().mesh = meshArray[selectedMesh];
     
        
    }

    public void SetFrameMesh(int i)
    {
        if (i >= frameMeshes.Length)
        {
            i = 0;
        }
        else if (i < 0)
        {
            i = frameMeshes.Length - 1;
        }
        GameObject partObject = GameObject.Find("Frame Mesh");
        partObject.GetComponent<MeshFilter>().mesh = frameMeshes[i];
        
        selectedFrameText.text = frameMeshes[i].name;
        selectedFrame = i+1;
    }
    public void SetBarsMesh(int i)
    {
        if (i >= barMeshes.Length)
        {
            i = 0;
        }
        else if (i < 0)
        {
            i = barMeshes.Length - 1;
        }
        GameObject partObject = GameObject.Find("Bars Mesh");
        partObject.GetComponent<MeshFilter>().mesh = barMeshes[i];
        selectedBarsText.text = barMeshes[i].name;
        selectedBars = i + 1;
    }
    public void SetSprocketMesh(int i)
    {
        if (i >= sprocketMeshes.Length)
        {
            i = 0;
        }
        else if (i < 0)
        {
            i = sprocketMeshes.Length - 1;
        }
        GameObject partObject = GameObject.Find("Sprocket Mesh");
        partObject.GetComponent<MeshFilter>().mesh = sprocketMeshes[i];
        selectedSprocketText.text = sprocketMeshes[i].name;
        selectedSprocket = i+1;
    }
    public void SetStemMesh(int i)
    {
        if (i >= stemMeshes.Length)
        {
            i = 0;
        }
        else if (i < 0)
        {
            i = stemMeshes.Length - 1;
        }
        GameObject partObject = GameObject.Find("Stem Mesh");
        partObject.GetComponent<MeshFilter>().mesh = stemMeshes[i];
        stemBolts.GetComponent<MeshFilter>().mesh = stemBoltMeshes[i];
        selectedStemText.text = stemMeshes[i].name;
        selectedStem = i+1;
    }
    public void SetCranksMesh(int i)
    {
        if (i >= cranksMeshes.Length)
        {
            i = 0;
        }
        else if (i < 0)
        {
            i = cranksMeshes.Length - 1;
        }
        GameObject partObject = GameObject.Find("Right Crank Arm Mesh");
        GameObject partObject2 = GameObject.Find("Left Crank Arm Mesh");
        partObject.GetComponent<MeshFilter>().mesh = cranksMeshes[i];
        partObject2.GetComponent<MeshFilter>().mesh = cranksMeshes[i];


        rightCrankBolts.GetComponent<MeshFilter>().mesh = crankBoltMeshes[i];
        leftCrankBolts.GetComponent<MeshFilter>().mesh = crankBoltMeshes[i];

        selectedCranksText.text = cranksMeshes[i].name;
        selectedCranks = i+1;
    }
    public void SetPegsMesh(int j)
    {
        if (j >= pegMeshes.Length)
        {
            j = 0;
        }
        else if (j < 0)
        {
            j = pegMeshes.Length - 1;
        }
        List<GameObject> partObjects = new List<GameObject>();

        foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
        {
            if (go.name == "Pegs Mesh")
                partObjects.Add(go);
        }

        for (int i = 0; i < partObjects.Count; i++)
        {
            partObjects[i].GetComponent<MeshFilter>().mesh = pegMeshes[j];
        }
        selectedPegsText.text = pegMeshes[j].name;
        selectedPegs = j+1;
    }
    public void SetSpokesMesh(int j)
    {
        if (j >= spokesMeshes.Length)
        {
            j = 0;
        }
        else if (j < 0)
        {
            j = spokesMeshes.Length - 1;
        }
        List<GameObject> partObjects = new List<GameObject>();
        foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
        {
            if (go.name == "Spokes Mesh")
                partObjects.Add(go);
        }

        for (int i = 0; i < partObjects.Count; i++)
        {
            partObjects[i].GetComponent<MeshFilter>().mesh = spokesMeshes[j];
        }
        selectedSpokesText.text = spokesMeshes[j].name;
        selectedSpokes = j+1;
    }

}