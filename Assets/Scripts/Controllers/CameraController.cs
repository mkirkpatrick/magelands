using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject target;
    public float damping = 3;
    public Vector3 offset;
    public Vector3 focus;

    Rigidbody rb;

    void Start()
    {
        target = GameObject.Find("Player");
        rb = target.GetComponent<Rigidbody>();
        //offset = transform.position - target.transform.position;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = target.transform.position + offset;
        Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);
        transform.position = position;

        transform.LookAt(target.transform.position + focus);
    }
}


