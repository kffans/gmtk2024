using UnityEngine;

public class Sword : MonoBehaviour
{
    public GameManager gameManager;
	public bool grabbed = false;
	private Vector3 mousePosPrev;
	private Vector3 mousePos;

    void Update() {
		//LeanTween.rotateAround(gameObject, Vector3.forward, 200f, 2f);
        if(gameManager.currentPhase == GameManager.Phase.Challenge && grabbed){
			Cursor.lockState = CursorLockMode.Locked;
			float rotation = 1f;
			transform.Rotate(new Vector3(0, 0, rotation));
			//mousePosPrev.position.x - mousePos.position.x
			
			//gameManager.challengeSuccess = true;
			//grabbed = false;
			//Cursor.lockState = CursorLockMode.None;
			if (Input.GetMouseButtonUp(0)){
				grabbed = false;
				Cursor.lockState = CursorLockMode.None;
			}
		}
    }
	
	public void StartGrabbing(){
		if(gameManager.currentPhase == GameManager.Phase.Challenge){
			grabbed = true;
		}
	}
}
