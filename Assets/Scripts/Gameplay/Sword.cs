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
			Screen.lockCursor = true;
			float rotation = 1f;
			transform.Rotate(new Vector3(0, 0, rotation));
			//mousePosPrev.position.x - mousePos.position.x
			
			//gameManager.challengeSuccess = false;
			if (Input.GetMouseButtonUp(0)){
				grabbed = false;
			}
		}
    }
	
	public void StartGrabbing(){
		if(gameManager.currentPhase == GameManager.Phase.Challenge){
			grabbed = true;
		}
	}
}
