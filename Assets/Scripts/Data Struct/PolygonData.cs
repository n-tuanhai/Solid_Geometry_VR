using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonData : MonoBehaviour
{
    public List<GameObject> pointList;
    public List<LineData> lineList;
    public bool isSelected;
    public Color color;
    public bool isUpdating;

    public enum PolygonType
    {
        Irregular = 0,
        Regular = 1
    }

    public PolygonType polygonType;

    private void OnEnable()
    {
        isSelected = false;
        isUpdating = false;
    }

    private void Start()
    {
        isSelected = false;
        isUpdating = false;
    }

    private void Update()
    {
        if (!isUpdating) return;
        InitMesh();
    }

    public void InitMesh()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("MeshFilter not found!");
            return;
        }
        
        meshFilter.mesh = null;
        
        Mesh mesh = meshFilter.mesh;

        meshFilter.mesh = new Mesh();
        mesh = meshFilter.mesh;
        
        mesh.Clear();

        //Vertices
        var vertex = new Vector3[pointList.Count];

        for (int i = 0; i < pointList.Count; i++)
        {
            vertex[i] = pointList[i].transform.position;
        }

        //UVs
        var uvs = new Vector2[vertex.Length];

        for (int i = 0; i < vertex.Length; i++)
        {
            if ((i % 2) == 0)
            {
                uvs[i] = new Vector2(0, 0);
            }
            else
            {
                uvs[i] = new Vector2(1, 1);
            }
        }

        //Assign data to mesh
        mesh.vertices = vertex;
        mesh.uv = uvs;
        mesh.triangles = Triangulate(vertex);

        //Recalculations
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();

        //Name the mesh
        mesh.name = "MyMesh";
        GetComponent<MeshCollider>().sharedMesh = mesh;
        color = GetComponent<MeshRenderer>().material.color;
        UpdateColor();
    }

    private int[] Triangulate(Vector3[] vertex)
    {
        var tris = new int[3 * (vertex.Length - 2)]; //3 verts per triangle * num triangles
        var c1 = 0;
        var c2 = 1;
        var c3 = 2;

        for (int i = 0; i < tris.Length; i += 3)
        {
            tris[i] = c1;
            tris[i + 1] = c2;
            tris[i + 2] = c3;

            c2++;
            c3++;
        }

        return tris;
    }

    public void PolygonSelectRequestHandler()
    {
        isSelected = !isSelected;
        UpdateColor();
    }

    private void UpdateColor()
    {
        var meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = isSelected ? Color.red : color;
    }
}