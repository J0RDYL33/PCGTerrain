using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastDown : MonoBehaviour
{
    public GameObject cube2;
    public GameObject cube3;
    public GameObject cube4;
    public float hoverError1;
    public float hoverError2;
    public float hoverError3;
    public float hoverError4;
    LandGen myLandGen;
    public bool[] spawnableFlat;
    public bool[] spawnableFlat2;
    public float[] positionToSpawn;
    public float[] positionToSpawn2;
    public float[] tester;
    RaycastHit hit1;
    RaycastHit hit2;
    RaycastHit hit3;
    RaycastHit hit4;

    void Start()
    {
        myLandGen = FindObjectOfType<LandGen>();
        spawnableFlat = new bool[myLandGen.zSize];
        spawnableFlat2 = new bool[myLandGen.sizePerRow[myLandGen.zSize]];
        positionToSpawn = new float[myLandGen.zSize];
        positionToSpawn2 = new float[spawnableFlat2.Length];
        tester = new float[myLandGen.zSize];
        StartCoroutine(Spawnable());
    }

    IEnumerator Spawnable()
    {
        for (int i = 0; i < myLandGen.zSize; i++)
        {
            hoverError1 = 0;
            hoverError2 = 0;
            hoverError3 = 0;
            hoverError4 = 0;

            if (myLandGen.sizePerRow[i] == myLandGen.sizePerRow[i+1])
                transform.position = new Vector3((myLandGen.sizePerRow[i] * 8) - 4 , 40, (i * 8) + 2);
            else
                transform.position = new Vector3((myLandGen.sizePerRow[i] * 8) - 8, 40, (i * 8) + 2);

            hit1 = new RaycastHit();
            Ray downRay1 = new Ray(transform.position, -Vector3.up);
            hit2 = new RaycastHit();
            Ray downRay2 = new Ray(cube2.transform.position, -Vector3.up);
            hit3 = new RaycastHit();
            Ray downRay3 = new Ray(cube3.transform.position, -Vector3.up);
            hit4 = new RaycastHit();
            Ray downRay4 = new Ray(cube4.transform.position, -Vector3.up);
            // Cast a ray straight downwards.
            if (Physics.Raycast(downRay1, out hit1))
            {
                hoverError1 = transform.position.y - hit1.distance;
            }
            if (Physics.Raycast(downRay2, out hit2))
            {
                hoverError2 = cube2.transform.position.y - hit2.distance;
            }
            if (Physics.Raycast(downRay3, out hit3))
            {
                hoverError3 = cube3.transform.position.y - hit2.distance;
            }
            if (Physics.Raycast(downRay4, out hit4))
            {
                hoverError4 = cube4.transform.position.y - hit4.distance;
            }

            if ((hoverError1 - hoverError4 <= 0.4f && hoverError1 - hoverError4 >= -0.4f) && (hoverError1 - hoverError3 <= 0.4f && hoverError1 - hoverError3 >= -0.4f) && (hoverError1 - hoverError2 <= 0.4f && hoverError1 - hoverError2 >= -0.4f))
            {
                positionToSpawn[i] = ((hoverError1 + hoverError2 + hoverError3 + hoverError4) / 4);
                tester[i] = hoverError4;
                spawnableFlat[i] = true;
            }
            else
            {
                spawnableFlat[i] = false;
                //positionToSpawn[i] = ((hoverError1 + hoverError2 + hoverError3 + hoverError4) /4);
                positionToSpawn[i] = 0;
                tester[i] = hoverError4;
            }
            yield return new WaitForSeconds(0.005f);
        }

        for (int i = 0; i < myLandGen.sizePerRow[myLandGen.zSize]; i++)
        {
            hoverError1 = 0;
            hoverError2 = 0;
            hoverError3 = 0;
            hoverError4 = 0;
            transform.position = new Vector3((i * 8) + 4, 40, (myLandGen.zSize * 8) - 4);

            RaycastHit hit1;
            Ray downRay1 = new Ray(transform.position, -Vector3.up);
            RaycastHit hit2;
            Ray downRay2 = new Ray(cube2.transform.position, -Vector3.up);
            RaycastHit hit3;
            Ray downRay3 = new Ray(cube3.transform.position, -Vector3.up);
            RaycastHit hit4;
            Ray downRay4 = new Ray(cube4.transform.position, -Vector3.up);
            // Cast a ray straight downwards.
            if (Physics.Raycast(downRay1, out hit1))
            {
                hoverError1 = transform.position.y - hit1.distance;
            }
            if (Physics.Raycast(downRay2, out hit2))
            {
                hoverError2 = cube2.transform.position.y - hit2.distance;
            }
            if (Physics.Raycast(downRay3, out hit3))
            {
                hoverError3 = cube3.transform.position.y - hit2.distance;
            }
            if (Physics.Raycast(downRay4, out hit4))
            {
                hoverError4 = cube4.transform.position.y - hit4.distance;
            }

            if (hoverError1 - hoverError4 <= 0.4f && hoverError1 - hoverError4 >= -0.4f)
            {
                positionToSpawn2[i] = hoverError4;
                spawnableFlat2[i] = true;
            }
            else
            {
                spawnableFlat2[i] = false;
            }
        }
        transform.position = new Vector3(-400, 400, -400);
    }

    public bool GetTagBellow(Vector3 position)
    {
        this.transform.position = position;

        hoverError1 = 0;
        hoverError2 = 0;
        hoverError3 = 0;
        hoverError4 = 0;

        RaycastHit hit1;
        Ray downRay1 = new Ray(transform.position, -Vector3.up);
        RaycastHit hit2;
        Ray downRay2 = new Ray(cube2.transform.position, -Vector3.up);
        RaycastHit hit3;
        Ray downRay3 = new Ray(cube3.transform.position, -Vector3.up);
        RaycastHit hit4;
        Ray downRay4 = new Ray(cube4.transform.position, -Vector3.up);
        // Cast a ray straight downwards.
        if (Physics.Raycast(downRay1, out hit1))
        {
            hoverError1 = transform.position.y - hit1.distance;
        }
        if (Physics.Raycast(downRay2, out hit2))
        {
            hoverError2 = cube2.transform.position.y - hit2.distance;
        }
        if (Physics.Raycast(downRay3, out hit3))
        {
            hoverError3 = cube3.transform.position.y - hit2.distance;
        }
        if (Physics.Raycast(downRay4, out hit4))
        {
            hoverError4 = cube4.transform.position.y - hit4.distance;
        }

        this.transform.position = new Vector3(400, 400, 400);

        if (hit1.distance > 0)
        {
            if (hit1.transform.tag == "Grass")
            {
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }

    private void OnDrawGizmos()
    {
        /*Gizmos.color = Color.green;
        Debug.DrawRay(transform.position, -Vector3.up * 1000, Color.green);
        Debug.DrawRay(cube2.transform.position, -Vector3.up * 1000, Color.green);
        Debug.DrawRay(cube3.transform.position, -Vector3.up * 1000, Color.green);
        Debug.DrawRay(cube4.transform.position, -Vector3.up * 1000, Color.green);*/


        /*Gizmos.DrawSphere(hit1.point, 1.0f);
        Gizmos.DrawSphere(hit2.point, 1.0f);
        Gizmos.DrawSphere(hit3.point, 1.0f);
        Gizmos.DrawSphere(hit4.point, 1.0f);*/
    }
}
