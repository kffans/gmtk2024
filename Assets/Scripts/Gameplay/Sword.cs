using UnityEngine;

public class Sword : MonoBehaviour
{
    public GameManager gameManager;
	private Vector3 mousePosPrev = new Vector3(0f,0f,0f);
	private Vector3 mousePos;

	public float progress = 0f;
	
	public enum DragState{
		None,
		Fresh,
		ForceEnd
	}
	
	public DragState dragState;
	
	void Start(){
		dragState = DragState.None;
	}

    void Update() {
        if(gameManager.currentPhase == GameManager.Phase.Challenge){
			if (Input.GetMouseButtonDown(0)) {
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

				if(hit.collider != null){
					dragState = DragState.Fresh;
					Cursor.lockState = CursorLockMode.Locked;
				}
			}
			
			if(dragState == DragState.Fresh){
				

				if(Input.GetMouseButtonUp(0)){
					Cursor.lockState = CursorLockMode.None;
					dragState = DragState.None;
				}
			}
			
			//progress
			float rotation = 1f;
			transform.Rotate(new Vector3(0, 0, rotation));
			mousePos = Input.mousePosition;
			
			float pullStrength = Mathf.Sqrt( Mathf.Pow(mousePosPrev.x - mousePos.x, 2) + Mathf.Pow(mousePosPrev.y - mousePos.y, 2) );
			
			//Debug.Log(mousePos + " ; " + pullStrength + " ; " + mousePosPrev + " ; " + mousePos);
			//gameManager.challengeSuccess = true;
			//progress = 0f;
			//mousePosPrev = Vector3(0f,0f,0f);
			
			
			mousePosPrev = mousePos;
		}
    }
}
