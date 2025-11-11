using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float sprintMultiplier = 2f;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 2f;
    public bool invertY = false;

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        // Lock cursor for free look
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * (invertY ? 1 : -1);

        rotationX += mouseY;
        rotationY += mouseX;

        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }

    void HandleMovement()
    {
        // Basic WASD movement
        Vector3 move = new Vector3(
            Input.GetAxis("Horizontal"),
            0f,
            Input.GetAxis("Vertical")
        );

        // Vertical movement with Q/E
        if (Input.GetKey(KeyCode.E)) move.y += 1f;
        if (Input.GetKey(KeyCode.Q)) move.y -= 1f;

        // Apply speed and sprint
        float speed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) speed *= sprintMultiplier;

        transform.Translate(move * speed * Time.deltaTime, Space.Self);
    }
}
