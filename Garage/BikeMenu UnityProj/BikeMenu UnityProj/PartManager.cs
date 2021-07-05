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
    public Slider tireWidth;
    public Slider frontTireWidth;
    public Slider rearTireWidth;

    public int partCount = 0;

    private GameObject leftHandTarget;
    private GameObject rightHandTarget;

    float maxSeatHeight = 1f;
    


    Transform bmx;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ogRearWheel = PartMaster.instance.GetPart(PartMaster.instance.rearHub).transform;
        chainMesh = PartMaster.instance.GetPart(PartMaster.instance.chain).transform;
        sprocketMesh = PartMaster.instance.GetPart(PartMaster.instance.sprocket).transform;
        seatHeightSlider.value = PartMaster.instance.GetPart(PartMaster.instance.seatPostAnchor).transform.localPosition.y;
        bmx = GameObject.Find("BMX").transform;

        leftHandTarget = PartMaster.instance.GetPart(PartMaster.instance.leftAnchor);
        rightHandTarget = PartMaster.instance.GetPart(PartMaster.instance.rightAnchor);
        leftHandTarget.transform.SetParent(PartMaster.instance.GetPart(PartMaster.instance.bars).transform);
        rightHandTarget.transform.SetParent(PartMaster.instance.GetPart(PartMaster.instance.bars).transform);

    }

    public void SetSeatHeight(float f)
    {
        PartMaster.instance.GetPart(PartMaster.instance.seatPostAnchor).transform.localPosition = new Vector3(0f, Mathf.Lerp(0f, this.maxSeatHeight, f), 0f);
        seatHeightSlider.value = f;
        
    }

    public void SeatUpDown()
    {
        PartMaster.instance.GetPart(PartMaster.instance.seatPostAnchor).transform.localPosition = new Vector3(0f, Mathf.Lerp(0f, this.maxSeatHeight, seatHeightSlider.value), 0f);
    }

    public void FrontTireWidth()
    {
        FindObjectOfType<BikeLoadOut>().SetFrontTireFatness(frontTireWidth.value);
    }

    public void RearTireWidth()
    {
        FindObjectOfType<BikeLoadOut>().SetBackTireFatness(rearTireWidth.value);
    }


    public void SetTireWidth(float width)
    {
        FindObjectOfType<BikeLoadOut>().SetBackTireFatness(width);
        FindObjectOfType<BikeLoadOut>().SetFrontTireFatness(width);
        tireWidth.value = width;
    }

    // Not yet implemented
    public void SetFrontTireWidth(float width)
    {
        FindObjectOfType<BikeLoadOut>().SetFrontTireFatness(width);
        frontTireWidth.value = width;
    }

    // Not yet implemented
    public void SetRearTireWidth(float width)
    {
        FindObjectOfType<BikeLoadOut>().SetBackTireFatness(width);
        rearTireWidth.value = width;
    }

    public float GetTireWidth()
    {
        return FindObjectOfType<BikeLoadOut>().GetBackTireFatness();
    }

    public void SwitchDriveSide()
    {
        LHD = !LHD;

        if (LHD)
        {
            ogRearWheel.localRotation = (Quaternion.Euler(0, 0, 270f));
            chainMesh.localPosition = (new Vector3(0.089f, 0, 0));
            sprocketMesh.localPosition = (new Vector3(0.0467f, 0.001f, 0));
            sprocketMesh.localRotation = (Quaternion.Euler(0, 0, 180f));
        }
        else
        {
            ogRearWheel.localRotation = (Quaternion.Euler(0, 0, 90f));
            chainMesh.localPosition = (new Vector3(0, 0, 0)); ;
            sprocketMesh.localPosition = (new Vector3(-0.0402f, 0.0013f, 0.0001f));
            sprocketMesh.localRotation = (Quaternion.Euler(0, 0, 0));
        }
    }

    public void SetLHD()
    {
        ogRearWheel.localRotation = (Quaternion.Euler(0, 0, 270f));
        chainMesh.localPosition = (new Vector3(0.089f, 0, 0));
        sprocketMesh.localPosition = (new Vector3(0.0467f, 0.001f, 0));
        sprocketMesh.localRotation = (Quaternion.Euler(0, 0, 180f));
        LHD = true;
    }

    public void SetRHD()
    {
        ogRearWheel.localRotation = (Quaternion.Euler(0, 0, 90f));
        chainMesh.localPosition = (new Vector3(0, 0, 0)); ;
        sprocketMesh.localPosition = (new Vector3(-0.0402f, 0.0013f, 0.0001f));
        sprocketMesh.localRotation = (Quaternion.Euler(0, 0, 0));
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
        FindObjectOfType<SeatApplyMod>().SetSeatCoverID(seatCount % FindObjectOfType<SeatApplyMod>().seatCovers.Length);
        seatCount++;
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
            tiresCount = id + 1;
        }
        else
        {
            BetterWheelsMod.instance.SetTireTread();
            tiresCount = 0;
        }

    }

    public void SetSeatID(int id)
    {
        FindObjectOfType<SeatApplyMod>().SetSeatCoverID(id % FindObjectOfType<SeatApplyMod>().seatCovers.Length);
        seatCount = (id % FindObjectOfType<SeatApplyMod>().seatCovers.Length) + 1;
    }

    public void SetSeatMesh()
    {
        CustomMeshManager.instance.SetSeatMesh(CustomMeshManager.instance.selectedSeat++);
    }

    public void ChangeGrips()
    {
        FindObjectOfType<BarsApplyMod>().SetGripsID(gripsCount % FindObjectOfType<BarsApplyMod>().gripMats.Length);
        gripsCount++;
   
    }

    public void SetGripsId(int id)
    {

        FindObjectOfType<BarsApplyMod>().SetGripsID(id % FindObjectOfType<BarsApplyMod>().gripMats.Length);
        gripsCount = id % FindObjectOfType<BarsApplyMod>().gripMats.Length + 1;

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
        Material m = PartMaster.instance.GetMaterial(PartMaster.instance.chain);
        m = newMat;
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
        CustomMeshManager.instance.SetFrameMesh(CustomMeshManager.instance.selectedFrame++);
    }

    public void SetForksMesh()
    {
        CustomMeshManager.instance.SetForksMesh(CustomMeshManager.instance.selectedForks++);
    }

    public void SetBarsMesh()
    {
        CustomMeshManager.instance.SetBarsMesh(CustomMeshManager.instance.selectedBars++);
    }

    public void SetFrontPegsMesh()
    {
        CustomMeshManager.instance.SetFrontPegsMesh(CustomMeshManager.instance.selectedFrontPegs++);
    }

    public void SetRearPegsMesh()
    {
        CustomMeshManager.instance.SetRearPegsMesh(CustomMeshManager.instance.selectedRearPegs++);
    }

    public void SetFrontSpokesMesh()
    {
        CustomMeshManager.instance.SetFrontSpokesMesh(CustomMeshManager.instance.selectedFrontSpokes++);
    }

    public void SetRearSpokesMesh()
    {
        CustomMeshManager.instance.SetRearSpokesMesh(CustomMeshManager.instance.selectedRearSpokes++);
    }

    public void SetFrontHubMesh()
    {
        CustomMeshManager.instance.SetFrontHubMesh(CustomMeshManager.instance.selectedFrontHub++);
    }

    public void SetRearHubMesh()
    {
        CustomMeshManager.instance.SetRearHubMesh(CustomMeshManager.instance.selectedRearHub++);
    }

    public void SetPedalsMesh()
    {
        CustomMeshManager.instance.SetPedalsMesh(CustomMeshManager.instance.selectedPedals++);
    }

    public void SetSprocketMesh()
    {
        CustomMeshManager.instance.SetSprocketMesh(CustomMeshManager.instance.selectedSprocket++);
    }

    public void SetStemMesh()
    {
        CustomMeshManager.instance.SetStemMesh(CustomMeshManager.instance.selectedStem++);
    }

    public void SetFrontSpokeAcc()
    {
        CustomMeshManager.instance.SetFrontSpokeAccMesh(CustomMeshManager.instance.selectedFrontAccessory++);
    }

    public void SetRearSpokeAcc()
    {
        CustomMeshManager.instance.SetRearSpokeAccMesh(CustomMeshManager.instance.selectedRearAccessory++);
    }

    public void SetCranksMesh()
    {
        CustomMeshManager.instance.SetCranksMesh(CustomMeshManager.instance.selectedCranks++);
    }
}

