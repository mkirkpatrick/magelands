using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject target;

    public float cameraHeight = 10f;
    public float cameraDistance = 10;

    public float orbitAngle = 45f;
    public float targetOrbitAngle = 0;
    public float orbitSpeed = 10f; // Negative to rotate backwards

    Rigidbody rb;

    void Start()
    {
        target = GameObject.Find("Player");
        rb = target.GetComponent<Rigidbody>();
        //offset = transform.position - target.transform.position;
    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.Space)) {
            targetOrbitAngle = target.transform.eulerAngles.y + 90;

            if (targetOrbitAngle < 0)
                targetOrbitAngle += 360;
            else if (targetOrbitAngle > 360)
                targetOrbitAngle -= 360;

            if ((targetOrbitAngle - orbitAngle) > 180)
                orbitSpeed = Mathf.Abs(orbitSpeed) * -1;
            else
                orbitSpeed = Mathf.Abs(orbitSpeed);
        }


        if (orbitAngle != targetOrbitAngle)
            orbitAngle += (orbitSpeed * Time.deltaTime);

        if (Mathf.Abs(targetOrbitAngle - orbitAngle) <= 1f)
            orbitAngle = targetOrbitAngle;

        if (orbitAngle > 360)
            orbitAngle -= 360;
        else if (orbitAngle < 0)
            orbitAngle += 360;

    }

    void LateUpdate()
    {
        float radians = Mathf.Deg2Rad * orbitAngle;
        transform.position = new Vector3( Mathf.Cos(radians) * cameraDistance, cameraHeight, Mathf.Sin(radians) * -cameraDistance ) + target.transform.position;
        transform.LookAt(target.transform.position);
    }
}


