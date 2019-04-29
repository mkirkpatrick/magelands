using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private Rigidbody rb;

    public bool isWalking = false;
    public bool isGrounded = false;

    public float moveSpeed;
    public Vector3 currentDestination;
    public Quaternion lookRotation;

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

        if (Vector3.Distance(currentDestination, transform.position) > .005f)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, currentDestination, moveSpeed * Time.deltaTime);
            transform.position = newPosition;
            Vector3 lookVector = (currentDestination - newPosition).normalized;  
            transform.LookAt(new Vector3(currentDestination.x, transform.position.y, currentDestination.z));
        }
        else {
            transform.position = currentDestination;
            transform.LookAt(transform.position + transform.forward);

        }
    }
}
