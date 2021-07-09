using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColourSetter : MonoBehaviour
{
    public static ColourSetter instance;
    public int currentPart = 0;

    public Slider slider;

    public Text selectedPartText;
    Color col;
    Color brakeColor = Color.black;
    Color cableColor = Color.black;

    private bool active = false;

    void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        SetCurrentPart(0);
    }

    public Color GetBrakeColor()
    {
        return brakeColor;
    }

    public Color GetCableColor()
    {
        return cableColor;
    }

    public void SetCurrentPart(int part)
    {
        currentPart = part;
    }

    public void SetText(String txt)
    {
        selectedPartText.text = txt;
    }

    public void SetActive(bool isActive)
    {
        active = isActive;
        
    }

    public bool IsActive()
    {
        return active;
    }

    void Update()
    {
        if (IsActive())
        {
            float hue, sat, bright;

            col = ColourPicker.instance.value;
            Color.RGBToHSV(col, out hue, out sat, out bright);
            col = Color.HSVToRGB(hue, sat, slider.value);

            try
            {
                SetColor(currentPart, col);
            }
            catch (Exception e)
            {
                Debug.Log("Error setting color " + e.Message + e.StackTrace);
            }
        }
    }

    public void SetColor(int key, Color c)
    {
        Debug.Log("Setting Color " + c + " on part number: " + key);
        switch (key)
        {
            case -1:
                PartMaster.instance.GetMaterials(PartMaster.instance.frontTire)[1].color = c;
                return;
            case -2:
                PartMaster.instance.GetMaterials(PartMaster.instance.rearTire)[1].color = c;
                return;
            case -3:
                if (BrakesManager.instance.IsEnabled())
                {
                    BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[2].color = c;
                    BrakesManager.instance.GetBarBrakes().GetComponent<Renderer>().materials[1].color = c;
                    brakeColor = c;
                }
                return;
            case -4:
                if (BrakesManager.instance.IsEnabled())
                {
                    BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[3].color = c;
                    BrakesManager.instance.GetBarBrakes().GetComponent<Renderer>().materials[2].color = c;
                    cableColor = c;
                }
                return;
        }
        if(key >= 0 && key < 44)
            PartMaster.instance.GetMaterial(key).color = c;
    }
}


