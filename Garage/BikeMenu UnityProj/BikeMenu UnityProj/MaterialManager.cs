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
        defaultMat = FindObjectOfType<BikeLoadOut>().GetPartMat(0);
    }

    public void Start()
    {
        defaultSeatMat = PartMaster.instance.GetMaterial(PartMaster.instance.seat);
        PartMaster.instance.SetMaterial(PartMaster.instance.chain, chainMatFix);
    }

    public void SetMaterial(Material mat)
    {
        List<int> activeList = ColourSetter.instance.GetActivePartList();
        foreach (int key in activeList)
        {
            Debug.Log("Setting material " + mat.name + " on part number: " + key);
            switch (key)
            {
                case -1:
                    Material[] mats = PartMaster.instance.GetMaterials(PartMaster.instance.frontTire);
                    mats[1] = mat;
                    PartMaster.instance.SetMaterials(PartMaster.instance.frontTire, mats);
                    break;
                case -2:
                    Material[] mats2 = PartMaster.instance.GetMaterials(PartMaster.instance.rearTire);
                    mats2[1] = mat;
                    PartMaster.instance.SetMaterials(PartMaster.instance.rearTire, mats2);
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
                    PartMaster.instance.SetMaterial(key, mat);
                    break;
            }
        }
        
    }

    public void SetDefaultMat()
    {
        List<int> activeList = ColourSetter.instance.GetActivePartList();
        foreach (int key in activeList)
        {
            Debug.Log("Setting default material " + defaultMat.name + " on part number: " + key);
            switch (key)
            {
                case -1:
                    Material[] mats = PartMaster.instance.GetMaterials(PartMaster.instance.frontTire);
                    mats[1] = defaultMat;
                    PartMaster.instance.SetMaterials(PartMaster.instance.frontTire, mats);
                    break;
                case -2:
                    Material[] mats2 = PartMaster.instance.GetMaterials(PartMaster.instance.rearTire);
                    mats2[1] = defaultMat;
                    PartMaster.instance.SetMaterials(PartMaster.instance.rearTire, mats2);
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
                    PartMaster.instance.SetMaterial(key, defaultMat);
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