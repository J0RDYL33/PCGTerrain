using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandGen : MonoBehaviour
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
    public GameObject rock;
    public MeshCollider mc;

    // Start is called before the first frame update
    void Start()
    {
        //Create the mesh we'll be working on
        mesh = new Mesh();
        mesh.name = "SandMesh";
        mesh = GetComponent<MeshFilter>().mesh;
        mc = GetComponent<MeshCollider>();

        currentXSize = xSize;

        GenerateMapShape();
        CreateShape();
        SpawnRocks();
        UpdateMesh();
    }

    void GenerateMapShape()
    {
        sizePerRow = new int[zSize+1];
        for (int i = 0; i <= zSize; i++)
        {
            int randy = Random.Range(0, 2);
            if (randy == 1)
            {
                currentXSize -= 1;
            }
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
                    if (sizePerRow[z] == sizePerRow[z+1])
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

    void SpawnRocks()
    {
        int lastRock = 0;
        for (int i = 0; i < zSize-10; i++)
        {
            if ((sizePerRow[i] - sizePerRow[i+4]) >= 3 && lastRock <= 0)
            {
                Instantiate(rock, new Vector3(sizePerRow[i] * 8, -3, (i * 8) - 4), Quaternion.Euler(-90, 0, 0));
                lastRock = 10;
            }
            else
            {
                lastRock -= 1;
            }
        }

        Instantiate(rock, new Vector3(sizePerRow[zSize] * 8, -3, (sizePerRow.Length * 8) - 4), Quaternion.Euler(-90, 0, 0));
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