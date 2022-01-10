using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGen : MonoBehaviour
{
    public GameObject rock;
    GameObject tempRock;
    // Start is called before the first frame update
    void Start()
    {
        SpawnRocks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnRocks()
    {
        int randomAmount = Random.Range(2, 5);
        for (int i = 0; i <= randomAmount; i++)
        {
            float randomX = Random.Range(-25.0f, 25.0f);
            float randomZ = Random.Range(-25.0f, 25.0f);
            int randomRotation = Random.Range(-180, 180);

            tempRock = Instantiate(rock, new Vector3(this.transform.position.x + randomX, this.transform.position.y - (i/2), this.transform.position.z + randomZ), 
                Quaternion.Euler(-90, randomRotation, 0));
            tempRock.transform.parent = this.gameObject.transform;
        }
    }
}
