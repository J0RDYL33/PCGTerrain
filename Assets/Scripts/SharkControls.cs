using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkControls : MonoBehaviour
{
    float speed = 10.0f;
    Rigidbody rb;
    float zRotation;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = -transform.right * speed;
        zRotation += Time.deltaTime * 20;
        transform.localRotation = Quaternion.Euler(-90, 0, zRotation);
    }
}
