using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierGen : MonoBehaviour
{
    public GameObject tower;
    public GameObject barrier;
    LandGen myLandGen;
    public bool[] towerEligible;
    int sinceLastTower = 4;
    GameObject tempTower;
    RaycastDown myRaycast;
    // Start is called before the first frame update
    void Start()
    {
        myLandGen = FindObjectOfType<LandGen>();
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
        for (int i = 0; i < towerEligible.Length-1; i++)
        {
            if (sinceLastTower >= 5 && myRaycast.spawnableFlat[i] == true)
            {
                if (towerEligible[i] == true)
                {
                    tempTower = Instantiate(tower, new Vector3((myLandGen.sizePerRow[i] * 8) - 3, myRaycast.positionToSpawn[i], (i * 8) + 4), Quaternion.identity);
                    tempTower.transform.parent = gameObject.transform;
                    sinceLastTower = 0;
                }
                else
                {
                    tempTower = Instantiate(tower, new Vector3((myLandGen.sizePerRow[i] * 8) - 8, myRaycast.positionToSpawn[i], (i * 8) + 3), Quaternion.Euler(0,45,0));
                    tempTower.transform.parent = gameObject.transform;
                    sinceLastTower = 0;
                }
            }
            else if (myRaycast.spawnableFlat[i] == true && myRaycast.positionToSpawn[i] == 0)
            {
                if (myLandGen.sizePerRow[i] == myLandGen.sizePerRow[i + 1])
                {
                    recentBarrier = Instantiate(barrier, new Vector3((myLandGen.sizePerRow[i] * 8) - 3, myRaycast.positionToSpawn[i] + 2.5f, (i * 8) + 3), Quaternion.identity);
                    recentBarrier.transform.parent = gameObject.transform;
                    recentBarrier.transform.localRotation = Quaternion.Euler(-45, 90, 0);
                    sinceLastTower++;
                }
                else
                {
                    recentBarrier = Instantiate(barrier, new Vector3((myLandGen.sizePerRow[i] * 8) - 8, myRaycast.positionToSpawn[i] + 2.5f, (i * 8) + 3), Quaternion.identity);
                    recentBarrier.transform.parent = gameObject.transform;
                    recentBarrier.transform.localRotation = Quaternion.Euler(-45, 45, 0);
                    sinceLastTower++;
                }
            }
            yield return new WaitForSeconds(0.005f);
        }

        sinceLastTower = 8;

        for (int i = 0; i < myLandGen.sizePerRow[myLandGen.sizePerRow.Length-1]-1; i++)
        {
            if (sinceLastTower >= 8 && myRaycast.spawnableFlat2[i] == true)
            {
                tempTower = Instantiate(tower, new Vector3((i * 8) + 4, myRaycast.positionToSpawn2[i], (myLandGen.zSize * 8)- 4), Quaternion.identity);
                tempTower.transform.parent = gameObject.transform;
                sinceLastTower = 0;
            }
            else
            {
                recentBarrier = Instantiate(barrier, new Vector3((i * 8) + 4, 2.5f, (myLandGen.zSize * 8) - 4), Quaternion.identity);
                recentBarrier.transform.parent = gameObject.transform;
                recentBarrier.transform.localRotation = Quaternion.Euler(-45, 0, 0);
                sinceLastTower++;
            }
        }
    }    
}
