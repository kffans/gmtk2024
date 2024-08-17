using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject? menuCanvas;
    public GameObject? optionsCanvas;
    public GameObject? creditsCanvas;

    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level); 
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Gra została zamknięta"); 
    }

    public void OpenOptions()
    {
        menuCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
        creditsCanvas.SetActive(false);
    }

    public void QuitOptions()
    {
        menuCanvas.SetActive(true);
        optionsCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
    }

    public void OpenCredits()
    {
        menuCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
        creditsCanvas.SetActive(true);
    }

    public void QuitCredits()
    {
        menuCanvas.SetActive(true);
        optionsCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
    }
}
