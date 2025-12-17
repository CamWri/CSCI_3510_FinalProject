using UnityEngine;

public class ScaleGunPosition : MonoBehaviour
{
    public Camera mainCamera;

    public float xOffest = 0.75f;

    public float yOffset = 0.25f;

    public float zDistance = 1.5f; // in front of the camera

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Update()
    {
        // Gun stays relative to viewport
        Vector3 viewportPos = new Vector3(xOffest, yOffset, zDistance);
        Vector3 targetPos = mainCamera.ViewportToWorldPoint(viewportPos);

        // Smooth movement
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 10f);
    }
}