using UnityEngine;

public class Pause : MonoBehaviour
{
	bool isPaused = false;
	public GameObject pauseScreen;
	
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused) {
			pauseScreen.SetActive(true);
			isPaused = true;
            Time.timeScale = 0f;
        }
		else if (Input.GetKeyDown(KeyCode.Escape) && isPaused) {
			pauseScreen.SetActive(false);
			isPaused = false;
            Time.timeScale = 1f;
        }
    }
}
