using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public float sensX;
    public float sensY;

    [Header("Recoil")]
    private Vector3 targetRecoil = Vector3.zero;
    private Vector3 currentRecoil = Vector3.zero;

    public Transform orientation;

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void Update() {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation - currentRecoil.y, yRotation - currentRecoil.x, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void ApplyRecoil(Vector3 maxRecoil, float recoilAmount, float recoilSpeed)
    {
        float recoilX = Random.Range(-maxRecoil.x, maxRecoil.x) * recoilAmount;
        float recoilY = Random.Range(0, maxRecoil.y) * recoilAmount;

        targetRecoil += new Vector3(recoilX, recoilY, 0);

        currentRecoil = Vector3.MoveTowards(currentRecoil, targetRecoil, recoilSpeed);
    }

    public void ResetRecoil(float resetRecoilSpeed)
    {
        currentRecoil = Vector3.MoveTowards(currentRecoil, Vector3.zero, Time.deltaTime*resetRecoilSpeed);
        targetRecoil = Vector3.MoveTowards(targetRecoil, Vector3.zero, Time.deltaTime*resetRecoilSpeed);
    }
}
