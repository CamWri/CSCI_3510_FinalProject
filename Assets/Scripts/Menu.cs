using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    

    public void OnPlayMainButton()
    {
        SceneManager.LoadScene("ProceduralGeneration");
    }

    public void OnExitButton()
    {
        Application.Quit();
    }
}
