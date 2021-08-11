﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TextureManager : MonoBehaviour
{
    public static TextureManager instance;

    public InputField urlInput;
    public InputField urlNorm;
    public InputField urlMet;
    public Slider shinySlide;
    public Slider metalSlide;

    public InputField texTileX;
    public InputField texTileY;
    public InputField normTileX;
    public InputField normTileY;
    public InputField metTileX;
    public InputField metTileY;

    public Dictionary<int, string> albedoList = new Dictionary<int, string>();
    public Dictionary<int, string> normalList = new Dictionary<int, string>();
    public Dictionary<int, string> metallicList = new Dictionary<int, string>();

    public Texture OriginalFrameTex;
    public Texture OriginalBarsTex;
    public Texture OriginalForksTex;
    public Texture OriginalSeatTex;
    public Texture OriginalTire1Tex;
    public Texture OriginalTire2Tex;
    public Texture OriginalTire1WallTex;
    public Texture OriginalTire2WallTex;
    public Texture OriginalRimTex;
    public Texture OriginalHubTex;

    void Awake()
    {
        instance = this;
        InitDictionaries();
    }

    void Start()
    {
        StoreOriginalTextures();
    }

    void InitDictionaries()
    {

        albedoList.Add(-1, "");
        albedoList.Add(-2, "");
        normalList.Add(-1, "");
        normalList.Add(-2, "");
        metallicList.Add(-1, "");
        metallicList.Add(-2, "");

        foreach (KeyValuePair<int, GameObject> pair in PartMaster.instance.partList)
        {
            if (PartMaster.instance.HasRenderer(pair.Key))
            {
                albedoList.Add(pair.Key, "");
                normalList.Add(pair.Key, "");
                metallicList.Add(pair.Key, "");
            }
        }
    }

    public void SetTextureTilingX()
    {
        int val;
        bool success = Int32.TryParse(texTileX.text, out val);
        if (success)
            TexTilingHelper("_MainTex", true, val);
    }

    public void SetTextureTilingY()
    {
        int val;
        bool success = Int32.TryParse(texTileX.text, out val);
        if (success)
            TexTilingHelper("_MainTex", false, val);
    }

    public void SetNormalTilingX()
    {
        int val;
        bool success = Int32.TryParse(texTileX.text, out val);
        if (success)
            TexTilingHelper("_BumpMap", true, val);
    }

    public void SetNormalTilingY()
    {
        int val;
        bool success = Int32.TryParse(texTileX.text, out val);
        if (success)
            TexTilingHelper("_BumpMap", false, val);
    }

    public void SetMetalTilingX()
    {
        int val;
        bool success = Int32.TryParse(texTileX.text, out val);
        if (success)
            TexTilingHelper("_MetallicGlossMap", true, val);
    }

    public void SetMetalTilingY()
    {
        int val;
        bool success = Int32.TryParse(texTileX.text, out val);
        if (success)
            TexTilingHelper("_MetallicGlossMap", false, val);
    }

    public void ResetTileInputs()
    {
        texTileX.text = "";
        texTileY.text = "";
        normTileX.text = "";
        normTileY.text = "";
        metTileX.text = "";
        metTileY.text = "";
    }

    public void TexTilingHelper(string propName, bool isX, int val)
    {
        List<int> activeList = ColourSetter.instance.GetActivePartList();
        foreach (int selectedPart in activeList)
        {
            Material[] mats;
            mats = PartMaster.instance.GetMaterials(selectedPart);
            foreach (Material m in mats)
            {
                if(isX)
                    m.SetTextureScale(propName, new Vector2(val, m.GetTextureScale(propName).y));
                else
                    m.SetTextureScale(propName, new Vector2(m.GetTextureScale(propName).x, val));
            }
        }
    }

    public void SetMetallicness()
    {
        List<int> activeList = ColourSetter.instance.GetActivePartList();
        foreach (int selectedPart in activeList)
        {
            if (selectedPart == -1)
            {
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[1].SetFloat("_Metallic", metalSlide.value);
            }
            else if (selectedPart == -2)
            {
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[1].SetFloat("_Metallic", metalSlide.value);
            }
            else if (selectedPart == -3)
            {
                BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[2].SetFloat("_Metallic", metalSlide.value);
                BrakesManager.instance.GetBarBrakes().GetComponent<Renderer>().materials[1].SetFloat("_Metallic", metalSlide.value);
            }
            else if (selectedPart == -4)
            {
                BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[3].SetFloat("_Metallic", metalSlide.value);
                BrakesManager.instance.GetBarBrakes().GetComponent<Renderer>().materials[2].SetFloat("_Metallic", metalSlide.value);
            }
            else
            {
                Material[] mats;
                mats = PartMaster.instance.GetMaterials(selectedPart);
                foreach (Material m in mats)
                {
                    PartMaster.instance.GetMaterial(selectedPart).SetFloat("_Metallic", metalSlide.value);
                }
            }
        }
    }

    public void SetShininess()
    {
        List<int> activeList = ColourSetter.instance.GetActivePartList();
        foreach (int selectedPart in activeList)
        {

            if (selectedPart == -1)
            {

                if (PartMaster.instance.GetMaterial(selectedPart).GetTexture("_MetallicGlossMap") == null)
                    PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[1].SetFloat("_Glossiness", shinySlide.value);
                else
                    PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[1].SetFloat("_GlossMapScale", shinySlide.value);

            }
            else if (selectedPart == -2)
            {
                if (PartMaster.instance.GetMaterial(selectedPart).GetTexture("_MetallicGlossMap") == null)
                    PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[1].SetFloat("_Glossiness", shinySlide.value);
                else
                    PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[1].SetFloat("_GlossMapScale", shinySlide.value);
            }
            else if (selectedPart == -3)
            {
                if (BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[2].GetTexture("_MetallicGlossMap") == null)
                {
                    BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[2].SetFloat("_Glossiness", shinySlide.value);
                    BrakesManager.instance.GetBarBrakes().GetComponent<Renderer>().materials[1].SetFloat("_Glossiness", shinySlide.value);
                }
                else
                {
                    BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[2].SetFloat("_GlossMapScale", shinySlide.value);
                    BrakesManager.instance.GetBarBrakes().GetComponent<Renderer>().materials[1].SetFloat("_GlossMapScale", shinySlide.value);
                }
            }
            else if (selectedPart == -4)
            {
                if (BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[3].GetTexture("_MetallicGlossMap") == null)
                {
                    BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[3].SetFloat("_Glossiness", shinySlide.value);
                    BrakesManager.instance.GetBarBrakes().GetComponent<Renderer>().materials[2].SetFloat("_Glossiness", shinySlide.value);
                }
                else
                {
                    BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[3].SetFloat("_GlossMapScale", shinySlide.value);
                    BrakesManager.instance.GetBarBrakes().GetComponent<Renderer>().materials[2].SetFloat("_GlossMapScale", shinySlide.value);
                }
            }
            else
            {
                if (PartMaster.instance.GetMaterial(selectedPart).GetTexture("_MetallicGlossMap") == null)
                    PartMaster.instance.GetMaterial(selectedPart).SetFloat("_Glossiness", shinySlide.value);
                else
                    PartMaster.instance.GetMaterial(selectedPart).SetFloat("_GlossMapScale", shinySlide.value);
            }

        }
    }

    public void ClearTextures(int key)
    {
        if (!albedoList.ContainsKey(key))
            return;
        albedoList[key] = "";
        normalList[key] = "";
        metallicList[key] = "";
    }

    public void SetTexture()
    {
        StartCoroutine(SetTextureEnum());
        Resources.UnloadUnusedAssets();
    }

    public void SetNormal()
    {
        StartCoroutine(SetNormalEnum());
        Resources.UnloadUnusedAssets();
    }

    public void SetMetallic()
    {
        StartCoroutine(SetMetallicEnum());
        Resources.UnloadUnusedAssets();
    }

    public void SetTexture(int partNum, string url)
    {
        if (url == "" || url == null)
            return;
        StartCoroutine(SetTextureEnum(partNum, url));
        Resources.UnloadUnusedAssets();
    }

    public void SetNormal(int partNum, string url)
    {
        if (url == "" || url == null)
            return;
        StartCoroutine(SetNormalEnum(partNum, url));
        Resources.UnloadUnusedAssets();
    }

    public void SetMetallic(int partNum, string url)
    {
        if (url == "" || url == null)
            return;
        StartCoroutine(SetMetallicEnum(partNum, url));
        Resources.UnloadUnusedAssets();
    }

    public void RemoveTexture()
    {
        List<int> activeList = ColourSetter.instance.GetActivePartList();
        foreach (int key in activeList)
            StartCoroutine(SetTextureBlank(key));
        Resources.UnloadUnusedAssets();
    }

    public void RemoveNormal()
    {
        List<int> activeList = ColourSetter.instance.GetActivePartList();
        foreach (int key in activeList)
            StartCoroutine(SetNormalBlank(key));
        Resources.UnloadUnusedAssets();
    }

    public void RemoveMetallic()
    {
        List<int> activeList = ColourSetter.instance.GetActivePartList();
        foreach (int key in activeList)
            StartCoroutine(SetMetallicBlank(key));
        Resources.UnloadUnusedAssets();
    }

    public void RemoveTexture(int partNum)
    {
        StartCoroutine(SetTextureBlank(partNum));
        Resources.UnloadUnusedAssets();
    }

    public void RemoveNormal(int partNum)
    {
        StartCoroutine(SetNormalBlank(partNum));
        Resources.UnloadUnusedAssets();
    }

    public void RemoveMetallic(int partNum)
    {
        StartCoroutine(SetMetallicBlank(partNum));
        Resources.UnloadUnusedAssets();
    }

    public void StoreOriginalTextures()
    {
        OriginalFrameTex = PartMaster.instance.GetMaterial(PartMaster.instance.frame).GetTexture("_MainTex");
        OriginalBarsTex = PartMaster.instance.GetMaterial(PartMaster.instance.bars).GetTexture("_MainTex");
        OriginalForksTex = PartMaster.instance.GetMaterial(PartMaster.instance.forks).GetTexture("_MainTex");
        OriginalSeatTex = PartMaster.instance.GetMaterial(PartMaster.instance.seat).GetTexture("_MainTex");
        OriginalRimTex = PartMaster.instance.GetMaterial(PartMaster.instance.frontRim).GetTexture("_MainTex");
        OriginalHubTex = PartMaster.instance.GetMaterial(PartMaster.instance.frontHub).GetTexture("_MainTex");


        OriginalTire1Tex = PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[0].GetTexture("_MainTex");
        OriginalTire2Tex = PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[0].GetTexture("_MainTex");

        OriginalTire1WallTex = PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[1].GetTexture("_MainTex");
        OriginalTire2WallTex = PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[1].GetTexture("_MainTex");
    }

    public void SetOriginalTextures()
    {

        PartMaster.instance.GetMaterial(PartMaster.instance.frame).SetTexture("_MainTexture", OriginalFrameTex);
        PartMaster.instance.GetMaterial(PartMaster.instance.bars).SetTexture("_MainTexture", OriginalBarsTex);
        PartMaster.instance.GetMaterial(PartMaster.instance.forks).SetTexture("_MainTexture", OriginalForksTex);
        PartMaster.instance.GetMaterial(PartMaster.instance.seat).SetTexture("_MainTexture", OriginalSeatTex);

        PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[0].SetTexture("_MainTexture", OriginalTire1Tex);
        PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[0].SetTexture("_MainTexture", OriginalTire2Tex);
        PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[1].SetTexture("_MainTexture", OriginalTire1WallTex);
        PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[1].SetTexture("_MainTexture", OriginalTire2WallTex);

        PartMaster.instance.GetMaterial(PartMaster.instance.frontRim).SetTexture("_MainTexture", OriginalRimTex);
        PartMaster.instance.GetMaterial(PartMaster.instance.rearRim).SetTexture("_MainTexture", OriginalRimTex);

        PartMaster.instance.GetMaterial(PartMaster.instance.frontHub).SetTexture("_MainTexture", OriginalHubTex);
        PartMaster.instance.GetMaterial(PartMaster.instance.rearHub).SetTexture("_MainTexture", OriginalHubTex);
        ClearTextures(0);
        ClearTextures(1);
        ClearTextures(36);
        ClearTextures(2);
        ClearTextures(10);
        ClearTextures(11);
        ClearTextures(6);
        ClearTextures(7);
        ClearTextures(8);
        ClearTextures(9);
        ClearTextures(-1);
        ClearTextures(-2);
        ClearTextures(16);

        Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// Helper method for setting all types of textures
    /// </summary>
    /// <param name="tex"></param>
    /// <param name="list"></param>
    /// <param name="inputField"></param>
    /// <param name="partNum"></param>
    /// <param name="texType"></param>
    /// <param name="url"></param>
    /// <param name="matIndex"></param>
    /// <param name="enableKeyword"></param>
    void TexHelper(Texture2D tex, Dictionary<int, string> list, InputField inputField, int partNum, string texType, string enableKeyword = "", string url = "")
    {
        bool flag = (!enableKeyword.Equals("") && tex != null);
        bool flag2 = (!enableKeyword.Equals("") && tex == null);
        if (partNum == -1) //Front Tire Wall
        {
            Material[] mats = PartMaster.instance.GetMaterials(PartMaster.instance.frontTire);
            if (flag)
                mats[1].EnableKeyword(enableKeyword);
            if (flag2)
                mats[1].DisableKeyword(enableKeyword);
            mats[1].SetTexture(texType, tex);
            PartMaster.instance.SetMaterials(PartMaster.instance.frontTire, mats);
        }
        else if (partNum == -2) // Rear Tire Wall
        {
            Material[] mats2 = PartMaster.instance.GetMaterials(PartMaster.instance.rearTire);
            if (flag)
                mats2[1].EnableKeyword(enableKeyword);
            if (flag2)
                mats2[1].DisableKeyword(enableKeyword);
            mats2[1].SetTexture(texType, tex);
            PartMaster.instance.SetMaterials(PartMaster.instance.rearTire, mats2);
        }
        else if (partNum == -3)
        {
            if (BrakesManager.instance.IsEnabled())
            {
                if (flag)
                {
                    BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[2].EnableKeyword(enableKeyword);
                    BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[1].EnableKeyword(enableKeyword);
                }
                if (flag2)
                {
                    BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[2].DisableKeyword(enableKeyword);
                    BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[1].DisableKeyword(enableKeyword);
                }

                BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[2].SetTexture(texType, tex);
                BrakesManager.instance.GetBarBrakes().GetComponent<Renderer>().materials[1].SetTexture(texType, tex);
            }
        }
        else if (partNum == -4)
        {
            if (BrakesManager.instance.IsEnabled())
            {
                if (flag)
                {
                    BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[3].EnableKeyword(enableKeyword);
                    BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[2].EnableKeyword(enableKeyword);
                }
                if (flag2)
                {
                    BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[3].DisableKeyword(enableKeyword);
                    BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[2].DisableKeyword(enableKeyword);
                }
                BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[3].SetTexture(texType, tex);
                BrakesManager.instance.GetBarBrakes().GetComponent<Renderer>().materials[2].SetTexture(texType, tex);
            }
        }
        else // Everything else
        {
            if (flag)
                PartMaster.instance.GetMaterials(partNum)[0].EnableKeyword(enableKeyword);
            if (flag2)
                PartMaster.instance.GetMaterials(partNum)[0].DisableKeyword(enableKeyword);
            PartMaster.instance.GetMaterials(partNum)[0].SetTexture(texType, tex);
        }
        if (inputField == null)
            list[partNum] = url;
        else
        {
            list[partNum] = inputField.text;
        }
    }


    IEnumerator SetTextureEnum()
    {
        WWW www = new WWW(urlInput.text);
        yield return www;
        List<int> activeList = ColourSetter.instance.GetActivePartList();
        foreach (int key in activeList)
        {
            TexHelper(www.texture, albedoList, urlInput, key, "_MainTex");
        }
        urlInput.text = "";
    }

    IEnumerator SetNormalEnum()
    {
        WWW www = new WWW(urlNorm.text);
        yield return www;
        Texture2D normalTexture = ConvertToNormalMap(www.texture);

        List<int> activeList = ColourSetter.instance.GetActivePartList();
        foreach (int key in activeList)
        {
            TexHelper(normalTexture, normalList, urlNorm, key, "_BumpMap", "_NORMALMAP");
        }
        urlNorm.text = "";
    }

    IEnumerator SetNormalEnum(int partNum, string url)
    {
        WWW www = new WWW(url);
        yield return www;
        Texture2D normalTexture = ConvertToNormalMap(www.texture);

        TexHelper(normalTexture, normalList, null, partNum, "_BumpMap", "_NORMALMAP", url);
    }

    private Texture2D ConvertToNormalMap(Texture2D texture)
    {
        Texture2D normalTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, true, true);
        normalTexture.filterMode = FilterMode.Trilinear;
        Color[] pixels = texture.GetPixels(0, 0, texture.width, texture.height);
        normalTexture.SetPixels(pixels);
        normalTexture.Apply(true, false);

        //SaveTextureAsPNG(normalTexture, Application.dataPath + "/TestNormals/TempNormal.png");

        return normalTexture;
    }

    public static void SaveTextureAsPNG(Texture2D _texture, string _fullPath)
    {
        byte[] _bytes = _texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(_fullPath, _bytes);
        Debug.Log(_bytes.Length / 1024 + "Kb was saved as: " + _fullPath);
    }

    IEnumerator SetMetallicEnum()
    {
        WWW www = new WWW(urlMet.text);
        yield return www;
        List<int> activeList = ColourSetter.instance.GetActivePartList();
        foreach (int key in activeList)
        {
            TexHelper(www.texture, metallicList, urlMet, key, "_MetallicGlossMap", "_METALLICGLOSSMAP");
        }
        urlMet.text = "";
    }

    IEnumerator SetMetallicEnum(int partNum, string url)
    {
        WWW www = new WWW(url);
        yield return www;
        TexHelper(www.texture, metallicList, null, partNum, "_MetallicGlossMap", "_METALLICGLOSSMAP", url);
    }

    IEnumerator SetTextureEnum(int partNum, string url)
    {
        WWW www = new WWW(url);
        yield return www;
        TexHelper(www.texture, albedoList, null, partNum, "_MainTex", "", url);
    }

    IEnumerator SetTextureBlank(int partNum)
    {
        yield return new WaitForEndOfFrame();
        TexHelper(null, albedoList, null, partNum, "_MainTex");
    }



    IEnumerator SetNormalBlank(int partNum)
    {
        yield return new WaitForEndOfFrame();
        TexHelper(null, normalList, null, partNum, "_BumpMap", "_NORMALMAP");
    }

    IEnumerator SetMetallicBlank(int partNum)
    {
        yield return new WaitForEndOfFrame();
        TexHelper(null, metallicList, null, partNum, "_MetallicGlossMap", "_METALLICGLOSSMAP");
    }

}
