using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml;
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
    private BetterWheelsMod betterWheels;
    private PartManager partManager;
    private MaterialManager matManager;

    public GameObject overwriteWarning;

    private Material a_glossy;

    string lastSelectedPreset;

    private void Awake()
    {
        // Setup
        path = Application.dataPath + "//GarageContent/GarageSaves/";
        errorPath = Application.dataPath + "//GarageContent/GarageErrorLog.txt";
        cs = FindObjectOfType<ColourSetter>();
        betterWheels = FindObjectOfType<BetterWheelsMod>();
        partManager = FindObjectOfType<PartManager>();
        matManager = FindObjectOfType<MaterialManager>();
        a_glossy = FindObjectOfType<BikeLoadOut>().GetPartMat(0);
        instance = this;
        File.WriteAllText(errorPath, "");
        lastSelectedPreset = PlayerPrefs.GetString("lastPreset");
        if (File.Exists(Path.Combine(path, lastSelectedPreset + ".preset"))) // Load last used preset if there is one
            StartCoroutine(LoadLastSelected());
        origInfotxt = infoBoxText.text;
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
                LoadWheels();
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
            Debug.Log("Saving Wheels...");
            SaveWheels();
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
        saveList.treadID = betterWheels.GetBetterWheels() ? saveList.treadID = 3 : saveList.treadID = partManager.tiresCount - 1;
    }

    /// <summary>
    /// Load the tire tread material
    /// </summary>
    private void LoadTireTread()
    {
        partManager.SetTireTread(loadList.treadID);
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
    /// Save whether the better wheels mod is on
    /// </summary>
    private void SaveWheels()
    {
        saveList.betterWheels = betterWheels.GetBetterWheels();
        saveList.hasFrontHubChanged = betterWheels.CheckFront();
        saveList.hasRearHubChanged = betterWheels.CheckRear();
    }

    /// <summary>
    /// Load whether the better wheels mod is on
    /// </summary>
    private void LoadWheels()
    {
        if (loadList.betterWheels)
        {
            betterWheels.ApplyMod();
        }
        else
        {
            betterWheels.DisableMod();
        }

        if (loadList.hasFrontHubChanged)
            betterWheels.ChangeFrontHub();
        if (loadList.hasRearHubChanged)
            betterWheels.ChangeRearHub();
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
                        FindObjectOfType<BikeLoadOut>().SetPartMaterial(BetterWheelsMod.instance.betterWheelMat, p.partNum, true);
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
                        {
                            if (betterWheels.CheckFront())
                                CustomMeshManager.instance.SetFrontHubMesh(pm.partNum);
                        }
                        else if (pm.isCustom && !File.Exists(pm.fileName)) {

                            if (betterWheels.CheckFront())
                                CustomMeshManager.instance.SetFrontHubMesh(0);
                        }
                            break;
                    case "rearHub":
                        if ((pm.isCustom && File.Exists(pm.fileName)) || !pm.isCustom)
                        {
                            if (betterWheels.CheckRear())
                                CustomMeshManager.instance.SetRearHubMesh(pm.partNum);
                        }
                        else if (pm.isCustom && !File.Exists(pm.fileName))
                        {
                            if (betterWheels.CheckRear())
                                CustomMeshManager.instance.SetRearHubMesh(0);
                        }
                            break;
                    case "seat":
                        if ((pm.isCustom && File.Exists(pm.fileName)) || !pm.isCustom)
                            CustomMeshManager.instance.SetSeatMesh(pm.partNum);
                        else if (pm.isCustom && !File.Exists(pm.fileName))
                            CustomMeshManager.instance.SetSeatMesh(0);
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
