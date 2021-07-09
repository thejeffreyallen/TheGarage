using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class BetterWheelsMod : MonoBehaviour
{
    public static BetterWheelsMod instance;

    bool modEnabled;

    public Mesh newRim;
    public Mesh newTire;
    public Mesh newFrontHub;
    public Mesh newRearHub;
    public Material betterWheelMat;
    public Material[] tireMats;
    private Mesh oldRim;
    private Mesh oldTire;
    private Mesh oldHub;
    private Material oldRimMat;
    private Material oldHubMat;
    private Material[] oldTireMats;

    bool hasFrontHubChanged = false;
    bool hasRearHubChanged = false;

    private void Start()
    {
        //Store old parts
        oldRim = PartMaster.instance.GetMesh(PartMaster.instance.frontRim);
        oldHub = PartMaster.instance.GetMesh(PartMaster.instance.frontHub);
        oldTire = PartMaster.instance.GetPart(PartMaster.instance.frontTire).GetComponent<MeshBlendShape>().referenceMesh;
        //Store old materials
        oldRimMat = PartMaster.instance.GetMaterial(PartMaster.instance.frontRim);
        oldHubMat = PartMaster.instance.GetMaterial(PartMaster.instance.frontHub);
        oldTireMats = PartMaster.instance.GetMaterials(PartMaster.instance.frontTire); ;

        instance = this;
    }

    public void SetMod()
    {
        modEnabled = !modEnabled;

        if (modEnabled)
            ApplyMod();
        else
            DisableMod();
    }

    public bool GetBetterWheels()
    {
        return modEnabled;
    }

    public void SetTireTread()
    {
        PartMaster.instance.GetPart(PartMaster.instance.frontTire).GetComponent<Renderer>().materials = tireMats;
        PartMaster.instance.GetPart(PartMaster.instance.rearTire).GetComponent<Renderer>().materials = tireMats;
    }

    public void ChangeFrontHub()
    {
        hasFrontHubChanged = true;
    }

    public void ChangeRearHub()
    {
        hasRearHubChanged = true;
    }

    public bool CheckFront()
    {
        return hasFrontHubChanged;
    }

    public bool CheckRear()
    {
        return hasRearHubChanged;
    }

    public void ApplyMod()
    {
        //Front wheel rim
        PartMaster.instance.GetPart(PartMaster.instance.frontRim).GetComponent<MeshFilter>().mesh = newRim;
        PartMaster.instance.GetPart(PartMaster.instance.frontRim).GetComponent<Renderer>().material = betterWheelMat;
        //Front Wheel hub
        PartMaster.instance.GetPart(PartMaster.instance.frontHub).GetComponent<MeshFilter>().mesh = newFrontHub;
        PartMaster.instance.GetPart(PartMaster.instance.frontHub).GetComponent<Renderer>().material = betterWheelMat;
        //Front wheel tire
        PartMaster.instance.GetPart(PartMaster.instance.frontTire).GetComponent<MeshBlendShape>().referenceMesh = newTire;
        PartMaster.instance.GetPart(PartMaster.instance.frontTire).GetComponent<Renderer>().materials = tireMats;

        //Rear wheel rim
        PartMaster.instance.GetPart(PartMaster.instance.rearRim).GetComponent<MeshFilter>().mesh = newRim;
        PartMaster.instance.GetPart(PartMaster.instance.rearRim).GetComponent<Renderer>().material = betterWheelMat;
        //Rear wheel tire
        PartMaster.instance.GetPart(PartMaster.instance.rearTire).GetComponent<MeshBlendShape>().referenceMesh = newTire;
        PartMaster.instance.GetPart(PartMaster.instance.rearTire).GetComponent<Renderer>().materials = tireMats;
        //rear wheel hub
        PartMaster.instance.GetPart(PartMaster.instance.rearHub).GetComponent<MeshFilter>().mesh = newRearHub;
        PartMaster.instance.GetPart(PartMaster.instance.rearHub).GetComponent<Renderer>().material = betterWheelMat;

        modEnabled = true;
        PartManager.instance.tiresCount = 3;
        hasFrontHubChanged = false;
        hasRearHubChanged = false;

    }

    public void DisableMod()
    {
        //Front wheel rim
        PartMaster.instance.GetPart(PartMaster.instance.frontRim).GetComponent<MeshFilter>().mesh = oldRim;
        PartMaster.instance.GetPart(PartMaster.instance.frontRim).GetComponent<Renderer>().material = oldRimMat;
        //Front Wheel hub
        PartMaster.instance.GetPart(PartMaster.instance.frontHub).GetComponent<MeshFilter>().mesh = oldHub;
        PartMaster.instance.GetPart(PartMaster.instance.frontHub).GetComponent<Renderer>().material = oldHubMat;
        //Front wheel tire
        PartMaster.instance.GetPart(PartMaster.instance.frontTire).GetComponent<MeshBlendShape>().referenceMesh = oldTire;
        PartMaster.instance.GetPart(PartMaster.instance.frontTire).GetComponent<Renderer>().materials = oldTireMats;

        //Rear wheel rim
        PartMaster.instance.GetPart(PartMaster.instance.rearRim).GetComponent<MeshFilter>().mesh = oldRim;
        PartMaster.instance.GetPart(PartMaster.instance.rearRim).GetComponent<Renderer>().material = oldRimMat;
        //Rear wheel tire
        PartMaster.instance.GetPart(PartMaster.instance.rearTire).GetComponent<MeshBlendShape>().referenceMesh = oldTire;
        PartMaster.instance.GetPart(PartMaster.instance.rearTire).GetComponent<Renderer>().materials = oldTireMats;
        //rear wheel hub
        PartMaster.instance.GetPart(PartMaster.instance.rearHub).GetComponent<MeshFilter>().mesh = oldHub;
        PartMaster.instance.GetPart(PartMaster.instance.rearHub).GetComponent<Renderer>().material = oldHubMat;

        modEnabled = false;
    }

    public void OnChangeHubFront()
    {
        if (!modEnabled)
            return;
        PartMaster.instance.GetPart(PartMaster.instance.frontHub).GetComponent<Renderer>().material = oldHubMat;
    }

    public void OnChangeHubRear()
    {
        if (!modEnabled)
            return;
        PartMaster.instance.GetPart(PartMaster.instance.rearHub).GetComponent<Renderer>().material = oldHubMat;
    }

}