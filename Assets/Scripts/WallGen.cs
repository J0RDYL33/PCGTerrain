using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGen : MonoBehaviour
{
    public GameObject wall;
    GrassGen myLandGen;
    int sinceLastTower = 4;
    public bool[] towerEligible;
    GameObject tempTower;
    RaycastDown myRaycast;
    // Start is called before the first frame update
    void Start()
    {
        myLandGen = FindObjectOfType<GrassGen>();
        myRaycast = FindObjectOfType<RaycastDown>();
        towerEligible = myLandGen.towerEligible;

        StartCoroutine(SpawnTowers());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnTowers()
    {
        GameObject recentBarrier;
        for (int i = 0; i < myLandGen.zSize; i++)
        {
                if (towerEligible[i] == true)
                {
                    tempTower = Instantiate(wall, new Vector3((myLandGen.sizePerRow[i] * 8), 3.31f, (i * 8) + 4), Quaternion.Euler(0, 90, 0));
                    tempTower.transform.parent = gameObject.transform;
                    sinceLastTower = 0;
                }
                else
                {
                    tempTower = Instantiate(wall, new Vector3((myLandGen.sizePerRow[i] * 8) - 4, 3.31f, (i * 8) + 4), Quaternion.Euler(0, 45, 0));
                tempTower.transform.localScale = new Vector3(12, 4.5f, 1);
                    tempTower.transform.parent = gameObject.transform;
                    sinceLastTower = 0;
                }
            yield return new WaitForSeconds(0.005f);
        }

        sinceLastTower = 8;

        for (int i = 0; i < myLandGen.sizePerRow[myLandGen.sizePerRow.Length - 1]; i++)
        {
                tempTower = Instantiate(wall, new Vector3((i * 8) + 4, 3.31f, (myLandGen.zSize * 8)), Quaternion.identity);
                tempTower.transform.parent = gameObject.transform;
                sinceLastTower = 0;
        }
    }
}