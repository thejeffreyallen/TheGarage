using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// A class that will handle the initial calls to Find() and serve as a central location for all the bike parts since Find() is a very expensive function call.
/// </summary>
class PartMaster : MonoBehaviour
{
    public static PartMaster instance;
    public Dictionary<int, GameObject> partList;
    public bool isDone = false;

    public int frame = 0;
    public int bars = 1;
    public int forks = 2;
    public int stem = 3;
    public int frontSpokes = 4;
    public int rearSpokes = 5;
    public int frontRim = 6;
    public int rearRim = 7;
    public int frontHub = 8;
    public int rearHub = 9;
    public int frontTire = 10;
    public int rearTire = 11;
    public int frontNipples = 12;
    public int rearNipples = 13;
    public int frontWheel = 14;
    public int rearWheel = 15;
    public int leftGrip = 16;
    public int rightGrip = 17;
    public int leftPedal = 18;
    public int rightPedal = 19;
    public int leftPedalAxle = 20;
    public int rightPedalAxle = 21;
    public int leftPedalCap = 22;
    public int rightPedalCap = 23;
    public int stemBolts = 24;
    public int leftCrankBolt = 25;
    public int rightCrankBolt = 26;
    public int leftAnchor = 27;
    public int rightAnchor = 28;
    public int barEnds = 29;
    public int headSet = 30;
    public int headSetSpacers = 31;
    public int frontPegs = 32;
    public int rearPegs = 33;
    public int sprocket = 34;
    public int bottomBracket = 35;
    public int seat = 36;
    public int seatPost = 37;
    public int seatPostAnchor = 38;
    public int seatClamp = 39;
    public int seatClampBolt = 40;
    public int leftCrank = 41;
    public int rightCrank = 42;
    public int chain = 43;

    /// <summary>
    /// Need to find all bike parts and assign them to a public GameObject for other classes to use
    /// </summary>
    void Awake()
    {
        instance = this;
        InitPartList();
    }

    public void InitPartList()
    {
        string errorPath = Application.dataPath + "//GarageContent/GarageErrorLog.txt";
        try
        {
            partList = new Dictionary<int, GameObject>();
            Transform[] barsJoint = GameObject.Find("BMX:Bars_Joint").GetComponentsInChildren<Transform>(true);
            Transform[] frameJoint = GameObject.Find("BMX:Frame_Joint").GetComponentsInChildren<Transform>(true);

            foreach (Transform t in barsJoint)
            {
                switch (t.gameObject.name)
                {
                    case "Pegs Mesh":
                        partList.Add(frontPegs, t.gameObject);
                        break;
                    case "Nipples Mesh":
                        partList.Add(frontNipples, t.gameObject);
                        break;
                    case "Spokes Mesh":
                        partList.Add(frontSpokes, t.gameObject);
                        break;
                    case "Rim Mesh":
                        partList.Add(frontRim, t.gameObject);
                        break;
                    case "Hub Mesh":
                        partList.Add(frontHub, t.gameObject);
                        break;
                    case "Tire Mesh":
                        partList.Add(frontTire, t.gameObject);
                        break;
                    case "Left Anchor":
                        partList.Add(leftAnchor, t.gameObject);
                        break;
                    case "Right Anchor":
                        partList.Add(rightAnchor, t.gameObject);
                        break;
                    case "Stem Bolts Mesh":
                        partList.Add(stemBolts, t.gameObject);
                        break;
                    case "Bars Mesh":
                        partList.Add(bars, t.gameObject);
                        break;
                    case "BMX:Wheel":
                        partList.Add(frontWheel, t.gameObject);
                        break;
                    case "Bar Ends Mesh":
                        partList.Add(barEnds, t.gameObject);
                        break;
                    case "Left Grip":
                        Transform[] tran1 = t.gameObject.GetComponentsInChildren<Transform>();
                        if (tran1[0].gameObject.name == "Left Grip Mesh")
                            partList.Add(leftGrip, tran1[0].gameObject);
                        else
                            partList.Add(leftGrip, tran1[1].gameObject);
                        break;
                    case "Right Grip":
                        Transform[] tran2 = t.gameObject.GetComponentsInChildren<Transform>();
                        if (tran2[0].gameObject.name == "Left Grip Mesh")
                            partList.Add(rightGrip, tran2[0].gameObject);
                        else
                            partList.Add(rightGrip, tran2[1].gameObject);
                        break;
                    case "Forks Mesh":
                        partList.Add(forks, t.gameObject);
                        break;
                    case "Headset Mesh":
                        partList.Add(headSet, t.gameObject);
                        break;
                    case "Stem Mesh":
                        if (!(t.gameObject.GetComponent<MeshFilter>() == null))
                            partList.Add(stem, t.gameObject);
                        break;
                    case "Headset Spacers Mesh":
                        partList.Add(headSetSpacers, t.gameObject);
                        break;
                    default:
                        break;
                }

            }

            foreach (Transform t in frameJoint)
            {
                switch (t.gameObject.name)
                {
                    case "Pegs Mesh":
                        partList.Add(rearPegs, t.gameObject);
                        break;
                    case "Nipples Mesh":
                        partList.Add(rearNipples, t.gameObject);
                        break;
                    case "BMX:Wheel 1":
                        partList.Add(rearWheel, t.gameObject);
                        break;
                    case "Spokes Mesh":
                        partList.Add(rearSpokes, t.gameObject);
                        break;
                    case "Rim Mesh":
                        partList.Add(rearRim, t.gameObject);
                        break;
                    case "Hub Mesh":
                        partList.Add(rearHub, t.gameObject);
                        break;
                    case "Tire Mesh":
                        partList.Add(rearTire, t.gameObject);
                        break;
                    case "Seat Post":
                        partList.Add(seatPost, t.gameObject);
                        break;
                    case "Seat Post Anchor":
                        partList.Add(seatPostAnchor, t.gameObject);
                        break;
                    case "Seat Mesh":
                        partList.Add(seat, t.gameObject);
                        break;
                    case "Seat Clamp Mesh":
                        partList.Add(seatClamp, t.gameObject);
                        break;
                    case "Seat_Clamp_Bolt":
                        partList.Add(seatClampBolt, t.gameObject);
                        break;
                    case "Chain Mesh":
                        partList.Add(chain, t.gameObject);
                        break;
                    case "Frame Mesh":
                        partList.Add(frame, t.gameObject);
                        break;
                    case "BB Mesh":
                        partList.Add(bottomBracket, t.gameObject);
                        break;
                    case "Right Crank Arm Mesh":
                        partList.Add(rightCrank, t.gameObject);
                        break;
                    case "Left Crank Arm Mesh":
                        partList.Add(leftCrank, t.gameObject);
                        break;
                    case "Sprocket Mesh":
                        partList.Add(sprocket, t.gameObject);
                        break;
                    case "Right_Crankarm_Cap":
                        partList.Add(rightCrankBolt, t.gameObject);
                        break;
                    case "Left_Crankarm_Cap":
                        partList.Add(leftCrankBolt, t.gameObject);
                        break;
                    case "BMX:LeftPedal_Joint":
                        Transform[] tran1 = t.gameObject.GetComponentsInChildren<Transform>();
                        foreach (Transform tr in tran1)
                        {
                            if (tr.gameObject.name.Equals("Pedal Mesh"))
                                partList.Add(leftPedal, tr.gameObject);
                            if (tr.gameObject.name.Equals("Pedal_01_axis"))
                                partList.Add(leftPedalAxle, tr.gameObject);
                            if (tr.gameObject.name.Equals("Pedal Cap Mesh"))
                                partList.Add(leftPedalCap, tr.gameObject);

                        }
                        break;
                    case "BMX:RightPedal_Joint":
                        Transform[] tran2 = t.gameObject.GetComponentsInChildren<Transform>();
                        foreach (Transform tr in tran2)
                        {
                            if(tr.gameObject.name.Equals("Pedal Mesh"))
                                partList.Add(rightPedal, tr.gameObject);
                            if (tr.gameObject.name.Equals("Pedal_01_axis"))
                                partList.Add(rightPedalAxle, tr.gameObject);
                            if (tr.gameObject.name.Equals("Pedal Cap Mesh"))
                                partList.Add(rightPedalCap, tr.gameObject);

                        }
                        break;
                    default:
                        break;
                }
            }
        }
        catch (Exception e)
        {
            File.AppendAllText(errorPath, "\n" + DateTime.Now + "\nRANDOM ERRORS: " + "Error while initializing part list in PartMaster.cs. " + e.Message + e.StackTrace);
            Debug.Log("Error while initializing part list in PartMaster.cs. " + e.Message + e.StackTrace);
        }
        isDone = true;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
            Debug.Log("Total number of parts found: " + partList.Count);
    }

    public GameObject GetPart(int key)
    {
        if (!partList.ContainsKey(key))
        {
            Debug.Log("Key not found in part list");
            return null;
        }
        return partList[key];
    }

    public Mesh GetMesh(int key)
    {
        if (!partList.ContainsKey(key))
        {
            Debug.Log("Key not found in part list");
            return null;
        }
        return partList[key].GetComponent<MeshFilter>().mesh;
    }

    public void SetMesh(int key, Mesh mesh)
    {
        if (!partList.ContainsKey(key))
        {
            Debug.Log("Key not found in part list");
            return;
        }
        partList[key].GetComponent<MeshFilter>().mesh = mesh;
    }

    public Material GetMaterial(int key)
    {
        if (!partList.ContainsKey(key))
        {
            Debug.Log("Key not found in part list");
            return null;
        }
        return partList[key].GetComponent<MeshRenderer>().material;
    }

    public void SetMaterial(int key, Material mat)
    {
        if (!partList.ContainsKey(key))
        {
            Debug.Log("Key not found in part list");
            return;
        }
        Material[] mats = partList[key].GetComponent<MeshRenderer>().materials;
        if (mats.Length > 1)
        {
            Debug.Log("More than one material");
            mats[0] = mat;
        }
        partList[key].GetComponent<Renderer>().material = mat;
    }

    public Material[] GetMaterials(int key)
    {
        if (!partList.ContainsKey(key))
        {
            Debug.Log("Key not found in part list");
            return null;
        }
        return partList[key].GetComponent<MeshRenderer>().materials;
    }

    /// <summary>
    /// Experimental method
    /// </summary>
    /// <param name="key"></param>
    public void AddCollision(int key)
    {
        if (!partList.ContainsKey(key))
        {
            Debug.Log("Key not found in part list");
            return;
        }
        partList[key].AddComponent<MeshCollider>();
        partList[key].GetComponent<MeshCollider>().sharedMesh = GetMesh(key);
    }

}

