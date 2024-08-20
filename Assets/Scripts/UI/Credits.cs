using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public RawImage whiteOut;
	public Texture2D cursorImage;
	
	// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LeanTween.value(whiteOut.gameObject, updateColor, new Color(1f,1f,1f,1f), new Color(1f,1f,1f,0f), 0.5f);
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		Cursor.SetCursor(cursorImage, new Vector2(20f,0f), CursorMode.ForceSoftware);		
    }
	
	public void LoadLevel(string level)
    {
        Time.timeScale = 1f;
		SceneManager.LoadScene(level); 
    }
	
	void updateColor(Color val){
		whiteOut.color = val;
	}
	
}
