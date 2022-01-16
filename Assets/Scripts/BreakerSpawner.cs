using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakerSpawner : MonoBehaviour
{
    public GameObject windBreaker;
    LandGen myLandGen;
    // Start is called before the first frame update
    void Start()
    {
        myLandGen = FindObjectOfType<LandGen>();

        for (int i = 0; i < myLandGen.zSize - 10; i++)
        {
            int toSpawnBreaker = Random.Range(0, 10);
            if (toSpawnBreaker == 0)
            {
                Instantiate(windBreaker, new Vector3((myLandGen.sizePerRow[i] * 8)- 64, 0, (i * 8) - 4), Quaternion.Euler(-90, 90, -25));
            }
        }
    }
}
