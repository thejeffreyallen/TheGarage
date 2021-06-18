using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

[XmlRoot("BikeSave")]
[XmlInclude(typeof(PartColor))]
public class SaveList
{
    public int frame;
    public int bars;
    public int forks;
    public int sprocket;
    public int stem;
    public int cranks;
    public int frontPegs;
    public int rearPegs;
    public int pedals;
    public int spokes;

    public bool brakes;
    public bool betterWheels;
    public bool flanges;
    public bool LHD;

    public float seatAngle;
    public float barsAngle;
    public float bikeScale;
    public float tireWidth;
    public float seatHeight;

    public int seatID;
    public int gripsID;
    public int treadID;

    public Color seatPostColor;
    public Color chainColor;
    public Color brakesColor;

    public int seatPostMat;

    public List<PartColor> partColors;
    public List<PartMaterial> partMaterials;
    public List<PartTexture> partTextures;

    public SaveList()
    {
        partColors = new List<PartColor>();
        partMaterials = new List<PartMaterial>();
        partTextures = new List<PartTexture>();
    }

}

[XmlType("PartColor")]
public class PartColor
{
    public int partNum;
    public float r;
    public float g;
    public float b;
    public float a;

    public PartColor(int partNum, Color col)
    {
        this.partNum = partNum;
        this.r = col.r;
        this.g = col.g;
        this.b = col.b;
        this.a = col.a;
    }

    public PartColor()
    {

    }
}

[XmlType("PartTexture")]
public class PartTexture
{
    public string url;
    public int partNum;
    public bool normal;
    public bool metallic;

    public PartTexture(string url, int partNum, bool normal, bool metallic)
    {
        this.url = url;
        this.partNum = partNum;
        this.normal = normal;
        this.metallic = metallic;
    }

    public PartTexture()
    {

    }

}

[XmlType("PartMaterial")]
public class PartMaterial
{
    public int matID;
    public int partNum;

    public PartMaterial(int matID, int partNum)
    {
        this.matID = matID;
        this.partNum = partNum;
    }

    public PartMaterial()
    {

    }

}





