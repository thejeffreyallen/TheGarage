using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Class to represent all data of a saved bike preset
/// </summary>
[XmlRoot("BikeSave")]
[XmlInclude(typeof(PartColor))]
public class SaveList
{
    //TODO
    /*
    public int frontHubGuards;
    public int rearHubGuards;
    
    public int frontSpokeAccessories;
    public int rearSpokeAccessories;
    */

    public bool hasFrontHubChanged;
    public bool hasRearHubChanged;
    public bool brakes;
    public bool betterWheels;
    public bool flanges;
    public bool LHD;

    public float seatAngle;
    public float barsAngle;
    public float bikeScale;
    public float frontTireWidth;
    public float rearTireWidth;
    public float seatHeight;

    public int seatID;
    public int gripsID;
    public int treadID;

    public int seatPostMat;
    public int frontTireMat;
    public int rearTireMat;
    public int frontTireWallMat;
    public int rearTireWallMat;
    public int frontRimMat;
    public int rearRimMat;
    public int frontHubMat;
    public int rearHubMat;
    public int frontSpokesMat;
    public int rearSpokesMat;
    public int frontNipplesMat;
    public int rearNipplesMat;

    public List<PartMesh> partMeshes;
    public List<PartColor> partColors;
    public List<PartMaterial> partMaterials;
    public List<PartTexture> partTextures;

    public SaveList()
    {
        partMeshes = new List<PartMesh>();
        partColors = new List<PartColor>();
        partMaterials = new List<PartMaterial>();
        partTextures = new List<PartTexture>();
    }

}

/// <summary>
/// Class to represent part number and color associated with the bike part
/// </summary>
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
        r = col.r;
        g = col.g;
        b = col.b;
        a = col.a;
    }

    public PartColor()
    {

    }
}

/// <summary>
/// Class to represent part number, texture url, and whether the texture is main, normal or metallic
/// </summary>
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

/// <summary>
/// Class to represent a part number and associated material
/// </summary>
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

[XmlType("PartMesh")]
public class PartMesh
{
    public int partNum;
    public bool isCustom;
    public string fileName;
    public string partName;

    public PartMesh(int partNum, bool isCustom, string fileName, string partName)
    {
        this.partNum = partNum;
        this.isCustom = isCustom;
        this.fileName = fileName;
        this.partName = partName;
    }

    public PartMesh()
    {

    }

}





