using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProceduralMesh : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        MakeMesh();
    }

    // Update is called once per frame
    void Update()
    {
     
    }
    void MakeMesh()
    {
        vertices = new Vector3[]{ new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, .1f), new Vector3(.1f, 0f, 0f)};
        triangles = new int[] { 0, 1, 2, 2, 1, 0};

        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}
