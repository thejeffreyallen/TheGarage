using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using System.Collections;

public class MaterialManager : MonoBehaviour
{
    public static MaterialManager instance;
    public Material[] customMats;
    private int matCount = 0;
    
    [HideInInspector]
    public Material defaultMat;
    public Material OskersFrameMat;
    public Material OskersForksMat;
    public Material OskersBarsMat;
    private Material defaultSeatMat;

    public Material chainMatFix;

    private void Awake()
    {
        StartCoroutine(DestroyNormal());
        instance = this;
    }

    public void Start()
    {
        defaultMat = FindObjectOfType<BikeLoadOut>().GetPartMat(0);
        defaultSeatMat = PartMaster.instance.GetMaterial(PartMaster.instance.seat);
        PartMaster.instance.SetMaterial(PartMaster.instance.chain, chainMatFix);
    }

    public void SetMaterial(Material mat)
    {
        int key = ColourSetter.instance.currentPart;
        Debug.Log("Setting material " + mat.name + " on part number: " + key);
        switch (key)
        {
            case -1:
                PartMaster.instance.SetMaterial(1, PartMaster.instance.frontTire, mat);
                break;
            case -2:
                PartMaster.instance.SetMaterial(1, PartMaster.instance.rearTire, mat);
                break;
            case -3:
                if (BrakesManager.instance.IsEnabled())
                {
                    BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[2] = mat;
                    BrakesManager.instance.GetBarBrakes().GetComponent<Renderer>().materials[1] = mat;
                }
                break;
            case -4:
                if (BrakesManager.instance.IsEnabled())
                {
                    BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[3] = mat;
                    BrakesManager.instance.GetBarBrakes().GetComponent<Renderer>().materials[2] = mat;
                }
                break;
            default:
                PartMaster.instance.SetMaterial(ColourSetter.instance.currentPart, mat);
                break;
        }
        
    }

    public void SetDefaultMat()
    {
        int key = ColourSetter.instance.currentPart;
        Debug.Log("Setting default material " + defaultMat.name + " on part number: " + key);
        switch (key)
        {
            case -1:
                PartMaster.instance.SetMaterial(1, PartMaster.instance.frontTire, defaultMat);
                break;
            case -2:
                PartMaster.instance.SetMaterial(1, PartMaster.instance.rearTire, defaultMat);
                break;
            case -3:
                if (BrakesManager.instance.IsEnabled())
                {
                    BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[2] = defaultMat;
                    BrakesManager.instance.GetBarBrakes().GetComponent<Renderer>().materials[1] = defaultMat;
                }
                break;
            case -4:
                if (BrakesManager.instance.IsEnabled())
                {
                    BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[3] = defaultMat;
                    BrakesManager.instance.GetBarBrakes().GetComponent<Renderer>().materials[2] = defaultMat;
                }
                break;
            default:
                PartMaster.instance.SetMaterial(ColourSetter.instance.currentPart, defaultMat);
                break;

        }
        
    }

    public void SwapMaterial(int key)
    {
        Debug.Log("Swap material " + customMats[matCount].name + " on part number: " + key);

        if (matCount < customMats.Length)
        {
            switch (key)
            {
                case -1:
                    PartMaster.instance.SetMaterial(1, PartMaster.instance.frontTire, customMats[matCount]);
                    break;
                case -2:
                    PartMaster.instance.SetMaterial(1, PartMaster.instance.rearTire, customMats[matCount]);
                    break;
                case -3:
                    if (BrakesManager.instance.IsEnabled())
                    {
                        BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[2] = customMats[matCount];
                        BrakesManager.instance.GetBarBrakes().GetComponent<Renderer>().materials[1] = customMats[matCount];
                    }
                    break;
                case -4:
                    if (BrakesManager.instance.IsEnabled())
                    {
                        BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[3] = customMats[matCount];
                        BrakesManager.instance.GetBarBrakes().GetComponent<Renderer>().materials[2] = customMats[matCount];
                    }
                    break;
                default:
                    PartMaster.instance.SetMaterial(ColourSetter.instance.currentPart, customMats[matCount]);
                    break;

            }
            matCount++;
        }
        else
        {
            matCount = 0;
            switch (key)
            {
                case -1:
                    PartMaster.instance.SetMaterial(1, PartMaster.instance.frontTire, defaultMat);
                    break;
                case -2:
                    PartMaster.instance.SetMaterial(1, PartMaster.instance.rearTire, defaultMat);
                    break;
                case -3:
                    if (BrakesManager.instance.IsEnabled())
                    {
                        BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[2] = defaultMat;
                        BrakesManager.instance.GetBarBrakes().GetComponent<Renderer>().materials[1] = defaultMat;
                    }
                    break;
                case -4:
                    if (BrakesManager.instance.IsEnabled())
                    {
                        BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[3] = defaultMat;
                        BrakesManager.instance.GetBarBrakes().GetComponent<Renderer>().materials[2] = defaultMat;
                    }
                    break;
                default:
                    PartMaster.instance.SetMaterial(ColourSetter.instance.currentPart, defaultMat);
                    break;

            }
        }
    }

    private IEnumerator DestroyNormal()
    {
        yield return new WaitForSeconds(0.1f);
        if (defaultMat == null)
            defaultMat = FindObjectOfType<BikeLoadOut>().GetPartMat(0);
        defaultMat.SetFloat("_DetailNormalMapScale", 0f);
        defaultMat.color = Color.black;
        Debug.Log("Destroying detail normal maps ");
        for (int i = 0; i < 20; i++)
        {
                Material m = FindObjectOfType<BikeLoadOut>().GetPartMat(i);
                if (m.name.ToLower().Contains("a_glossy"))
                {
                    FindObjectOfType<BikeLoadOut>().SetPartMaterial(defaultMat, i, true);
                }
                
        }
        yield break;
    }

}