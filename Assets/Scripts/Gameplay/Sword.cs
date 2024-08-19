using UnityEngine;

public class Sword : MonoBehaviour
{
    private static Quaternion Identity = Quaternion.identity;
	
	public GameManager gameManager;
	public Transform hand;
	private Vector3 mousePosPrev = new Vector3(0f,0f,0f);
	private Vector3 mousePos;

	public float progress = 0f;
	private float swingRotationMin = -130;
	private float swingRotationMax = 0;
	
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
        if(gameManager.currentPhase == GameManager.Phase.Challenge && !Pause.isPaused){
			if (Input.GetMouseButtonDown(0)) {
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

				if(hit.collider != null){
					dragState = DragState.Fresh;
					Cursor.lockState = CursorLockMode.Locked;
					hand.gameObject.SetActive(true);
				}
			}
			
			if(dragState == DragState.Fresh){
				progress+=0.3f;
				//progress
				//transform.Rotate(new Vector3(0, 0, rotation));
				RotateZTo(gameObject.transform, swingRotationMin + progress);
				mousePos = Input.mousePosition;
				
				hand.position = gameManager.currentSwordBlade.position;

				if(Input.GetMouseButtonUp(0)){
					Cursor.lockState = CursorLockMode.None;
					dragState = DragState.None;
					hand.gameObject.SetActive(false);
				}
				
				if(progress >= swingRotationMax - swingRotationMin){ //-130 -> 0
					Cursor.lockState = CursorLockMode.None;
					dragState = DragState.None;
					hand.gameObject.SetActive(false);
					gameManager.challengeSuccess = true;
				}
			}
			
			
			
			//float pullStrength = Mathf.Sqrt( Mathf.Pow(mousePosPrev.x - mousePos.x, 2) + Mathf.Pow(mousePosPrev.y - mousePos.y, 2) );
			
			//Debug.Log(mousePos + " ; " + pullStrength + " ; " + mousePosPrev + " ; " + mousePos);
			//progress = 0f;
			//mousePosPrev = Vector3(0f,0f,0f);
			
			
			mousePosPrev = mousePos;
		}
		if(Pause.isPaused){
			Cursor.lockState = CursorLockMode.None;
			dragState = DragState.None;
			hand.gameObject.SetActive(false);
		}
    }
	
	public static Vector3 XY(Vector3 vector, float z=0f){ vector.z=z; return vector; }
	
	public static void RotateZTo(Transform obj, float z){
		Vector3 angles = obj.rotation.eulerAngles;
		obj.rotation = Identity;
		obj.Rotate(XY(angles, z));
	}
}
