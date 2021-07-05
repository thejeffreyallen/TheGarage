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
        if (ColourSetter.instance.GetSeatPostBool())
        {
            PartMaster.instance.SetMaterial(PartMaster.instance.seatPost,mat);
        }
        else if (ColourSetter.instance.GetSeatBool())
        {
            PartMaster.instance.SetMaterial(PartMaster.instance.seat, mat);
        }
        else
        {
            FindObjectOfType<BikeLoadOut>().SetPartMaterial(mat, ColourSetter.instance.currentPart, true);
        }
        
    }

    public void SetDefaultMat()
    {
        if (ColourSetter.instance.GetSeatPostBool())
        {
            PartMaster.instance.SetMaterial(PartMaster.instance.seatPost, defaultMat);
        }
        else if (ColourSetter.instance.GetSeatBool())
        {
            PartMaster.instance.SetMaterial(PartMaster.instance.seat, defaultMat);
        }
        else
            FindObjectOfType<BikeLoadOut>().SetPartMaterial(defaultMat, FindObjectOfType<ColourSetter>().currentPart, true);
    }

    public void SwapMaterial(int part)
    {
        if (matCount < customMats.Length)
        {
            PartMaster.instance.SetMaterial(part, customMats[matCount]);
            matCount++;
        }
        else
        {
            matCount = 0;
            PartMaster.instance.SetMaterial(part, defaultMat);
        }
    }

    private IEnumerator DestroyNormal()
    {
        yield return new WaitForSeconds(0.1f);
        if (defaultMat == null)
            defaultMat = FindObjectOfType<BikeLoadOut>().GetPartMat(0);
        defaultMat.SetFloat("_DetailNormalMapScale", 0f);
        defaultMat.color = Color.black;
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