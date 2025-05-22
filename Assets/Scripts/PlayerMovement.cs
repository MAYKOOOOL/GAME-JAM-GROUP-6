using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public Transform playerCamera;
    public float lookSpeedX = 2f;
    public float lookSpeedY = 2f;
    public GameObject crosshair;
    public float crosshairDistance = 5f;
    public TextMeshProUGUI pickupMessageText;

    private Rigidbody rb;
    private float moveSpeed;
    private bool isGrounded;
    private GameObject heldObject = null;
    private Rigidbody heldObjectRb = null;
    private float yaw = 0f;
    private float pitch = 0f;

    public ItemChecklistManager checklistManager;
/*    private bool isGameOver = false;*/



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveSpeed = walkSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pickupMessageText.gameObject.SetActive(false);
    }

    void Update()
    {
/*        if(!isGameOver) return;*/
        GroundCheck();
        Move();
        Sprint();
        Jump();
        HandlePickupAndThrow();
        CameraLook();
        InteractWithCrosshair();
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }
/*
    public void StopPlayer()
    {
        isGameOver = true;
        rb.velocity = Vector3.zero;
    }*/


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
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, pickupRange))
        {
            if (hit.collider.CompareTag("PickUp") || hit.collider.CompareTag("Required"))
            {
                heldObject = hit.collider.gameObject;
                heldObjectRb = heldObject.GetComponent<Rigidbody>();
                heldObjectRb.isKinematic = true;
                heldObject.transform.SetParent(holdPoint);

                if (hit.collider.CompareTag("Required"))
                {
                    ShowPickupMessage("You've picked up a special item!");
                    checklistManager.CrossOutItem(hit.collider.gameObject);
                    Destroy(heldObject);  
                }
                else if (hit.collider.CompareTag("PickUp"))
                {
                    ShowPickupMessage("You've picked up a random item!");

                }
            }
        }
    }

    void ThrowObject()
    {
        heldObject.transform.SetParent(null);
        heldObjectRb.isKinematic = false;
        heldObjectRb.AddForce(playerCamera.forward * throwForce, ForceMode.Impulse);
        heldObject = null;
        heldObjectRb = null;
    }

    void CameraLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeedX;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeedY;
        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -80f, 80f);
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        playerCamera.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    void InteractWithCrosshair()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, crosshairDistance))
        {
            if (hit.collider.CompareTag("PickUp") || hit.collider.CompareTag("Required"))
            {
                if (crosshair != null)
                {
                    crosshair.SetActive(true);
                }
            }
            else
            {
                if (crosshair != null)
                {
                    crosshair.SetActive(false);
                }
            }
        }
    }

    void ShowPickupMessage(string message)
    {
        pickupMessageText.text = message;
        pickupMessageText.gameObject.SetActive(true);
        Invoke("HidePickupMessage", 2f);
    }

    void HidePickupMessage()
    {
        pickupMessageText.gameObject.SetActive(false);
    }
}
