using System;
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
    public Text selectedPartText;

    Parts selectedPart;

    public string frameURL ="";
    public string barsURL ="";
    public string seatURL = "";
    public string forksURL = "";
    public string tireURL = "";
    public string tireWallURL = "";
    public string rimsURL = "";
    public string hubsURL = "";

    public string frameURLN = "";
    public string barsURLN = "";
    public string seatURLN = "";
    public string forksURLN = "";
    public string tireURLN = "";
    public string tireWallURLN = "";
    public string rimsURLN = "";
    public string hubsURLN = "";

    public string frameURLM = "";
    public string barsURLM = "";
    public string seatURLM = "";
    public string forksURLM = "";
    public string tireURLM = "";
    public string tireWallURLM = "";
    public string rimsURLM = "";
    public string hubsURLM = "";

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

    public enum Parts
    {
        Frame,
        Bars,
        Seat,
        Forks,
        Tires,
        Tire_Wall,
        Rims,
        Hubs
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StoreOriginalTextures();
    }

    public void SetUrlsEmpty()
    {
        frameURL = "";
        barsURL = "";
        forksURL = "";
        seatURL = "";
        tireURL = "";
        tireWallURL = "";
        rimsURL = "";
        hubsURL = "";
    }

    public void Update()
    {
        if (this.shinySlide.IsActive())
        {
            switch (selectedPart)
            {
                case Parts.Bars:
                    PartMaster.instance.GetMaterial(PartMaster.instance.bars).SetFloat("_Glossiness", shinySlide.value);
                    break;
                case Parts.Forks:
                    PartMaster.instance.GetMaterial(PartMaster.instance.forks).SetFloat("_Glossiness", shinySlide.value);
                    break;
                case Parts.Frame:
                    PartMaster.instance.GetMaterial(PartMaster.instance.frame).SetFloat("_Glossiness", shinySlide.value);
                    break;
                case Parts.Seat:
                    PartMaster.instance.GetMaterial(PartMaster.instance.seat).SetFloat("_Glossiness", shinySlide.value);
                    break;
                case Parts.Tires:
                    PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[0].SetFloat("_Glossiness", shinySlide.value);
                    PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[0].SetFloat("_Glossiness", shinySlide.value);
                    break;
                case Parts.Tire_Wall:
                    PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[1].SetFloat("_Glossiness", shinySlide.value);
                    PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[1].SetFloat("_Glossiness", shinySlide.value);
                    break;
                case Parts.Rims:
                    PartMaster.instance.GetMaterial(PartMaster.instance.frontRim).SetFloat("_Glossiness", shinySlide.value);
                    PartMaster.instance.GetMaterial(PartMaster.instance.rearRim).SetFloat("_Glossiness", shinySlide.value);
                    break;
                case Parts.Hubs:
                    PartMaster.instance.GetMaterial(PartMaster.instance.frontHub).SetFloat("_Glossiness", shinySlide.value);
                    PartMaster.instance.GetMaterial(PartMaster.instance.rearHub).SetFloat("_Glossiness", shinySlide.value);
                    break;
                default:
                    break;
            }   
        }
    }

    public void SetSelectedPart(int bikePart)
    {
        selectedPart = (Parts)bikePart;
        Debug.Log("Selected Part " + selectedPart);
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
        StartCoroutine(SetTextureBlank((int)selectedPart));
        Resources.UnloadUnusedAssets();
    }

    public void RemoveNormal()
    {
        StartCoroutine(SetNormalBlank((int)selectedPart));
        Resources.UnloadUnusedAssets();
    }

    public void RemoveMetallic()
    {
        StartCoroutine(SetMetallicBlank((int) selectedPart));
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

        Resources.UnloadUnusedAssets();
    }


    IEnumerator SetTextureEnum()
    {
        WWW www = new WWW(urlInput.text);
        while (!www.isDone)
        {
            yield return new WaitForEndOfFrame();
        }

        switch (selectedPart)
        {
            case Parts.Bars:
                PartMaster.instance.GetMaterial(PartMaster.instance.bars).mainTexture = www.texture;
                barsURL = urlInput.text;
                break;
            case Parts.Forks:
                PartMaster.instance.GetMaterial(PartMaster.instance.forks).mainTexture = www.texture;
                forksURL = urlInput.text;
                break;
            case Parts.Frame:
                PartMaster.instance.GetMaterial(PartMaster.instance.frame).mainTexture = www.texture;
                frameURL = urlInput.text;
                break;
            case Parts.Seat:
                PartMaster.instance.GetMaterial(PartMaster.instance.seat).mainTexture = www.texture;
                seatURL = urlInput.text;
                break;
            case Parts.Tires:
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[0].mainTexture = www.texture;
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[0].mainTexture = www.texture;
                tireURL = urlInput.text;
                break;
            case Parts.Tire_Wall:
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[1].mainTexture = www.texture;
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[1].mainTexture = www.texture;
                tireWallURL = urlInput.text;
                break;
            case Parts.Rims:
                PartMaster.instance.GetMaterial(PartMaster.instance.frontRim).mainTexture = www.texture;
                PartMaster.instance.GetMaterial(PartMaster.instance.rearRim).mainTexture = www.texture;
                rimsURL = urlInput.text;
                break;
            case Parts.Hubs:
                PartMaster.instance.GetMaterial(PartMaster.instance.frontHub).mainTexture = www.texture;
                PartMaster.instance.GetMaterial(PartMaster.instance.rearHub).mainTexture = www.texture;
                hubsURL = urlInput.text;
                break;
        }

        urlInput.text = "";
    }

    IEnumerator SetNormalEnum()
    {
        WWW www = new WWW(urlNorm.text);
        while (!www.isDone)
        {
            yield return new WaitForEndOfFrame();
        }


        Texture2D normalTexture = new Texture2D(www.texture.width, www.texture.height, TextureFormat.ARGB32, true, true);
        Color32[] colours = www.texture.GetPixels32();
        for (int i = 0; i < colours.Length; i++)
        {
            Color32 c = colours[i];
            c.a = c.r;
            c.r = c.b = c.g;
            colours[i] = c;
        }
        normalTexture.SetPixels32(colours);
        normalTexture.Apply(true, false);

        switch (selectedPart)
        {
            case Parts.Bars:
                PartMaster.instance.GetMaterial(PartMaster.instance.bars).EnableKeyword("_NORMALMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.bars).SetTexture("_BumpMap", normalTexture);
                barsURLN = urlNorm.text;
                break;
            case Parts.Forks:
                PartMaster.instance.GetMaterial(PartMaster.instance.forks).EnableKeyword("_NORMALMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.forks).SetTexture("_BumpMap", normalTexture);
                forksURLN = urlNorm.text;
                break;
            case Parts.Frame:
                PartMaster.instance.GetMaterial(PartMaster.instance.frame).EnableKeyword("_NORMALMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.frame).SetTexture("_BumpMap", normalTexture);
                StartCoroutine(SetMetallicEnum());
                frameURLN = urlNorm.text;
                break;
            case Parts.Seat:
                PartMaster.instance.GetMaterial(PartMaster.instance.seat).EnableKeyword("_NORMALMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.seat).SetTexture("_BumpMap", normalTexture);
                seatURLN = urlNorm.text;
                break;
            case Parts.Tires:
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[0].EnableKeyword("_NORMALMAP");
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[0].SetTexture("_BumpMap", normalTexture);
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[0].EnableKeyword("_NORMALMAP");
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[0].SetTexture("_BumpMap", normalTexture);
                tireURLN = urlNorm.text;
                break;
            case Parts.Tire_Wall:
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[1].EnableKeyword("_NORMALMAP");
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[1].SetTexture("_BumpMap", normalTexture);
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[1].EnableKeyword("_NORMALMAP");
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[1].SetTexture("_BumpMap", normalTexture);
                tireWallURLN = urlNorm.text;
                break;
            case Parts.Rims:
                PartMaster.instance.GetMaterial(PartMaster.instance.frontRim).EnableKeyword("_NORMALMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.frontRim).SetTexture("_BumpMap", normalTexture);
                PartMaster.instance.GetMaterial(PartMaster.instance.rearRim).EnableKeyword("_NORMALMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.rearRim).SetTexture("_BumpMap", normalTexture);
                rimsURLN = urlNorm.text;
                break;
            case Parts.Hubs:
                PartMaster.instance.GetMaterial(PartMaster.instance.frontHub).EnableKeyword("_NORMALMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.frontHub).SetTexture("_BumpMap", normalTexture);
                PartMaster.instance.GetMaterial(PartMaster.instance.rearHub).EnableKeyword("_NORMALMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.rearHub).SetTexture("_BumpMap", normalTexture);
                hubsURLN = urlNorm.text;
                break;
        }

        urlNorm.text = "";
    }

    IEnumerator SetNormalEnum(int partNum, string url)
    {
        WWW www = new WWW(url);
        while (!www.isDone)
        {
            yield return new WaitForEndOfFrame();
        }


        Texture2D normalTexture = new Texture2D(www.texture.width, www.texture.height, TextureFormat.ARGB32, true, true);
        Color32[] colours = www.texture.GetPixels32();
        for (int i = 0; i < colours.Length; i++)
        {
            Color32 c = colours[i];
            c.a = c.r;
            c.r = c.b = c.g;
            colours[i] = c;
        }
        normalTexture.SetPixels32(colours);
        normalTexture.Apply(true, false);

        switch ((Parts) partNum)
        {
            case Parts.Bars:
                PartMaster.instance.GetMaterial(PartMaster.instance.bars).EnableKeyword("_NORMALMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.bars).SetTexture("_BumpMap", normalTexture);
                barsURLN = url;
                break;
            case Parts.Forks:
                PartMaster.instance.GetMaterial(PartMaster.instance.forks).EnableKeyword("_NORMALMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.forks).SetTexture("_BumpMap", normalTexture);
                forksURLN = url;
                break;
            case Parts.Frame:
                PartMaster.instance.GetMaterial(PartMaster.instance.frame).EnableKeyword("_NORMALMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.frame).SetTexture("_BumpMap", normalTexture);
                StartCoroutine(SetMetallicEnum());
                frameURLN = url;
                break;
            case Parts.Seat:
                PartMaster.instance.GetMaterial(PartMaster.instance.seat).EnableKeyword("_NORMALMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.seat).SetTexture("_BumpMap", normalTexture);
                seatURLN = url;
                break;
            case Parts.Tires:
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[0].EnableKeyword("_NORMALMAP");
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[0].SetTexture("_BumpMap", normalTexture);
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[0].EnableKeyword("_NORMALMAP");
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[0].SetTexture("_BumpMap", normalTexture);
                tireURLN = urlNorm.text;
                break;
            case Parts.Tire_Wall:
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[1].EnableKeyword("_NORMALMAP");
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[1].SetTexture("_BumpMap", normalTexture);
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[1].EnableKeyword("_NORMALMAP");
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[1].SetTexture("_BumpMap", normalTexture);
                tireWallURLN = urlNorm.text;
                break;
            case Parts.Rims:
                PartMaster.instance.GetMaterial(PartMaster.instance.frontRim).EnableKeyword("_NORMALMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.frontRim).SetTexture("_BumpMap", normalTexture);
                PartMaster.instance.GetMaterial(PartMaster.instance.rearRim).EnableKeyword("_NORMALMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.rearRim).SetTexture("_BumpMap", normalTexture);
                rimsURLN = urlNorm.text;
                break;
            case Parts.Hubs:
                PartMaster.instance.GetMaterial(PartMaster.instance.frontHub).EnableKeyword("_NORMALMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.frontHub).SetTexture("_BumpMap", normalTexture);
                PartMaster.instance.GetMaterial(PartMaster.instance.rearHub).EnableKeyword("_NORMALMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.rearHub).SetTexture("_BumpMap", normalTexture);
                hubsURLN = urlNorm.text;
                break;
        }
    }

    IEnumerator SetMetallicEnum()
    {
        WWW www = new WWW(urlMet.text);
        while (!www.isDone)
        {
            yield return new WaitForEndOfFrame();
        }

        switch (selectedPart)
        {
            case Parts.Bars:
                PartMaster.instance.GetMaterial(PartMaster.instance.bars).EnableKeyword("_METALLICGLOSSMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.bars).SetTexture("_MetallicGlossMap", www.texture);
                barsURLM = urlMet.text;
                break;
            case Parts.Forks:
                PartMaster.instance.GetMaterial(PartMaster.instance.forks).EnableKeyword("_METALLICGLOSSMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.forks).SetTexture("_MetallicGlossMap", www.texture);
                forksURLM = urlMet.text;
                break;
            case Parts.Frame:
                PartMaster.instance.GetMaterial(PartMaster.instance.frame).EnableKeyword("_METALLICGLOSSMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.frame).SetTexture("_MetallicGlossMap", www.texture);
                frameURLM = urlMet.text;
                break;
            case Parts.Seat:
                PartMaster.instance.GetMaterial(PartMaster.instance.seat).EnableKeyword("_METALLICGLOSSMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.seat).SetTexture("_MetallicGlossMap", www.texture);
                seatURLM = urlMet.text;
                break;
            case Parts.Tires:
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[0].EnableKeyword("_METALLICGLOSSMAP");
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[0].SetTexture("_MetallicGlossMap", www.texture);
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[0].EnableKeyword("_METALLICGLOSSMAP");
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[0].SetTexture("_MetallicGlossMap", www.texture);
                tireURLM = urlMet.text;
                break;
            case Parts.Tire_Wall:
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[1].EnableKeyword("_METALLICGLOSSMAP");
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[1].SetTexture("_MetallicGlossMap", www.texture);
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[1].EnableKeyword("_METALLICGLOSSMAP");
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[1].SetTexture("_MetallicGlossMap", www.texture);
                tireWallURLM = urlMet.text;
                break;
            case Parts.Rims:
                PartMaster.instance.GetMaterial(PartMaster.instance.frontRim).EnableKeyword("_METALLICGLOSSMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.frontRim).SetTexture("_MetallicGlossMap", www.texture);
                PartMaster.instance.GetMaterial(PartMaster.instance.rearRim).EnableKeyword("_METALLICGLOSSMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.rearRim).SetTexture("_MetallicGlossMap", www.texture);
                rimsURLM = urlMet.text;
                break;
            case Parts.Hubs:
                PartMaster.instance.GetMaterial(PartMaster.instance.frontHub).EnableKeyword("_METALLICGLOSSMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.frontHub).SetTexture("_MetallicGlossMap", www.texture);
                PartMaster.instance.GetMaterial(PartMaster.instance.rearHub).EnableKeyword("_METALLICGLOSSMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.rearHub).SetTexture("_MetallicGlossMap", www.texture);
                hubsURLM = urlMet.text;
                break;
        }
        urlMet.text = "";
    }

    IEnumerator SetMetallicEnum(int partNum, string url)
    {
        WWW www = new WWW(url);
        while (!www.isDone)
        {
            yield return new WaitForEndOfFrame();
        }

        switch ((Parts) partNum)
        {
            case Parts.Bars:
                PartMaster.instance.GetMaterial(PartMaster.instance.bars).EnableKeyword("_METALLICGLOSSMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.bars).SetTexture("_MetallicGlossMap", www.texture);
                barsURLM = url;
                break;
            case Parts.Forks:
                PartMaster.instance.GetMaterial(PartMaster.instance.forks).EnableKeyword("_METALLICGLOSSMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.forks).SetTexture("_MetallicGlossMap", www.texture);
                forksURLM = url;
                break;
            case Parts.Frame:
                PartMaster.instance.GetMaterial(PartMaster.instance.frame).EnableKeyword("_METALLICGLOSSMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.frame).SetTexture("_MetallicGlossMap", www.texture);
                frameURLM = url;
                break;
            case Parts.Seat:
                PartMaster.instance.GetMaterial(PartMaster.instance.seat).EnableKeyword("_METALLICGLOSSMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.seat).SetTexture("_MetallicGlossMap", www.texture);
                seatURLM = url;
                break;
            case Parts.Tires:
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[0].EnableKeyword("_METALLICGLOSSMAP");
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[0].SetTexture("_MetallicGlossMap", www.texture);
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[0].EnableKeyword("_METALLICGLOSSMAP");
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[0].SetTexture("_MetallicGlossMap", www.texture);
                tireURLM = urlMet.text;
                break;
            case Parts.Tire_Wall:
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[1].EnableKeyword("_METALLICGLOSSMAP");
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[1].SetTexture("_MetallicGlossMap", www.texture);
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[1].EnableKeyword("_METALLICGLOSSMAP");
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[1].SetTexture("_MetallicGlossMap", www.texture);
                tireWallURLM = urlMet.text;
                break;
            case Parts.Rims:
                PartMaster.instance.GetMaterial(PartMaster.instance.frontRim).EnableKeyword("_METALLICGLOSSMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.frontRim).SetTexture("_MetallicGlossMap", www.texture);
                PartMaster.instance.GetMaterial(PartMaster.instance.rearRim).EnableKeyword("_METALLICGLOSSMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.rearRim).SetTexture("_MetallicGlossMap", www.texture);
                rimsURLM = urlMet.text;
                break;
            case Parts.Hubs:
                PartMaster.instance.GetMaterial(PartMaster.instance.frontHub).EnableKeyword("_METALLICGLOSSMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.frontHub).SetTexture("_MetallicGlossMap", www.texture);
                PartMaster.instance.GetMaterial(PartMaster.instance.rearHub).EnableKeyword("_METALLICGLOSSMAP");
                PartMaster.instance.GetMaterial(PartMaster.instance.rearHub).SetTexture("_MetallicGlossMap", www.texture);
                hubsURLM = urlMet.text;
                break;
        }
    }

    IEnumerator SetTextureEnum(int partNum, string url)
    {
        WWW www = new WWW(url);
        while (!www.isDone)
        {
            yield return new WaitForEndOfFrame();
        }

        switch ((Parts)partNum)
        {
            case Parts.Bars:
                PartMaster.instance.GetMaterial(PartMaster.instance.bars).mainTexture = www.texture;
                barsURL = url;
                break;
            case Parts.Forks:
                PartMaster.instance.GetMaterial(PartMaster.instance.forks).mainTexture = www.texture;
                forksURL = url;
                break;
            case Parts.Frame:
                PartMaster.instance.GetMaterial(PartMaster.instance.frame).mainTexture = www.texture;
                frameURL = url;
                break;
            case Parts.Seat:
                PartMaster.instance.GetMaterial(PartMaster.instance.seat).mainTexture = www.texture;
                seatURL = url;
                break;
            case Parts.Tires:
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[0].mainTexture = www.texture;
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[0].mainTexture = www.texture;
                tireURL = url;
                break;
            case Parts.Tire_Wall:
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[1].mainTexture = www.texture;
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[1].mainTexture = www.texture;
                tireWallURL = url;
                break;
            case Parts.Rims:
                PartMaster.instance.GetMaterial(PartMaster.instance.frontRim).mainTexture = www.texture;
                PartMaster.instance.GetMaterial(PartMaster.instance.rearRim).mainTexture = www.texture;
                rimsURL = url;
                break;
            case Parts.Hubs:
                PartMaster.instance.GetMaterial(PartMaster.instance.frontHub).mainTexture = www.texture;
                PartMaster.instance.GetMaterial(PartMaster.instance.rearHub).mainTexture = www.texture;
                hubsURL = url;
                break;
        }
    }

    IEnumerator SetTextureBlank(int partNum)
    {
        yield return new WaitForEndOfFrame();
        switch ((Parts)partNum)
        {
            case Parts.Bars:
                PartMaster.instance.GetMaterial(PartMaster.instance.bars).mainTexture = null;
                barsURL = "";
                break;
            case Parts.Forks:
                PartMaster.instance.GetMaterial(PartMaster.instance.forks).mainTexture = null;
                forksURL = "";
                break;
            case Parts.Frame:
                PartMaster.instance.GetMaterial(PartMaster.instance.frame).mainTexture = null;
                frameURL = "";
                break;
            case Parts.Seat:
                PartMaster.instance.GetMaterial(PartMaster.instance.seat).mainTexture = null;
                seatURL = "";
                break;
            case Parts.Tires:
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[0].mainTexture = null;
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[0].mainTexture = null;
                tireURL = "";
                break;
            case Parts.Tire_Wall:
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[1].mainTexture = null;
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[1].mainTexture = null;
                tireWallURL = "";
                break;
            case Parts.Rims:
                PartMaster.instance.GetMaterial(PartMaster.instance.frontRim).mainTexture = null;
                PartMaster.instance.GetMaterial(PartMaster.instance.rearRim).mainTexture = null;
                rimsURL = "";
                break;
            case Parts.Hubs:
                PartMaster.instance.GetMaterial(PartMaster.instance.frontHub).mainTexture = null;
                PartMaster.instance.GetMaterial(PartMaster.instance.rearHub).mainTexture = null;
                hubsURL = "";
                break;
        }
    }

    IEnumerator SetNormalBlank(int partNum)
    {
        yield return new WaitForEndOfFrame();
        switch ((Parts)partNum)
        {
            case Parts.Bars:
                PartMaster.instance.GetMaterial(PartMaster.instance.bars).SetTexture("_BumpMap", null);
                barsURLN = "";
                break;
            case Parts.Forks:
                PartMaster.instance.GetMaterial(PartMaster.instance.forks).SetTexture("_BumpMap", null);
                forksURLN = "";
                break;
            case Parts.Frame:
                PartMaster.instance.GetMaterial(PartMaster.instance.frame).SetTexture("_BumpMap", null);
                frameURLN = "";
                break;
            case Parts.Seat:
                PartMaster.instance.GetMaterial(PartMaster.instance.seat).SetTexture("_BumpMap", null);
                seatURLN = "";
                break;
            case Parts.Tires:
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[0].SetTexture("_BumpMap", null);
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[0].SetTexture("_BumpMap", null);
                tireURLN = "";
                break;
            case Parts.Tire_Wall:
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[1].SetTexture("_BumpMap", null);
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[1].SetTexture("_BumpMap", null);
                tireWallURLN = "";
                break;
            case Parts.Rims:
                PartMaster.instance.GetMaterial(PartMaster.instance.frontRim).SetTexture("_BumpMap", null);
                PartMaster.instance.GetMaterial(PartMaster.instance.rearRim).SetTexture("_BumpMap", null);
                rimsURLN = "";
                break;
            case Parts.Hubs:
                PartMaster.instance.GetMaterial(PartMaster.instance.frontHub).SetTexture("_BumpMap", null);
                PartMaster.instance.GetMaterial(PartMaster.instance.rearHub).SetTexture("_BumpMap", null);
                hubsURLN = "";
                break;
        }
    }

    IEnumerator SetMetallicBlank(int partNum)
    {
        yield return new WaitForEndOfFrame();
        switch ((Parts)partNum)
        {
            case Parts.Bars:
                PartMaster.instance.GetMaterial(PartMaster.instance.bars).SetTexture("_MetallicGlossMap", null);
                barsURLM = "";
                break;
            case Parts.Forks:
                PartMaster.instance.GetMaterial(PartMaster.instance.forks).SetTexture("_MetallicGlossMap", null);
                forksURLM = "";
                break;
            case Parts.Frame:
                PartMaster.instance.GetMaterial(PartMaster.instance.frame).SetTexture("_MetallicGlossMap", null);
                frameURLM = "";
                break;
            case Parts.Seat:
                PartMaster.instance.GetMaterial(PartMaster.instance.seat).SetTexture("_MetallicGlossMap", null);
                seatURLM = "";
                break;
            case Parts.Tires:
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[0].SetTexture("_MetallicGlossMap", null);
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[0].SetTexture("_MetallicGlossMap", null);
                tireURLM = "";
                break;
            case Parts.Tire_Wall:
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[1].SetTexture("_MetallicGlossMap", null);
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[1].SetTexture("_MetallicGlossMap", null);
                tireWallURLM = "";
                break;
            case Parts.Rims:
                PartMaster.instance.GetMaterial(PartMaster.instance.frontRim).SetTexture("_MetallicGlossMap", null);
                PartMaster.instance.GetMaterial(PartMaster.instance.rearRim).SetTexture("_MetallicGlossMap", null);
                rimsURLM = "";
                break;
            case Parts.Hubs:
                PartMaster.instance.GetMaterial(PartMaster.instance.frontHub).SetTexture("_MetallicGlossMap", null);
                PartMaster.instance.GetMaterial(PartMaster.instance.rearHub).SetTexture("_MetallicGlossMap", null);
                hubsURLM = "";
                break;
        }
    }

    public string GetImageLink(Parts bikePart)
    {
        switch (bikePart)
        {
            case Parts.Bars:
                return barsURL;
            case Parts.Forks:
                return forksURL;
            case Parts.Frame:
                return frameURL;
            case Parts.Seat:
                return seatURL;
            case Parts.Tires:
                return tireURL;
            case Parts.Tire_Wall:
                return tireWallURL;
            case Parts.Rims:
                return rimsURL;
            case Parts.Hubs:
                return hubsURL;
        }

        return null;
    }
}
