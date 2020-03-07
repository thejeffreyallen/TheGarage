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
    private Transform[] stockFrontWheel;
    private Transform[] stockRearWheel;
    public Material betterWheelMat;
    public Material[] tireMats;
    private Mesh oldRim;
    private Mesh oldTire;
    private Mesh oldHub;
    private Material oldRimMat;
    private Material oldHubMat;
    private Material[] oldTireMats;


    private void Start()
    {
        stockFrontWheel = GameObject.Find("BMX:Wheel").GetComponentsInChildren<Transform>();
        stockRearWheel = GameObject.Find("BMX:Wheel 1").GetComponentsInChildren<Transform>();
        //Store old parts
        oldRim = stockFrontWheel[1].gameObject.GetComponent<MeshFilter>().mesh;
        oldHub = stockFrontWheel[3].gameObject.GetComponent<MeshFilter>().mesh;
        oldTire = stockFrontWheel[6].gameObject.GetComponent<MeshFilter>().mesh;
        //Store old materials
        oldRimMat = stockFrontWheel[1].gameObject.GetComponent<Renderer>().material;
        oldHubMat = stockFrontWheel[3].gameObject.GetComponent<Renderer>().material;
        oldTireMats = stockFrontWheel[6].gameObject.GetComponent<Renderer>().materials;

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
        return this.modEnabled;
    }

    public void SetTireTread()
    {
        stockFrontWheel[6].gameObject.GetComponent<Renderer>().materials = tireMats;
        stockRearWheel[3].gameObject.GetComponent<Renderer>().materials = tireMats;
    }

    public void ApplyMod()
    {
        

        //Front wheel rim
        stockFrontWheel[1].gameObject.GetComponent<MeshFilter>().mesh = newRim;
        stockFrontWheel[1].gameObject.GetComponent<Renderer>().material = betterWheelMat;
        //Front Wheel hub
        stockFrontWheel[3].gameObject.GetComponent<MeshFilter>().mesh = newFrontHub;
        stockFrontWheel[3].gameObject.GetComponent<Renderer>().material = betterWheelMat;
        //Front wheel tire
        stockFrontWheel[6].gameObject.GetComponent<MeshFilter>().mesh = newTire;
        stockFrontWheel[6].gameObject.GetComponent<Renderer>().materials = tireMats;

        //Rear wheel rim
        stockRearWheel[1].gameObject.GetComponent<MeshFilter>().mesh = newRim;
        stockRearWheel[1].gameObject.GetComponent<Renderer>().material = betterWheelMat;
        //Rear wheel tire
        stockRearWheel[3].gameObject.GetComponent<MeshFilter>().mesh = newTire;
        stockRearWheel[3].gameObject.GetComponent<Renderer>().materials = tireMats;
        //rear wheel hub
        stockRearWheel[4].gameObject.GetComponent<MeshFilter>().mesh = newRearHub;
        stockRearWheel[4].gameObject.GetComponent<Renderer>().material = betterWheelMat;
        this.modEnabled = true;

    }

    public void DisableMod()
    {
        //Front wheel rim
        stockFrontWheel[1].gameObject.GetComponent<MeshFilter>().mesh = oldRim;
        stockFrontWheel[1].gameObject.GetComponent<Renderer>().material = oldRimMat;
        //Front Wheel hub
        stockFrontWheel[3].gameObject.GetComponent<MeshFilter>().mesh = oldHub;
        stockFrontWheel[3].gameObject.GetComponent<Renderer>().material = oldHubMat;
        //Front wheel tire
        stockFrontWheel[6].gameObject.GetComponent<MeshFilter>().mesh = oldTire;
        stockFrontWheel[6].gameObject.GetComponent<Renderer>().materials = oldTireMats;

        //Rear wheel rim
        stockRearWheel[1].gameObject.GetComponent<MeshFilter>().mesh = oldRim;
        stockRearWheel[1].gameObject.GetComponent<Renderer>().material = oldRimMat;
        //Rear wheel tire
        stockRearWheel[3].gameObject.GetComponent<MeshFilter>().mesh = oldTire;
        stockRearWheel[3].gameObject.GetComponent<Renderer>().materials = oldTireMats;
        //rear wheel hub
        stockRearWheel[4].gameObject.GetComponent<MeshFilter>().mesh = oldHub;
        stockRearWheel[4].gameObject.GetComponent<Renderer>().material = oldHubMat;

        this.modEnabled = false;
    }

}