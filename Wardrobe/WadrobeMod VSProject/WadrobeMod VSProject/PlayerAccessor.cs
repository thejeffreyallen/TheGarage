using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerAccessor : MonoBehaviour
{
    public static PlayerAccessor instance;

    public UnityEngine.Material newShirtMat;

    List<Material> clothingMats = new List<Material>();

    public enum PlayerClothing
    {
        hat,
        shirt,
        pants,
        shoes,
        head,
        body,
        hand
    }

    void Start()
    {
        instance = this;

        FindMaterials();
        SetTransparent();
    }

    void FindMaterials()
    {
        clothingMats.Add(GameObject.Find("Baseball Cap_R").GetComponent<Renderer>().material);

        GetRenderer(PlayerClothing.shirt).material = newShirtMat;
        clothingMats.Add(GameObject.Find("shirt_geo").GetComponent<Renderer>().material);

        clothingMats.Add(GameObject.Find("pants_geo").GetComponent<Renderer>().material);
        clothingMats.Add(GameObject.Find("shoes_geo").GetComponent<Renderer>().material);
        clothingMats.Add(GameObject.Find("body_geo").GetComponent<Renderer>().materials[0]);
        clothingMats.Add(GameObject.Find("body_geo").GetComponent<Renderer>().materials[1]);
        clothingMats.Add(GameObject.Find("body_geo").GetComponent<Renderer>().materials[2]);
    }

    public void SetTransparent()
    {
        //shirt
        clothingMats[1].EnableKeyword("_ALPHATEST_ON");

        //pants
        clothingMats[2].EnableKeyword("_ALPHATEST_ON");
    }

    public Material GetClothingMaterial(PlayerClothing playerClothing)
    {
        switch (playerClothing)
        {
            case PlayerClothing.hat:
                clothingMats[0].color = Color.white;
                return clothingMats[0];
            case PlayerClothing.shirt:
                return clothingMats[1];
            case PlayerClothing.pants:
                return clothingMats[2];
            case PlayerClothing.shoes:
                return clothingMats[3];
            case PlayerClothing.head:
                return clothingMats[4];
            case PlayerClothing.body:
                return clothingMats[5];
            case PlayerClothing.hand:
                return clothingMats[6];
        }

        return null;
    }

    public Renderer GetRenderer(PlayerClothing playerClothing)
    {
        switch (playerClothing)
        {
            case PlayerClothing.hat:
                clothingMats[0].color = Color.white;
                return GameObject.Find("Baseball Cap_R").GetComponent<Renderer>();
            case PlayerClothing.shirt:
                return GameObject.Find("shirt_geo").GetComponent<Renderer>();
            case PlayerClothing.pants:
                return GameObject.Find("pants_geo").GetComponent<Renderer>();
            case PlayerClothing.shoes:
                return GameObject.Find("shoes_geo").GetComponent<Renderer>();
            case PlayerClothing.head:
                return GameObject.Find("body_geo").GetComponent<Renderer>();
            case PlayerClothing.body:
                return GameObject.Find("body_geo").GetComponent<Renderer>();
            case PlayerClothing.hand:
                return GameObject.Find("body_geo").GetComponent<Renderer>();
        }

        return null;
    }
}
