using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public static FloorManager Instance;  // Singleton instance

    [Header("Floor Data")]
    public int currentFloor = 1;          // Public so other scripts can read it

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Persist across scenes
        }
        else
        {
            Destroy(gameObject);  // Avoid duplicates
        }
    }

    /// <summary>
    /// Call this when moving to the next floor.
    /// </summary>
    public void NextFloor()
    {
        currentFloor++;
        Debug.Log("Floor: " + currentFloor);
    }

    /// <summary>
    /// Optionally, reset to floor 1.
    /// </summary>
    public void ResetFloor()
    {
        currentFloor = 1;
    }
}
