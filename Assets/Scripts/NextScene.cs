using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public void RegenerateScene()
    {
        if (FloorManager.Instance != null)
        {
            FloorManager.Instance.NextFloor();
        }
        else
        {
            Debug.LogError("FloorManager.Instance is null! Make sure FloorManager exists in the first scene.");
        }

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
