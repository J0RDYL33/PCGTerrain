using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGen : MonoBehaviour
{
    public Vector3[] vertices;
    bool[] waterUp;
    int[] triangles;
    Mesh mesh;
    bool wave = true;
    public int[] sizePerRow;
    public int totalVertices;
    public int xSize = 20;
    public int zSize = 20;
    int fullWaterRows;
    float[] waterHeights;
    LandGen myLandGen;

    // Start is called before the first frame update
    void Start()
    {
        //Create the mesh
        mesh = new Mesh();
        mesh = GetComponent<MeshFilter>().mesh;

        myLandGen = FindObjectOfType<LandGen>();

        SetWaterSize();
        CreateShape();
        UpdateMesh();
        //StartCoroutine(WaterWaves());
    }

    //Size is the same for the difference between sea and land
    //After, it's sea - land
    void SetWaterSize()
    {
        sizePerRow = new int[zSize + 2];
        for (int i = 0; i <= (zSize - myLandGen.zSize); i++)
        {
            sizePerRow[i] = xSize;
            fullWaterRows++;
            totalVertices += sizePerRow[i] + 1;
        }

        for (int i = 0; i < myLandGen.zSize; i++)
        {
            sizePerRow[i + fullWaterRows+1] = xSize - myLandGen.sizePerRow[myLandGen.xSize - i];
            totalVertices += sizePerRow[fullWaterRows + i+1] + 1;
        }
    }

    void CreateShape()
    {
        vertices = new Vector3[totalVertices];
        waterUp = new bool[vertices.Length];
        waterHeights = new float[vertices.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= sizePerRow[z]; x++)
            {
                float randY = Random.Range(0.8f, 1.2f);
                float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
                if (x != sizePerRow[z])
                    vertices[i] = new Vector3(x * 8, y * randY, z * 8);
                else
                    vertices[i] = new Vector3(x * 8, 0, z * 8);
                waterHeights[i] = y;
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
                    }
                    else
                    {
                        triangles[tris + 0] = vert + 0;
                        triangles[tris + 1] = vert + sizePerRow[z] + 1;
                        triangles[tris + 2] = vert + 1;
                    }
                }
                vert++;
                tris += 6;
                UpdateMesh();
                //yield return new WaitForSeconds(0.001f);
            }
            vert++;
        }
        UpdateMesh();
    }

    //0.7 - 2.5
    IEnumerator WaterWaves()
    {
        while (wave == true)
        {
            for (int i = 0; i < waterHeights.Length; i++)
            {
                //Decide if vertices should go up or down
                if (vertices[i].y < 0.7)
                {
                    waterUp[i] = true;
                }
                else if (vertices[i].y > 1.2)
                {
                    waterUp[i] = false;
                }
                //Add/minus value from that
                if (waterUp[i] == true)
                {
                    vertices[i].y += 0.05f;
                }
                else
                {
                    vertices[i].y -= 0.05f;
                }
            }
            UpdateMesh();
            yield return new WaitForSeconds(0.075f);
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.Optimize();
    }

    //private void OnDrawGizmos()
    //{
    //    if (vertices == null)
    //        return;

    //    for (int i = 0; i < vertices.Length; i++)
    //    {
    //        Gizmos.DrawSphere(vertices[i], .1f);
    //    }
    //}
}
