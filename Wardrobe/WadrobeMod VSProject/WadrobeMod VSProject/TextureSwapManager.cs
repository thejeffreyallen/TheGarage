using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;

public class TextureSwapManager : MonoBehaviour
{
    public static TextureSwapManager instance;

    public Dropdown clothingDropdown;
    public InputField url;

    string chosenURL;

    string saveErrors;

    void Start()
    {
        instance = this;
    }

    public void SetTexture()
    {
        try
        {
            chosenURL = url.text;
            // StartCoroutine(SetTexture_Enum());
            ButtonCreator.instance.CreateButton(chosenURL);
            TextureSaving.instance.SetClothingSave(chosenURL, clothingDropdown.value);
        }
        catch (Exception ex)
        {
            {
                this.saveErrors = string.Concat(new string[]
                {
                    this.saveErrors,
                    ex.Message,
                    "\n ",
                    ex.StackTrace,
                    "\n "
                });
            }
            File.AppendAllText(Application.dataPath + "//TheHouseErrorLog.txt", string.Concat(new object[]
            {
                "\n",
                DateTime.Now,
                "\nERRORS: ",
                this.saveErrors
            }));
        }
    }

    public void SetTexture(string url)
    {
        try { 
        chosenURL = url;
        StartCoroutine(SetTexture_Enum());
        TextureSaving.instance.SetClothingSave(url, clothingDropdown.value);
        }
        catch (Exception ex)
        {
            {
                this.saveErrors = string.Concat(new string[]
                {
                    this.saveErrors,
                    ex.Message,
                    "\n ",
                    ex.StackTrace,
                    "\n "
                });
            }
            File.AppendAllText(Application.dataPath + "//TheHouseErrorLog.txt", string.Concat(new object[]
            {
                "\n",
                DateTime.Now,
                "\nERRORS: ",
                this.saveErrors
            }));
        }
    }

    public void SetTexture(string url, int clothingPiece)
    {
        try
        {
            chosenURL = url;
            StartCoroutine(SetTexture_Enum(clothingPiece));
            TextureSaving.instance.SetClothingSave(url, clothingPiece);
        }
        catch (Exception ex)
        {
            {
                this.saveErrors = string.Concat(new string[]
                {
                    this.saveErrors,
                    ex.Message,
                    "\n ",
                    ex.StackTrace,
                    "\n "
                });
            }
            File.AppendAllText(Application.dataPath + "//TheHouseErrorLog.txt", string.Concat(new object[]
            {
                "\n",
                DateTime.Now,
                "\nERRORS: ",
                this.saveErrors
            }));
        }
    }

    IEnumerator SetTexture_Enum()
    {
        WWW www = new WWW(chosenURL);
        while (!www.isDone)
            yield return new WaitForEndOfFrame();
        Texture2D texture = www.texture;

        Material mat = PlayerAccessor.instance.GetClothingMaterial((PlayerAccessor.PlayerClothing)clothingDropdown.value);
        mat.mainTexture = texture;

        ManikinManager.instance.SetClothingTexture(clothingDropdown.value, mat.mainTexture);

        yield break;
    }

    IEnumerator SetTexture_Enum(int clothing)
    {
        /* Renderer r = PlayerAccessor.instance.GetRenderer((PlayerAccessor.PlayerClothing)clothing);

         Davinci.get().load(chosenURL).into(r).start();

         ManikinManager.instance.SetClothingTexture(clothingDropdown.value, r.material.mainTexture);*/

        WWW www = new WWW(chosenURL);
        while (!www.isDone)
            yield return new WaitForEndOfFrame();
        Texture2D texture = www.texture;

        Material mat = PlayerAccessor.instance.GetClothingMaterial((PlayerAccessor.PlayerClothing)clothing);
        mat.mainTexture = texture;

        ManikinManager.instance.SetClothingTexture(clothingDropdown.value, mat.mainTexture);

        yield break;
    }
}
