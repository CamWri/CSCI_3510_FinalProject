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

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    void HandleMouseLook()
    { 
        // --- Mouse Change in Direction ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * (invertY ? 1 : -1);

        rotationX += mouseY;
        rotationY += mouseX;

        // --- In World Rotation ---
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }

    void HandleMovement()
    {
        // --- WASD Movement  ---
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool sprint = Input.GetKey(KeyCode.LeftShift);
        bool up = Input.GetKey(KeyCode.E);
        bool down = Input.GetKey(KeyCode.Q);

        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        direction.y = (up ? 1f : 0f) + (down ? -1f : 0f);

        if (direction.magnitude > 1f)
            direction.Normalize();

        float speed = sprint ? moveSpeed * sprintMultiplier : moveSpeed;

        transform.Translate(direction * speed * Time.deltaTime, Space.Self);
    }
}
