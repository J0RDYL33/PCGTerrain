using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassGen : MonoBehaviour
{
    public Vector3[] vertices;
    public int[] sizePerRow;
    int currentXSize;
    public int totalVertices;
    int[] triangles;
    Mesh mesh;
    Vector2[] uvs;
    MeshFilter mf;
    public int xSize = 20;
    public int zSize = 20;
    public bool[] towerEligible;
    public int[] endOfLine;
    LandGen myLandGen;

    // Start is called before the first frame update
    void Start()
    {
        //Create the mesh we'll be working on
        mesh = new Mesh();
        mesh = GetComponent<MeshFilter>().mesh;
        myLandGen = FindObjectOfType<LandGen>();
        currentXSize = xSize;

        GenerateMapShape();
        CreateShape();
        UpdateMesh();
    }

    void GenerateMapShape()
    {
        sizePerRow = new int[zSize + 1];
        for (int i = 0; i <= zSize; i++)
        {
            currentXSize = myLandGen.sizePerRow[i] - 10;
            sizePerRow[i] = currentXSize;
            totalVertices += currentXSize + 1;
        }
        totalVertices += currentXSize + 1;
        sizePerRow[sizePerRow.Length - 1] = sizePerRow[sizePerRow.Length - 2] - 1;
    }

    void CreateShape()
    {
        vertices = new Vector3[totalVertices];
        uvs = new Vector2[totalVertices];
        towerEligible = new bool[zSize + 1]/*sizePerRow[sizePerRow.Length]]*/;
        endOfLine = new int[zSize + 1];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= sizePerRow[z]; x++)
            {
                float randY = Random.Range(0.8f, 1.2f);
                float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
                if (z == zSize || z == zSize - 1)
                {
                    vertices[i] = new Vector3(x * 8, 0, z * 8);
                    uvs[i] = new Vector2(x * 0.25f, z * 0.25f);
                }
                else if (x != sizePerRow[z] && x != sizePerRow[z] - 1)
                {
                    vertices[i] = new Vector3(x * 8, y * randY, z * 8);
                    uvs[i] = new Vector2(x * 0.25f, z * 0.25f);
                }
                else
                {
                    vertices[i] = new Vector3(x * 8, 0, z * 8);
                    uvs[i] = new Vector2(x * 0.25f, z * 0.25f);
                }
                endOfLine[z] = i;
                i++;
            }
        }
        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < sizePerRow[z]; x++)
            {
                if (x != sizePerRow[z] - 1)
                {
                    triangles[tris + 0] = vert + 0;
                    triangles[tris + 1] = vert + sizePerRow[z] + 1;
                    triangles[tris + 2] = vert + 1;
                    triangles[tris + 3] = vert + 1;
                    triangles[tris + 4] = vert + sizePerRow[z] + 1;
                    triangles[tris + 5] = vert + sizePerRow[z] + 2;
                }

                if (x == sizePerRow[z] - 1)
                {
                    if (sizePerRow[z] == sizePerRow[z + 1])
                    {
                        triangles[tris + 0] = vert + 0;
                        triangles[tris + 1] = vert + sizePerRow[z] + 1;
                        triangles[tris + 2] = vert + 1;
                        triangles[tris + 3] = vert + 1;
                        triangles[tris + 4] = vert + sizePerRow[z] + 1;
                        triangles[tris + 5] = vert + sizePerRow[z] + 2;
                        towerEligible[z] = true;
                    }
                    else
                    {
                        triangles[tris + 0] = vert + 0;
                        triangles[tris + 1] = vert + sizePerRow[z] + 1;
                        triangles[tris + 2] = vert + 1;
                        towerEligible[z] = false;
                    }
                }

                vert++;
                tris += 6;
                UpdateMesh();
            }
            vert++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals();

        mf = GetComponent<MeshFilter>();
        mf.sharedMesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = null;
        GetComponent<MeshCollider>().sharedMesh = mf.mesh;
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
            return;

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }
}
