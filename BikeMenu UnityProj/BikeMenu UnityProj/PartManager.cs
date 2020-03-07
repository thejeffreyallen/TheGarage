using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartManager : MonoBehaviour
{

    public static PartManager instance;
    private Transform ogRearWheel;
    private Transform chainMesh;
    private Transform sprocketMesh;
    private bool LHD = false;
    public int seatCount = 1;
    public int gripsCount = 1;
    public int tiresCount = 1;

    private bool flangesVisible = true;

    public Slider seatHeightSlider;
    public Slider seatAngleSlider;
    public Slider bikeScaleSlider;
    public Slider barsAngleSlider;
    public int partCount = 0;

    private GameObject leftHandTarget;
    private GameObject rightHandTarget;
    


    Transform bmx;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ogRearWheel = GameObject.Find("Hub Mesh").transform;
        chainMesh = GameObject.Find("Chain Mesh").transform;
        sprocketMesh = GameObject.Find("Sprocket Mesh").transform;

        bmx = GameObject.Find("BMX").transform;

        leftHandTarget = GameObject.Find("Left Anchor");
        rightHandTarget = GameObject.Find("Right Anchor");
        leftHandTarget.transform.SetParent(GameObject.Find("Bars Mesh").transform);
        rightHandTarget.transform.SetParent(GameObject.Find("Bars Mesh").transform);

    }



    public void SwitchDriveSide()
    {
        LHD = !LHD;

        if (LHD)
        {
            ogRearWheel.localRotation = (Quaternion.Euler(0, 0, 270f));
            chainMesh.localPosition = (new Vector3(0.089f, 0, 0));
            sprocketMesh.localPosition = (new Vector3(0.0467f, 0.001f, 0));
        }
        else
        {
            ogRearWheel.localRotation = (Quaternion.Euler(0, 0, 90f));
            chainMesh.localPosition = (new Vector3(0, 0, 0)); ;
            sprocketMesh.localPosition = (new Vector3(-0.0402f, 0.0013f, 0.0001f));
        }
    }

    public void SetLHD()
    {
        ogRearWheel.localRotation = (Quaternion.Euler(0, 0, 270f));
        chainMesh.localPosition = (new Vector3(0.089f, 0, 0));
        sprocketMesh.localPosition = (new Vector3(0.0467f, 0.001f, 0));
        LHD = true;
    }

    public void SetRHD()
    {
        ogRearWheel.localRotation = (Quaternion.Euler(0, 0, 90f));
        chainMesh.localPosition = (new Vector3(0, 0, 0)); ;
        sprocketMesh.localPosition = (new Vector3(-0.0402f, 0.0013f, 0.0001f));
        LHD = false;
    }

    public bool GetDriveSide()
    {
        return this.LHD;
    }

    public float GetSeatHeight()
    {
        return seatHeightSlider.value;
    }

    public void SetSeatHeight(float f)
    {
        FindObjectOfType<SeatApplyMod>().SetSeatHeight(f);
        seatHeightSlider.value = f;
    }

    public void SeatUpDown()
    {
        FindObjectOfType<SeatApplyMod>().SetSeatHeight(seatHeightSlider.value);
    }

    public void SeatAngle()
    {
        FindObjectOfType<SeatApplyMod>().SetSeatAnglePerc(seatAngleSlider.value);
    }

    public void SetSeatAngle(float f)
    {
        FindObjectOfType<SeatApplyMod>().SetSeatAnglePerc(f);
        seatAngleSlider.value = f;
    }

    public void ChangeSeat()
    {
        if (seatCount < FindObjectOfType<SeatApplyMod>().seatCovers.Length)
        {
            FindObjectOfType<SeatApplyMod>().SetSeatCoverID(seatCount);
            seatCount++;
        }
        else
        {
            FindObjectOfType<SeatApplyMod>().SetSeatCoverID(0);
            seatCount = 1;
        }
    }

    public void BarsAngle()
    {
        FindObjectOfType<BarsApplyMod>().SetBarsAnglePerc(barsAngleSlider.value);
        
    }

    public void SetBarsAngle(float f)
    {
        FindObjectOfType<BarsApplyMod>().SetBarsAnglePerc(f);
        barsAngleSlider.value = f;
    }

    public void ChangeTireTread()
    {
        if (tiresCount < 3)
        {
            FindObjectOfType<BikeLoadOut>().SetFrontTireTextureID(tiresCount);
            FindObjectOfType<BikeLoadOut>().SetBackTireTextureID(tiresCount);
            tiresCount++;
        }
        else
        {
            BetterWheelsMod.instance.SetTireTread();
            tiresCount = 0;
        }
    }

    public void SetTireTread(int id)
    {
        if (id < 3)
        {
            FindObjectOfType<BikeLoadOut>().SetFrontTireTextureID(id);
            FindObjectOfType<BikeLoadOut>().SetBackTireTextureID(id);
        }
        if (id == 3)
        {
            BetterWheelsMod.instance.SetTireTread();
            tiresCount = 0;
        }
        else
        {
            tiresCount = id + 1;
        }

    }

    public void SetSeatID(int id)
    {
        
        FindObjectOfType<SeatApplyMod>().SetSeatCoverID(id);
        if (id == FindObjectOfType<SeatApplyMod>().seatCovers.Length)
        {
            seatCount = 0;
        }
        else
        {
            seatCount = id + 1;
        }
        
    }

    public void ChangeGrips()
    {
        if (gripsCount < FindObjectOfType<BarsApplyMod>().gripMats.Length)
        {
            FindObjectOfType<BarsApplyMod>().SetGripsID(gripsCount);
            gripsCount++;
        }
        else
        {
            FindObjectOfType<BarsApplyMod>().SetGripsID(0);
            gripsCount = 1;
        }
        
    }

    public void SetGripsId(int id)
    {

        FindObjectOfType<BarsApplyMod>().SetGripsID(id);
        if (id == FindObjectOfType<BarsApplyMod>().gripMats.Length)
        {
            gripsCount = 0;
        }
        else
        {
            gripsCount = id + 1;
        }

    }

    public void ToggleFlanges()
    {
        flangesVisible = !flangesVisible;
        FindObjectOfType<BarsApplyMod>().SetFlanges(flangesVisible);
    }

    public void SetFlangesOff()
    {
        FindObjectOfType<BarsApplyMod>().SetFlanges(false);
        flangesVisible = false;
    }

    public void SetFlangesOn()
    {
        FindObjectOfType<BarsApplyMod>().SetFlanges(true);
        flangesVisible = true;
    }

    public bool GetFlangesVisible()
    {
        return this.flangesVisible;
    }


    public void SetChainMat(Material newMat)
    {
        GameObject.Find("Chain Mesh").GetComponent<Renderer>().material = newMat;
    }

    public void SetBikeScale()
    {
        float size = bikeScaleSlider.value;
        bmx.localScale = new Vector3(size, size, size);
    }

    public void SetBikeScale(float scale)
    {
        bmx.localScale = new Vector3(scale, scale, scale);
        bikeScaleSlider.value = scale;
    }

    public void SetFrameMesh()
    {
        CustomMeshManager.instance.SetMesh("Frame Mesh", CustomMeshManager.instance.frameMeshes, CustomMeshManager.instance.selectedFrame++, CustomMeshManager.instance.selectedFrameText);
    }

    public void SetBarsMesh()
    {
        CustomMeshManager.instance.SetMesh("Bars Mesh", CustomMeshManager.instance.barMeshes, CustomMeshManager.instance.selectedBars++, CustomMeshManager.instance.selectedBarsText);
    }

    public void SetPegsMesh()
    {
        CustomMeshManager.instance.SetMultipleMesh("Pegs Mesh", CustomMeshManager.instance.pegMeshes, CustomMeshManager.instance.selectedPegs++, CustomMeshManager.instance.selectedPegsText);
    }

    public void SetSpokesMesh()
    {
        CustomMeshManager.instance.SetMultipleMesh("Spokes Mesh", CustomMeshManager.instance.spokesMeshes, CustomMeshManager.instance.selectedSpokes++, CustomMeshManager.instance.selectedSpokesText);
    }

    public void SetSprocketMesh()
    {
        CustomMeshManager.instance.SetMesh("Sprocket Mesh", CustomMeshManager.instance.sprocketMeshes, CustomMeshManager.instance.selectedSprocket++, CustomMeshManager.instance.selectedSprocketText);
    }

    public void SetStemMesh()
    {
        CustomMeshManager.instance.SetMesh("Stem Mesh", CustomMeshManager.instance.stemMeshes, CustomMeshManager.instance.selectedStem++, CustomMeshManager.instance.selectedStemText);
    }

    public void SetCranksMesh()
    {
        int selected = CustomMeshManager.instance.selectedCranks++;
        Material m = GameObject.Find("Right Crank Arm Mesh").GetComponent<Renderer>().material;
        CustomMeshManager.instance.SetMesh("Right Crank Arm Mesh", CustomMeshManager.instance.cranksMeshes, selected, CustomMeshManager.instance.selectedCranksText);
        CustomMeshManager.instance.SetMesh("Left Crank Arm Mesh", CustomMeshManager.instance.cranksMeshes, selected, CustomMeshManager.instance.selectedCranksText);
        GameObject.Find("Left Crank Arm Mesh").GetComponent<Renderer>().material = m;
    }
}

