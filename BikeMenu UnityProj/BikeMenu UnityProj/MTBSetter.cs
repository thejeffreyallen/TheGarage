using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MTBSetter : MonoBehaviour
{
    Transform bike;

    bool active = false;

    List<Mesh> defaultMeshes = new List<Mesh>();
    public List<Mesh> mtbMeshes;

    void Update()
    {
        if (bike == null)
            bike = GameObject.Find("BMX").transform;
    }

    public void SetMTB()
    {
        active = !active;

        if (active)
            EnableMTB();
        else
            DisableMTB();
    }

    void EnableMTB()
    {
        SwapMesh("Bars Mesh", mtbMeshes[0]);
    }

    void DisableMTB()
    {
        SwapMesh("Bars Mesh", defaultMeshes[0]);
    }

    void SwapMesh(string partName, Mesh customMesh)
    {
        Mesh part = GameObject.Find(partName).GetComponent<MeshFilter>().mesh;
        defaultMeshes.Add(part);

        GameObject.Find(partName).GetComponent<MeshFilter>().mesh = customMesh;
    }
}
