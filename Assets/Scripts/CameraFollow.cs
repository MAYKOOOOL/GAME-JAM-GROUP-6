using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform player;
    public float distance = 4f;
    public float height = 2f;

    public float normalSensitivity = 100f;
    public float boostedSensitivity = 200f;

    public float smoothSpeed = 10f; // Higher = smoother

    private float yaw = 0f;
    private float pitch = 15f;

    private float currentYaw;
    private float currentPitch;
    private float yawVelocity;
    private float pitchVelocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yaw = player.eulerAngles.y;
        currentYaw = yaw;
        currentPitch = pitch;
    }

    void LateUpdate()
    {
        bool isBoosting = Input.GetMouseButton(0);
        float sensitivity = isBoosting ? boostedSensitivity : normalSensitivity;

        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, 5f, 60f);

        // Smooth camera rotation using damping
        currentYaw = Mathf.SmoothDampAngle(currentYaw, yaw, ref yawVelocity, 1f / smoothSpeed);
        currentPitch = Mathf.SmoothDamp(currentPitch, pitch, ref pitchVelocity, 1f / smoothSpeed);

        // Calculate final camera rotation and position
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
        Vector3 offset = rotation * new Vector3(0f, 0f, -distance) + new Vector3(0, height, 0);
        transform.position = player.position + offset;

        transform.LookAt(player.position + Vector3.up * 1.5f);

        // Rotate player to match yaw
        player.rotation = Quaternion.Euler(0f, currentYaw, 0f);
    }
}
