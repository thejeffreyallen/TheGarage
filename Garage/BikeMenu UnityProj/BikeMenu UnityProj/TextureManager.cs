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

    int selectedPart;

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
        albedoList.Add(0, "");
        albedoList.Add(1, "");
        albedoList.Add(36, "");
        albedoList.Add(2, "");
        albedoList.Add(10, "");
        albedoList.Add(11, "");
        albedoList.Add(6, "");
        albedoList.Add(7, "");
        albedoList.Add(8, "");
        albedoList.Add(9, "");
        albedoList.Add(-1, "");
        albedoList.Add(-2, "");

        normalList.Add(0, "");
        normalList.Add(1, "");
        normalList.Add(36, "");
        normalList.Add(2, "");
        normalList.Add(10, "");
        normalList.Add(11, "");
        normalList.Add(6, "");
        normalList.Add(7, "");
        normalList.Add(8, "");
        normalList.Add(9, "");
        normalList.Add(-1, "");
        normalList.Add(-2, "");

        metallicList.Add(0, "");
        metallicList.Add(1, "");
        metallicList.Add(36, "");
        metallicList.Add(2, "");
        metallicList.Add(10, "");
        metallicList.Add(11, "");
        metallicList.Add(6, "");
        metallicList.Add(7, "");
        metallicList.Add(8, "");
        metallicList.Add(9, "");
        metallicList.Add(-1, "");
        metallicList.Add(-2, "");
    }

    
    public void Update()
    {
        if (shinySlide.gameObject.transform.parent.gameObject.activeInHierarchy)
        {
            Debug.Log("Setting glossiness for part number: " + selectedPart);
            if (selectedPart == -1)
            {
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[1].SetFloat("_Glossiness", shinySlide.value);
            }
            else if (selectedPart == -2)
            {
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[1].SetFloat("_Glossiness", shinySlide.value);
            }
            else if (selectedPart == -3)
            {
                Debug.Log("Do nothing yet for part number " + selectedPart);
            }
            else if (selectedPart == -4)
            {
                Debug.Log("Do nothing yet for part number " + selectedPart);
            }
            else
            {
                PartMaster.instance.GetMaterial(selectedPart).SetFloat("_Glossiness", shinySlide.value);
            }
        }
    }
    

    public void SetSelectedPart(int bikePart)
    {
        selectedPart = bikePart;
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
        if (partNum == -1) //Front Tire Wall
        {
            if(!enableKeyword.Equals(""))
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[1].EnableKeyword(enableKeyword);
            PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[1].SetTexture(texType, tex);
        }
        else if (partNum == -2) // Rear Tire Wall
        {
            if (!enableKeyword.Equals(""))
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[1].EnableKeyword(enableKeyword);
            PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[1].SetTexture(texType, tex);
        }
        else // Everything else
        {
            if (!enableKeyword.Equals(""))
                PartMaster.instance.GetMaterials(partNum)[0].EnableKeyword(enableKeyword);
            PartMaster.instance.GetMaterials(partNum)[0].SetTexture(texType, tex);
        }
        if (inputField == null)
            list[partNum] = url;
        else
        {
            list[partNum] = inputField.text;
            inputField.text = "";
        }
    }


    IEnumerator SetTextureEnum()
    {
        WWW www = new WWW(urlInput.text);
        while (!www.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
        TexHelper(www.texture, albedoList, urlInput, selectedPart, "_MainTex");
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
        TexHelper(normalTexture, normalList, urlNorm, selectedPart, "_BumpMap", "_NORMALMAP");
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
        TexHelper(normalTexture, normalList, null, partNum, "_BumpMap", "_NORMALMAP", url);
    }

    IEnumerator SetMetallicEnum()
    {
        WWW www = new WWW(urlMet.text);
        while (!www.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
        TexHelper(www.texture, metallicList, urlMet, selectedPart, "_MetallicGlossMap", "_METALLICGLOSSMAP");
    }

    IEnumerator SetMetallicEnum(int partNum, string url)
    {
        WWW www = new WWW(url);
        while (!www.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
        TexHelper(www.texture, metallicList, null, partNum, "_MetallicGlossMap", "_METALLICGLOSSMAP", url);
    }

    IEnumerator SetTextureEnum(int partNum, string url)
    {
        WWW www = new WWW(url);
        while (!www.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
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
        TexHelper(null, normalList, null, partNum, "_BumpMap");
    }

    IEnumerator SetMetallicBlank(int partNum)
    {
        yield return new WaitForEndOfFrame();
        TexHelper(null, metallicList, null, partNum, "_MetallicGlossMap");
    }

}
