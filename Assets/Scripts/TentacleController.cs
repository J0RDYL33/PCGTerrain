using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleController : MonoBehaviour
{
    Animator myAnimator;
    public float randomWait;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        randomWait = Random.Range(0.1f, 3.2f);
        StartCoroutine(StartIdle(randomWait));
    }

    IEnumerator StartIdle(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        myAnimator.SetBool("MoveToIdle", true);
    }
}
