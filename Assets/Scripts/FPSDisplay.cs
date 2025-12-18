using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("GetFPS", 1, 1);
    }

    void GetFPS()
    {
        int fps = (int) (1f/ Time.unscaledDeltaTime);
        HUDController.Instance.UpdateFrameRateText(fps);
    }
}
