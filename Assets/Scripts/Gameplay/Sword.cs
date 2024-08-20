using UnityEngine;
using UnityEngine.UI;

public class Sword : MonoBehaviour
{
    private static Quaternion Identity = Quaternion.identity;
	
	public GameManager gameManager;
	public Transform hand;
	public Transform arms;
	private int armsLevel = 0;

	public float progress = 0f;
	public float armsProgress = 0f;
	public static float swingRotationMin = -130;
	public static float swingRotationMax = 10;
	public float ChallengeFactor = 1f;
	
	public Texture2D[] armsTexture;
	
	public TMPro.TextMeshProUGUI debug;
	
	public enum DragState{
		None,
		Fresh,
		ForceEnd
	}
	
	public DragState dragState;
	
	void Start(){
		dragState = DragState.None;
		ChallengeFactor = 50f;
	}

    void Update() {
        if(gameManager.currentPhase == GameManager.Phase.Challenge){
			
			if (Input.GetMouseButtonDown(0)) {
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				if(hit.collider != null){
					dragState = DragState.Fresh;
					hand.gameObject.SetActive(true);
					if(Input.GetMouseButtonDown(0)){
						Cursor.lockState = CursorLockMode.Locked;
						Cursor.visible = false;
					}
				}
			}
			//AudioManager.instance.PlaySFX("button_click");
			if(dragState == DragState.Fresh){
				hand.gameObject.SetActive(true);
				RotateZTo(gameObject.transform, swingRotationMin + progress);
				RotateZTo(arms, armsProgress);
				
				Vector3 forceVector = Quaternion.Euler(0, 0, (swingRotationMin+progress+90f)) * Vector3.up;
				forceVector = new Vector3(forceVector.x, forceVector.y,0f);
				float rotation = (Input.GetAxis("Mouse X")*forceVector.x + Input.GetAxis("Mouse Y")*forceVector.y) * Time.deltaTime * ChallengeFactor;
				if(progress+rotation > 0){
					progress+=rotation;
					armsProgress+=rotation;
				}
				
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
				
				if(progress >= swingRotationMax - swingRotationMin){ //-130 -> 10    winning condition
					hand.gameObject.SetActive(false);
					armsLevel=0;
					progress=0f;
					gameManager.challengeSuccess = true;
				}
			}
		}
    }
	
	public static Vector3 XY(Vector3 vector, float z=0f){ vector.z=z; return vector; }
	
	public static void RotateZTo(Transform obj, float z){
		Vector3 angles = obj.rotation.eulerAngles;
		obj.rotation = Identity;
		obj.Rotate(XY(angles, z));
	}
}
