using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float sprintSpeed = 15f;
    public float jumpForce = 5f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Transform holdPoint; 
    public float throwForce = 10f;
    public float pickupRange = 2f; 

    private Rigidbody rb;
    private float moveSpeed;
    private bool isGrounded;

    private GameObject heldObject = null; 
    private Rigidbody heldObjectRb = null;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveSpeed = walkSpeed;
    }

    void Update()
    {
        GroundCheck();
        Move();
        Sprint();
        Jump();
        HandlePickupAndThrow();
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        Debug.Log("Grounded: " + isGrounded);
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        Vector3 velocity = move.normalized * moveSpeed;

        Vector3 currentVelocity = rb.velocity;
        rb.velocity = new Vector3(velocity.x, currentVelocity.y, velocity.z);
    }

    void Sprint()
    {
        moveSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void HandlePickupAndThrow()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
            {
                TryPickupObject();
            }
            else
            {
                ThrowObject();
            }
        }

        if (heldObject != null)
        {
            heldObject.transform.position = holdPoint.position;
        }
    }

    void TryPickupObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickupRange))
        {
            if (hit.collider.CompareTag("PickUp"))
            {
                heldObject = hit.collider.gameObject;
                heldObjectRb = heldObject.GetComponent<Rigidbody>();

                heldObjectRb.isKinematic = true;
                heldObject.transform.SetParent(holdPoint);
            }
        }
    }

    void ThrowObject()
    {
        heldObject.transform.SetParent(null);
        heldObjectRb.isKinematic = false;

        heldObjectRb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);

        heldObject = null;
        heldObjectRb = null;
    }
}
