using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGen : MonoBehaviour
{
    bool mainPath = true;
    int directionFacing = 1;
    public GameObject path;
    public GameObject spawnPoint;
    RaycastDown raycaster;
    public int directionToSpawn;
    public bool leftSpawnable;
    public bool rightSpawnable;
    public bool backSpawnable;
    public bool forwardSpawnable;
    //bool goodDirection = false;
    public string[] weCanSpawn;
    public int intThatCan;
    public GameObject[] houses;
    string result = "";

    // Start is called before the first frame update
    void Start()
    {
        intThatCan = 0;
        raycaster = FindObjectOfType<RaycastDown>();
        leftSpawnable = raycaster.GetTagBellow(new Vector3(spawnPoint.transform.position.x - 27.5f, 400, spawnPoint.transform.position.z + 1));
        rightSpawnable = raycaster.GetTagBellow(new Vector3(spawnPoint.transform.position.x + 27.5f, 400, spawnPoint.transform.position.z + 1));
        backSpawnable = raycaster.GetTagBellow(new Vector3(spawnPoint.transform.position.x + 1, 400, spawnPoint.transform.position.z - 27.5f));
        forwardSpawnable = raycaster.GetTagBellow(new Vector3(spawnPoint.transform.position.x + 1, 400, spawnPoint.transform.position.z + 27.5f));

        if (leftSpawnable == true)
            intThatCan++;
        if (rightSpawnable == true)
            intThatCan++;
        if (backSpawnable == true)
            intThatCan++;
        if (forwardSpawnable == true)
            intThatCan++;

        weCanSpawn = new string[intThatCan];

        int currentinArr = 0;
        if (leftSpawnable == true)
        {
            weCanSpawn[currentinArr] = "left";
            currentinArr++;
        }
        if (rightSpawnable == true)
        {
            weCanSpawn[currentinArr] = "right";
            currentinArr++;
        }
        if (backSpawnable == true)
        {
            weCanSpawn[currentinArr] = "back";
            currentinArr++;
        }
        if (forwardSpawnable == true)
        {
            weCanSpawn[currentinArr] = "forward";
        }

        StartCoroutine(SpawnNewMainPath());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValues()
    {

    }

    IEnumerator SpawnNewMainPath()
    {
        //Roll a number between 1-3, that's how many new paths are going to spawn
        //Roll 1-3, decides which direction to spawn them
        //Spawn it at the PathEndPoint and rotate it to the direction it needs to be
        //If the end point is off the grass, don't spawn it

        int numOfPaths = Random.Range(1, 5);
        result = "";
        if (weCanSpawn.Length != 0)
        {
            directionToSpawn = Random.Range(0, weCanSpawn.Length);
            result = weCanSpawn[directionToSpawn];

            if (result == "left")
            {
                GameObject tempPath = Instantiate(path, spawnPoint.transform.position, Quaternion.Euler(0, -90, 0));
                tempPath.name = "Path";
            }
            else if (result == "right")
            {
                GameObject tempPath = Instantiate(path, spawnPoint.transform.position, Quaternion.Euler(0, 90, 0));
                tempPath.name = "Path";
            }
            else if (result == "forward")
            {
                GameObject tempPath = Instantiate(path, spawnPoint.transform.position, Quaternion.Euler(0, 0, 0));
                tempPath.name = "Path";
            }
            else if (result == "back")
            {
                GameObject tempPath = Instantiate(path, spawnPoint.transform.position, Quaternion.Euler(0, 180, 0));
                tempPath.name = "Path";
            }
        }

        yield return new WaitForSeconds(Random.Range(0.1f, 2.0f));
        if (weCanSpawn.Length >= 2 && numOfPaths == 4)
        {
            if (rightSpawnable == true && result != "right" && raycaster.GetTagBellow(new Vector3(spawnPoint.transform.position.x + 27.5f, 400, spawnPoint.transform.position.z + 1)) == true)
            {
                GameObject tempPath = Instantiate(path, spawnPoint.transform.position, Quaternion.Euler(0, 90, 0));
                tempPath.name = "Path";
            }
            else if (leftSpawnable == true && result != "left" && raycaster.GetTagBellow(new Vector3(spawnPoint.transform.position.x - 27.5f, 400, spawnPoint.transform.position.z + 1)) == true)
            {
                GameObject tempPath = Instantiate(path, spawnPoint.transform.position, Quaternion.Euler(0, -90, 0));
                tempPath.name = "Path";
            }
            else if (forwardSpawnable == true && result != "forward" && raycaster.GetTagBellow(new Vector3(spawnPoint.transform.position.x + 1, 400, spawnPoint.transform.position.z + 27.5f)) == true)
            {
                GameObject tempPath = Instantiate(path, spawnPoint.transform.position, Quaternion.Euler(0, 0, 0));
                tempPath.name = "Path";
            }
            else if (backSpawnable == true && result != "back" && raycaster.GetTagBellow(new Vector3(spawnPoint.transform.position.x + 1, 400, spawnPoint.transform.position.z - 27.5f)) == true)
            {
                GameObject tempPath = Instantiate(path, spawnPoint.transform.position, Quaternion.Euler(0, 180, 0));
                tempPath.name = "Path";
            }
        }

        SpawnHouses();
    }

    void SpawnHouses()
    {
        int houseToSpawn = Random.Range(0, 5);
        if (result == "left" && raycaster.GetTagBellow(new Vector3(spawnPoint.transform.position.x - 11.5f, 400, spawnPoint.transform.position.z -14f)) == true)
        {
            Vector3 newSpawn = spawnPoint.transform.position;
            newSpawn.x -= 13.5f;
            newSpawn.z -= 13.5f;
            Instantiate(houses[houseToSpawn], newSpawn, Quaternion.Euler(0, 180, 0));
            Debug.Log("Spawned left");
        }
        else if (result == "right" && raycaster.GetTagBellow(new Vector3(spawnPoint.transform.position.x + 11.5f, 400, spawnPoint.transform.position.z + 19.5f)) == true)
        {
            Vector3 newSpawn = spawnPoint.transform.position;
            newSpawn.x += 13.5f;
            newSpawn.z += 13.5f;
            Instantiate(houses[houseToSpawn], newSpawn, Quaternion.Euler(0, 0, 0));
            Debug.Log("Spawned right");
        }
        else if (result == "forward" && raycaster.GetTagBellow(new Vector3(spawnPoint.transform.position.x - 11.5f, 400, spawnPoint.transform.position.z + 19.5f)) == true)
        {
            Vector3 newSpawn = spawnPoint.transform.position;
            newSpawn.x -= 13.5f;
            newSpawn.z += 13.5f;
            Instantiate(houses[houseToSpawn], newSpawn, Quaternion.Euler(0, -90, 0));
            Debug.Log("Spawned forward");
        }
        else if (result == "back" && raycaster.GetTagBellow(new Vector3(spawnPoint.transform.position.x + 11.5f, 400, spawnPoint.transform.position.z - 19.5f)) == true)
        {
            Vector3 newSpawn = spawnPoint.transform.position;
            newSpawn.x += 13.5f;
            newSpawn.z -= 13.5f;
            Instantiate(houses[houseToSpawn], newSpawn, Quaternion.Euler(0, 90, 0));
            Debug.Log("Spawned back");
        }
    }
}
