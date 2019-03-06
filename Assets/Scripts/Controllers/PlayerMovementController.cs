using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private Rigidbody rb;

    public float moveSpeed;
    public Vector3 currentDestination;

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("Ground");
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, mask))
            {
                if (hit.collider.tag == "Ground")
                    currentDestination = hit.point;
            }  
        }

        if (currentDestination != transform.position)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, currentDestination, moveSpeed * Time.deltaTime);
            transform.position = newPosition;
        }
    }
}
