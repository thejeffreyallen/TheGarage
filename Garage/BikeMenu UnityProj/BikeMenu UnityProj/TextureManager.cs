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

    private List<GameObject> tires = new List<GameObject>();
    private List<GameObject> rims = new List<GameObject>();
    private List<GameObject> hubs = new List<GameObject>();

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
        foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
        {
            if (tires.Count == rims.Count && tires.Count == hubs.Count && tires.Count == 2)
                return;
            if (go.name.Equals("Tire Mesh"))
            {
                Debug.Log("Found " + go.name);
                tires.Add(go);
            }
            if (go.name.Equals("Rim Mesh"))
            {
                Debug.Log("Found " + go.name);
                rims.Add(go);
            }

            if (go.name.Equals("Hubs Mesh"))
            {
                Debug.Log("Found " + go.name);
                hubs.Add(go);
            }
        }
        StoreOriginalTextures();
        instance = this;
    }

    void Start()
    {
        
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
                    GameObject.Find("Bars Mesh").GetComponent<Renderer>().material.SetFloat("_Glossiness", shinySlide.value);
                    break;
                case Parts.Forks:
                    GameObject.Find("Forks Mesh").GetComponent<Renderer>().material.SetFloat("_Glossiness", shinySlide.value);
                    break;
                case Parts.Frame:
                    GameObject.Find("Frame Mesh").GetComponent<Renderer>().material.SetFloat("_Glossiness", shinySlide.value);
                    break;
                case Parts.Seat:
                    GameObject.Find("Seat Mesh").GetComponent<Renderer>().material.SetFloat("_Glossiness", shinySlide.value);
                    break;
                case Parts.Tires:
                    for (int i = 0; i < tires.Count; i++)
                    {
                        tires[i].GetComponent<Renderer>().material.SetFloat("_Glossiness", shinySlide.value);
                    }
                    break;
                case Parts.Tire_Wall:

                    for (int i = 0; i < tires.Count; i++)
                    {
                        tires[i].GetComponent<Renderer>().materials[1].SetFloat("_Glossiness", shinySlide.value);
                    }
                    break;
                case Parts.Rims:
                    for (int i = 0; i < rims.Count; i++)
                    {
                        rims[i].GetComponent<Renderer>().material.SetFloat("_Glossiness", shinySlide.value);
                    }
                    break;
                case Parts.Hubs:
                    for (int i = 0; i < hubs.Count; i++)
                    {
                        hubs[i].GetComponent<Renderer>().material.SetFloat("_Glossiness", shinySlide.value);
                    }
                    break;
                default:
                    break;
            }   
        }
    }

    public void SetSelectedPart(int bikePart)
    {
        selectedPart = (Parts)bikePart;
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
        StartCoroutine(SetTextureBlank(FindObjectOfType<ColourSetter>().currentPart));
        Resources.UnloadUnusedAssets();
    }

    public void RemoveNormal()
    {
        StartCoroutine(SetNormalBlank(FindObjectOfType<ColourSetter>().currentPart));
        Resources.UnloadUnusedAssets();
    }

    public void RemoveMetallic()
    {
        StartCoroutine(SetMetallicBlank(FindObjectOfType<ColourSetter>().currentPart));
        Resources.UnloadUnusedAssets();
    }

    public void StoreOriginalTextures()
    {
        OriginalFrameTex = GameObject.Find("Frame Mesh").GetComponent<Renderer>().material.mainTexture;
        OriginalBarsTex = GameObject.Find("Bars Mesh").GetComponent<Renderer>().material.mainTexture;
        OriginalForksTex = GameObject.Find("Forks Mesh").GetComponent<Renderer>().material.mainTexture;
        OriginalSeatTex = GameObject.Find("Seat Mesh").GetComponent<Renderer>().material.mainTexture;
        OriginalRimTex = GameObject.Find("Rim Mesh").GetComponent<Renderer>().material.mainTexture;
        OriginalHubTex = GameObject.Find("Hub Mesh").GetComponent<Renderer>().material.mainTexture;


        OriginalTire1Tex = tires[0].GetComponent<Renderer>().material.mainTexture;
        OriginalTire2Tex = tires[1].GetComponent<Renderer>().material.mainTexture;

        OriginalTire1WallTex = tires[0].GetComponent<Renderer>().materials[1].mainTexture;
        OriginalTire2WallTex = tires[1].GetComponent<Renderer>().materials[1].mainTexture;      
    }

    public void SetOriginalTextures()
    {
        GameObject.Find("Frame Mesh").GetComponent<Renderer>().material.mainTexture = OriginalFrameTex;
        GameObject.Find("Bars Mesh").GetComponent<Renderer>().material.mainTexture = OriginalBarsTex;
        GameObject.Find("Forks Mesh").GetComponent<Renderer>().material.mainTexture = OriginalForksTex;
        GameObject.Find("Seat Mesh").GetComponent<Renderer>().material.mainTexture = OriginalSeatTex;

        tires[0].GetComponent<Renderer>().material.mainTexture = OriginalTire1Tex;
        tires[1].GetComponent<Renderer>().material.mainTexture = OriginalTire2Tex;
        tires[0].GetComponent<Renderer>().materials[1].mainTexture = OriginalTire1WallTex;
        tires[1].GetComponent<Renderer>().materials[1].mainTexture = OriginalTire2WallTex;

        for (int i = 0; i < rims.Count; i++)
        {
            rims[i].GetComponent<Renderer>().material.mainTexture = OriginalRimTex;
        }

        for (int i = 0; i < hubs.Count; i++)
        {
            hubs[i].GetComponent<Renderer>().material.mainTexture = OriginalRimTex;
        }
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
                GameObject.Find("Bars Mesh").GetComponent<Renderer>().material.mainTexture = www.texture;
                barsURL = urlInput.text;
                break;
            case Parts.Forks:
                GameObject.Find("Forks Mesh").GetComponent<Renderer>().material.mainTexture = www.texture;
                forksURL = urlInput.text;
                break;
            case Parts.Frame:
                GameObject.Find("Frame Mesh").GetComponent<Renderer>().material.mainTexture = www.texture;
                frameURL = urlInput.text;
                break;
            case Parts.Seat:
                GameObject.Find("Seat Mesh").GetComponent<Renderer>().material.mainTexture = www.texture;
                seatURL = urlInput.text;
                break;
            case Parts.Tires:
                for (int i = 0; i < tires.Count; i++)
                {
                    tires[i].GetComponent<Renderer>().material.mainTexture = www.texture;
                }
                tireURL = urlInput.text;
                break;
            case Parts.Tire_Wall:
                for (int i = 0; i < tires.Count; i++)
                {
                    tires[i].GetComponent<Renderer>().materials[1].mainTexture = www.texture;
                }
                tireWallURL = urlInput.text;
                break;
            case Parts.Rims:
                for (int i = 0; i < rims.Count; i++)
                {
                    rims[i].GetComponent<Renderer>().material.mainTexture = www.texture;
                }
                rimsURL = urlInput.text;
                break;
            case Parts.Hubs:
                for (int i = 0; i < hubs.Count; i++)
                {
                    hubs[i].GetComponent<Renderer>().material.mainTexture = www.texture;
                }
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
        normalTexture.Apply();

        switch (selectedPart)
        {
            case Parts.Bars:
                GameObject.Find("Bars Mesh").GetComponent<Renderer>().material.EnableKeyword("_NORMALMAP");
                GameObject.Find("Bars Mesh").GetComponent<Renderer>().material.SetTexture("_BumpMap", normalTexture);
                barsURLN = urlNorm.text;
                break;
            case Parts.Forks:
                GameObject.Find("Forks Mesh").GetComponent<Renderer>().material.EnableKeyword("_NORMALMAP");
                GameObject.Find("Forks Mesh").GetComponent<Renderer>().material.SetTexture("_BumpMap", normalTexture);
                forksURLN = urlNorm.text;
                break;
            case Parts.Frame:
                GameObject.Find("Frame Mesh").GetComponent<Renderer>().material.EnableKeyword("_NORMALMAP");
                GameObject.Find("Frame Mesh").GetComponent<Renderer>().material.SetTexture("_BumpMap", normalTexture);
                StartCoroutine(SetMetallicEnum());
                frameURLN = urlNorm.text;
                break;
            case Parts.Seat:
                GameObject.Find("Seat Mesh").GetComponent<Renderer>().material.EnableKeyword("_NORMALMAP");
                GameObject.Find("Seat Mesh").GetComponent<Renderer>().material.SetTexture("_BumpMap", normalTexture);
                seatURLN = urlNorm.text;
                break;
            case Parts.Tires:
                for (int i = 0; i < tires.Count; i++)
                {
                    tires[i].GetComponent<Renderer>().material.EnableKeyword("_NORMALMAP");
                    tires[i].GetComponent<Renderer>().material.SetTexture("_BumpMap", normalTexture);
                }
                tireURLN = urlNorm.text;
                break;
            case Parts.Tire_Wall:

                for (int i = 0; i < tires.Count; i++)
                {
                    tires[i].GetComponent<Renderer>().material.EnableKeyword("_NORMALMAP");
                    tires[i].GetComponent<Renderer>().materials[1].SetTexture("_BumpMap", normalTexture);
                }
                tireWallURLN = urlNorm.text;
                break;
            case Parts.Rims:

                for (int i = 0; i < rims.Count; i++)
                {
                    rims[i].GetComponent<Renderer>().material.EnableKeyword("_NORMALMAP");
                    rims[i].GetComponent<Renderer>().material.SetTexture("_BumpMap", normalTexture);
                }
                rimsURLN = urlNorm.text;
                break;
            case Parts.Hubs:

                for (int i = 0; i < hubs.Count; i++)
                {
                    hubs[i].GetComponent<Renderer>().material.EnableKeyword("_NORMALMAP");
                    hubs[i].GetComponent<Renderer>().material.SetTexture("_BumpMap", normalTexture);
                }
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
        normalTexture.Apply();

        switch ((Parts) partNum)
        {
            case Parts.Bars:
                GameObject.Find("Bars Mesh").GetComponent<Renderer>().material.EnableKeyword("_NORMALMAP");
                GameObject.Find("Bars Mesh").GetComponent<Renderer>().material.SetTexture("_BumpMap", normalTexture);
                barsURLN = url;
                break;
            case Parts.Forks:
                GameObject.Find("Forks Mesh").GetComponent<Renderer>().material.EnableKeyword("_NORMALMAP");
                GameObject.Find("Forks Mesh").GetComponent<Renderer>().material.SetTexture("_BumpMap", normalTexture);
                forksURLN = url;
                break;
            case Parts.Frame:
                GameObject.Find("Frame Mesh").GetComponent<Renderer>().material.EnableKeyword("_NORMALMAP");
                GameObject.Find("Frame Mesh").GetComponent<Renderer>().material.SetTexture("_BumpMap", normalTexture);
                StartCoroutine(SetMetallicEnum());
                frameURLN = url;
                break;
            case Parts.Seat:
                GameObject.Find("Seat Mesh").GetComponent<Renderer>().material.EnableKeyword("_NORMALMAP");
                GameObject.Find("Seat Mesh").GetComponent<Renderer>().material.SetTexture("_BumpMap", normalTexture);
                seatURLN = url;
                break;
            case Parts.Tires:
                for (int i = 0; i < tires.Count; i++)
                {
                    tires[i].GetComponent<Renderer>().material.EnableKeyword("_NORMALMAP");
                    tires[i].GetComponent<Renderer>().material.SetTexture("_BumpMap", normalTexture);
                }
                tireURLN = url;
                break;
            case Parts.Tire_Wall:
                for (int i = 0; i < tires.Count; i++)
                {
                    tires[i].GetComponent<Renderer>().material.EnableKeyword("_NORMALMAP");
                    tires[i].GetComponent<Renderer>().materials[1].SetTexture("_BumpMap", normalTexture);
                }
                tireWallURLN = url;
                break;
            case Parts.Rims:
                for (int i = 0; i < rims.Count; i++)
                {
                    rims[i].GetComponent<Renderer>().material.EnableKeyword("_NORMALMAP");
                    rims[i].GetComponent<Renderer>().material.SetTexture("_BumpMap", normalTexture);
                }
                rimsURLN = url;
                break;
            case Parts.Hubs:
                for (int i = 0; i < hubs.Count; i++)
                {
                    hubs[i].GetComponent<Renderer>().material.EnableKeyword("_NORMALMAP");
                    hubs[i].GetComponent<Renderer>().material.SetTexture("_BumpMap", normalTexture);
                }
                hubsURLN = url;
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
                GameObject.Find("Bars Mesh").GetComponent<Renderer>().material.EnableKeyword("_METALLICGLOSSMAP");
                GameObject.Find("Bars Mesh").GetComponent<Renderer>().material.SetTexture("_MetallicGlossMap", www.texture);
                barsURLM = urlMet.text;
                break;
            case Parts.Forks:
                GameObject.Find("Forks Mesh").GetComponent<Renderer>().material.EnableKeyword("_METALLICGLOSSMAP");
                GameObject.Find("Forks Mesh").GetComponent<Renderer>().material.SetTexture("_MetallicGlossMap", www.texture);
                forksURLM = urlMet.text;
                break;
            case Parts.Frame:
                GameObject.Find("Frame Mesh").GetComponent<Renderer>().material.EnableKeyword("_METALLICGLOSSMAP");
                GameObject.Find("Frame Mesh").GetComponent<Renderer>().material.SetTexture("_MetallicGlossMap", www.texture);
                frameURLM = urlMet.text;
                break;
            case Parts.Seat:
                GameObject.Find("Seat Mesh").GetComponent<Renderer>().material.EnableKeyword("_METALLICGLOSSMAP");
                GameObject.Find("Seat Mesh").GetComponent<Renderer>().material.SetTexture("_MetallicGlossMap", www.texture);
                seatURLM = urlMet.text;
                break;
            case Parts.Tires:
                for (int i = 0; i < tires.Count; i++)
                {
                    tires[i].GetComponent<Renderer>().material.EnableKeyword("_METALLICGLOSSMAP");
                    tires[i].GetComponent<Renderer>().material.SetTexture("_MetallicGlossMap", www.texture);
                }
                tireURLM = urlMet.text;
                break;
            case Parts.Tire_Wall:
                for (int i = 0; i < tires.Count; i++)
                {
                    tires[i].GetComponent<Renderer>().material.EnableKeyword("_METALLICGLOSSMAP");
                    tires[i].GetComponent<Renderer>().materials[1].SetTexture("_MetallicGlossMap", www.texture);
                }
                tireWallURLM = urlMet.text;
                break;
            case Parts.Rims:
                for (int i = 0; i < rims.Count; i++)
                {
                    rims[i].GetComponent<Renderer>().material.EnableKeyword("_METALLICGLOSSMAP");
                    rims[i].GetComponent<Renderer>().material.SetTexture("_MetallicGlossMap", www.texture);
                }
                rimsURLM = urlMet.text;
                break;
            case Parts.Hubs:
                for (int i = 0; i < hubs.Count; i++)
                {
                    hubs[i].GetComponent<Renderer>().material.EnableKeyword("_METALLICGLOSSMAP");
                    hubs[i].GetComponent<Renderer>().material.SetTexture("_MetallicGlossMap", www.texture);
                }
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
                GameObject.Find("Bars Mesh").GetComponent<Renderer>().material.EnableKeyword("_METALLICGLOSSMAP");
                GameObject.Find("Bars Mesh").GetComponent<Renderer>().material.SetTexture("_MetallicGlossMap", www.texture);
                barsURLM = url;
                break;
            case Parts.Forks:
                GameObject.Find("Forks Mesh").GetComponent<Renderer>().material.EnableKeyword("_METALLICGLOSSMAP");
                GameObject.Find("Forks Mesh").GetComponent<Renderer>().material.SetTexture("_MetallicGlossMap", www.texture);
                forksURLM = url;
                break;
            case Parts.Frame:
                GameObject.Find("Frame Mesh").GetComponent<Renderer>().material.EnableKeyword("_METALLICGLOSSMAP");
                GameObject.Find("Frame Mesh").GetComponent<Renderer>().material.SetTexture("_MetallicGlossMap", www.texture);
                frameURLM = url;
                break;
            case Parts.Seat:
                GameObject.Find("Seat Mesh").GetComponent<Renderer>().material.EnableKeyword("_METALLICGLOSSMAP");
                GameObject.Find("Seat Mesh").GetComponent<Renderer>().material.SetTexture("_MetallicGlossMap", www.texture);
                seatURLM = url;
                break;
            case Parts.Tires:
                for (int i = 0; i < tires.Count; i++)
                {
                    tires[i].GetComponent<Renderer>().material.EnableKeyword("_METALLICGLOSSMAP");
                    tires[i].GetComponent<Renderer>().material.SetTexture("_MetallicGlossMap", www.texture);
                }
                tireURLM = url;
                break;
            case Parts.Tire_Wall:
                for (int i = 0; i < tires.Count; i++)
                {
                    tires[i].GetComponent<Renderer>().material.EnableKeyword("_METALLICGLOSSMAP");
                    tires[i].GetComponent<Renderer>().materials[1].SetTexture("_MetallicGlossMap", www.texture);
                }
                tireWallURLM = url;
                break;
            case Parts.Rims:
                for (int i = 0; i < rims.Count; i++)
                {
                    rims[i].GetComponent<Renderer>().material.EnableKeyword("_METALLICGLOSSMAP");
                    rims[i].GetComponent<Renderer>().material.SetTexture("_MetallicGlossMap", www.texture);
                }
                rimsURLM = url;
                break;
            case Parts.Hubs:
                for (int i = 0; i < hubs.Count; i++)
                {
                    hubs[i].GetComponent<Renderer>().material.EnableKeyword("_METALLICGLOSSMAP");
                    hubs[i].GetComponent<Renderer>().material.SetTexture("_MetallicGlossMap", www.texture);
                }
                hubsURLM = url;
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
                GameObject.Find("Bars Mesh").GetComponent<Renderer>().material.mainTexture = www.texture;
                barsURL = url;
                break;
            case Parts.Forks:
                GameObject.Find("Forks Mesh").GetComponent<Renderer>().material.mainTexture = www.texture;
                forksURL = url;
                break;
            case Parts.Frame:
                GameObject.Find("Frame Mesh").GetComponent<Renderer>().material.mainTexture = www.texture;
                frameURL = url;
                break;
            case Parts.Seat:
                GameObject.Find("Seat Mesh").GetComponent<Renderer>().material.mainTexture = www.texture;
                seatURL = url;
                break;
            case Parts.Tires:
                for (int i = 0; i < tires.Count; i++)
                {
                    tires[i].GetComponent<Renderer>().material.mainTexture = www.texture;
                }
                tireURL = url;
                break;
            case Parts.Tire_Wall:
                for (int i = 0; i < tires.Count; i++)
                {
                    tires[i].GetComponent<Renderer>().materials[1].mainTexture = www.texture;
                }
                tireWallURL = url;
                break;
            case Parts.Rims:
                for (int i = 0; i < rims.Count; i++)
                {
                    rims[i].GetComponent<Renderer>().material.mainTexture = www.texture;
                }
                rimsURL = url;
                break;
            case Parts.Hubs:
                for (int i = 0; i < hubs.Count; i++)
                {
                    hubs[i].GetComponent<Renderer>().material.mainTexture = www.texture;
                }
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
                GameObject.Find("Bars Mesh").GetComponent<Renderer>().material.mainTexture = null;
                barsURL = "";
                break;
            case Parts.Forks:
                GameObject.Find("Forks Mesh").GetComponent<Renderer>().material.mainTexture = null;
                forksURL = "";
                break;
            case Parts.Frame:
                GameObject.Find("Frame Mesh").GetComponent<Renderer>().material.mainTexture = null;
                frameURL = "";
                break;
            case Parts.Seat:
                GameObject.Find("Seat Mesh").GetComponent<Renderer>().material.mainTexture = null;
                seatURL = "";
                break;
            case Parts.Tires:
                for (int i = 0; i < tires.Count; i++)
                {
                    tires[i].GetComponent<Renderer>().material.mainTexture = null;
                }
                tireURL = "";
                break;
            case Parts.Tire_Wall:
                for (int i = 0; i < tires.Count; i++)
                {
                    tires[i].GetComponent<Renderer>().materials[1].mainTexture = null;
                }
                tireWallURL = "";
                break;
            case Parts.Rims:
                for (int i = 0; i < rims.Count; i++)
                {
                    rims[i].GetComponent<Renderer>().material.mainTexture = null;
                }
                rimsURL = "";
                break;
            case Parts.Hubs:
                for (int i = 0; i < hubs.Count; i++)
                {
                    hubs[i].GetComponent<Renderer>().material.mainTexture = null;
                }
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
                GameObject.Find("Bars Mesh").GetComponent<Renderer>().material.SetTexture("_BumpMap", null);
                barsURLN = "";
                break;
            case Parts.Forks:
                GameObject.Find("Forks Mesh").GetComponent<Renderer>().material.SetTexture("_BumpMap", null);
                forksURLN = "";
                break;
            case Parts.Frame:
                GameObject.Find("Frame Mesh").GetComponent<Renderer>().material.SetTexture("_BumpMap", null);
                frameURLN = "";
                break;
            case Parts.Seat:
                GameObject.Find("Seat Mesh").GetComponent<Renderer>().material.SetTexture("_BumpMap", null);
                seatURLN = "";
                break;
            case Parts.Tires:
                for (int i = 0; i < tires.Count; i++)
                {
                    tires[i].GetComponent<Renderer>().material.SetTexture("_BumpMap", null);
                }
                tireURLN = "";
                break;
            case Parts.Tire_Wall:
                for (int i = 0; i < tires.Count; i++)
                {
                    tires[i].GetComponent<Renderer>().materials[1].SetTexture("_BumpMap", null);
                }
                tireWallURLN = "";
                break;
            case Parts.Rims:
                for (int i = 0; i < rims.Count; i++)
                {
                    rims[i].GetComponent<Renderer>().material.SetTexture("_BumpMap", null);
                }
                rimsURLN = "";
                break;
            case Parts.Hubs:
                for (int i = 0; i < hubs.Count; i++)
                {
                    hubs[i].GetComponent<Renderer>().material.SetTexture("_BumpMap", null);
                }
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
                GameObject.Find("Bars Mesh").GetComponent<Renderer>().material.SetTexture("_MetallicGlossMap", null);
                barsURLM = "";
                break;
            case Parts.Forks:
                GameObject.Find("Forks Mesh").GetComponent<Renderer>().material.SetTexture("_MetallicGlossMap", null);
                forksURLM = "";
                break;
            case Parts.Frame:
                GameObject.Find("Frame Mesh").GetComponent<Renderer>().material.SetTexture("_MetallicGlossMap", null);
                frameURLM = "";
                break;
            case Parts.Seat:
                GameObject.Find("Seat Mesh").GetComponent<Renderer>().material.SetTexture("_MetallicGlossMap", null);
                seatURLM = "";
                break;
            case Parts.Tires:
                for (int i = 0; i < tires.Count; i++)
                {
                    tires[i].GetComponent<Renderer>().material.SetTexture("_MetallicGlossMap", null);
                }
                tireURLM = "";
                break;
            case Parts.Tire_Wall:
                for (int i = 0; i < tires.Count; i++)
                {
                    tires[i].GetComponent<Renderer>().materials[1].SetTexture("_MetallicGlossMap", null);
                }
                tireWallURLM = "";
                break;
            case Parts.Rims:
                for (int i = 0; i < rims.Count; i++)
                {
                    rims[i].GetComponent<Renderer>().material.SetTexture("_MetallicGlossMap", null);
                }
                rimsURLM = "";
                break;
            case Parts.Hubs:
                for (int i = 0; i < hubs.Count; i++)
                {
                    hubs[i].GetComponent<Renderer>().material.SetTexture("_MetallicGlossMap", null);
                }
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
