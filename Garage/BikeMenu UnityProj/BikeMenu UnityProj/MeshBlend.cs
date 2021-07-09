using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Class to blend two meshes together
/// </summary>
public class MeshBlend : MonoBehaviour
{
    public bool debug;
    [Range(0f, 1f)]
    public float perc;
    public Mesh shape01;
    public Mesh shape02;
    public Mesh referenceMesh;
    private Mesh baseMesh;
    private Vector3[] vertices;
    private MeshFilter filter;

    private void Awake()
    {
        filter = base.GetComponent<MeshFilter>();
        baseMesh = new Mesh();
        baseMesh.name = "Instanced Tire";
        baseMesh.vertices = new Vector3[referenceMesh.vertices.Length];
        baseMesh.vertices = referenceMesh.vertices;
        baseMesh.triangles = new int[referenceMesh.triangles.Length];
        baseMesh.triangles = referenceMesh.triangles;
        baseMesh.normals = new Vector3[referenceMesh.normals.Length];
        baseMesh.normals = referenceMesh.normals;
        baseMesh.tangents = new Vector4[referenceMesh.tangents.Length];
        baseMesh.tangents = referenceMesh.tangents;
        baseMesh.uv = new Vector2[referenceMesh.uv.Length];
        baseMesh.uv = referenceMesh.uv;
        baseMesh.uv2 = new Vector2[referenceMesh.uv2.Length];
        baseMesh.uv2 = referenceMesh.uv2;
        baseMesh.subMeshCount = 2;
        baseMesh.SetIndices(referenceMesh.GetIndices(0), MeshTopology.Triangles, 0);
        baseMesh.SetIndices(referenceMesh.GetIndices(1), MeshTopology.Triangles, 1);
        vertices = new Vector3[baseMesh.vertices.Length];
        vertices = baseMesh.vertices;
        if (debug)
        {
            SetMeshBlend(perc);
        }
        filter.mesh = baseMesh;
    }


    public void SetMeshBlend(float percent)
    {
        perc = Mathf.Clamp01(percent);
        Vector3[] array = shape01.vertices;
        Vector3[] array2 = shape02.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = Vector3.Lerp(array[i], array2[i], perc);
        }
        baseMesh.vertices = vertices;
    }


    public float GetPercent()
    {
        return perc;
    }


   
}
