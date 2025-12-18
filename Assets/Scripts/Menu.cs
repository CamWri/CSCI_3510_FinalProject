using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    

    public void OnPlayMainButton()
    {
        SceneManager.LoadScene("CamdenProceduralGeneration");
    }

    public void OnPlayDemoButton()
    {
        SceneManager.LoadScene("GunDemoScene");
    }

    public void OnExitButton()
    {
        Application.Quit();
    }
}
