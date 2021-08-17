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

    private List<int> activeParts; // for multi part editing
    public int currentPart;

    public List<Toggle> toggleList;
    public List<Toggle> infoToggles;

    public Slider slider;

    public Text selectedPartText;
    Color col;
    Color brakeColor = Color.black;
    Color cableColor = Color.black;
    public bool rotationOn = false;
    public GameObject rotationIndicator;
    private bool active = false;
    //Dictionary<int, Stack<Vector3>> history = new Dictionary<int, Stack<Vector3>>();

    void Awake()
    {
        instance = this;
        activeParts = new List<int>();
        foreach (Toggle t in infoToggles)
        {
            if (PlayerPrefs.GetInt(t.gameObject.name) == 0)
            {
                t.gameObject.SetActive(true);
            }
            else
            {
                t.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Toggle a part on or off
    /// </summary>
    /// <param name="toggle"></param>
    public void ToggleButton(Toggle toggle) {
        Graphic g = toggle.targetGraphic;
        Text t = g.gameObject.GetComponentInChildren<Text>();
        if (toggle.isOn)
        {
            g.color = Color.white;
            t.color = Color.black;
        }
        else {
            g.color = new Color(0f, 0f, 0f, 0.7333f);
            t.color = Color.white;
        }
    }

    public void DontShowAgain(Toggle _info)
    {
        
        if(!_info.isOn)
            PlayerPrefs.SetInt(_info.gameObject.name, 0);
        else
            PlayerPrefs.SetInt(_info.gameObject.name, 1);
    }

    /// <summary>
    /// Deselect all selected parts
    /// </summary>
    public void DeselectAll()
    {
        foreach (Toggle toggle in toggleList)
        {
            toggle.isOn = false;
            Graphic g = toggle.targetGraphic;
            Text t = g.gameObject.GetComponentInChildren<Text>();
            g.color = new Color(0f, 0f, 0f, 0.7333f);
            t.color = Color.white;
        }
        ClearActiveParts();
    }

    public List<int> GetActivePartList()
    {
        return this.activeParts;
    }

    void OnEnable()
    {
       
    }

    public void TogglePartActive(int part)
    {
        if (activeParts.Contains(part))
            RemoveActivePart(part);
        else
            SetPartActive(part);
    }

    public void SetCurrentPartActive()
    {
        activeParts.Add(currentPart);
    }

    public void SetPartActive(int part)
    {
        if (activeParts.Contains(part))
            RemoveActivePart(part);
        else
            activeParts.Add(part);
    }

    public void RemoveActivePart(int part)
    {
        activeParts.Remove(part);
    }

    public void ClearActiveParts()
    {
        activeParts.Clear();
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
        SetPartActive(part);
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

    public void ToggleRotation()
    {
        rotationIndicator.SetActive(!rotationIndicator.activeInHierarchy);
        rotationOn = !rotationOn;
    }

    //TODO Add color options for bar/frame and wheel accessories
    void Update()
    {
        if (IsActive() && MenuManager.instance.colourMenu.activeInHierarchy)
        {
            float hue, sat, bright;

            col = ColourPicker.instance.value;
            Color.RGBToHSV(col, out hue, out sat, out bright);
            col = Color.HSVToRGB(hue, sat, slider.value);
            foreach (int key in activeParts)
            {
                try
                {
                    SetColor(key, col);
                }
                catch (Exception e)
                {
                    Debug.Log("Error setting color " + e.Message + e.StackTrace);
                }
            }
            
        }
        if (MenuManager.instance.experimentalMenu.activeInHierarchy)
        {
            if (Input.GetKeyUp(KeyCode.LeftShift))
                ToggleRotation();
            foreach (int key in activeParts)
            {
                
                if (Input.GetKey(KeyCode.Keypad8))
                {
                    if(rotationOn)
                        PartMaster.instance.RotatePart(key, "y", 0.1f);
                    else
                        PartMaster.instance.MovePart(key, "y", 0.001f);
                }
                if (Input.GetKey(KeyCode.Keypad2))
                {
                    if (rotationOn)
                        PartMaster.instance.RotatePart(key, "y", -0.1f);
                    else
                        PartMaster.instance.MovePart(key, "y", -0.001f);
                }
                if (Input.GetKey(KeyCode.Keypad4))
                {
                    if (rotationOn)
                        PartMaster.instance.RotatePart(key, "x", 0.1f);
                    else
                        PartMaster.instance.MovePart(key, "z", 0.001f);
                }
                if (Input.GetKey(KeyCode.Keypad6))
                {
                    if (rotationOn)
                        PartMaster.instance.RotatePart(key, "x", -0.1f);
                    else
                        PartMaster.instance.MovePart(key, "z", -0.001f);
                }
                if (Input.GetKey(KeyCode.Keypad7))
                {
                    if (rotationOn)
                        PartMaster.instance.RotatePart(key, "z", 0.1f);
                    else
                        PartMaster.instance.MovePart(key, "x", 0.001f);
                }
                if (Input.GetKey(KeyCode.Keypad9))
                {
                    if (rotationOn)
                        PartMaster.instance.RotatePart(key, "z", -0.1f);
                    else
                        PartMaster.instance.MovePart(key, "x", -0.001f);
                }

                if (Input.GetKey(KeyCode.KeypadPlus))
                    PartMaster.instance.Scale(key, true, 0.001f);
                if(Input.GetKey(KeyCode.KeypadMinus))
                    PartMaster.instance.Scale(key, false, 0.001f);
            }
        }
        /*
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            PlayerPrefs.DeleteKey("ShowAgain");
        }
        */
    }

    public void SetColor(int key, Color c)
    {
       // Debug.Log("Setting Color " + c + " on part number: " + key);
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
        if(key >= 0 && key < PartMaster.instance.partList.Count)
            PartMaster.instance.GetMaterial(key).color = c;
    }

    public void MovePart(string axis)
    {
        foreach (int key in activeParts)
        {
            if(axis.Equals("+y"))
                PartMaster.instance.MovePart(key, "y", 0.01f);
            if (axis.Equals("-y"))
                PartMaster.instance.MovePart(key, "y", -0.01f);
            if (axis.Equals("+x"))
                PartMaster.instance.MovePart(key, "x", 0.01f);
            if (axis.Equals("-x"))
                PartMaster.instance.MovePart(key, "x", -0.01f);
            if (axis.Equals("+z"))
                PartMaster.instance.MovePart(key, "z", 0.01f);
            if (axis.Equals("-z"))
                PartMaster.instance.MovePart(key, "z", -0.01f);
        }
            
    }
}


