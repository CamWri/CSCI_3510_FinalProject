using UnityEngine;
using UnityEngine.SceneManagement;

public class DDOLCleaner : MonoBehaviour
{
    public static void CleanDDOL()
    {
        // Create a dummy to find the DDOL scene
        GameObject temp = new GameObject("DDOL_Finder");
        DontDestroyOnLoad(temp);
        Scene ddolScene = temp.scene;

        // Destroy everything in the DDOL scene except the finder
        foreach (GameObject go in ddolScene.GetRootGameObjects())
        {
            if (go != temp)
                Destroy(go);
        }

        Destroy(temp);
        Debug.Log("Cleared all DontDestroyOnLoad objects.");
    }
}