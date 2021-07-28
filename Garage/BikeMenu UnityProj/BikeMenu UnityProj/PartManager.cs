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
    private bool LHD = true;
    public Material[] tireMats;
    public Material[] tireWallMats;
    public Material tireWallFix;
    public int seatCount = 1;
    public int gripsCount = 1;
    public int frontTiresCount = 1;
    public int frontTireWallCount = 1;
    public int rearTiresCount = 1;
    public int rearTireWallCount = 1;

    private bool flangesVisible = true;

    public Slider seatHeightSlider;
    public Slider seatAngleSlider;
    public Slider bikeScaleSlider;
    public Slider barsAngleSlider;
    public Slider frontTireWidth;
    public Slider rearTireWidth;

    public int partCount = 0;

    private GameObject leftHandTarget;
    private GameObject rightHandTarget;

    float maxSeatHeight = 1f;

    public GameObject bmx;

    private void Awake()
    {
        instance = this;
        bmx = GameObject.Find("BMX");
    }

    private void Start()
    {
        ogRearWheel = PartMaster.instance.GetPart(PartMaster.instance.rearHub).transform;
        chainMesh = PartMaster.instance.GetPart(PartMaster.instance.chain).transform;
        sprocketMesh = PartMaster.instance.GetPart(PartMaster.instance.sprocket).transform;
        seatAngleSlider.value = bmx.GetComponentInChildren<SeatApplyMod>().GetSeatAnglePerc();


        leftHandTarget = PartMaster.instance.GetPart(PartMaster.instance.leftAnchor);
        rightHandTarget = PartMaster.instance.GetPart(PartMaster.instance.rightAnchor);
        leftHandTarget.transform.SetParent(PartMaster.instance.GetPart(PartMaster.instance.bars).transform);
        rightHandTarget.transform.SetParent(PartMaster.instance.GetPart(PartMaster.instance.bars).transform);
        Material[] frontMats = PartMaster.instance.GetMaterials(PartMaster.instance.frontTire);
        Material[] rearMats = PartMaster.instance.GetMaterials(PartMaster.instance.rearTire);
        frontMats[1] = tireWallFix;
        rearMats[1] = tireWallFix;

        PartMaster.instance.SetMaterials(PartMaster.instance.frontTire, frontMats);
        PartMaster.instance.SetMaterials(PartMaster.instance.rearTire, rearMats);

    }

    public void SetSeatHeight(float f)
    {
        PartMaster.instance.GetPart(PartMaster.instance.seatPostAnchor).transform.localPosition = new Vector3(0f, Mathf.Lerp(0f, maxSeatHeight, f), 0f);
        seatHeightSlider.value = f;

    }

    public void SeatUpDown()
    {
        PartMaster.instance.GetPart(PartMaster.instance.seatPostAnchor).transform.localPosition = new Vector3(0f, Mathf.Lerp(0f, maxSeatHeight, seatHeightSlider.value), 0f);
    }

    public void FrontTireWidth()
    {
        bmx.GetComponentInChildren<BikeLoadOut>().SetFrontTireFatness(frontTireWidth.value);
    }

    public void RearTireWidth()
    {
        bmx.GetComponentInChildren<BikeLoadOut>().SetBackTireFatness(rearTireWidth.value);
    }


    public void SetFrontTireWidth(float width)
    {
        bmx.GetComponentInChildren<BikeLoadOut>().SetFrontTireFatness(width);
        frontTireWidth.value = width;
    }

    public void SetRearTireWidth(float width)
    {
        bmx.GetComponentInChildren<BikeLoadOut>().SetBackTireFatness(width);
        rearTireWidth.value = width;
    }

    public float GetTireWidth()
    {
        return bmx.GetComponentInChildren<BikeLoadOut>().GetBackTireFatness();
    }

    public void SwitchDriveSide()
    {
        LHD = !LHD;

        if (!LHD)
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

    public void SetRHD()
    {
        ogRearWheel.localRotation = (Quaternion.Euler(0, 0, 270f));
        chainMesh.localPosition = (new Vector3(0.089f, 0, 0));
        sprocketMesh.localPosition = (new Vector3(0.0467f, 0.001f, 0));
        sprocketMesh.localRotation = (Quaternion.Euler(0, 0, 180f));
        LHD = false;
    }

    public void SetLHD()
    {
        ogRearWheel.localRotation = (Quaternion.Euler(0, 0, 90f));
        chainMesh.localPosition = (new Vector3(0, 0, 0)); ;
        sprocketMesh.localPosition = (new Vector3(-0.0402f, 0.0013f, 0.0001f));
        sprocketMesh.localRotation = (Quaternion.Euler(0, 0, 0));
        LHD = true;
    }

    public bool GetDriveSide()
    {
        return LHD;
    }

    public float GetSeatHeight()
    {
        return seatHeightSlider.value;
    }

    public void SeatAngle()
    {
        bmx.GetComponentInChildren<SeatApplyMod>().SetSeatAnglePerc(seatAngleSlider.value);
    }

    public void SetSeatAngle(float f)
    {
        bmx.GetComponentInChildren<SeatApplyMod>().SetSeatAnglePerc(f);
        seatAngleSlider.value = f;
    }

    public void ChangeSeat()
    {
        bmx.GetComponentInChildren<SeatApplyMod>().SetSeatCoverID(seatCount % bmx.GetComponentInChildren<SeatApplyMod>().seatCovers.Length);
        seatCount = (seatCount % bmx.GetComponentInChildren<SeatApplyMod>().seatCovers.Length) + 1;
    }

    public void BarsAngle()
    {
        bmx.GetComponentInChildren<BarsApplyMod>().SetBarsAnglePerc(barsAngleSlider.value);

    }

    public void SetBarsAngle(float f)
    {
        bmx.GetComponentInChildren<BarsApplyMod>().SetBarsAnglePerc(f);
        barsAngleSlider.value = f;
    }

    public void ChangeFrontTireTread()
    {
        SetFrontTireTread(frontTiresCount);
    }

    public void ChangeRearTireTread()
    {
        SetRearTireTread(rearTiresCount);
    }

    public void SetFrontTireTread(int id)
    {
        int index = (id % tireMats.Length);
        Material[] mats = PartMaster.instance.GetMaterials(PartMaster.instance.frontTire);
        mats[0] = tireMats[index];
        PartMaster.instance.SetMaterials(PartMaster.instance.frontTire, mats);
        TextureManager.instance.normalList[PartMaster.instance.frontTire] = "";
        frontTiresCount = index + 1;
    }

    public void SetRearTireTread(int id)
    {
        int index = (id % tireMats.Length);
        Material[] mats = PartMaster.instance.GetMaterials(PartMaster.instance.rearTire);
        mats[0] = tireMats[index];
        PartMaster.instance.SetMaterials(PartMaster.instance.rearTire, mats);
        TextureManager.instance.normalList[PartMaster.instance.rearTire] = "";
        rearTiresCount = index + 1;
    }

    public void ChangeFrontTireWall()
    {
        SetFrontTireWall(frontTireWallCount);
    }

    public void SetFrontTireWall(int id)
    {
        int index = (id % tireWallMats.Length);
        Material[] mats = PartMaster.instance.GetMaterials(PartMaster.instance.frontTire);
        mats[1] = tireWallMats[index];
        PartMaster.instance.SetMaterials(PartMaster.instance.frontTire, mats);
        TextureManager.instance.normalList[-1] = "";
        TextureManager.instance.albedoList[-1] = "";
        frontTireWallCount = index + 1;
    }

    public void ChangeRearTireWall()
    {
        SetRearTireWall(rearTireWallCount);
    }

    public void SetRearTireWall(int id)
    {
        int index = (id % tireWallMats.Length);
        Material[] mats = PartMaster.instance.GetMaterials(PartMaster.instance.rearTire);
        mats[1] = tireWallMats[index];
        PartMaster.instance.SetMaterials(PartMaster.instance.rearTire, mats);
        TextureManager.instance.normalList[-2] = "";
        TextureManager.instance.albedoList[-2] = "";
        rearTireWallCount = index + 1;
    }

    public void SetSeatID(int id)
    {
        bmx.GetComponentInChildren<SeatApplyMod>().SetSeatCoverID(id % bmx.GetComponentInChildren<SeatApplyMod>().seatCovers.Length);
        seatCount = (id % bmx.GetComponentInChildren<SeatApplyMod>().seatCovers.Length) + 1;
    }



    public void ChangeGrips()
    {
        bmx.GetComponentInChildren<BarsApplyMod>().SetGripsID(gripsCount % bmx.GetComponentInChildren<BarsApplyMod>().gripMats.Length);
        gripsCount++;

    }

    public void SetGripsId(int id)
    {

        bmx.GetComponentInChildren<BarsApplyMod>().SetGripsID(id % bmx.GetComponentInChildren<BarsApplyMod>().gripMats.Length);
        gripsCount = id % bmx.GetComponentInChildren<BarsApplyMod>().gripMats.Length + 1;

    }

    public void ToggleFlanges()
    {
        flangesVisible = !flangesVisible;
        bmx.GetComponentInChildren<BarsApplyMod>().SetFlanges(flangesVisible);
    }

    public void SetFlangesOff()
    {
        bmx.GetComponentInChildren<BarsApplyMod>().SetFlanges(false);
        flangesVisible = false;
    }

    public void SetFlangesOn()
    {
        bmx.GetComponentInChildren<BarsApplyMod>().SetFlanges(true);
        flangesVisible = true;
    }

    public bool GetFlangesVisible()
    {
        return flangesVisible;
    }


    public void SetChainMat(Material newMat)
    {
        Material m = PartMaster.instance.GetMaterial(PartMaster.instance.chain);
        m = newMat;
    }

    public void SetBikeScale()
    {
        float size = bikeScaleSlider.value;
        bmx.transform.localScale = new Vector3(size, size, size);
    }

    public void SetBikeScale(float scale)
    {
        bmx.transform.localScale = new Vector3(scale, scale, scale);
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

    public void SetSeatMesh()
    {
        CustomMeshManager.instance.SetSeatMesh(CustomMeshManager.instance.selectedSeat++);
    }

    public void SetBarAcc()
    {
        CustomMeshManager.instance.SetBarAccMesh(CustomMeshManager.instance.selectedBarAccessory++);
    }

    public void SetFrameAcc()
    {
        CustomMeshManager.instance.SetFrameAccMesh(CustomMeshManager.instance.selectedFrameAccessory++);
    }

    public void SetFrontRimMesh()
    {
        CustomMeshManager.instance.SetFrontRimMesh(CustomMeshManager.instance.selectedFrontRim++);
    }

    public void SetRearRimMesh()
    {
        CustomMeshManager.instance.SetRearRimMesh(CustomMeshManager.instance.selectedRearRim++);
    }

    public void SetFrontHubGuardMesh()
    {
        CustomMeshManager.instance.SetFrontHubGuardMesh(CustomMeshManager.instance.selectedFrontHubGuard++);
    }

    public void SetRearHubGuardMesh()
    {
        CustomMeshManager.instance.SetRearHubGuardMesh(CustomMeshManager.instance.selectedRearHubGuard++);
    }

    public void SetSeatPostMesh()
    {
        CustomMeshManager.instance.SetSeatPostMesh(CustomMeshManager.instance.selectedSeatPost++);
    }
}

