﻿using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SavingManager : MonoBehaviour
{
    public static SavingManager instance;
    public Text[] saveNames;
    private string path;
    int partsCount = 0;
    private string saveData;
    private string error;
    private string saveErrors;
    private Scanner scan;
    private ColourSetter cs;
    private BetterWheelsMod betterWheels;
    private PartManager partManager;
    private MaterialManager matManager;

    public GameObject overwriteWarning;


    private Material a_glossy;

    string lastSelectedPreset;

    private void Awake()
    {
        path = Application.dataPath + "//GarageSaves/";
        cs = FindObjectOfType<ColourSetter>();
        betterWheels = FindObjectOfType<BetterWheelsMod>();
        partManager = FindObjectOfType<PartManager>();
        matManager = FindObjectOfType<MaterialManager>();
        a_glossy = FindObjectOfType<BikeLoadOut>().GetPartMat(0);
        instance = this;
        File.WriteAllText(Application.dataPath + "//GarageErrorLog.txt", "");

        lastSelectedPreset = PlayerPrefs.GetString("lastPreset");
        if (File.Exists(Path.Combine(path, lastSelectedPreset + ".preset")))
            StartCoroutine(LoadLastSelected());
    }

    public void Save()
    {
        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }
        SaveSlotNames();
        File.AppendAllText(Application.dataPath + "//GarageErrorLog.txt", error);
    }

    public void Load(string presetName)
    {
        try
        {
            LoadSlotNames(presetName);
            PlayerPrefs.SetString("lastPreset", presetName);
        }
        catch(Exception e)
        {
            error += e.Message + " " + e.StackTrace;
        }
        File.AppendAllText(Application.dataPath + "//GarageErrorLog.txt", error);
    }

    private void LoadSlotNames(string presetName)
    {
        error = "";
        bool flag = File.Exists(this.path + presetName + ".preset");
        if (flag)
        {
            
            try
            {
                TextureManager.instance.SetOriginalTextures();
                this.scan = new Scanner(File.ReadAllText(this.path + presetName + ".preset"));
                this.LoadBrakes();
                this.LoadSeatAngle();
                this.LoadBarsAngle();
                this.LoadSeatID();
                this.LoadGripsID();
                this.LoadBikeScale();
                this.LoadWheels();
                this.LoadFlanges();
                this.LoadSeatHeight();
                this.LoadDriveSide();
                this.LoadMaterials();
                this.LoadTireTread();
                for (int i = 0; i < 20; i++)
                {
                    float num = (float)this.scan.nextDouble();
                    float num2 = (float)this.scan.nextDouble();
                    float num3 = (float)this.scan.nextDouble();
                    float num4 = (float)this.scan.nextDouble();
                    FindObjectOfType<BikeLoadOut>().SetColor(new Color(num, num2, num3, num4), i);
                }
                this.LoadSeatChainColors();
                this.LoadBrakesColor();
                this.LoadMeshes();
                this.LoadTextures();
                this.scan.Close();
                this.scan.Dispose();
                Material m = GameObject.Find("Right Crank Arm Mesh").GetComponent<Renderer>().material;
                GameObject.Find("Left Crank Arm Mesh").GetComponent<Renderer>().material = m;
            }
            catch (Exception e)
            {
                error += e.Message + " " + e.StackTrace;
            }
            
        }
        
    }

    private void SaveSlotNames()
    {
        this.saveData = "";
        this.saveErrors = "";
        try
        {
            this.SaveBrakes();
            this.SaveSeatAngle();
            this.SaveBarsAngle();
            this.SaveSeatID();
            this.SaveGripsID();
            this.SaveBikeScale();
            this.SaveWheels();
            this.SaveFlanges();
            this.SaveSeatHeight();
            this.SaveDriveSide();
            this.SaveMaterials();
            this.SaveTireTread();
            for (int i = 0; i < 20; i++)
            {
                this.saveColors(i);
            }
            this.SaveSeatPostChainColors();
            this.SaveBrakeColor();
            this.SaveMeshes();
            this.SaveTextures();
            for (int j = 0; j < this.saveNames.Length; j++)
            {
                bool flag = !File.Exists(this.path + this.saveNames[j].text + ".preset");
                if (flag)
                {
                    File.WriteAllText(this.path + this.saveNames[j].text + ".preset", this.saveData);
                }
                else
                {
                    this.overwriteWarning.SetActive(true);
                }
            }
        }
        catch (Exception e)
        {
            saveErrors += e.Message + " " + e.StackTrace + " "; 
        }
    }

    public void SetOverwrite()
    {
        File.WriteAllText(this.path + this.saveNames[0].text + ".preset", this.saveData);
    }

    private void SaveSeatAngle()
    {
        try
        {
            saveData += FindObjectOfType<SeatApplyMod>().GetSeatAnglePerc() + " ";
        }
        catch (Exception e)
        {
            saveErrors += e.Message + " " + e.StackTrace + " ";
        }
    }

    private void LoadSeatAngle()
    {
        try
        {
            float angle = (float)scan.nextDouble();
            FindObjectOfType<SeatApplyMod>().SetSeatAnglePerc(angle);
            partManager.seatAngleSlider.value = angle;
        }
        catch (Exception e)
        {
            error += e.Message + " " + e.StackTrace + " ";
        }
    }

    private void SaveBarsAngle()
    {
        try
        {
            saveData += FindObjectOfType<BarsApplyMod>().GetBarsAnglePerc() + " ";
        }
        catch (Exception e)
        {
            saveErrors += e.Message + " " + e.StackTrace + " ";
        }
    }

    private void LoadBarsAngle()
    {
        try
        {
            float angle = (float)scan.nextDouble();
            FindObjectOfType<BarsApplyMod>().SetBarsAnglePerc(angle);
            partManager.barsAngleSlider.value = angle;
        }
        catch (Exception e)
        {
            error += e.Message + " " + e.StackTrace + " ";
        }
    }


    private void SaveBrakes()
    {
        try
        {
            if (BrakesManager.instance.brakesEnabled)
                saveData += "1 ";
            else
                saveData += "0 ";
        }
        catch (Exception e)
        {
            saveErrors += e.Message + " " + e.StackTrace + " ";
        }
    }

    private void LoadBrakes()
    {
        try
        {
            int bin = scan.nextInt();
            if (bin == 1)
            {
                BrakesManager.instance.SetBrakes(true);
            }
            else
            {
                BrakesManager.instance.SetBrakes(false);
            }
        }
        catch (Exception e)
        {
            error += e.Message + " " + e.StackTrace;
        }
    }

    private void SaveTireTread()
    {
        try
        {
            this.saveData += this.partManager.tiresCount - 1 + " ";
        }
        catch (Exception e)
        {
            saveErrors += e.Message + " " + e.StackTrace + " ";
        }
    }

    private void LoadTireTread()
    {
        try
        {
            int id = scan.nextInt();
            this.partManager.SetTireTread(id);
        }
        catch (Exception e)
        {
            error += e.Message + " " + e.StackTrace;
        }
    }

    private void SaveSeatID()
    {
        try
        {
            this.saveData += this.partManager.seatCount - 1 + " ";
        }
        catch (Exception e)
        {
            saveErrors += e.Message + " " + e.StackTrace + " ";
        }
    }

    private void LoadSeatID()
    {
        try
        {
            int id = scan.nextInt();
            this.partManager.SetSeatID(id);
        }
        catch (Exception e)
        {
            error += e.Message + " " + e.StackTrace;
        }
    }

    private void SaveGripsID()
    {
        try
        {
            this.saveData += this.partManager.gripsCount - 1 + " ";
        }
        catch (Exception e)
        {
            saveErrors += e.Message + " " + e.StackTrace + " ";
        }
    }

    private void LoadGripsID()
    {
        try
        {
            int id = scan.nextInt();
            this.partManager.SetGripsId(id);
        }
        catch (Exception e)
        {
            error += e.Message + " " + e.StackTrace;
        }
    }

    private void SaveBikeScale()
    {
        try
        {
            this.saveData = this.saveData + this.partManager.bikeScaleSlider.value + " ";
        }
        catch (Exception e)
        {
            saveErrors += e.Message + " " + e.StackTrace + " ";
        }
    }

    private void LoadBikeScale()
    {
        try
        {
            double num = this.scan.nextDouble();
            this.partManager.SetBikeScale((float)num);
        }
        catch (Exception e)
        {
            error += e.Message + " " + e.StackTrace;
        }
    }

    private void saveColors(int partNum)
    {
        try
        {
            saveData += FindObjectOfType<BikeLoadOut>().GetColor(partNum).r + " ";
            saveData += FindObjectOfType<BikeLoadOut>().GetColor(partNum).g + " ";
            saveData += FindObjectOfType<BikeLoadOut>().GetColor(partNum).b + " ";
            saveData += FindObjectOfType<BikeLoadOut>().GetColor(partNum).a + " ";
        }
        catch (Exception e)
        {
            saveErrors += e.Message + " " + e.StackTrace + " ";
        }
    }

    private void SaveBrakeColor()
    {
        try
        {
            if (BrakesManager.instance.brakesEnabled)
            {
                saveData += cs.GetBrakesColor().r + " ";
                saveData += cs.GetBrakesColor().g + " ";
                saveData += cs.GetBrakesColor().b + " ";
                saveData += cs.GetBrakesColor().a + " ";
            }
        }
        catch (Exception e)
        {
            saveErrors += e.Message + " " + e.StackTrace + " ";
        }
    }

    private void LoadBrakesColor()
    {
        try
        {
            if (BrakesManager.instance.brakesEnabled)
            {
                float r = (float)scan.nextDouble();
                float g = (float)scan.nextDouble();
                float b = (float)scan.nextDouble();
                float a = (float)scan.nextDouble();
                cs.SetBrakesColor(r, g, b, a);
            }
        }
        catch (Exception e)
        {
            error += e.Message + " " + e.StackTrace;
        }
    }

    private void SaveSeatPostChainColors()
    {
        try
        {
            saveData += cs.GetSeatPostColor().r + " ";
            saveData += cs.GetSeatPostColor().g + " ";
            saveData += cs.GetSeatPostColor().b + " ";
            saveData += cs.GetSeatPostColor().a + " ";

            saveData += cs.GetChainColor().r + " ";
            saveData += cs.GetChainColor().g + " ";
            saveData += cs.GetChainColor().b + " ";
            saveData += cs.GetChainColor().a + " ";
        }
        catch (Exception e)
        {
            saveErrors += e.Message + " " + e.StackTrace + " ";
        }
    }

    private void SaveWheels()
    {
        try
        {
            if (betterWheels.GetBetterWheels())
            {
                saveData += "1 ";
            }
            else
            {
                saveData += "0 ";
            }
        }
        catch (Exception e)
        {
            saveErrors += e.Message + " " + e.StackTrace + " ";
        }
    }

    private void LoadWheels()
    {
        try
        {
            int set = scan.nextInt();
            if (set == 1)
            {
                betterWheels.ApplyMod();
            }
            else
            {
                betterWheels.DisableMod();
            }
        }
        catch (Exception e)
        {
            error += e.Message + " " + e.StackTrace;
        }
    }


    private void LoadSeatChainColors()
    {
        try
        {
            float r = (float)scan.nextDouble();
            float g = (float)scan.nextDouble();
            float b = (float)scan.nextDouble();
            float a = (float)scan.nextDouble();
            float r2 = (float)scan.nextDouble();
            float g2 = (float)scan.nextDouble();
            float b2 = (float)scan.nextDouble();
            float a2 = (float)scan.nextDouble();
            cs.SetSeatPostColor(r, g, b, a);
            cs.SetChainColor(r2, g2, b2, a2);
        }
        catch (Exception e)
        {
            error += e.Message + " " + e.StackTrace;
        }
    }

    private void SaveFlanges()
    {
        try
        {
            if (partManager.GetFlangesVisible())
            {
                saveData += "0 ";
            }
            else
            {
                saveData += "1 ";
            }
        }
        catch (Exception e)
        {
            saveErrors += e.Message + " " + e.StackTrace + " ";
        }
    }

    private void LoadFlanges()
    {
        try
        {
            int num = this.scan.nextInt();
            bool flag = num == 1;
            if (flag)
            {
                this.partManager.SetFlangesOff();
            }
            else
            {
                this.partManager.SetFlangesOn();
            }
        }
        catch (Exception e)
        {
            error += e.Message + " " + e.StackTrace;
        }
    }

    private void SaveSeatHeight()
    {
        try
        {
            float seatHeight = partManager.GetSeatHeight();
            saveData += seatHeight + " ";
        }
        catch (Exception e)
        {
            saveErrors += e.Message + " " + e.StackTrace + " ";
        }
    }

    private void LoadSeatHeight()
    {
        try
        {
            float set = (float)scan.nextDouble();
            partManager.SetSeatHeight(set);
        }
        catch (Exception e)
        {
            error += e.Message + " " + e.StackTrace;
        }
    }

    private void SaveDriveSide()
    {
        try
        {
            if (partManager.GetDriveSide())
            {
                saveData += "1 ";
            }
            else
            {
                saveData += "0 ";
            }
        }
        catch (Exception e)
        {
            saveErrors += e.Message + " " + e.StackTrace + " ";
        }
    }

    private void LoadDriveSide()
    {
        try
        {
            int num = this.scan.nextInt();
            bool flag = num == 1;
            if (flag)
            {
                this.partManager.SetLHD();
            }
            else
            {
                this.partManager.SetRHD();
            }
        }
        catch (Exception e)
        {
            error += e.Message + " " + e.StackTrace;
        }
    }

    private void SaveMaterials()
    {
        try
        {
            for (int i = 0; i < 20; i++)
            {
                Material mat = FindObjectOfType<BikeLoadOut>().GetPartMat(i);

                switch (mat.name.ToLower())
                {
                    case "a_glossy (instance)":
                        saveData += i + " 0 ";
                        partsCount++;
                        break;
                    case "jetfuel (instance)":
                        saveData += i + " 1 ";
                        partsCount++;
                        break;
                    case "flat (instance)":
                        saveData += i + " 2 ";
                        partsCount++;
                        break;
                    case "chrome (instance)":
                        saveData += i + " 3 ";
                        partsCount++;
                        break;
                    case "bubble (instance)":
                        saveData += i + " 4 ";
                        partsCount++;
                        break;
                    case "rusty (instance)":
                        saveData += i + " 5 ";
                        partsCount++;
                        break;
                    case "green jetfuel (instance)":
                        saveData += i + " 6 ";
                        partsCount++;
                        break;
                    case "spokes (instance)":
                        saveData += i + " 0 ";
                        partsCount++;
                        break;
                    case "forks (instance)":
                        saveData += i + " 0 ";
                        partsCount++;
                        break;
                    case "nipples (instance)":
                        saveData += i + " 0 ";
                        partsCount++;
                        break;
                    case "wheels (instance)":
                        saveData += i + " 7 ";
                        partsCount++;
                        break;
                    default:
                        saveData += i + " 9 ";
                        break;
                }
            }
            Material material = GameObject.Find("Seat Post").GetComponent<Renderer>().material;
            string text2 = material.name.ToLower();
            switch (text2)
            {
                case "a_glossy (instance)":
                    saveData += "0 ";
                    break;
                case "jetfuel (instance)":
                    saveData += "1 ";
                    break;
                case "flat (instance)":
                    saveData += "2 ";
                    break;
                case "chrome (instance)":
                    saveData += "3 ";
                    break;
                case "bubble (instance)":
                    saveData += "4 ";
                    break;
                case "rusty (instance)":
                    saveData += "5 ";
                    break;
                case "green jetfuel (instance)":
                    saveData += "6 ";
                    break;
                default:
                    saveData += "9 ";
                    break;
            }

        }
        catch (Exception e)
        {
            saveErrors += e.Message + " " + e.StackTrace + " ";
        }

    }

    private void LoadMaterials()
    {
        try
        {
            for (int i = 0; i < 20; i++)
            {


                int partNum = scan.nextInt();
                int set = scan.nextInt();
                switch (set)
                {
                    case 0:
                        FindObjectOfType<BikeLoadOut>().SetPartMaterial(a_glossy, partNum, true);
                        break;
                    case 1:
                        FindObjectOfType<BikeLoadOut>().SetPartMaterial(matManager.customMats[0], partNum, true);
                        break;
                    case 2:
                        FindObjectOfType<BikeLoadOut>().SetPartMaterial(matManager.customMats[1], partNum, true);
                        break;
                    case 3:
                        FindObjectOfType<BikeLoadOut>().SetPartMaterial(matManager.customMats[2], partNum, true);
                        break;
                    case 4:
                        FindObjectOfType<BikeLoadOut>().SetPartMaterial(matManager.customMats[3], partNum, true);
                        break;
                    case 5:
                        FindObjectOfType<BikeLoadOut>().SetPartMaterial(matManager.customMats[4], partNum, true);
                        break;
                    case 6:
                        FindObjectOfType<BikeLoadOut>().SetPartMaterial(matManager.customMats[5], partNum, true);
                        break;
                    case 7:
                        FindObjectOfType<BikeLoadOut>().SetPartMaterial(BetterWheelsMod.instance.betterWheelMat, partNum, true);
                        break;
                    default:
                        break;
                }

            }
            switch (this.scan.nextInt())
            {
                case 0:
                    GameObject.Find("Seat Post").GetComponent<Renderer>().material = this.a_glossy;
                    break;
                case 1:
                    GameObject.Find("Seat Post").GetComponent<Renderer>().material = this.matManager.customMats[0];
                    break;
                case 2:
                    GameObject.Find("Seat Post").GetComponent<Renderer>().material = this.matManager.customMats[1];
                    break;
                case 3:
                    GameObject.Find("Seat Post").GetComponent<Renderer>().material = this.matManager.customMats[2];
                    break;
                case 4:
                    GameObject.Find("Seat Post").GetComponent<Renderer>().material = this.matManager.customMats[3];
                    break;
                case 5:
                    GameObject.Find("Seat Post").GetComponent<Renderer>().material = this.matManager.customMats[4];
                    break;
                case 6:
                    GameObject.Find("Seat Post").GetComponent<Renderer>().material = this.matManager.customMats[5];
                    break;
            }
        }
        catch (Exception e)
        {
            error += e.Message + " " + e.StackTrace;
        }
    }

    private void SaveTextures()
    {
        try
        {
            saveData += "TEXTURES: ";
            if (TextureManager.instance.frameURL != "")
            {
                saveData += "frame " + TextureManager.instance.frameURL + " ";
            }
            if (TextureManager.instance.barsURL != "")
            {
                saveData += "bars " + TextureManager.instance.barsURL + " ";
            }
            if (TextureManager.instance.seatURL != "")
            {
                saveData += "seat " + TextureManager.instance.seatURL + " ";
            }
            if (TextureManager.instance.forksURL != "")
            {
                saveData += "forks " + TextureManager.instance.forksURL + " ";
            }
            if (TextureManager.instance.tireURL != "")
            {
                saveData += "tire " + TextureManager.instance.tireURL + " ";
            }
            if (TextureManager.instance.tireWallURL != "")
            {
                saveData += "wall " + TextureManager.instance.tireWallURL + " ";
            }
            else
            {
                return;
            }
        }
        catch (Exception e)
        {
            saveErrors += e.Message + " " + e.StackTrace + " ";
        }
    }

    private void LoadTextures()
    {
        string[] words;
        try
        {
            words = scan.ReadToEnd().Split(' ');
            for (int i = 0; i < words.Length; i+=2)
            {
                string key = words[i];
                string url = words[i+1];
                if (key == "frame")
                {
                    TextureManager.instance.SetTexture(0, url);
                }
                else if (key == "bars")
                {
                    TextureManager.instance.SetTexture(1, url);
                }
                else if (key == "seat")
                {
                    TextureManager.instance.SetTexture(2, url);
                }
                else if (key == "forks")
                {
                    TextureManager.instance.SetTexture(3, url);
                }
                else if (key == "tire")
                {
                    TextureManager.instance.SetTexture(4, url);
                }
                else if (key == "wall")
                {
                    TextureManager.instance.SetTexture(5, url);
                }

            }
        }
        catch(Exception e)
        {
            error += e.Message + " " + e.StackTrace;
        }
    }

    private void SaveMeshes()
    {
        try
        {
            saveData += CustomMeshManager.instance.selectedFrame - 1 + " ";
            saveData += CustomMeshManager.instance.selectedBars - 1 + " ";
            saveData += CustomMeshManager.instance.selectedSprocket - 1 + " ";
            saveData += CustomMeshManager.instance.selectedStem - 1 + " ";
            saveData += CustomMeshManager.instance.selectedCranks - 1 + " ";
            saveData += CustomMeshManager.instance.selectedPegs - 1 + " ";
            saveData += CustomMeshManager.instance.selectedSpokes - 1 + " ";
        }
        catch (Exception e)
        {
            saveErrors += e.Message + " " + e.StackTrace + " ";
        }
    }

    private void LoadMeshes()
    {
        try
        {
            int num = this.scan.nextInt();
            CustomMeshManager.instance.SetFrameMesh(num);
            num = this.scan.nextInt();
            CustomMeshManager.instance.SetBarsMesh(num);
            num = this.scan.nextInt();
            CustomMeshManager.instance.SetSprocketMesh(num);
            num = this.scan.nextInt();
            CustomMeshManager.instance.SetStemMesh(num);
            num = this.scan.nextInt();
            CustomMeshManager.instance.SetCranksMesh(num);
            num = this.scan.nextInt();
            CustomMeshManager.instance.SetPegsMesh(num);
            num = this.scan.nextInt();
            CustomMeshManager.instance.SetSpokesMesh(num);
        }
        catch (Exception e)
        {
            error += e.Message + " " + e.StackTrace;
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
            error += e.Message + " " + e.StackTrace;
        }
    }
}
