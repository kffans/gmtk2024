using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    #nullable enable
	public GameObject? menuCanvas;
    public GameObject? optionsCanvas;
	#nullable disable
	public GameObject bg;
	public Texture2D cursorImage;
	
	void Start(){
		AudioManager.instance.PlayMusic("menu");
		//move background
		LeanTween.moveX(bg, 200f, 5f).setLoopPingPong().setEase(LeanTweenType.easeOutQuad).setRepeat(-1);
		//sets cursor image
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		Cursor.SetCursor(cursorImage, new Vector2(20f,0f), CursorMode.ForceSoftware);
	}
	
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
    }

    public void QuitOptions()
    {
        menuCanvas.SetActive(true);
        optionsCanvas.SetActive(false);
    }

    public void OpenCredits()
    {
        menuCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
    }

    public void QuitCredits()
    {
        menuCanvas.SetActive(true);
        optionsCanvas.SetActive(false);
    }
}
