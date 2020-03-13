using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

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

    public Slider slide;

    private void Awake()
    {
        this.filter = base.GetComponent<MeshFilter>();
        this.baseMesh = new Mesh();
        this.baseMesh.name = "Instanced Tire";
        this.baseMesh.vertices = new Vector3[this.referenceMesh.vertices.Length];
        this.baseMesh.vertices = this.referenceMesh.vertices;
        this.baseMesh.triangles = new int[this.referenceMesh.triangles.Length];
        this.baseMesh.triangles = this.referenceMesh.triangles;
        this.baseMesh.normals = new Vector3[this.referenceMesh.normals.Length];
        this.baseMesh.normals = this.referenceMesh.normals;
        this.baseMesh.tangents = new Vector4[this.referenceMesh.tangents.Length];
        this.baseMesh.tangents = this.referenceMesh.tangents;
        this.baseMesh.uv = new Vector2[this.referenceMesh.uv.Length];
        this.baseMesh.uv = this.referenceMesh.uv;
        this.baseMesh.uv2 = new Vector2[this.referenceMesh.uv2.Length];
        this.baseMesh.uv2 = this.referenceMesh.uv2;
        this.baseMesh.subMeshCount = 2;
        this.baseMesh.SetIndices(this.referenceMesh.GetIndices(0), MeshTopology.Triangles, 0);
        this.baseMesh.SetIndices(this.referenceMesh.GetIndices(1), MeshTopology.Triangles, 1);
        this.vertices = new Vector3[this.baseMesh.vertices.Length];
        this.vertices = this.baseMesh.vertices;
        if (this.debug)
        {
            this.SetMeshBlend(this.perc);
        }
        this.filter.mesh = this.baseMesh;
    }

    public void Update()
    {
        SetMeshBlend(this.perc);
    }

    public void SetMeshBlend(float percent)
    {
        this.perc = Mathf.Clamp01(percent);
        Vector3[] array = this.shape01.vertices;
        Vector3[] array2 = this.shape02.vertices;
        for (int i = 0; i < this.vertices.Length; i++)
        {
            this.vertices[i] = Vector3.Lerp(array[i], array2[i], this.perc);
        }
        this.baseMesh.vertices = this.vertices;
    }

    public void SetMeshBlend()
    {
        this.perc = Mathf.Clamp01(slide.value);
        Vector3[] array = this.shape01.vertices;
        Vector3[] array2 = this.shape02.vertices;
        for (int i = 0; i < this.vertices.Length; i++)
        {
            this.vertices[i] = Vector3.Lerp(array[i], array2[i], this.perc);
        }
        this.baseMesh.vertices = this.vertices;
    }

    public float GetPercent()
    {
        return this.perc;
    }


   
}
