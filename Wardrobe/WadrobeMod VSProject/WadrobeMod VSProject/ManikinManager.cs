using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ManikinManager : MonoBehaviour
{
    public static ManikinManager instance;

    public Material[] clothingMats;

    void Start()
    {
        instance = this;
    }

    void OnEnable()
    {
        CopyPlayerTextures();

        //Make them transparent
        for (int i = 0; i < clothingMats.Length; i++)
        {
            clothingMats[i].EnableKeyword("_ALPHATEST_ON");
        }
    }

    void OnMouseDrag()
    {
        transform.Rotate(Vector3.up * -Input.GetAxis("Mouse X"));
    }

    public void SetClothingTexture(int arrayNum, Texture texture)
    {
        clothingMats[arrayNum].mainTexture = texture;
    }

    public void CopyPlayerTextures()
    {
        for (int i = 0; i < clothingMats.Length; i++)
        {
            clothingMats[i].mainTexture = PlayerAccessor.instance.GetClothingMaterial((PlayerAccessor.PlayerClothing)i).mainTexture;
        }
    }
}
