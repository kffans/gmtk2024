using UnityEngine;
using UnityEngine.UI;

public class Sword : MonoBehaviour
{
    private static Quaternion Identity = Quaternion.identity;
	
	public GameManager gameManager;
	public Transform hand;
	private Vector3 mousePosPrev = new Vector3(0f,0f,0f);
	private Vector3 mousePos;
	public Transform arms;
	private int armsLevel = 0;

	public float progress = 0f;
	public float armsProgress = 0f;
	public static float swingRotationMin = -130;
	public static float swingRotationMax = 10;
	
	public Texture2D[] armsTexture;
	
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
				armsProgress+=0.3f;

				RotateZTo(gameObject.transform, swingRotationMin + progress);
				RotateZTo(arms, armsProgress);
				
				mousePos = Input.mousePosition;
				
				//Debug.Log(gameObject.transform.rotation.z*180);
				
				hand.position = gameManager.currentSwordBlade.position;
				
				
				//arms image
				if(progress >= -95 - swingRotationMin && armsLevel==0){ //-75
					Debug.Log("arms2");
					armsLevel++;
					arms.GetComponent<RawImage>().texture = armsTexture[armsLevel];
					armsProgress = -20f;
					RotateZTo(arms, armsProgress);
				}
				if(progress >= -30 - swingRotationMin && armsLevel==1){ //8
					Debug.Log("arms3");
					armsLevel++;
					arms.GetComponent<RawImage>().texture = armsTexture[armsLevel];
					armsProgress = -38f;
					RotateZTo(arms, armsProgress);
				}
				

				if(Input.GetMouseButtonUp(0)){ //if no longer dragging
					Cursor.lockState = CursorLockMode.None;
					dragState = DragState.None;
					hand.gameObject.SetActive(false);
				}
				if(progress >= swingRotationMax - swingRotationMin){ //-130 -> 0    winning condition
					Cursor.lockState = CursorLockMode.None;
					dragState = DragState.None;
					hand.gameObject.SetActive(false);
					
					armsLevel=0;
					progress=0f;
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
