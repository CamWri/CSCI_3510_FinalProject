using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public static MoveCamera Instance;

    private GameObject cameraPosition;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            cameraPosition = GameObject.Find("CameraPos");
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void LateUpdate() {
        transform.position = cameraPosition.transform.position;
    }
}
