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
    private Color frontTireColor;
    private Color rearTireColor;
    private Color frontRimColor;
    private Color rearRimColor;
    private Color leftGripColor;
    private Color rightGripColor;
    private Color frontTireWallColor;
    private Color rearTireWallColor;

    private Transform[] rightGrips;
    private Transform[] leftGrips;

    private bool chain = false;
    private bool seatPost = false;
    private bool brakes = false;
    private bool cable = false;
    private bool frontTire = false;
    private bool rearTire = false;
    private bool leftGrip = false;
    private bool rightGrip = false;
    private bool frontRim = false;
    private bool rearRim = false;
    private bool frontTireWall = false;
    private bool rearTireWall = false;

    private Transform[] stockFrontWheel;
    private Transform[] stockRearWheel;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        stockFrontWheel = GameObject.Find("BMX:Wheel").GetComponentsInChildren<Transform>();
        stockRearWheel = GameObject.Find("BMX:Wheel 1").GetComponentsInChildren<Transform>();

        this.seatPostCol = GameObject.Find("Seat Post").GetComponent<Renderer>().material.color;
        this.chainColor = GameObject.Find("Chain Mesh").GetComponent<Renderer>().material.color;

        this.frontTireColor = stockFrontWheel[6].gameObject.GetComponent<Renderer>().material.color;
        this.rearTireColor = stockRearWheel[3].gameObject.GetComponent<Renderer>().material.color;

        leftGrips = GameObject.Find("Left Grip").GetComponentsInChildren<Transform>();
        rightGrips = GameObject.Find("Right Grip").GetComponentsInChildren<Transform>();

        leftGripColor = leftGrips[1].gameObject.GetComponent<Renderer>().material.color;
        rightGripColor = rightGrips[1].gameObject.GetComponent<Renderer>().material.color;

        frontRimColor = stockFrontWheel[1].gameObject.GetComponent<Renderer>().material.color;
        rearRimColor = stockRearWheel[1].gameObject.GetComponent<Renderer>().material.color;

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
        frontTire = false;
        rearTire = false;
        leftGrip = false;
        rightGrip = false;
        frontRim = false;
        rearRim = false;
        frontTireWall = false;
        rearTireWall = false;

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
        frontTire = false;
        rearTire = false;
        leftGrip = false;
        rightGrip = false;
        frontRim = false;
        rearRim = false;
        frontTireWall = false;
        rearTireWall = false;
    }

    public void setSeatPostColor()
    {
        this.seatPost = true;
        this.chain = false;
        this.brakes = false;
        cable = false;
        frontTire = false;
        rearTire = false;
        leftGrip = false;
        rightGrip = false;
        frontRim = false;
        rearRim = false;
        frontTireWall = false;
        rearTireWall = false;
    }

    public bool getSeatPostBool()
    {
        return this.seatPost;
    }

    public void setFrontTireColor()
    {
        this.brakes = false;
        this.seatPost = false;
        this.chain = false;
        cable = false;
        frontTire = true;
        rearTire = false;
        leftGrip = false;
        rightGrip = false;
        frontRim = false;
        rearRim = false;
        frontTireWall = false;
        rearTireWall = false;
    }

    public void SetFrontTireColor(float r, float g, float b, float a)
    {
        this.frontTireColor = new Color(r, g, b, a);
        stockFrontWheel[6].gameObject.GetComponent<Renderer>().material.color = frontTireColor;
    }

    public void SetRearTireColor(float r, float g, float b, float a)
    {
        this.rearTireColor = new Color(r, g, b, a);
        stockRearWheel[3].gameObject.GetComponent<Renderer>().material.color = rearTireColor;
    }

    public Color getFrontTireColor()
    {
        return this.frontTireColor;
    }

    public void setRearTireColor()
    {
        this.brakes = false;
        this.seatPost = false;
        this.chain = false;
        cable = false;
        frontTire = false;
        rearTire = true;
        leftGrip = false;
        rightGrip = false;
        frontRim = false;
        rearRim = false;
        frontTireWall = false;
        rearTireWall = false;
    }

    public Color getRearTireColor()
    {
        return this.rearTireColor;
    }

    public void SetBrakesColor()
    {
        this.brakes = true;
        this.seatPost = false;
        this.chain = false;
        cable = false;
        frontTire = false;
        rearTire = false;
        leftGrip = false;
        rightGrip = false;
        frontRim = false;
        rearRim = false;
        frontTireWall = false;
        rearTireWall = false;
    }

    public void SetCableColor()
    {
        cable = true;
        this.brakes = false;
        this.seatPost = false;
        this.chain = false;
        frontTire = false;
        rearTire = false;
        leftGrip = false;
        rightGrip = false;
        frontRim = false;
        rearRim = false;
        frontTireWall = false;
        rearTireWall = false;
    }

    public void SetLeftGripColor()
    {
        cable = false;
        this.brakes = false;
        this.seatPost = false;
        this.chain = false;
        frontTire = false;
        rearTire = false;
        leftGrip = true;
        rightGrip = false;
        frontRim = false;
        rearRim = false;
        frontTireWall = false;
        rearTireWall = false;
    }

    public void SetLeftGripColor(float r, float g, float b, float a)
    {
        this.leftGripColor = new Color(r, g, b, a);
        leftGrips[1].gameObject.GetComponent<Renderer>().material.color = leftGripColor;
        leftGrips[2].gameObject.GetComponent<Renderer>().material.color = leftGripColor;
    }

    public void SetRightGripColor()
    {
        cable = false;
        this.brakes = false;
        this.seatPost = false;
        this.chain = false;
        frontTire = false;
        rearTire = false;
        leftGrip = false;
        rightGrip = true;
        frontRim = false;
        rearRim = false;
        frontTireWall = false;
        rearTireWall = false;
    }

    public void SetRightGripColor(float r, float g, float b, float a)
    {
        this.rightGripColor = new Color(r, g, b, a);
        rightGrips[1].gameObject.GetComponent<Renderer>().material.color = rightGripColor;
        rightGrips[2].gameObject.GetComponent<Renderer>().material.color = rightGripColor;
    }

    public void SetFrontRimColor()
    {
        cable = false;
        this.brakes = false;
        this.seatPost = false;
        this.chain = false;
        frontTire = false;
        rearTire = false;
        leftGrip = false;
        rightGrip = false;
        frontRim = true;
        rearRim = false;
        frontTireWall = false;
        rearTireWall = false;
    }

    public void SetFrontRimColor(float r, float g, float b, float a)
    {
        frontRimColor = new Color(r,g,b,a);
        stockFrontWheel[1].gameObject.GetComponent<Renderer>().material.color = frontRimColor;
    }

    public void SetRearRimColor()
    {
        cable = false;
        this.brakes = false;
        this.seatPost = false;
        this.chain = false;
        frontTire = false;
        rearTire = false;
        leftGrip = false;
        rightGrip = false;
        frontRim = false;
        rearRim = true;
        frontTireWall = false;
        rearTireWall = false;
    }

    public void SetFrontTireWallColor()
    {
        cable = false;
        this.brakes = false;
        this.seatPost = false;
        this.chain = false;
        frontTire = false;
        rearTire = false;
        leftGrip = false;
        rightGrip = false;
        frontRim = false;
        rearRim = false;
        frontTireWall = true;
        rearTireWall = false;
    }

    public void SetRearTireWallColor()
    {
        cable = false;
        this.brakes = false;
        this.seatPost = false;
        this.chain = false;
        frontTire = false;
        rearTire = false;
        leftGrip = false;
        rightGrip = false;
        frontRim = false;
        rearRim = false;
        frontTireWall = false;
        rearTireWall = true;
    }

    public void SetRearTireWallColor(float r, float g, float b, float a)
    {
        rearTireWallColor = new Color(r, g, b, a);
        stockRearWheel[3].gameObject.GetComponent<Renderer>().materials[1].color = rearTireWallColor;
    }

    public void SetFrontTireWallColor(float r, float g, float b, float a)
    {
        frontTireWallColor = new Color(r, g, b, a);
        stockFrontWheel[6].gameObject.GetComponent<Renderer>().materials[1].color = frontTireWallColor;
    }

    public void SetRearRimColor(float r, float g, float b, float a)
    {
        rearRimColor = new Color(r,g,b,a);
        stockRearWheel[1].gameObject.GetComponent<Renderer>().material.color = rearRimColor;
    }

    public Color GetFrontTireWallColor()
    {
        return this.frontTireWallColor;
    }

    public Color GetRearTireWallColor()
    {
        return this.rearTireWallColor;
    }

    public Color GetFrontRimColor()
    {
        return this.frontRimColor;
    }

    public Color GetRearRimColor()
    {
        return this.rearRimColor;
    }

    public Color GetLeftGripColor()
    {
        return this.leftGripColor;
    }

    public Color GetRightGripColor()
    {
        return this.rightGripColor;
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
        else if (this.frontTire == true)
        {
            frontTireColor = col;
            stockFrontWheel[6].gameObject.GetComponent<Renderer>().material.color = frontTireColor;

        }
        else if (this.rearTire == true)
        {
            rearTireColor = col;
            stockRearWheel[3].gameObject.GetComponent<Renderer>().material.color = rearTireColor;
        }
        else if (this.leftGrip == true)
        {
            leftGripColor = col;
            leftGrips[1].gameObject.GetComponent<Renderer>().material.color = leftGripColor;
            leftGrips[2].gameObject.GetComponent<Renderer>().material.color = leftGripColor;
        }
        else if (this.rightGrip == true)
        {
            rightGripColor = col;
            rightGrips[1].gameObject.GetComponent<Renderer>().material.color = rightGripColor;
            rightGrips[2].gameObject.GetComponent<Renderer>().material.color = rightGripColor;
        }
        else if (frontRim == true)
        {
            frontRimColor = col;
            stockFrontWheel[1].gameObject.GetComponent<Renderer>().material.color = frontRimColor;
        }
        else if (rearRim == true)
        {
            rearRimColor = col;
            stockRearWheel[1].gameObject.GetComponent<Renderer>().material.color = rearRimColor;
        }
        else if (frontTireWall == true)
        {
            frontTireWallColor = col;
            stockFrontWheel[6].gameObject.GetComponent<Renderer>().materials[1].color = frontTireWallColor;
        }
        else if (rearTireWall == true)
        {
            rearTireWallColor = col;
            stockRearWheel[3].gameObject.GetComponent<Renderer>().materials[1].color = rearTireWallColor;
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

    public Color GetBrakeCableColor()
    {
        return this.cableColor;
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


