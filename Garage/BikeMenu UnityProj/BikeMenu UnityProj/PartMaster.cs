﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// A class that will handle the initial calls to Find() and serve as a central location for all the bike parts since Find() is a very expensive function call.
/// </summary>
public class PartMaster : MonoBehaviour
{

    public struct TransformData
    {
        public Vector3 LocalPosition;
        public Vector3 LocalEulerRotation;
        public Vector3 LocalScale;

        public TransformData(Transform transform)
        {
            LocalPosition = transform.localPosition;
            LocalEulerRotation = transform.localEulerAngles;
            LocalScale = transform.localScale;
        }

        public void ApplyTo(Transform transform)
        {
            transform.localPosition = LocalPosition;
            transform.localEulerAngles = LocalEulerRotation;
            transform.localScale = LocalScale;
        }
    }

    public struct MaterialData
    {
        public float glossiness;
        public float glossMapScale;
        public float texTileX, texTileY;
        public float normTileX, normTileY;
        public float metTileX, metTileY;
        public float metallic;

        public MaterialData(float glossiness, float glossMapScale, float texTileX, float texTileY, float normTileX, float normTileY, float metTileX, float metTileY, float metallic)
        {
            this.glossiness = glossiness;
            this.glossMapScale = glossMapScale;
            this.texTileX = texTileX;
            this.texTileY = texTileY;
            this.normTileX = normTileX;
            this.normTileY = normTileY;
            this.metTileX = metTileX;
            this.metTileY = metTileY;
            this.metallic = metallic;
        }
    }

    public static PartMaster instance;
    public Dictionary<int, GameObject> partList;
    public Dictionary<int, TransformData> origTrans;
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
    public int barsJ = 44;
    public int frameJ = 45;
    public int frontWheelCol = 46;
    public int rearWheelCol = 47;
    public int frontAcc = 48;
    public int rearAcc = 49;
    public int barAcc = 50;
    public int frameAcc = 51;
    public int frontHubG = 52;
    public int rearHubG = 53;
    public int rightGripFlange = 54;
    public int leftGripFlange = 55;
    public int leftPedalTarget = 56;
    public int rightPedalTarget = 57;
    public int leftPedalJoint = 58;
    public int rightPedalJoint = 59;
    public int driveTrain = 60;

    

    GameObject accFront;
    GameObject accRear;
    GameObject barAccessory;
    GameObject frameAccesory;
    GameObject frontHubGuard;
    GameObject rearHubGuard;

    public GameObject localBMX;

    /// <summary>
    /// Need to find all bike parts and assign them to a public GameObject for other classes to use
    /// </summary>
    void Awake()
    {
        instance = this;
        InitPartList();
        origTrans = new Dictionary<int, TransformData>();
    }

    void Start()
    {
        accFront = new GameObject("FrontAccessory");
        accFront.AddComponent<MeshFilter>();
        accFront.GetComponent<MeshFilter>().mesh = CustomMeshManager.instance.accessoryMeshes[0];
        accFront.AddComponent<MeshRenderer>();
        accFront.GetComponent<MeshRenderer>().material = CustomMeshManager.instance.accMats[0];
        accFront = Instantiate(accFront, PartMaster.instance.GetPart(PartMaster.instance.frontSpokes).transform);

        accRear = new GameObject("RearAccessory");
        accRear.AddComponent<MeshFilter>();
        accRear.GetComponent<MeshFilter>().mesh = CustomMeshManager.instance.accessoryMeshes[0];
        accRear.AddComponent<MeshRenderer>();
        accRear.GetComponent<MeshRenderer>().material = CustomMeshManager.instance.accMats[0];
        accRear = Instantiate(accRear, PartMaster.instance.GetPart(PartMaster.instance.rearSpokes).transform);

        barAccessory = new GameObject("barAccessory");
        barAccessory.AddComponent<MeshFilter>();
        barAccessory.GetComponent<MeshFilter>().mesh = CustomMeshManager.instance.accessoryMeshes[0];
        barAccessory.AddComponent<MeshRenderer>();
        barAccessory.GetComponent<MeshRenderer>().material = MaterialManager.instance.defaultMat;
        barAccessory = Instantiate(barAccessory, PartMaster.instance.GetPart(PartMaster.instance.bars).transform);

        frameAccesory = new GameObject("frameAccesory");
        frameAccesory.AddComponent<MeshFilter>();
        frameAccesory.GetComponent<MeshFilter>().mesh = CustomMeshManager.instance.accessoryMeshes[0];
        frameAccesory.AddComponent<MeshRenderer>();
        frameAccesory.GetComponent<MeshRenderer>().material = MaterialManager.instance.defaultMat;
        frameAccesory = Instantiate(frameAccesory, PartMaster.instance.GetPart(PartMaster.instance.frame).transform);

        frontHubGuard = new GameObject("frontHubGuard");
        frontHubGuard.AddComponent<MeshFilter>();
        frontHubGuard.GetComponent<MeshFilter>().mesh = CustomMeshManager.instance.accessoryMeshes[0];
        frontHubGuard.AddComponent<MeshRenderer>();
        frontHubGuard.GetComponent<MeshRenderer>().material = MaterialManager.instance.defaultMat;
        frontHubGuard = Instantiate(frontHubGuard, PartMaster.instance.GetPart(PartMaster.instance.frontPegs).transform);

        rearHubGuard = new GameObject("rearHubGuard");
        rearHubGuard.AddComponent<MeshFilter>();
        rearHubGuard.GetComponent<MeshFilter>().mesh = CustomMeshManager.instance.accessoryMeshes[0];
        rearHubGuard.AddComponent<MeshRenderer>();
        rearHubGuard.GetComponent<MeshRenderer>().material = MaterialManager.instance.defaultMat;
        rearHubGuard = Instantiate(rearHubGuard, PartMaster.instance.GetPart(PartMaster.instance.rearPegs).transform);

        partList.Add(frontAcc, accFront);
        partList.Add(rearAcc, accRear);
        partList.Add(barAcc, barAccessory);
        partList.Add(frameAcc, frameAccesory);
        partList.Add(frontHubG, frontHubGuard);
        partList.Add(rearHubG, rearHubGuard);

        SetMaterial(forks, MaterialManager.instance.defaultMat);
        SetMaterial(rightCrank, MaterialManager.instance.defaultMat);
        SetMaterial(leftCrank, MaterialManager.instance.defaultMat);

        foreach (KeyValuePair<int, GameObject> pair in partList) {
            origTrans.Add(pair.Key, new TransformData(pair.Value.transform));
        }
    }

    public void SetMaterialData(int key, float glossiness, float glossMapScale, float metallic, float texTileX, float texTileY, float normTileX, float normTileY, float metTileX, float metTileY)
    {
        try
        {
            Material[] material = GetMaterials(key);
            if (material == null)
                return;
            material[0].SetFloat("_Glossiness", glossiness);
            material[0].SetFloat("_GlossMapScale", glossMapScale);
            material[0].SetFloat("_Metallic", metallic);
            material[0].SetTextureScale("_MainTex", new Vector2(texTileX, texTileY));
            material[0].SetTextureScale("_BumpMap", new Vector2(normTileX, normTileY));
            material[0].SetTextureScale("_MetallicGlossMap", new Vector2(metTileX, metTileY));
            SetMaterials(key, material);
        }
        catch (Exception e)
        {
            Debug.Log("Error in SetMaterialData(). " + e.Message + "\n" + e.StackTrace);
        }
    }

    /// <summary>
    /// Finds and stores all bike parts for use in other classes
    /// </summary>
    public void InitPartList()
    {
        string errorPath = Application.dataPath + "//GarageContent/GarageErrorLog.txt";
        try
        {
            partList = new Dictionary<int, GameObject>();
            localBMX = GameObject.Find("BMX");
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
                        {
                            partList.Add(leftGrip, tran1[0].gameObject);
                        }
                        else
                        {
                            partList.Add(leftGrip, tran1[1].gameObject);
                        }
                        break;
                    case "Right Grip":
                        Transform[] tran2 = t.gameObject.GetComponentsInChildren<Transform>();
                        if (tran2[0].gameObject.name == "Left Grip Mesh")
                        {
                            partList.Add(rightGrip, tran2[0].gameObject);
                        }
                        else
                        {
                            partList.Add(rightGrip, tran2[1].gameObject);
                        }
                        break;
                    case "Left Grip Mesh 01":
                        partList.Add(leftGripFlange, t.gameObject);
                        break;
                    case "Right Grip Mesh 01":
                        partList.Add(rightGripFlange, t.gameObject);
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
                        partList.Add(leftPedalJoint, t.gameObject);
                        Transform[] tran1 = t.gameObject.GetComponentsInChildren<Transform>();
                        foreach (Transform tr in tran1)
                        {
                            if (tr.gameObject.name.Equals("Pedal Mesh"))
                                partList.Add(leftPedal, tr.gameObject);
                            if (tr.gameObject.name.Equals("Pedal_01_axis"))
                                partList.Add(leftPedalAxle, tr.gameObject);
                            if (tr.gameObject.name.Equals("Pedal Cap Mesh"))
                                partList.Add(leftPedalCap, tr.gameObject);
                            if (tr.gameObject.name.Equals("leftPedalTarget"))
                                partList.Add(leftPedalTarget, tr.gameObject);

                        }
                        break;
                    case "BMX:RightPedal_Joint":
                        partList.Add(rightPedalJoint, t.gameObject);
                        Transform[] tran2 = t.gameObject.GetComponentsInChildren<Transform>();
                        foreach (Transform tr in tran2)
                        {
                            if(tr.gameObject.name.Equals("Pedal Mesh"))
                                partList.Add(rightPedal, tr.gameObject);
                            if (tr.gameObject.name.Equals("Pedal_01_axis"))
                                partList.Add(rightPedalAxle, tr.gameObject);
                            if (tr.gameObject.name.Equals("Pedal Cap Mesh"))
                                partList.Add(rightPedalCap, tr.gameObject);
                            if (tr.gameObject.name.Equals("leftPedalTarget"))
                                partList.Add(rightPedalTarget, tr.gameObject);

                        }
                        break;
                    case "BMX:DriveTrain_Joint":
                        partList.Add(driveTrain, t.gameObject);
                        break;
                    default:
                        break;
                }
            }
            partList.Add(barsJ, GameObject.Find("BMX:Bars_Joint"));
            partList.Add(frameJ, GameObject.Find("BMX:Frame_Joint"));
            partList.Add(frontWheelCol, GameObject.Find("FrontWheelCollider"));
            partList.Add(rearWheelCol, GameObject.Find("BackWheelCollider"));
        }
        catch (Exception e)
        {
            File.AppendAllText(errorPath, "\n" + DateTime.Now + "\nRANDOM ERRORS: " + "Error while initializing part list in PartMaster.cs. " + e.Message + e.StackTrace);
            Debug.Log("Error while initializing part list in PartMaster.cs. " + e.Message + e.StackTrace);
        }
        isDone = true;
    }

    /// <summary>
    ///  Get a bike part's GameObject
    /// </summary>
    /// <param name="key"> The part number associated with the bike part </param>
    /// <returns> The GameObject at partList[key] </returns>
    public GameObject GetPart(int key)
    {
        if (!partList.ContainsKey(key))
        {
            Debug.Log("Key not found in part list at GetPart() method");
            return null;
        }

        return partList[key];
    }

    /// <summary>
    /// Get a bike part's mesh
    /// </summary>
    /// <param name="key"> The part number associated with the bike part </param>
    /// <returns> The mesh at partList[key] </returns>
    public Mesh GetMesh(int key)
    {
        if (!partList.ContainsKey(key))
        {
            Debug.Log("Key not found in part list at GetMesh() method");
            return null;
        }
        if (partList[key].GetComponent<MeshFilter>() == null)
        {
            return null;
        }
        return partList[key].GetComponent<MeshFilter>().mesh;
    }

    /// <summary>
    /// Change the mesh of a bike part
    /// </summary>
    /// <param name="key"> The part number associated with the bike part </param>
    /// <param name="mesh"> The new mesh to change to </param>
    public void SetMesh(int key, Mesh mesh)
    {
        if (!partList.ContainsKey(key))
        {
            Debug.Log("Key not found in part list at SetMesh() method");
            return;
        }
        if (partList[key].GetComponent<MeshFilter>() == null)
        {
            return;
        }
        partList[key].GetComponent<MeshFilter>().mesh = mesh;

    }

    /// <summary>
    /// Get the bike part's main material
    /// </summary>
    /// <param name="key"> The part number associated with the bike part </param>
    /// <returns> The Material at partList[key]</returns>
    public Material GetMaterial(int key)
    {
        if (!partList.ContainsKey(key))
        {
            Debug.Log("Key not found in part list at GetMaterial() method");
            return null;
        }
        if (partList[key].GetComponent<MeshRenderer>() == null)
        {
            return null;
        }
        return partList[key].GetComponent<MeshRenderer>().material;
    }

    /// <summary>
    /// Change the material of a bike part
    /// </summary>
    /// <param name="key"> The part number associated with the bike part </param>
    /// <param name="mat"> The material to change to </param>
    public void SetMaterial(int key, Material mat)
    {
        if (!partList.ContainsKey(key))
        {
            Debug.Log("Key not found in part list at SetMaterial() method");
            return;
        }
        if (partList[key].GetComponent<MeshRenderer>() == null)
        {
            return;
        }
        Material[] mats = partList[key].GetComponent<MeshRenderer>().materials;
        if (mats.Length > 1)
        {
            mats[0] = mat;
            SetMaterials(key, mats);
            return;
        }
        partList[key].GetComponent<Renderer>().material = mat;
    }

    /// <summary>
    /// Get all the materials assigned to a bike part
    /// </summary>
    /// <param name="key"> The part number associated with the bike part </param>
    /// <returns> The material array at partList[key] </returns>
    public Material[] GetMaterials(int key)
    {
        if (!partList.ContainsKey(key))
        {
            Debug.Log("Key not found in part list at GetMaterials() method");
            return null;
        }
        if (partList[key].GetComponent<MeshRenderer>() == null)
        {
            return null;
        }
        return partList[key].GetComponent<MeshRenderer>().materials;
    }

    public void SetMaterials(int key, Material[] mats) {
        if (!partList.ContainsKey(key))
        {
            Debug.Log("Key not found in part list at SetMaterial() method");
            return;
        }
        if (partList[key].GetComponent<MeshRenderer>() == null)
        {
            return;
        }

        partList[key].GetComponent<MeshRenderer>().materials = mats;
    }

    public void MovePart(int key, string axis, float pos)
    {
        Transform part = GetPart(key).transform;
        Debug.Log("Moving " + part.gameObject.name);
        if (axis.Equals("x"))
            part.localPosition = new Vector3(part.localPosition.x + pos, part.localPosition.y, part.localPosition.z);
        if (axis.Equals("y"))
            part.localPosition = new Vector3(part.localPosition.x, part.localPosition.y + pos, part.localPosition.z);
        if (axis.Equals("z"))
            part.localPosition = new Vector3(part.localPosition.x, part.localPosition.y, part.localPosition.z + pos);
    }

    public Vector3 GetPosition(int key)
    {
        return GetPart(key).transform.localPosition;
    }

    public void SetPosition(int key, Vector3 pos)
    {
        GetPart(key).transform.localPosition = pos;
    }

    public void SetPartsVisible()
    {
        foreach (int key in ColourSetter.instance.GetActivePartList())
        {
            GameObject obj = GetPart(key);
            obj.SetActive(!obj.activeSelf);
        }
    }

    public void SetPartVisible(int key, bool isVisible)
    {
        GameObject obj = GetPart(key);
        obj.SetActive(isVisible);
    }

    public bool GetPartVisibe(int key)
    {
        GameObject obj = GetPart(key);
        return obj.activeInHierarchy;
    }

    public void Scale(bool positive)
    {
        foreach (int key in ColourSetter.instance.GetActivePartList())
        {
            GameObject obj = GetPart(key);
            /*
            if (key == 46 || key == 47)
            {
                WheelCollider wc = obj.GetComponent<WheelCollider>();
                wc.suspensionDistance = 0.08f;
                wc.forceAppPointDistance = 0.0f;
                JointSpring js = wc.suspensionSpring;
                js.spring = 90000f;
                js.damper = 9000f;
                js.targetPosition = 1;
                if (positive)
                {
                    wc.radius += 0.1f;
                }
                else
                {
                    wc.radius -= 0.1f;
                }
                continue;
            }
            else
            */
            {
                if (positive)
                    obj.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
                else
                    obj.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            }
        }
    }

    public Vector3 GetScale(int key)
    {
        return GetPart(key).transform.localScale;
    }

    public void SetScale(int key, Vector3 scale)
    {
        GameObject obj = GetPart(key);
        obj.transform.localScale = scale;
    }

    public void DuplicatePart()
    {
        foreach (int key in ColourSetter.instance.GetActivePartList())
        {
            GameObject obj = GetPart(key);
            obj = Instantiate(obj, obj.transform.parent);
            //obj.transform.localPosition = new Vector3(0,0,0);
        }
    }

    public void ResetTransforms()
    {
        foreach (int key in ColourSetter.instance.GetActivePartList())
        {
            if (!origTrans.ContainsKey(key))
            {
                Debug.Log("Original transform not found for part number " + key);
                return;
            }    
            GameObject obj = GetPart(key);
            TransformData td = origTrans[key];
            td.ApplyTo(obj.transform);
        }
    }
}

