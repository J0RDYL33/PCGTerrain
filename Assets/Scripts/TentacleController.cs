using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleController : MonoBehaviour
{
    Animator myAnimator;
    public float randomWait;
    public GameObject[] debris;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        randomWait = Random.Range(0.1f, 3.2f);
        StartCoroutine(StartIdle(randomWait));
        SpawnDebris();
    }

    IEnumerator StartIdle(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        myAnimator.SetBool("MoveToIdle", true);
    }

    public void AttackFunction()
    {
        int doAttack = Random.Range(0, 5);
        if (doAttack == 0)
            StartCoroutine(StartAttack());
    }

    public IEnumerator StartAttack()
    {
        myAnimator.SetBool("Attack", true);
        yield return new WaitForSeconds(3.5f);
        myAnimator.SetBool("Attack", false);
    }

    void SpawnDebris()
    {
        int whichDebris = Random.Range(0, debris.Length);
        Instantiate(debris[whichDebris], new Vector3(this.transform.position.x + 20, 0, this.transform.position.z), Quaternion.Euler(0, Random.Range(0,360), 0));

        whichDebris = Random.Range(0, debris.Length);
        Instantiate(debris[whichDebris], new Vector3(this.transform.position.x - 20, 0, this.transform.position.z), Quaternion.Euler(0, Random.Range(0, 360), 0));

        whichDebris = Random.Range(0, debris.Length);
        Instantiate(debris[whichDebris], new Vector3(this.transform.position.x, 0, this.transform.position.z + 20), Quaternion.Euler(0, Random.Range(0, 360), 0));

        whichDebris = Random.Range(0, debris.Length);
        Instantiate(debris[whichDebris], new Vector3(this.transform.position.x, 0, this.transform.position.z - 20), Quaternion.Euler(0, Random.Range(0, 360), 0));
    }
}
