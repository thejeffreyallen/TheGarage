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

    private void LoadSlotNames(string presetName)
    {
        error = "";
        
        bool flag = File.Exists(this.path + presetName + ".preset");
        if (flag)
        {
            Deserialize(presetName);
            try
            {
                
                TextureManager.instance.SetOriginalTextures();
                this.LoadBrakes();
                this.LoadSeatAngle();
                this.LoadBarsAngle();
                this.LoadSeatID();
                this.LoadGripsID();
                this.LoadBikeScale();
                this.LoadWheels();
                this.LoadFrontTireWidth();
                this.LoadRearTireWidth();
                this.LoadFlanges();
                this.LoadSeatHeight();
                this.LoadDriveSide();
                this.LoadMaterials();
                this.LoadTireTread();
                foreach (PartColor p in loadList.partColors) {
                    FindObjectOfType<BikeLoadOut>().SetColor(new Color(p.r, p.g, p.b, p.a), p.partNum);
                }
                this.LoadSeatChainColors();
                this.LoadBrakesColor();
                this.LoadMeshes();
                this.LoadTextures();

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

    private void SaveSlotNames()
    {
        this.saveErrors = "";
        try
        {
            Debug.Log("Saving Brakes On/Off...");
            this.SaveBrakes();
            Debug.Log("Saving Seat Angle...");
            this.SaveSeatAngle();
            Debug.Log("Saving Bars Angle...");
            this.SaveBarsAngle();
            Debug.Log("Saving Seat ID...");
            this.SaveSeatID();
            Debug.Log("Saving Grips ID...");
            this.SaveGripsID();
            Debug.Log("Saving Bike Scale...");
            this.SaveBikeScale();
            Debug.Log("Saving Wheels...");
            this.SaveWheels();
            Debug.Log("Saving Front Tire Width...");
            this.SaveFrontTireWidth();
            Debug.Log("Saving Rear Tire Width...");
            this.SaveRearTireWidth();
            Debug.Log("Saving Flanges On / Off...");
            this.SaveFlanges();
            Debug.Log("Saving Seat Height...");
            this.SaveSeatHeight();
            Debug.Log("Saving Drive Side...");
            this.SaveDriveSide();
            Debug.Log("Saving Materials...");
            this.SaveMaterials();
            Debug.Log("Saving Tire Tread...");
            this.SaveTireTread();
            Debug.Log("Saving Bike Colors...");
            for (int i = 0; i < 20; i++)
            {
               saveList.partColors.Add(new PartColor(i, FindObjectOfType<BikeLoadOut>().GetColor(i)));
            }
            this.SaveSeatPostChainColors();
            this.SaveBrakeColor();
            Debug.Log("Saving Meshes...");
            this.SaveMeshes();
            Debug.Log("Saving Textures...");
            this.SaveTextures();
            Debug.Log("Writing " + this.saveName.text + " to File...");

                bool flag = !File.Exists(this.path + this.saveName.text + ".preset");
                if (flag)
                {
                    Serialize(this.saveName.text);
                }
                else
                {
                    this.overwriteWarning.SetActive(true);
                }
        }
        catch (Exception e)
        {
            saveErrors += e.Message + "\n " + e.StackTrace + "\n "; 
        }
    }


    public void SetOverwrite()
    {
        Serialize(this.saveName.text);
    }

    private void SaveFrontTireWidth()
    {
        saveList.frontTireWidth = FindObjectOfType<BikeLoadOut>().GetFrontTireFatness();
    }

    private void SaveRearTireWidth()
    {
        saveList.rearTireWidth = FindObjectOfType<BikeLoadOut>().GetBackTireFatness();
    }

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

    private void SaveSeatAngle()
    {
        saveList.seatAngle = FindObjectOfType<SeatApplyMod>().GetSeatAnglePerc();
    }

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

    private void SaveBarsAngle()
    {
        saveList.barsAngle = FindObjectOfType<BarsApplyMod>().GetBarsAnglePerc();
    }

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


    private void SaveBrakes()
    {
        saveList.brakes = BrakesManager.instance.brakesEnabled;
    }

    private void LoadBrakes()
    {
        BrakesManager.instance.SetBrakes(loadList.brakes);
    }

    private void SaveTireTread()
    {
        saveList.treadID = betterWheels.GetBetterWheels() ? saveList.treadID = 3 : saveList.treadID = this.partManager.tiresCount - 1;
    }

    private void LoadTireTread()
    {
        this.partManager.SetTireTread(loadList.treadID);
    }

    private void SaveSeatID()
    {
        saveList.seatID = this.partManager.seatCount - 1;
    }

    private void LoadSeatID()
    {
        this.partManager.SetSeatID(loadList.seatID);
    }

    private void SaveGripsID()
    {
        saveList.gripsID = this.partManager.gripsCount - 1;
    }

    private void LoadGripsID()
    {
        this.partManager.SetGripsId(loadList.gripsID);
    }

    private void SaveBikeScale()
    {
        saveList.bikeScale = this.partManager.bikeScaleSlider.value;
    }

    private void LoadBikeScale()
    {
        this.partManager.SetBikeScale(loadList.bikeScale);
    }


    private void SaveBrakeColor()
    {
        try
        {
            if (BrakesManager.instance.brakesEnabled)
            {
                saveList.brakesColor = cs.GetBrakesColor();
            }
        }
        catch (Exception e)
        {
            saveErrors += e.Message + "\n " + e.StackTrace + "\n ";
        }
    }

    private void LoadBrakesColor()
    {
        try
        {
            if (BrakesManager.instance.brakesEnabled)
            {
                cs.SetBrakesColor(loadList.brakesColor);
            }
        }
        catch (Exception e)
        {
            error += e.Message + "\n " + e.StackTrace + "\n ";
        }
    }

    private void SaveSeatPostChainColors()
    {
        saveList.seatPostColor = cs.GetSeatPostColor();
        saveList.chainColor = cs.GetChainColor();
    }

    private void SaveWheels()
    {
        saveList.betterWheels = betterWheels.GetBetterWheels();
        saveList.hasFrontHubChanged = betterWheels.CheckFront();
        saveList.hasRearHubChanged = betterWheels.CheckRear();
    }

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


    private void LoadSeatChainColors()
    {
        try
        {
            cs.SetSeatPostColor(loadList.seatPostColor);
            cs.SetChainColor(loadList.chainColor);
        }
        catch (Exception e)
        {
            error += e.Message + "\n " + e.StackTrace + "\n ";
        }
    }

    private void SaveFlanges()
    {
        saveList.flanges = partManager.GetFlangesVisible();
    }

    private void LoadFlanges()
    {
            if (!loadList.flanges)
            {
                this.partManager.SetFlangesOff();
            }
            else
            {
                this.partManager.SetFlangesOn();
            }
    }

    private void SaveSeatHeight()
    {
        saveList.seatHeight = partManager.GetSeatHeight();
    }

    private void LoadSeatHeight()
    {
        partManager.SetSeatHeight(loadList.seatHeight);
    }

    private void SaveDriveSide()
    {
        saveList.LHD = partManager.GetDriveSide();
    }

    private void LoadDriveSide()
    {
        if (loadList.LHD)
        {
            this.partManager.SetLHD();
        }
        else
        {
            this.partManager.SetRHD();
        }
    }

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
            Material material = PartMaster.instance.GetMaterial(PartMaster.instance.seatPost);
            string text2 = material.name.ToLower();
            switch (text2)
            {
                case "a_glossy (instance)":
                    saveList.seatPostMat = 0;
                    break;
                case "a_glossy (instance) (instance)":
                    saveList.seatPostMat = 0;
                    break;
                case "jetfuel (instance)":
                    saveList.seatPostMat = 1;
                    break;
                case "flat (instance)":
                    saveList.seatPostMat = 2;
                    break;
                case "chrome (instance)":
                    saveList.seatPostMat = 3;
                    break;
                case "bubble (instance)":
                    saveList.seatPostMat = 4;
                    break;
                case "rusty (instance)":
                    saveList.seatPostMat = 5;
                    break;
                case "green jetfuel (instance)":
                    saveList.seatPostMat = 6;
                    break;
                default:
                    saveList.seatPostMat = 9;
                    break;
            }

        }
        catch (Exception e)
        {
            saveErrors += e.Message + "\n " + e.StackTrace + "\n ";
        }
    }

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
                    case 1:
                        FindObjectOfType<BikeLoadOut>().SetPartMaterial(matManager.customMats[0], p.partNum, true);
                        break;
                    case 2:
                        FindObjectOfType<BikeLoadOut>().SetPartMaterial(matManager.customMats[1], p.partNum, true);
                        break;
                    case 3:
                        FindObjectOfType<BikeLoadOut>().SetPartMaterial(matManager.customMats[2], p.partNum, true);
                        break;
                    case 4:
                        FindObjectOfType<BikeLoadOut>().SetPartMaterial(matManager.customMats[3], p.partNum, true);
                        break;
                    case 5:
                        FindObjectOfType<BikeLoadOut>().SetPartMaterial(matManager.customMats[4], p.partNum, true);
                        break;
                    case 6:
                        FindObjectOfType<BikeLoadOut>().SetPartMaterial(matManager.customMats[5], p.partNum, true);
                        break;
                    case 7:
                        FindObjectOfType<BikeLoadOut>().SetPartMaterial(BetterWheelsMod.instance.betterWheelMat, p.partNum, true);
                        break;
                    default:
                        break;
                }

            }
            switch (loadList.seatPostMat)
            {
                case 0:
                    PartMaster.instance.GetPart(PartMaster.instance.seatPost).GetComponent<MeshRenderer>().material = this.a_glossy;
                    break;
                case 1:
                    PartMaster.instance.GetPart(PartMaster.instance.seatPost).GetComponent<MeshRenderer>().material = this.matManager.customMats[0];
                    break;
                case 2:
                    PartMaster.instance.GetPart(PartMaster.instance.seatPost).GetComponent<MeshRenderer>().material = this.matManager.customMats[1];
                    break;
                case 3:
                    PartMaster.instance.GetPart(PartMaster.instance.seatPost).GetComponent<MeshRenderer>().material = this.matManager.customMats[2];
                    break;
                case 4:
                    PartMaster.instance.GetPart(PartMaster.instance.seatPost).GetComponent<MeshRenderer>().material = this.matManager.customMats[3];
                    break;
                case 5:
                    PartMaster.instance.GetPart(PartMaster.instance.seatPost).GetComponent<MeshRenderer>().material = this.matManager.customMats[4];
                    break;
                case 6:
                    PartMaster.instance.GetPart(PartMaster.instance.seatPost).GetComponent<MeshRenderer>().material = this.matManager.customMats[5];
                    break;
            }
        }
        catch (Exception e)
        {
            error += e.Message + "\n " + e.StackTrace + "\n ";
        }
    }

    private void SaveTextures()
    {
        try
        {
            if (TextureManager.instance.frameURL != "")
            {
                saveList.partTextures.Add(new PartTexture(TextureManager.instance.frameURL, 0, false, false));
            }
            if (TextureManager.instance.barsURL != "")
            {
                saveList.partTextures.Add(new PartTexture(TextureManager.instance.barsURL, 1, false, false));
            }
            if (TextureManager.instance.seatURL != "")
            {
                saveList.partTextures.Add(new PartTexture(TextureManager.instance.seatURL, 2, false, false));
            }
            if (TextureManager.instance.forksURL != "")
            {
                saveList.partTextures.Add(new PartTexture(TextureManager.instance.forksURL, 3, false, false));
            }
            if (TextureManager.instance.tireURL != "")
            {
                saveList.partTextures.Add(new PartTexture(TextureManager.instance.tireURL, 4, false, false));
            }
            if (TextureManager.instance.tireWallURL != "")
            {
                saveList.partTextures.Add(new PartTexture(TextureManager.instance.tireWallURL, 5, false, false));
            }
            if (TextureManager.instance.rimsURL != "")
            {
                saveList.partTextures.Add(new PartTexture(TextureManager.instance.rimsURL, 6, false, false));
            }
            if (TextureManager.instance.hubsURL != "")
            {
                saveList.partTextures.Add(new PartTexture(TextureManager.instance.hubsURL, 7, false, false));
            }

            // Normal Maps

            if (TextureManager.instance.frameURLN != "")
            {
                saveList.partTextures.Add(new PartTexture(TextureManager.instance.frameURLN, 0, true, false));
            }
            if (TextureManager.instance.barsURLN != "")
            {
                saveList.partTextures.Add(new PartTexture(TextureManager.instance.barsURLN, 1, true, false));
            }
            if (TextureManager.instance.seatURLN != "")
            {
                saveList.partTextures.Add(new PartTexture(TextureManager.instance.seatURLN, 2, true, false));
            }
            if (TextureManager.instance.forksURLN != "")
            {
                saveList.partTextures.Add(new PartTexture(TextureManager.instance.forksURLN, 3, true, false));
            }
            if (TextureManager.instance.tireURLN != "")
            {
                saveList.partTextures.Add(new PartTexture(TextureManager.instance.tireURLN, 4, true, false));
            }
            if (TextureManager.instance.tireWallURLN != "")
            {
                saveList.partTextures.Add(new PartTexture(TextureManager.instance.tireWallURLN, 5, true, false));
            }
            if (TextureManager.instance.rimsURLN != "")
            {
                saveList.partTextures.Add(new PartTexture(TextureManager.instance.rimsURLN, 6, true, false));
            }
            if (TextureManager.instance.hubsURLN != "")
            {
                saveList.partTextures.Add(new PartTexture(TextureManager.instance.hubsURLN, 7, true, false));
            }

            //Metallic Maps

            if (TextureManager.instance.frameURLM != "")
            {
                saveList.partTextures.Add(new PartTexture(TextureManager.instance.frameURLM, 0, false, true));
            }
            if (TextureManager.instance.barsURLM != "")
            {
                saveList.partTextures.Add(new PartTexture(TextureManager.instance.barsURLM, 1, false, true));
            }
            if (TextureManager.instance.seatURLM != "")
            {
                saveList.partTextures.Add(new PartTexture(TextureManager.instance.seatURLM, 2, false, true));
            }
            if (TextureManager.instance.forksURLM != "")
            {
                saveList.partTextures.Add(new PartTexture(TextureManager.instance.forksURLM, 3, false, true));
            }
            if (TextureManager.instance.tireURLM != "")
            {
                saveList.partTextures.Add(new PartTexture(TextureManager.instance.tireURLM, 4, false, true));
            }
            if (TextureManager.instance.tireWallURLM != "")
            {
                saveList.partTextures.Add(new PartTexture(TextureManager.instance.tireWallURLM, 5, false, true));
            }
            if (TextureManager.instance.rimsURLM != "")
            {
                saveList.partTextures.Add(new PartTexture(TextureManager.instance.rimsURLM, 6, false, true));
            }
            if (TextureManager.instance.hubsURLM != "")
            {
                saveList.partTextures.Add(new PartTexture(TextureManager.instance.hubsURLM, 7, false, true));
            }

        }
        catch (Exception e)
        {
            saveErrors += e.Message + "\n " + e.StackTrace + "\n ";
        }
    }

    private void LoadTextures()
    {
        try
        {
            foreach (PartTexture p in loadList.partTextures)
            {
                Debug.Log("Loading Texture: " + p.url);
                if (!p.metallic && !p.normal)
                    TextureManager.instance.SetTexture(p.partNum, p.url);
                else if (p.normal)
                    TextureManager.instance.SetNormal(p.partNum, p.url);
                else
                    TextureManager.instance.SetMetallic(p.partNum, p.url);
            }
        } catch (Exception e)
        {
            Debug.Log("An error occured when loading textures. " + e.Message + " : " + e.StackTrace);
        }
    }

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

    public void ChangeAlertText(string message)
    {
        infoBoxText.text = message;
    }

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
