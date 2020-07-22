using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

public class TextureSaving : MonoBehaviour
{
    public static TextureSaving instance;

    Texture2D[] hatTextures;
    Texture2D[] shirtTextures;
    Texture2D[] pantsTextures;
    Texture2D[] shoeTextures;
    Texture2D[] headTextures;
    Texture2D[] bodyTextures;
    Texture2D[] handTextures;

    string selectedHatTexture;
    string selectedShirtTexture;
    string selectedPantsTexture;
    string selectedShoesTexture;
    string selectedHeadTexture;
    string selectedBodyTexture;
    string selectedHandTexture;

    void Start()
    {
        instance = this;

        LoadTextures();

        hatTextures = RetrieveTextures("Hat");
        shirtTextures = RetrieveTextures("Shirt");
        pantsTextures = RetrieveTextures("Pants");
        shoeTextures = RetrieveTextures("Shoes");
        headTextures = RetrieveTextures("Head");
        bodyTextures = RetrieveTextures("Body");
        handTextures = RetrieveTextures("Hands-Feet");
    }

    Texture2D[] RetrieveTextures(string folderName)
    {
        string path = Application.dataPath + "//TheHouseContent/Textures/" + folderName;

        string[] files = Directory.GetFiles(path, "*.png", SearchOption.AllDirectories);
        Texture2D[] array = new Texture2D[files.Length];

        for (int i = 0; i < files.Length; i++)
        {
            byte[] bytes = File.ReadAllBytes(files[i]);
            Texture2D texture = new Texture2D(1024, 1024);
            ImageConversion.LoadImage(texture, bytes);
            array[i] = texture;

            if (folderName == "Hat")
                ButtonCreator.instance.CreateButton(files[i], 0);
            else if (folderName == "Shirt")
                ButtonCreator.instance.CreateButton(files[i], 1);
            else if (folderName == "Pants")
                ButtonCreator.instance.CreateButton(files[i], 2);
            else if (folderName == "Shoes")
                ButtonCreator.instance.CreateButton(files[i], 3);
            else if (folderName == "Head")
                ButtonCreator.instance.CreateButton(files[i], 4);
            else if (folderName == "Body")
                ButtonCreator.instance.CreateButton(files[i], 5);
            else if (folderName == "Hands-Feet")
                ButtonCreator.instance.CreateButton(files[i], 6);
        }

        return array;
    }

    public void SetClothingSave(string url, int clothingItem)
    {
        if (clothingItem == 0)
            selectedHatTexture = url;
        else if (clothingItem == 1)
            selectedShirtTexture = url;
        else if (clothingItem == 2)
            selectedPantsTexture = url;
        else if (clothingItem == 3)
            selectedShoesTexture = url;
        else if (clothingItem == 4)
            selectedHeadTexture = url;
        else if (clothingItem == 5)
            selectedBodyTexture = url;
        else if (clothingItem == 6)
            selectedHandTexture = url;
    }

    public void SaveSelectedTextures()
    {
        PlayerPrefs.SetString("hatTextureSave", selectedHatTexture);
        PlayerPrefs.SetString("shirtTextureSave", selectedShirtTexture);
        PlayerPrefs.SetString("pantsTextureSave", selectedPantsTexture);
        PlayerPrefs.SetString("shoesTextureSave", selectedShoesTexture);
        PlayerPrefs.SetString("headTextureSave", selectedHeadTexture);
        PlayerPrefs.SetString("bodyTextureSave", selectedBodyTexture);
        PlayerPrefs.SetString("handTextureSave", selectedHandTexture);
    }

    public void LoadTextures()
    {
        TextureSwapManager.instance.SetTexture(PlayerPrefs.GetString("hatTextureSave", ""), 0);
        TextureSwapManager.instance.SetTexture(PlayerPrefs.GetString("shirtTextureSave", ""), 1);
        TextureSwapManager.instance.SetTexture(PlayerPrefs.GetString("pantsTextureSave", ""), 2);
        TextureSwapManager.instance.SetTexture(PlayerPrefs.GetString("shoesTextureSave", ""), 3);
        TextureSwapManager.instance.SetTexture(PlayerPrefs.GetString("headTextureSave", ""), 4);
        TextureSwapManager.instance.SetTexture(PlayerPrefs.GetString("bodyTextureSave", ""), 5);
        TextureSwapManager.instance.SetTexture(PlayerPrefs.GetString("handTextureSave", ""), 6);
    }
}
