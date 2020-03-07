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
    public Material[] customMats;
    private int matCount = 0;

    [HideInInspector]
    public Material defaultMat;

    public Material chainMatFix;

    //Trying to remove the detail normal map.

    private void Awake()
    {
        StartCoroutine(DestroyNormal());
    }

    public void Start()
    {
        GameObject.Find("Chain Mesh").GetComponent<Renderer>().material = chainMatFix;
    }

    public void SetMaterial(Material mat)
    {
        if (FindObjectOfType<ColourSetter>().getSeatPostBool())
        {
            GameObject.Find("Seat Post").GetComponent<Renderer>().material = mat;
        }
        else {
            FindObjectOfType<BikeLoadOut>().SetPartMaterial(mat, FindObjectOfType<ColourSetter>().currentPart, true);
        }
        
    }

    public void SetDefaultMat()
    {
        FindObjectOfType<BikeLoadOut>().SetPartMaterial(defaultMat, FindObjectOfType<ColourSetter>().currentPart, true);
    }

    public void SwapMaterial(String name)
    {
        if (matCount < customMats.Length)
        {
            GameObject.Find(name + " Mesh").GetComponent<Renderer>().material = customMats[matCount];
            
            matCount++;
        }
        else
        {
            matCount = 0;
            GameObject.Find(name + " Mesh").GetComponent<Renderer>().material = defaultMat;
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
            try
            {
                Material m = FindObjectOfType<BikeLoadOut>().GetPartMat(i);
                if (m.name.ToLower().Contains("a_glossy"))
                {
                    FindObjectOfType<BikeLoadOut>().SetPartMaterial(defaultMat, i, true);
                }
                
            }
            catch (Exception e)
            {
                File.WriteAllText(Application.dataPath + "//errorLog.txt", e.Message + " " + e.StackTrace);
            }
        }
        yield break;
    }

}