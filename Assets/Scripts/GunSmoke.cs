using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSmoke : MonoBehaviour
{
    bool playThrough = true;
    public ParticleSystem smoke;
    public TentacleController thisTentacle;
    float randySmoke;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DoSmoke());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DoSmoke()
    {
        while (playThrough == true)
        {
            randySmoke = Random.Range(0.0f, 2.1f);
            smoke.Play();
            smoke.enableEmission = true;
            thisTentacle.AttackFunction();
            yield return new WaitForSeconds(2.0f);
            smoke.Stop();
            smoke.enableEmission = false;
            yield return new WaitForSeconds(1.0f + randySmoke);
        }
    }
}
