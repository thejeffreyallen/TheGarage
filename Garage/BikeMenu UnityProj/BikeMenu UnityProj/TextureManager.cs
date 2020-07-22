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

    Parts selectedPart;

    public string frameURL ="";
    public string barsURL ="";
    public string seatURL = "";
    public string forksURL = "";
    public string tireURL = "";
    public string tireWallURL = "";
    public string rimsURL = "";
    public string hubsURL = "";

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
        StoreOriginalTextures();
    }

    void Start()
    {
        instance = this;
        
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

    public void SetSelectedPart(int bikePart)
    {
        selectedPart = (Parts)bikePart;
    }

    public void SetTexture()
    {
        StartCoroutine(SetTextureEnum());
    }
    public void SetTexture(int partNum, string url)
    {
        StartCoroutine(SetTextureEnum(partNum, url));
    }

    public void RemoveTexture()
    {
        //StartCoroutine(SetTextureEnum(FindObjectOfType<ColourSetter>().currentPart, "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAOEAAADhCAMAAAAJbSJIAAAAA1BMVEX///+nxBvIAAAASElEQVR4nO3BgQAAAADDoPlTX+AIVQEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADwDcaiAAFXD1ujAAAAAElFTkSuQmCC"));
        StartCoroutine(SetTextureBlank(FindObjectOfType<ColourSetter>().currentPart));
    }

    public void StoreOriginalTextures()
    {
        OriginalFrameTex = GameObject.Find("Frame Mesh").GetComponent<Renderer>().material.mainTexture;
        OriginalBarsTex = GameObject.Find("Bars Mesh").GetComponent<Renderer>().material.mainTexture;
        OriginalForksTex = GameObject.Find("Forks Mesh").GetComponent<Renderer>().material.mainTexture;
        OriginalSeatTex = GameObject.Find("Seat Mesh").GetComponent<Renderer>().material.mainTexture;
        OriginalRimTex = GameObject.Find("Rim Mesh").GetComponent<Renderer>().material.mainTexture;
        OriginalHubTex = GameObject.Find("Hub Mesh").GetComponent<Renderer>().material.mainTexture;

        List<GameObject> tires = new List<GameObject>();
        foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
        {
            if (go.name == "Tire Mesh")
                tires.Add(go);
        }

        OriginalTire1Tex = tires[0].GetComponent<Renderer>().material.mainTexture;
        OriginalTire2Tex = tires[1].GetComponent<Renderer>().material.mainTexture;

        List<GameObject> tireWalls = new List<GameObject>();
        foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
        {
            if (go.name == "Tire Mesh")
                tireWalls.Add(go);
        }

        OriginalTire1WallTex = tireWalls[0].GetComponent<Renderer>().materials[1].mainTexture;
        OriginalTire2WallTex = tireWalls[1].GetComponent<Renderer>().materials[1].mainTexture;      
    }

    public void SetOriginalTextures()
    {
        GameObject.Find("Frame Mesh").GetComponent<Renderer>().material.mainTexture = OriginalFrameTex;
        GameObject.Find("Bars Mesh").GetComponent<Renderer>().material.mainTexture = OriginalBarsTex;
        GameObject.Find("Forks Mesh").GetComponent<Renderer>().material.mainTexture = OriginalForksTex;
        GameObject.Find("Seat Mesh").GetComponent<Renderer>().material.mainTexture = OriginalSeatTex;

        List<GameObject> tires = new List<GameObject>();
        foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
        {
            if (go.name == "Tire Mesh")
                tires.Add(go);
        }

        tires[0].GetComponent<Renderer>().material.mainTexture = OriginalTire1Tex;
        tires[1].GetComponent<Renderer>().material.mainTexture = OriginalTire2Tex;
        tires[0].GetComponent<Renderer>().materials[1].mainTexture = OriginalTire1WallTex;
        tires[1].GetComponent<Renderer>().materials[1].mainTexture = OriginalTire2WallTex;
        List<GameObject> rims = new List<GameObject>();
        foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
        {
            if (go.name == "Rim Mesh")
                rims.Add(go);
        }
        for (int i = 0; i < rims.Count; i++)
        {
            rims[i].GetComponent<Renderer>().material.mainTexture = OriginalRimTex;
        }

        List<GameObject> hubs = new List<GameObject>();
        foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
        {
            if (go.name == "Hub Mesh")
                hubs.Add(go);
        }
        for (int i = 0; i < hubs.Count; i++)
        {
            hubs[i].GetComponent<Renderer>().material.mainTexture = OriginalRimTex;
        }

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
                List<GameObject> tires = new List<GameObject>();
                foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
                {
                    if (go.name == "Tire Mesh")
                        tires.Add(go);
                }
                for (int i = 0; i < tires.Count; i++)
                {
                    tires[i].GetComponent<Renderer>().material.mainTexture = www.texture;
                }
                tireURL = urlInput.text;
                break;
            case Parts.Tire_Wall:
                List<GameObject> tireWalls = new List<GameObject>();
                foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
                {
                    if (go.name == "Tire Mesh")
                        tireWalls.Add(go);
                }
                for (int i = 0; i < tireWalls.Count; i++)
                {
                    tireWalls[i].GetComponent<Renderer>().materials[1].mainTexture = www.texture;
                }
                tireWallURL = urlInput.text;
                break;
            case Parts.Rims:
                List<GameObject> rims = new List<GameObject>();
                foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
                {
                    if (go.name == "Rim Mesh")
                        rims.Add(go);
                }
                for (int i = 0; i < rims.Count; i++)
                {
                    rims[i].GetComponent<Renderer>().material.mainTexture = www.texture;
                }
                rimsURL = urlInput.text;
                break;
            case Parts.Hubs:
                List<GameObject> hubs = new List<GameObject>();
                foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
                {
                    if (go.name == "Hub Mesh")
                        hubs.Add(go);
                }
                for (int i = 0; i < hubs.Count; i++)
                {
                    hubs[i].GetComponent<Renderer>().material.mainTexture = www.texture;
                }
                hubsURL = urlInput.text;
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
                List<GameObject> tires = new List<GameObject>();
                foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
                {
                    if (go.name == "Tire Mesh")
                        tires.Add(go);
                }
                for (int i = 0; i < tires.Count; i++)
                {
                    tires[i].GetComponent<Renderer>().material.mainTexture = www.texture;
                }
                tireURL = url;
                break;
            case Parts.Tire_Wall:
                List<GameObject> tireWalls = new List<GameObject>();
                foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
                {
                    if (go.name == "Tire Mesh")
                        tireWalls.Add(go);
                }
                for (int i = 0; i < tireWalls.Count; i++)
                {
                    tireWalls[i].GetComponent<Renderer>().materials[1].mainTexture = www.texture;
                }
                tireWallURL = url;
                break;
            case Parts.Rims:
                List<GameObject> rims = new List<GameObject>();
                foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
                {
                    if (go.name == "Rim Mesh")
                        rims.Add(go);
                }
                for (int i = 0; i < rims.Count; i++)
                {
                    rims[i].GetComponent<Renderer>().material.mainTexture = www.texture;
                }
                rimsURL = url;
                break;
            case Parts.Hubs:
                List<GameObject> hubs = new List<GameObject>();
                foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
                {
                    if (go.name == "Hub Mesh")
                        hubs.Add(go);
                }
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
                List<GameObject> tires = new List<GameObject>();
                foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
                {
                    if (go.name == "Tire Mesh")
                        tires.Add(go);
                }
                for (int i = 0; i < tires.Count; i++)
                {
                    tires[i].GetComponent<Renderer>().material.mainTexture = null;
                }
                tireURL = "";
                break;
            case Parts.Tire_Wall:
                List<GameObject> tireWalls = new List<GameObject>();
                foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
                {
                    if (go.name == "Tire Mesh")
                        tireWalls.Add(go);
                }
                for (int i = 0; i < tireWalls.Count; i++)
                {
                    tireWalls[i].GetComponent<Renderer>().materials[1].mainTexture = null;
                }
                tireWallURL = "";
                break;
            case Parts.Rims:
                List<GameObject> rims = new List<GameObject>();
                foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
                {
                    if (go.name == "Rim Mesh")
                        rims.Add(go);
                }
                for (int i = 0; i < rims.Count; i++)
                {
                    rims[i].GetComponent<Renderer>().material.mainTexture = null;
                }
                rimsURL = "";
                break;
            case Parts.Hubs:
                List<GameObject> hubs = new List<GameObject>();
                foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
                {
                    if (go.name == "Hub Mesh")
                        hubs.Add(go);
                }
                for (int i = 0; i < hubs.Count; i++)
                {
                    hubs[i].GetComponent<Renderer>().material.mainTexture = null;
                }
                hubsURL = "";
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
