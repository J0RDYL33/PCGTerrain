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
    { //Called in the start function, randomly generates the shape of the map
        //zSize is determined in the Editor, can be any size but high numbers will cause lag
        sizePerRow = new int[zSize + 1];
        //Loop over the code zSize number of times randomly keeping the next row the same size or one size less
        for (int i = 0; i <= zSize; i++)
        {
            int randy = Random.Range(0, 2);
            if (randy == 1)
            {
                currentXSize -= 1;
            }
            //Save the length of that row to its coresponding number in sizePerRow, then add the number of vertices to the total count
            sizePerRow[i] = currentXSize;
            totalVertices += currentXSize + 1;
        }
        //Add an extra row of vertices to close it off at the top of the map, then ensure the last row is less than the one before for a curved edge
        totalVertices += currentXSize + 1;
        sizePerRow[sizePerRow.Length - 1] = sizePerRow[sizePerRow.Length - 2] - 1;
    }

    void CreateShape()
    { //Called in the start function, initializes all variables of the map
        //Initialize vertices and uvs with the totalVertices worked out in the previous function, then initialize towerEligible and endOfLine with the size of the map
        vertices = new Vector3[totalVertices];
        uvs = new Vector2[totalVertices];
        towerEligible = new bool[zSize + 1];
        //Nested loop which assigns all vertices to their position worked out in the previous function, while also applying perlin noise to the height on the y axis
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= sizePerRow[z]; x++)
            {
                float randY = Random.Range(0.8f, 1.2f);
                float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
                //If z is at the last row, assign the height as 0 so the land and water match up, leaving no gap
                if (z == zSize || z == zSize - 1)
                {
                    vertices[i] = new Vector3(x * 8, 0, z * 8);
                    uvs[i] = new Vector2(x * 0.25f, z * 0.25f);
                }
                //If x isn't at the end of a row or the top of the map, apply the perlin noise to the y axis
                else if (x != sizePerRow[z] && x != sizePerRow[z] - 1)
                {
                    vertices[i] = new Vector3(x * 8, y * randY, z * 8);
                    uvs[i] = new Vector2(x * 0.25f, z * 0.25f);
                }
                //If z is at the end of that rows length, assign the height as 0 so the land and water match up, leaving no gap
                else
                {
                    vertices[i] = new Vector3(x * 8, 0, z * 8);
                    uvs[i] = new Vector2(x * 0.25f, z * 0.25f);
                }
                i++;
            }
        }
        //Assign triangles as the xsize multiplied by zsize multiplied by 6, as each grid square in the mesh will require 2 triangles
        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;
        //Nested loop over each row, then each length of that row assigning the placement of each vertices of each triangle
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < sizePerRow[z]; x++)
            {
                //If x isn't at the end of the row, run this if statement
                if (x != sizePerRow[z] - 1)
                {
                    triangles[tris + 0] = vert + 0;
                    triangles[tris + 1] = vert + sizePerRow[z] + 1;
                    triangles[tris + 2] = vert + 1;
                    triangles[tris + 3] = vert + 1;
                    triangles[tris + 4] = vert + sizePerRow[z] + 1;
                    triangles[tris + 5] = vert + sizePerRow[z] + 2;
                }
                //If x is at the end of the row, go into this if
                if (x == sizePerRow[z] - 1)
                {
                    //If the length of the next row is the same as the current row, generate a full block of triangles, and make towerEligible for that block true
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
                    //If the next row is shorter than the current, only generate one triangle to join the two rows up smoother, and make towerEligible false as it isn't a full block
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
                //UpdateMesh could be called here as an IEnumerator to show the land spawning in, otherwise it can be called in the start function after this to do it all at once
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
    { //Clear the current mesh, assign vertices, triangles and uvs, recalculate normals, then assign the shared mesh to the mesh in order for it to have a collider
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