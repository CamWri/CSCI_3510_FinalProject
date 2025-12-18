using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX = 400f;
    public float sensY = 400f;

    [Header("Recoil (Auto Smoothed)")]
    public Vector2 recoilOffset;       // Current recoil being applied
    private Vector2 recoilVelocity;    // Used for smooth damping
    public float recoilReturnSpeed = 8f;   // How fast recoil recenters
    public float recoilApplySpeed = 18f;   // How fast recoil jumps on shot

    public Transform orientation;

    private float xRotation;
    private float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleLook();
        ProcessRecoil();
    }

    private void HandleLook()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply recoil offset here
        transform.localRotation = Quaternion.Euler(xRotation - recoilOffset.y, yRotation - recoilOffset.x, 0f);
        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    // --- Smoothly returns recoil to zero ---
    private void ProcessRecoil()
    {
        recoilOffset = Vector2.SmoothDamp(
            recoilOffset,
            Vector2.zero,
            ref recoilVelocity,
            1f / recoilReturnSpeed
        );
    }

    // --- Called by Gun on every shot ---
    public void AddRecoil(Vector2 recoilAmount)
    {
        recoilOffset += recoilAmount;
    }
}