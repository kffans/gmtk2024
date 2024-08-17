using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    #nullable enable
	public GameObject? menuCanvas;
    public GameObject? optionsCanvas;
    public GameObject? creditsCanvas;
	#nullable disable

    public void LoadLevel(string level)
    {
        Time.timeScale = 1f;
		SceneManager.LoadScene(level); 
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game was closed"); 
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
