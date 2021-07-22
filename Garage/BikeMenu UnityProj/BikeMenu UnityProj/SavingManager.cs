using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;



public class SavingManager : MonoBehaviour
{
    public SaveList saveList;
    public SaveList loadList;
    public static SavingManager instance;
    public Text saveName;
    public InputField saveField;
    public Text placeHolder;
    public GameObject infoBox;
    public Text infoBoxText;
    string origInfotxt;

    private string path;
    private string errorPath;
    private string error;
    private string saveErrors;
    private ColourSetter cs;
    private PartManager partManager;
    private MaterialManager matManager;
    private PartMaster partMaster;
    public GameObject overwriteWarning;

    private Material a_glossy;

    string lastSelectedPreset;

    private void Awake()
    {
        // Setup
        path = Application.dataPath + "//GarageContent/GarageSaves/";
        errorPath = Application.dataPath + "//GarageContent/GarageErrorLog.txt";
        cs = FindObjectOfType<ColourSetter>();
        partManager = FindObjectOfType<PartManager>();
        matManager = FindObjectOfType<MaterialManager>();
        partMaster = FindObjectOfType<PartMaster>();
        instance = this;
        File.WriteAllText(errorPath, "");
        lastSelectedPreset = PlayerPrefs.GetString("lastPreset");
        if (File.Exists(Path.Combine(path, lastSelectedPreset + ".preset"))) // Load last used preset if there is one
            StartCoroutine(LoadLastSelected());
        else
            CustomMeshManager.instance.SwitchDefaultParts();
        origInfotxt = infoBoxText.text;
        a_glossy = partMaster.GetMaterial(partMaster.frame);
    }

    public IEnumerator SetDefault()
    {
        while (!CustomMeshManager.instance.isDoneLoading)
            yield return new WaitForEndOfFrame();
        CustomMeshManager.instance.SwitchDefaultParts();
    }

    /// <summary>
    /// Deserializes data from a .preset (xml) file into a SaveList object for loading of saved presets
    /// </summary>
    /// <param name="name"> Name of the preset to be loaded </param>
    public void Deserialize(String name)
    {
        try
        {
            Debug.Log("Starting load of: " + name + ".preset");
            XmlSerializer deserializer = new XmlSerializer(typeof(SaveList));
            TextReader reader = new StreamReader(Application.dataPath + "//GarageContent/GarageSaves/" + name + ".preset");
            object obj = deserializer.Deserialize(reader);
            loadList = (SaveList)obj;
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log("Error while reading from XML: " + e.Message);
        }
    }

    /// <summary>
    /// Serializes data from SaveList class and writes data to .preset (xml) file
    /// </summary>
    /// <param name="name"> Name of the preset to be saved </param>
    public void Serialize(String name)
    {
        try
        {
            Debug.Log("Serializing " + name + ".preset");
            if (String.IsNullOrEmpty(name))
                return;
            Debug.Log("Starting save of: " + name + ".preset");
            var serializer = new XmlSerializer(typeof(SaveList));
            var stream = new FileStream(Application.dataPath + "//GarageContent/GarageSaves/" + name+".preset", FileMode.Create); // Will create new file if file doesn't exist, else overwrites file
            serializer.Serialize(stream, saveList);
            stream.Close();
            saveField.text = "";
        }
        catch (Exception e)
        {
            Debug.Log("Error while writing to XML: " + e.Message);
        }
    }

    /// <summary>
    /// Save a new preset to file or overwrite an existing one
    /// </summary>
    public void Save()
    {
        saveList = new SaveList();
        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }
        if(String.IsNullOrEmpty(saveField.text)) {
            Debug.Log("Save Field cannot be blank. ");
            return;
        }
        SaveSlotNames();
        FindObjectOfType<PresetLister>().CheckFolder();
        File.AppendAllText(errorPath, "\n"+DateTime.Now + "\nSAVING ERRORS: " + saveErrors);
    }

    
    /// <summary>
    /// Load a preset from file
    /// </summary>
    /// <param name="presetName"> name of the preset to be loaded </param>
    public void Load(string presetName)
    {
        try
        {
            LoadSlotNames(presetName);
            PlayerPrefs.SetString("lastPreset", presetName);
        }
        catch(Exception e)
        {
            error += e.Message + "\n " + e.StackTrace + "\n ";
        }
        File.AppendAllText(errorPath, "\n"+DateTime.Now + "\nLOADING ERRORS: " + error);
    }

    /// <summary>
    /// Load a bike save from a .preset file
    /// </summary>
    /// <param name="presetName"> name of the .preset file </param>
    private void LoadSlotNames(string presetName)
    {
        error = "";
        
        bool flag = File.Exists(path + presetName + ".preset");
        if (flag)
        {
            Deserialize(presetName);
            try
            {
                
                TextureManager.instance.SetOriginalTextures();
                LoadBrakes();
                LoadSeatAngle();
                LoadBarsAngle();
                LoadSeatID();
                LoadGripsID();
                LoadBikeScale();
                LoadFrontTireWidth();
                LoadRearTireWidth();
                LoadFlanges();
                LoadSeatHeight();
                LoadDriveSide();
                LoadMaterials();
                LoadTireTread();
                foreach (PartColor p in loadList.partColors) {
                    cs.SetColor(p.partNum, new Color(p.r, p.g, p.b, p.a));
                }
                LoadMeshes();
                LoadTextures();
                LoadPartPositions();

                // Quick fix for weird normal map issue on left crank arm
                Material m = PartMaster.instance.GetMaterial(PartMaster.instance.rightCrank);
                PartMaster.instance.GetPart(PartMaster.instance.leftCrank).GetComponent<MeshRenderer>().material = m;
                Resources.UnloadUnusedAssets();
            }
            catch (Exception e)
            {
                error += e.Message + "\n " + e.StackTrace + "\n " + e.Source + "\n ";
            }
            
        }
        
    }

    /// <summary>
    /// Save a bike to a .preset file
    /// </summary>
    private void SaveSlotNames()
    {
        saveErrors = "";
        try
        {
            Debug.Log("Saving Brakes On/Off...");
            SaveBrakes();
            Debug.Log("Saving Seat Angle...");
            SaveSeatAngle();
            Debug.Log("Saving Bars Angle...");
            SaveBarsAngle();
            Debug.Log("Saving Seat ID...");
            SaveSeatID();
            Debug.Log("Saving Grips ID...");
            SaveGripsID();
            Debug.Log("Saving Bike Scale...");
            SaveBikeScale();
            Debug.Log("Saving Front Tire Width...");
            SaveFrontTireWidth();
            Debug.Log("Saving Rear Tire Width...");
            SaveRearTireWidth();
            Debug.Log("Saving Flanges On / Off...");
            SaveFlanges();
            Debug.Log("Saving Seat Height...");
            SaveSeatHeight();
            Debug.Log("Saving Drive Side...");
            SaveDriveSide();
            Debug.Log("Saving Materials...");
            SaveMaterials();
            Debug.Log("Saving Tire Tread...");
            SaveTireTread();
            Debug.Log("Saving Main Bike Colors...");
            for (int i = 0; i < 44; i++)
            {
                Material m = PartMaster.instance.GetMaterial(i);
                if (m != null)
                {
                    saveList.partColors.Add(new PartColor(i, m.color));
                }
            }
            Debug.Log("Saving Other Colors");
            SaveOtherColors();
            Debug.Log("Saving Meshes...");
            SaveMeshes();
            Debug.Log("Saving Textures...");
            SaveTextures();
            Debug.Log("Writing " + saveName.text + " to File...");
            SavePartPositions();

                bool flag = !File.Exists(path + saveName.text + ".preset");
                if (flag)
                {
                    Serialize(saveName.text);
                }
                else
                {
                    overwriteWarning.SetActive(true);
                }
        }
        catch (Exception e)
        {
            saveErrors += "In SaveSlotNames() method: " + e.Message + "\n " + e.StackTrace + "\n "; 
        }
    }

    /// <summary>
    /// Overwrite a current save
    /// </summary>
    public void SetOverwrite()
    {
        Serialize(saveName.text);
    }

    /// <summary>
    /// Save part positions
    /// </summary>
    public void SavePartPositions()
    {
        foreach (KeyValuePair<int, GameObject> pair in PartMaster.instance.partList)
        {
            saveList.partPositions.Add(new PartPosition(pair.Key, PartMaster.instance.GetPosition(pair.Key), PartMaster.instance.GetScale(pair.Key), PartMaster.instance.GetPartVisibe(pair.Key)));
        }
    }

    /// <summary>
    /// Load part positions
    /// </summary>
    public void LoadPartPositions()
    {
        if(loadList.partPositions.Count == 0)
            return;
        foreach (PartPosition partPos in loadList.partPositions)
        {
            PartMaster.instance.SetPosition(partPos.partNum, new Vector3(partPos.x, partPos.y, partPos.z));
            PartMaster.instance.SetScale(partPos.partNum, new Vector3(partPos.scaleX, partPos.scaleY, partPos.scaleZ));
            PartMaster.instance.SetPartVisible(partPos.partNum, partPos.isVisible);
        }
    }

    /// <summary>
    /// Save the front tire width
    /// </summary>
    private void SaveFrontTireWidth()
    {
        saveList.frontTireWidth = FindObjectOfType<BikeLoadOut>().GetFrontTireFatness();
    }

    /// <summary>
    /// Save the rear tire width
    /// </summary>
    private void SaveRearTireWidth()
    {
        saveList.rearTireWidth = FindObjectOfType<BikeLoadOut>().GetBackTireFatness();
    }

    /// <summary>
    /// Load the front tire width
    /// </summary>
    private void LoadFrontTireWidth()
    {
        try
        {
            float width = loadList.frontTireWidth;
            FindObjectOfType<BikeLoadOut>().SetFrontTireFatness(width);
        }
        catch (Exception e)
        {
            error += e.Message + "\n " + e.StackTrace + "\n ";
        }
    }

    /// <summary>
    /// Load the rear tire width
    /// </summary>
    private void LoadRearTireWidth()
    {
        try
        {
            float width = loadList.rearTireWidth;
            FindObjectOfType<BikeLoadOut>().SetBackTireFatness(width);
        }
        catch (Exception e)
        {
            error += e.Message + "\n " + e.StackTrace + "\n ";
        }
    }

    /// <summary>
    /// Save the seat angle
    /// </summary>
    private void SaveSeatAngle()
    {
        saveList.seatAngle = FindObjectOfType<SeatApplyMod>().GetSeatAnglePerc();
    }

    /// <summary>
    /// Load the seat angle
    /// </summary>
    private void LoadSeatAngle()
    {
        try
        {
            float angle = loadList.seatAngle;
            FindObjectOfType<SeatApplyMod>().SetSeatAnglePerc(angle);
            partManager.seatAngleSlider.value = angle;
        }
        catch (Exception e)
        {
            error += e.Message + "\n " + e.StackTrace + "\n ";
        }
    }

    /// <summary>
    /// Save the bars angle
    /// </summary>
    private void SaveBarsAngle()
    {
        saveList.barsAngle = FindObjectOfType<BarsApplyMod>().GetBarsAnglePerc();
    }

    /// <summary>
    /// Load the bars angle
    /// </summary>
    private void LoadBarsAngle()
    {
        try
        {
            float angle = loadList.barsAngle;
            FindObjectOfType<BarsApplyMod>().SetBarsAnglePerc(angle);
            partManager.barsAngleSlider.value = angle;
        }
        catch (Exception e)
        {
            error += e.Message + "\n " + e.StackTrace + "\n ";
        }
    }

    /// <summary>
    /// Save brakes either off or on
    /// </summary>
    private void SaveBrakes()
    {
        saveList.brakes = BrakesManager.instance.IsEnabled();
    }

    /// <summary>
    /// Load brakes either off or on
    /// </summary>
    private void LoadBrakes()
    {
        BrakesManager.instance.SetBrakes(loadList.brakes);
    }

    /// <summary>
    /// Save the tire tread material
    /// </summary>
    private void SaveTireTread()
    {
        saveList.frontTreadID = partManager.frontTiresCount == 0 ? 3 : partManager.frontTiresCount - 1;
        saveList.rearTreadID = partManager.rearTiresCount == 0 ? 3 : partManager.rearTiresCount - 1;
        saveList.frontWallID = partManager.frontTireWallCount == 0 ? 3 : partManager.frontTireWallCount - 1;
        saveList.rearWallID = partManager.rearTireWallCount == 0 ? 3 : partManager.rearTireWallCount - 1;
    }

    /// <summary>
    /// Load the tire tread material
    /// </summary>
    private void LoadTireTread()
    {
        partManager.SetFrontTireTread(loadList.frontTreadID);
        partManager.SetFrontTireWall(loadList.frontWallID);
        partManager.SetRearTireTread(loadList.rearTreadID);
        partManager.SetRearTireWall(loadList.rearWallID);
    }

    /// <summary>
    /// Save the seat material
    /// </summary>
    private void SaveSeatID()
    {
        saveList.seatID = partManager.seatCount - 1;
    }

    /// <summary>
    /// Load the seat material
    /// </summary>
    private void LoadSeatID()
    {
        partManager.SetSeatID(loadList.seatID);
    }

    /// <summary>
    /// Save the grips material
    /// </summary>
    private void SaveGripsID()
    {
        saveList.gripsID = partManager.gripsCount - 1;
    }

    /// <summary>
    /// Load the grips material
    /// </summary>
    private void LoadGripsID()
    {
        partManager.SetGripsId(loadList.gripsID);
    }

    /// <summary>
    /// Save the scale (size) of the bike
    /// </summary>
    private void SaveBikeScale()
    {
        saveList.bikeScale = partManager.bikeScaleSlider.value;
    }

    /// <summary>
    /// Load the scale (size) of the bike
    /// </summary>
    private void LoadBikeScale()
    {
        partManager.SetBikeScale(loadList.bikeScale);
    }

    /// <summary>
    /// Save the colors of the bike parts that were split into separate parts
    /// </summary>
    private void SaveOtherColors()
    {
        try
        {
            saveList.partColors.Add(new PartColor(-1, PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[1].color));
            saveList.partColors.Add(new PartColor(-2, PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[1].color));
            saveList.partColors.Add(new PartColor(-3, cs.GetBrakeColor()));
            saveList.partColors.Add(new PartColor(-4, cs.GetCableColor()));
        }
        catch (Exception e)
        {
            saveErrors += "In SaveOtherColors() method: " + e.Message + "\n " + e.StackTrace + "\n ";
        }
    }

    /// <summary>
    /// Save grip flanges on or off
    /// </summary>
    private void SaveFlanges()
    {
        saveList.flanges = partManager.GetFlangesVisible();
    }

    /// <summary>
    /// Load grip flanges on or off
    /// </summary>
    private void LoadFlanges()
    {
            if (!loadList.flanges)
            {
                partManager.SetFlangesOff();
            }
            else
            {
                partManager.SetFlangesOn();
            }
    }

    /// <summary>
    /// Save seat height
    /// </summary>
    private void SaveSeatHeight()
    {
        saveList.seatHeight = partManager.GetSeatHeight();
    }

    /// <summary>
    /// Load seat height
    /// </summary>
    private void LoadSeatHeight()
    {
        partManager.SetSeatHeight(loadList.seatHeight);
    }

    /// <summary>
    /// Save the drive side either left hand drive or right hand drive
    /// </summary>
    private void SaveDriveSide()
    {
        saveList.LHD = partManager.GetDriveSide();
    }

    /// <summary>
    /// Load the drive side either left hand drive or right hand drive
    /// </summary>
    private void LoadDriveSide()
    {
        if (loadList.LHD)
        {
            partManager.SetLHD();
        }
        else
        {
            partManager.SetRHD();
        }
    }

    /// <summary>
    /// Save the part materials
    /// </summary>
    private void SaveMaterials()
    {
        string debug = "";
        try
        {
            for (int i = 0; i < 20; i++)
            {
                Material mat = FindObjectOfType<BikeLoadOut>().GetPartMat(i);
                debug += mat.name.ToLower() + "\n ";
                switch (mat.name.ToLower())
                {
                    case "a_glossy (instance)":
                        saveList.partMaterials.Add(new PartMaterial(0, i));
                        break;
                    case "a_glossy (instance) (instance)":
                        saveList.partMaterials.Add(new PartMaterial(0, i));
                        break;
                    case "jetfuel (instance)":
                        saveList.partMaterials.Add(new PartMaterial(1, i));
                        break;
                    case "flat (instance)":
                        saveList.partMaterials.Add(new PartMaterial(2, i));
                        break;
                    case "chrome (instance)":
                        saveList.partMaterials.Add(new PartMaterial(3, i));
                        break;
                    case "bubble (instance)":
                        saveList.partMaterials.Add(new PartMaterial(4, i));
                        break;
                    case "rusty (instance)":
                        saveList.partMaterials.Add(new PartMaterial(5, i));
                        break;
                    case "green jetfuel (instance)":
                        saveList.partMaterials.Add(new PartMaterial(6, i));
                        break;
                    case "spokes (instance)":
                        saveList.partMaterials.Add(new PartMaterial(0, i));
                        break;
                    case "forks (instance)":
                        saveList.partMaterials.Add(new PartMaterial(0, i));
                        break;
                    case "nipples (instance)":
                        saveList.partMaterials.Add(new PartMaterial(0, i));
                        break;
                    case "wheels (instance)":
                        saveList.partMaterials.Add(new PartMaterial(7, i));
                        break;
                    default:
                        saveList.partMaterials.Add(new PartMaterial(9, i));
                        break;
                }
            }

            saveList.seatPostMat = GetMaterialHelper(PartMaster.instance.seatPost);
            saveList.frontTireMat = GetMaterialHelper(PartMaster.instance.frontTire);
            saveList.rearTireMat = GetMaterialHelper(PartMaster.instance.rearTire);
            saveList.frontTireWallMat = GetMaterialHelper(PartMaster.instance.frontTire, 1);
            saveList.rearTireWallMat = GetMaterialHelper(PartMaster.instance.rearTire, 1);
            saveList.frontRimMat = GetMaterialHelper(PartMaster.instance.frontRim);
            saveList.rearRimMat = GetMaterialHelper(PartMaster.instance.rearRim);
            saveList.frontHubMat = GetMaterialHelper(PartMaster.instance.frontHub);
            saveList.rearHubMat = GetMaterialHelper(PartMaster.instance.rearHub);
            saveList.frontSpokesMat = GetMaterialHelper(PartMaster.instance.frontSpokes);
            saveList.rearSpokesMat = GetMaterialHelper(PartMaster.instance.rearSpokes);
            saveList.frontNipplesMat = GetMaterialHelper(PartMaster.instance.frontNipples);
            saveList.rearNipplesMat = GetMaterialHelper(PartMaster.instance.rearNipples);

            foreach (KeyValuePair<int, GameObject> pair in PartMaster.instance.partList)
            {
                Material material = PartMaster.instance.GetMaterial(pair.Key);
                if (material == null)
                    continue;
                saveList.matData.Add(new MatData(pair.Key, material.GetFloat("_Glossiness"), material.GetFloat("_GlossMapScale")));
            }
        }
        catch (Exception e)
        {
            saveErrors += e.Message + "\n " + e.StackTrace + "\n ";
        }
    }

    /// <summary>
    /// Helper method to save part materials
    /// </summary>
    /// <param name="key"> The key to get the bike part from the PartMaster class </param>
    /// <param name="index"> Optional parameter to select a material from a material array </param>
    /// <returns> Material ID number </returns>
    private int GetMaterialHelper(int key, int index = 0)
    {

        Material material = PartMaster.instance.GetMaterials(key)[index];
        int result;
        string text = material.name.ToLower();
        switch (text)
        {
            case "a_glossy (instance)":
                result = 0;
                break;
            case "a_glossy (instance) (instance)":
                result = 0;
                break;
            case "jetfuel (instance)":
                result = 1;
                break;
            case "flat (instance)":
                result = 2;
                break;
            case "chrome (instance)":
                result = 3;
                break;
            case "bubble (instance)":
                result = 4;
                break;
            case "rusty (instance)":
                result = 5;
                break;
            case "green jetfuel (instance)":
                result = 6;
                break;
            default:
                result = 9;
                break;
        }
        return result;
    }

    /// <summary>
    /// Load bike part materials
    /// </summary>
    private void LoadMaterials()
    {
        try
        {
            foreach (PartMaterial p in loadList.partMaterials)
            {
                switch (p.matID)
                {
                    case 0:
                        FindObjectOfType<BikeLoadOut>().SetPartMaterial(MaterialManager.instance.defaultMat, p.partNum, true);
                        break;
                    case 7:
                        break;
                    case 9:
                        break;
                    default:
                        FindObjectOfType<BikeLoadOut>().SetPartMaterial(matManager.customMats[p.matID-1], p.partNum, true);
                        break;
                }

            }
            SetMaterialHelper(PartMaster.instance.seatPost, loadList.seatPostMat);
            SetMaterialHelper(PartMaster.instance.frontTire, loadList.frontTireMat);
            SetMaterialHelper(PartMaster.instance.rearTire, loadList.rearTireMat);
            SetMaterialHelper(PartMaster.instance.frontTire, loadList.frontTireWallMat, 1);
            SetMaterialHelper(PartMaster.instance.rearTire, loadList.rearTireWallMat, 1);
            SetMaterialHelper(PartMaster.instance.frontRim, loadList.frontRimMat);
            SetMaterialHelper(PartMaster.instance.rearRim, loadList.rearRimMat);
            SetMaterialHelper(PartMaster.instance.frontHub, loadList.frontHubMat);
            SetMaterialHelper(PartMaster.instance.rearHub, loadList.rearHubMat);
            SetMaterialHelper(PartMaster.instance.frontSpokes, loadList.frontSpokesMat);
            SetMaterialHelper(PartMaster.instance.rearSpokes, loadList.rearSpokesMat);
            SetMaterialHelper(PartMaster.instance.frontNipples, loadList.frontNipplesMat);
            SetMaterialHelper(PartMaster.instance.rearNipples, loadList.rearNipplesMat);
            foreach (MatData matData in loadList.matData)
            {
                PartMaster.instance.SetMaterialData(matData.key, matData.glossiness, matData.glossMapScale);
            }
        }
        catch (Exception e)
        {
            error += e.Message + "\n " + e.StackTrace + "\n ";
        }
    }

    /// <summary>
    /// Helper method to load bike part materials
    /// </summary>
    /// <param name="key"> Key to use to get bike part from PartMaster class </param>
    /// <param name="mat"> Material ID to set </param>
    /// <param name="index"> Optional parameter to set a specific material in a material array </param>
    public void SetMaterialHelper(int key, int mat, int index = 0)
    {
        if (mat == 0)
            PartMaster.instance.GetPart(key).GetComponent<MeshRenderer>().materials[index] = a_glossy;
        else if (mat == 9)
            return;
        else
            PartMaster.instance.GetPart(key).GetComponent<MeshRenderer>().materials[index] = matManager.customMats[mat - 1];
    }

    private void SaveTextures()
    {
        try
        {
            foreach (KeyValuePair<int, string> entry in TextureManager.instance.albedoList) {
                saveList.partTextures.Add(new PartTexture(entry.Value, entry.Key, false, false));
            }

            // Normal Maps
            foreach (KeyValuePair<int, string> entry in TextureManager.instance.normalList)
            {
                saveList.partTextures.Add(new PartTexture(entry.Value, entry.Key, true, false));
            }

            //Metallic Maps

            foreach (KeyValuePair<int, string> entry in TextureManager.instance.metallicList)
            {
                saveList.partTextures.Add(new PartTexture(entry.Value, entry.Key, false, true));
            }

        }
        catch (Exception e)
        {
            saveErrors += e.Message + "\n " + e.StackTrace + "\n ";
        }
    }

    /// <summary>
    /// Load bike part textures
    /// </summary>
    private void LoadTextures()
    {
        try
        {
            foreach (PartTexture p in loadList.partTextures)
            {
                Debug.Log("Loading Texture: " + p.url);
                if (!p.url.Equals("") && !p.url.Equals("."))
                {
                    if (!p.metallic && !p.normal)
                        TextureManager.instance.SetTexture(p.partNum, p.url);
                    else if (p.normal)
                        TextureManager.instance.SetNormal(p.partNum, p.url);
                    else
                        TextureManager.instance.SetMetallic(p.partNum, p.url);
                }
                else if(p.url.Equals("."))
                {
                    if (!p.metallic && !p.normal)
                        TextureManager.instance.RemoveTexture(p.partNum);
                    else if (p.normal)
                        TextureManager.instance.RemoveNormal(p.partNum);
                    else
                        TextureManager.instance.RemoveMetallic(p.partNum);
                }
            }
        } catch (Exception e)
        {
            Debug.Log("An error occured when loading textures. " + e.Message + " : " + e.StackTrace);
        }
    }

    /// <summary>
    /// Save bike part meshes
    /// </summary>
    private void SaveMeshes()
    {
        int index = (CustomMeshManager.instance.selectedFrame - 1) % CustomMeshManager.instance.frames.Count;
        saveList.partMeshes.Add(new PartMesh(index, CustomMeshManager.instance.frames[index].isCustom, CustomMeshManager.instance.frames[index].fileName, "frame"));

        index = (CustomMeshManager.instance.selectedBars - 1) % CustomMeshManager.instance.bars.Count;
        saveList.partMeshes.Add(new PartMesh(index, CustomMeshManager.instance.bars[index].isCustom, CustomMeshManager.instance.bars[index].fileName, "bars"));

        index = (CustomMeshManager.instance.selectedSprocket - 1) % CustomMeshManager.instance.sprockets.Count;
        saveList.partMeshes.Add(new PartMesh(index, CustomMeshManager.instance.sprockets[index].isCustom, CustomMeshManager.instance.sprockets[index].fileName, "sprocket"));

        index = (CustomMeshManager.instance.selectedStem - 1) % CustomMeshManager.instance.stems.Count;
        saveList.partMeshes.Add(new PartMesh(index, CustomMeshManager.instance.stems[index].isCustom, CustomMeshManager.instance.stems[index].fileName, "stem"));

        index = (CustomMeshManager.instance.selectedCranks - 1) % CustomMeshManager.instance.cranks.Count;
        saveList.partMeshes.Add(new PartMesh(index, CustomMeshManager.instance.cranks[index].isCustom, CustomMeshManager.instance.cranks[index].fileName, "cranks"));

        index = (CustomMeshManager.instance.selectedFrontSpokes - 1) % CustomMeshManager.instance.spokes.Count;
        saveList.partMeshes.Add(new PartMesh(index, CustomMeshManager.instance.spokes[index].isCustom, CustomMeshManager.instance.spokes[index].fileName, "frontSpokes"));

        index = (CustomMeshManager.instance.selectedRearSpokes - 1) % CustomMeshManager.instance.spokes.Count;
        saveList.partMeshes.Add(new PartMesh(index, CustomMeshManager.instance.spokes[index].isCustom, CustomMeshManager.instance.spokes[index].fileName, "rearSpokes"));

        index = (CustomMeshManager.instance.selectedForks - 1) % CustomMeshManager.instance.forks.Count;
        saveList.partMeshes.Add(new PartMesh(index, CustomMeshManager.instance.forks[index].isCustom, CustomMeshManager.instance.forks[index].fileName, "forks"));

        index = (CustomMeshManager.instance.selectedFrontPegs - 1) % CustomMeshManager.instance.pegs.Count;
        saveList.partMeshes.Add(new PartMesh(index, CustomMeshManager.instance.pegs[index].isCustom, CustomMeshManager.instance.pegs[index].fileName, "frontPegs"));

        index = (CustomMeshManager.instance.selectedRearPegs - 1) % CustomMeshManager.instance.pegs.Count;
        saveList.partMeshes.Add(new PartMesh(index, CustomMeshManager.instance.pegs[index].isCustom, CustomMeshManager.instance.pegs[index].fileName, "rearPegs"));

        index = (CustomMeshManager.instance.selectedFrontHub - 1) % CustomMeshManager.instance.hubs.Count;
        saveList.partMeshes.Add(new PartMesh(index, CustomMeshManager.instance.hubs[index].isCustom, CustomMeshManager.instance.hubs[index].fileName, "frontHub"));

        index = (CustomMeshManager.instance.selectedRearHub - 1) % CustomMeshManager.instance.hubs.Count;
        saveList.partMeshes.Add(new PartMesh(index, CustomMeshManager.instance.hubs[index].isCustom, CustomMeshManager.instance.hubs[index].fileName, "rearHub"));

        index = (CustomMeshManager.instance.selectedSeat - 1) % CustomMeshManager.instance.seats.Count;
        saveList.partMeshes.Add(new PartMesh(index, CustomMeshManager.instance.seats[index].isCustom, CustomMeshManager.instance.seats[index].fileName, "seat"));

        index = (CustomMeshManager.instance.selectedPedals - 1) % CustomMeshManager.instance.pedals.Count;
        saveList.partMeshes.Add(new PartMesh(index, CustomMeshManager.instance.pedals[index].isCustom, CustomMeshManager.instance.pedals[index].fileName, "pedals"));

        index = (CustomMeshManager.instance.selectedFrontAccessory - 1) % CustomMeshManager.instance.accessories.Count;
        saveList.partMeshes.Add(new PartMesh(index, CustomMeshManager.instance.accessories[index].isCustom, CustomMeshManager.instance.accessories[index].fileName, "frontSpokeAccessory"));

        index = (CustomMeshManager.instance.selectedRearAccessory - 1) % CustomMeshManager.instance.accessories.Count;
        saveList.partMeshes.Add(new PartMesh(index, CustomMeshManager.instance.accessories[index].isCustom, CustomMeshManager.instance.accessories[index].fileName, "rearSpokeAccessory"));

        index = (CustomMeshManager.instance.selectedFrontRim - 1) % CustomMeshManager.instance.rims.Count;
        saveList.partMeshes.Add(new PartMesh(index, CustomMeshManager.instance.rims[index].isCustom, CustomMeshManager.instance.rims[index].fileName, "frontRim"));

        index = (CustomMeshManager.instance.selectedRearRim - 1) % CustomMeshManager.instance.rims.Count;
        saveList.partMeshes.Add(new PartMesh(index, CustomMeshManager.instance.rims[index].isCustom, CustomMeshManager.instance.rims[index].fileName, "rearRim"));

        index = (CustomMeshManager.instance.selectedBarAccessory - 1) % CustomMeshManager.instance.barAccessories.Count;
        saveList.partMeshes.Add(new PartMesh(index, CustomMeshManager.instance.barAccessories[index].isCustom, CustomMeshManager.instance.barAccessories[index].fileName, "barAccessory"));

        index = (CustomMeshManager.instance.selectedFrameAccessory - 1) % CustomMeshManager.instance.frameAccessories.Count;
        saveList.partMeshes.Add(new PartMesh(index, CustomMeshManager.instance.frameAccessories[index].isCustom, CustomMeshManager.instance.frameAccessories[index].fileName, "frameAccessory"));

        index = (CustomMeshManager.instance.selectedFrontHubGuard - 1) % CustomMeshManager.instance.frontHubGuards.Count;
        saveList.partMeshes.Add(new PartMesh(index, CustomMeshManager.instance.frontHubGuards[index].isCustom, CustomMeshManager.instance.frontHubGuards[index].fileName, "frontHubGuard"));

        index = (CustomMeshManager.instance.selectedRearHubGuard - 1) % CustomMeshManager.instance.rearHubGuards.Count;
        saveList.partMeshes.Add(new PartMesh(index, CustomMeshManager.instance.rearHubGuards[index].isCustom, CustomMeshManager.instance.rearHubGuards[index].fileName, "rearHubGuard"));

        index = (CustomMeshManager.instance.selectedSeatPost - 1) % CustomMeshManager.instance.seatPosts.Count;
        saveList.partMeshes.Add(new PartMesh(index, CustomMeshManager.instance.seatPosts[index].isCustom, CustomMeshManager.instance.seatPosts[index].fileName, "seatPost"));

    }

    /// <summary>
    /// Change the text that is displayed on the alert box
    /// </summary>
    /// <param name="message"> Message to display </param>
    public void ChangeAlertText(string message)
    {
        infoBoxText.text = message;
    }

    /// <summary>
    /// Load bike part meshes
    /// </summary>
    private void LoadMeshes()
    {
        try
        {
            foreach (PartMesh pm in loadList.partMeshes)
            {
                if (pm.isCustom) {
                    if (!File.Exists(pm.fileName)) {
                        ChangeAlertText("Error loading mesh: " + pm.fileName + " is a dependancy of this save file and could not be found. The save will continue to load, but it will load the default mesh in place of the missing custom mesh.");
                        if(!infoBox.activeSelf)
                            infoBox.SetActive(true);
                    }
                }
                switch (pm.partName)
                {
                    case "frame":
                        if((pm.isCustom && File.Exists(pm.fileName)) || !pm.isCustom)
                            CustomMeshManager.instance.SetFrameMesh(pm.partNum);
                        else if(pm.isCustom && !File.Exists(pm.fileName))
                            CustomMeshManager.instance.SetFrameMesh(0);
                        break;
                    case "bars":
                        if ((pm.isCustom && File.Exists(pm.fileName)) || !pm.isCustom)
                            CustomMeshManager.instance.SetBarsMesh(pm.partNum);
                        else if (pm.isCustom && !File.Exists(pm.fileName))
                            CustomMeshManager.instance.SetBarsMesh(0);
                        break;
                    case "sprocket":
                        if ((pm.isCustom && File.Exists(pm.fileName)) || !pm.isCustom)
                            CustomMeshManager.instance.SetSprocketMesh(pm.partNum);
                        else if (pm.isCustom && !File.Exists(pm.fileName))
                            CustomMeshManager.instance.SetSprocketMesh(0);
                        break;
                    case "stem":
                        if ((pm.isCustom && File.Exists(pm.fileName)) || !pm.isCustom)
                            CustomMeshManager.instance.SetStemMesh(pm.partNum);
                        else if (pm.isCustom && !File.Exists(pm.fileName))
                            CustomMeshManager.instance.SetStemMesh(0);
                        break;
                    case "cranks":
                        if ((pm.isCustom && File.Exists(pm.fileName)) || !pm.isCustom)
                            CustomMeshManager.instance.SetCranksMesh(pm.partNum);
                        else if (pm.isCustom && !File.Exists(pm.fileName))
                            CustomMeshManager.instance.SetCranksMesh(0);
                        break;
                    case "frontSpokes":
                        if ((pm.isCustom && File.Exists(pm.fileName)) || !pm.isCustom)
                            CustomMeshManager.instance.SetFrontSpokesMesh(pm.partNum);
                        else if (pm.isCustom && !File.Exists(pm.fileName))
                            CustomMeshManager.instance.SetFrontSpokesMesh(0);
                        break;
                    case "rearSpokes":
                        if ((pm.isCustom && File.Exists(pm.fileName)) || !pm.isCustom)
                            CustomMeshManager.instance.SetRearSpokesMesh(pm.partNum);
                        else if (pm.isCustom && !File.Exists(pm.fileName))
                            CustomMeshManager.instance.SetRearSpokesMesh(0);
                        break;
                    case "pedals":
                        if ((pm.isCustom && File.Exists(pm.fileName)) || !pm.isCustom)
                            CustomMeshManager.instance.SetPedalsMesh(pm.partNum);
                        else if (pm.isCustom && !File.Exists(pm.fileName))
                            CustomMeshManager.instance.SetPedalsMesh(0);
                        break;
                    case "forks":
                        if ((pm.isCustom && File.Exists(pm.fileName)) || !pm.isCustom)
                            CustomMeshManager.instance.SetForksMesh(pm.partNum);
                        else if (pm.isCustom && !File.Exists(pm.fileName))
                            CustomMeshManager.instance.SetForksMesh(0);
                        break;
                    case "frontPegs":
                        if ((pm.isCustom && File.Exists(pm.fileName)) || !pm.isCustom)
                            CustomMeshManager.instance.SetFrontPegsMesh(pm.partNum);
                        else if (pm.isCustom && !File.Exists(pm.fileName))
                            CustomMeshManager.instance.SetFrontPegsMesh(0);
                        break;
                    case "rearPegs":
                        if ((pm.isCustom && File.Exists(pm.fileName)) || !pm.isCustom)
                            CustomMeshManager.instance.SetRearPegsMesh(pm.partNum);
                        else if (pm.isCustom && !File.Exists(pm.fileName))
                            CustomMeshManager.instance.SetRearPegsMesh(0);
                        break;
                    case "frontHub":
                        if ((pm.isCustom && File.Exists(pm.fileName)) || !pm.isCustom)
                                CustomMeshManager.instance.SetFrontHubMesh(pm.partNum);
                        else if (pm.isCustom && !File.Exists(pm.fileName))
                                CustomMeshManager.instance.SetFrontHubMesh(0);
                            break;
                    case "rearHub":
                        if ((pm.isCustom && File.Exists(pm.fileName)) || !pm.isCustom)
                            CustomMeshManager.instance.SetRearHubMesh(pm.partNum);
                        else if (pm.isCustom && !File.Exists(pm.fileName))
                            CustomMeshManager.instance.SetRearHubMesh(0);
                            break;
                    case "seat":
                        if ((pm.isCustom && File.Exists(pm.fileName)) || !pm.isCustom)
                            CustomMeshManager.instance.SetSeatMesh(pm.partNum);
                        else if (pm.isCustom && !File.Exists(pm.fileName))
                            CustomMeshManager.instance.SetSeatMesh(0);
                        break;
                    case "frontRim":
                        if ((pm.isCustom && File.Exists(pm.fileName)) || !pm.isCustom)
                            CustomMeshManager.instance.SetFrontRimMesh(pm.partNum);
                        else if (pm.isCustom && !File.Exists(pm.fileName))
                            CustomMeshManager.instance.SetFrontRimMesh(0);
                        break;
                    case "rearRim":
                        if ((pm.isCustom && File.Exists(pm.fileName)) || !pm.isCustom)
                            CustomMeshManager.instance.SetRearRimMesh(pm.partNum);
                        else if (pm.isCustom && !File.Exists(pm.fileName))
                            CustomMeshManager.instance.SetRearRimMesh(0);
                        break;
                    case "frontSpokeAccessory":
                        if ((pm.isCustom && File.Exists(pm.fileName)) || !pm.isCustom)
                            CustomMeshManager.instance.SetFrontSpokeAccMesh(pm.partNum);
                        else if (pm.isCustom && !File.Exists(pm.fileName))
                            CustomMeshManager.instance.SetFrontSpokeAccMesh(0);
                        break;
                    case "rearSpokeAccessory":
                        if ((pm.isCustom && File.Exists(pm.fileName)) || !pm.isCustom)
                            CustomMeshManager.instance.SetRearSpokeAccMesh(pm.partNum);
                        else if (pm.isCustom && !File.Exists(pm.fileName))
                            CustomMeshManager.instance.SetRearSpokeAccMesh(0);
                        break;
                    case "barAccessory":
                        if ((pm.isCustom && File.Exists(pm.fileName)) || !pm.isCustom)
                            CustomMeshManager.instance.SetBarAccMesh(pm.partNum);
                        else if (pm.isCustom && !File.Exists(pm.fileName))
                            CustomMeshManager.instance.SetBarAccMesh(0);
                        break;
                    case "frameAccessory":
                        if ((pm.isCustom && File.Exists(pm.fileName)) || !pm.isCustom)
                            CustomMeshManager.instance.SetFrameAccMesh(pm.partNum);
                        else if (pm.isCustom && !File.Exists(pm.fileName))
                            CustomMeshManager.instance.SetFrameAccMesh(0);
                        break;
                    case "frontHubGuard":
                        if ((pm.isCustom && File.Exists(pm.fileName)) || !pm.isCustom)
                            CustomMeshManager.instance.SetFrontHubGuardMesh(pm.partNum);
                        else if (pm.isCustom && !File.Exists(pm.fileName))
                            CustomMeshManager.instance.SetFrontHubGuardMesh(0);
                        break;
                    case "rearHubGuard":
                        if ((pm.isCustom && File.Exists(pm.fileName)) || !pm.isCustom)
                            CustomMeshManager.instance.SetRearHubGuardMesh(pm.partNum);
                        else if (pm.isCustom && !File.Exists(pm.fileName))
                            CustomMeshManager.instance.SetRearHubGuardMesh(0);
                        break;
                    case "seatPost":
                        if ((pm.isCustom && File.Exists(pm.fileName)) || !pm.isCustom)
                            CustomMeshManager.instance.SetSeatPostMesh(pm.partNum);
                        else if (pm.isCustom && !File.Exists(pm.fileName))
                            CustomMeshManager.instance.SetSeatPostMesh(0);
                        break;
                    default:
                        break;
                }
            }
        }
        catch (Exception e)
        {
            error += e.Message + "\n " + e.StackTrace + "\n ";
        }
    }

    /// <summary>
    /// Loads the last selected preset
    /// </summary>
    /// <returns>null</returns>
    IEnumerator LoadLastSelected()
    {
        yield return new WaitForSeconds(1);
        try
        {   
            Load(lastSelectedPreset);
        }
        catch (Exception e)
        {
            error += e.Message + "\n " + e.StackTrace + "\n ";
        }
    }
}
