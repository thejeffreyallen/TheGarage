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
    public int currentPart;

    public Slider slider;

    public Text selectedPartText;

    private Color seatPostCol;
    private Color chainColor;
    private Color barBrakesColor;
    private Color frameBrakesColor;
    private Color cableColor;

    private bool chain = false;
    private bool seatPost = false;
    private bool brakes = false;
    private bool cable = true;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        this.seatPostCol = GameObject.Find("Seat Post").GetComponent<Renderer>().material.color;
        this.chainColor = GameObject.Find("Chain Mesh").GetComponent<Renderer>().material.color;
        this.barBrakesColor = BrakesManager.instance.GetBarBrakes().GetComponent<Renderer>().materials[1].color;
        this.frameBrakesColor = BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[2].color;
        this.cableColor = BrakesManager.instance.GetBarBrakes().GetComponent<Renderer>().materials[2].color;
    }

    void OnEnable()
    {
        SetCurrentPart(0);
    }

    public void SetCurrentPart(int part)
    {
        currentPart = part;
        chain = false;
        seatPost = false;
        brakes = false;
        cable = false;
    }

    public void SetText(String txt)
    {
        selectedPartText.text = txt;
    }

    public void setChainColor()
    {
        this.chain = true;
        this.seatPost = false;
        this.brakes = false;
        cable = false;
    }

    public void setSeatPostColor()
    {
        this.seatPost = true;
        this.chain = false;
        this.brakes = false;
        cable = false;
    }

    public bool getSeatPostBool()
    {
        return this.seatPost;
    }

    public void SetBrakesColor()
    {
        this.brakes = true;
        this.seatPost = false;
        this.chain = false;
        cable = false;
    }

    public void SetCableColor()
    {
        cable = true;
        this.brakes = false;
        this.seatPost = false;
        this.chain = false;   
    }

    public Color GetCableColor()
    {
        return this.cableColor;
    }

    public bool GetBrakesBool()
    {
        return this.brakes;
    }

    void Update()
    {
        float hue, sat, bright;

        Color col = FindObjectOfType<ColourPicker>().value;
        Color.RGBToHSV(col, out hue, out sat, out bright);
        col = Color.HSVToRGB(hue, sat, slider.value);
        if (this.chain == true)
        {
            chainColor = col;
            GameObject.Find("Chain Mesh").GetComponent<Renderer>().material.color = chainColor;
        }
        else if (this.seatPost == true)
        {
            seatPostCol = col;
            GameObject.Find("Seat Post").GetComponent<Renderer>().material.color = seatPostCol;
        }
        else if (this.brakes == true)
        {
            barBrakesColor = col;
            frameBrakesColor = col;
            BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[2].color = frameBrakesColor;
            BrakesManager.instance.GetBarBrakes().GetComponent<Renderer>().materials[1].color = barBrakesColor;
        }
        else if (this.cable == true)
        {
            cableColor = col;
            BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[3].color = cableColor;
            BrakesManager.instance.GetBarBrakes().GetComponent<Renderer>().materials[2].color = cableColor;
        }
        else
        {
            FindObjectOfType<BikeLoadOut>().SetColor(col, currentPart);
        }
    }

    public Color GetSeatPostColor()
    {
        return this.seatPostCol;
    }

    public void SetSeatPostColor(float r, float g, float b, float a)
    {
        this.seatPostCol = new Color(r, g, b, a);
        GameObject.Find("Seat Post").GetComponent<Renderer>().material.color = seatPostCol;
    }

    public Color GetChainColor()
    {
        return this.chainColor;
    }

    public void SetChainColor(float r, float g, float b, float a)
    {
        this.chainColor = new Color(r, g, b, a);
        GameObject.Find("Chain Mesh").GetComponent<Renderer>().material.color = chainColor;
    }

    public Color GetBrakesColor()
    {
        return this.barBrakesColor;
    }

    public void SetBrakesColor(float r, float g, float b, float a)
    {
        this.barBrakesColor = new Color(r, g, b, a);
        this.frameBrakesColor = new Color(r, g, b, a);
        BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[2].color = frameBrakesColor;
        BrakesManager.instance.GetBarBrakes().GetComponent<Renderer>().materials[1].color = barBrakesColor;
    }

    public void SetCableColor(float r, float g, float b, float a)
    {
        this.cableColor = new Color(r, g, b, a);
        BrakesManager.instance.GetFrameBrakes().GetComponent<Renderer>().materials[3].color = cableColor;
        BrakesManager.instance.GetBarBrakes().GetComponent<Renderer>().materials[2].color = cableColor;
    }
}


