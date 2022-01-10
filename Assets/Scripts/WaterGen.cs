using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGen : MonoBehaviour
{
    public Vector3[] vertices;
    public bool[] movableVertices;
    public int[] sizePerRow;
    int currentXSize;
    public int totalVertices;
    public int[] triangles;
    Mesh mesh;
    Vector2[] uvs;
    MeshFilter mf;
    public int xSize = 20;
    public int zSize = 20;
    LandGen myLandGen;
    bool[] waterUp;
    float[] waterHeights;
    bool wave = true;

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
        StartCoroutine(WaterWaves());
    }

    void GenerateMapShape()
    {
        sizePerRow = new int[zSize + 1];
        for (int i = 0; i <= myLandGen.zSize; i++)
        {
            sizePerRow[i] = xSize - myLandGen.sizePerRow[i];
            totalVertices += sizePerRow[i] + 1;
        }

        for (int i = myLandGen.zSize; i <= zSize; i++)
        {
            sizePerRow[i] = xSize;
            totalVertices += sizePerRow[i] + 1;
        }
        //for (int i = 0; i <= zSize; i++)
        //{
        //    int randy = Random.Range(0, 2);
        //    if (randy == 1)
        //    {
        //        currentXSize -= 1;
        //    }
        //    sizePerRow[i] = currentXSize;
        //    totalVertices += currentXSize + 1;
        //}
        //totalVertices += currentXSize + 1;
    }

    void CreateShape()
    {
        vertices = new Vector3[totalVertices];
        uvs = new Vector2[totalVertices];
        movableVertices = new bool[totalVertices];
        waterUp = new bool[vertices.Length];
        waterHeights = new float[vertices.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= sizePerRow[z]; x++)
            {
                float randY = Random.Range(0.8f, 1.0f);
                float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
                if (z == myLandGen.zSize && x >= (zSize - myLandGen.sizePerRow[z]))
                {
                    vertices[i] = new Vector3(-x * 8, 0, z * 8);
                    movableVertices[i] = false;
                    uvs[i] = new Vector2(x * 0.1f, z * 0.1f);
                }
                else if (x != sizePerRow[z])
                {
                    vertices[i] = new Vector3(-x * 8, y * randY, z * 8);
                    movableVertices[i] = true;
                    uvs[i] = new Vector2(x * 0.1f, z * 0.1f);
                }
                else
                {
                    vertices[i] = new Vector3(-x * 8, 0, z * 8);
                    movableVertices[i] = false;
                    uvs[i] = new Vector2(x * 0.1f, z * 0.1f);
                }
                i++;
            }
        }
        //Calculate how many extra triangles are needed
        int additionalTris = 0;
        for (int i = 0; i < myLandGen.zSize; i++)
        {
            if (sizePerRow[i] != sizePerRow[i+1])
            {
                additionalTris += 3;
            }
        }
        triangles = new int[(totalVertices * 6)];

        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < sizePerRow[z]; x++)
            {
                //triangles[tris + 0] = vert + 0;
                //triangles[tris + 1] = vert + 1;
                //triangles[tris + 2] = vert + sizePerRow[z] + 1;
                //triangles[tris + 3] = vert + 1;
                //triangles[tris + 4] = vert + sizePerRow[z] + 2;
                //triangles[tris + 5] = vert + sizePerRow[z] + 1;
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + sizePerRow[z] + 2;
                triangles[tris + 2] = vert + sizePerRow[z] + 1;
                triangles[tris + 3] = vert + 0;
                triangles[tris + 4] = vert + 1;
                triangles[tris + 5] = vert + sizePerRow[z] + 2;

                UpdateMesh();
                if (x == sizePerRow[z] -1)
                {
                    if (sizePerRow[z] != sizePerRow[z + 1] && z != myLandGen.zSize-1)
                    {
                        triangles[tris + 6] = vert + 1;
                        triangles[tris + 7] = vert + sizePerRow[z] + 3;
                        triangles[tris + 8] = vert + sizePerRow[z] + 2;
                        tris += 3;
                    }
                    else if (z == myLandGen.zSize-1 && myLandGen.sizePerRow[z] == myLandGen.sizePerRow[z+1]+1)
                    {
                        triangles[tris + 6] = vert + 1;
                        triangles[tris + 7] = vert + sizePerRow[z] + 3;
                        triangles[tris + 8] = vert + sizePerRow[z] + 2;
                        tris += 3;
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

    IEnumerator WaterWaves()
    {
        while (wave == true)
        {
            for (int i = 0; i < waterHeights.Length; i++)
            {
                //Decide if vertices should go up or down
                if (movableVertices[i] == true)
                {
                    if (vertices[i].y < -1.4f)
                    {
                        waterUp[i] = true;
                    }
                    else if (vertices[i].y > 0.0f)
                    {
                        waterUp[i] = false;
                    }
                    //Add/minus value from that
                    if (waterUp[i] == true)
                    {
                        vertices[i].y += 0.1f;
                    }
                    else
                    {
                        vertices[i].y -= 0.1f;
                    }
                }
            }
            UpdateMesh();
            yield return new WaitForSeconds(0.1f);
        }
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
