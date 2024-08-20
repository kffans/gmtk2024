using UnityEngine;
using UnityEngine.UI;

public class Sword : MonoBehaviour
{
    private static Quaternion Identity = Quaternion.identity;
	
	public GameManager gameManager;
	public Transform hand;
	public Transform arms;
	public RawImage armsImage;
	private int armsLevel = 0;
	public Texture2D[] armsTexture;

	public float progress = 0f;
	public float armsProgress = 0f;
	public static float swingRotationMin = -130;
	public static float swingRotationMax = 10;
	public static float ChallengeFactor = 1f;
	
	
	public TMPro.TextMeshProUGUI debug;
	
	public bool tutorialDone = false;
	public TMPro.TextMeshProUGUI tutorialText;
	
	public enum DragState{
		None,
		Fresh,
		ForceEnd
	}
	
	public DragState dragState;
	
	void Start(){
		dragState = DragState.None;
		armsImage = arms.GetComponent<RawImage>();
		ChallengeFactor = 1f;
	}

    void Update() {
        if(gameManager.currentPhase == GameManager.Phase.Challenge){
			if(!tutorialDone){
				tutorialDone = true;
				LeanTween.value(tutorialText.gameObject, updateTutorialTextColor, new Color(1f,1f,1f,0f), new Color(1f,1f,1f,1f), 0.2f);
			}
			
			if (Input.GetMouseButtonDown(0)) {
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				if(hit.collider != null){
					dragState = DragState.Fresh;
					hand.gameObject.SetActive(true);
					Cursor.visible = false;
					LeanTween.value(tutorialText.gameObject, updateTutorialTextColor, new Color(1f,1f,1f,1f), new Color(1f,1f,1f,0f), 0.2f);
					if(Input.GetMouseButtonDown(0)){
						Cursor.lockState = CursorLockMode.Locked;
					}
				}
			}
			//AudioManager.instance.PlaySFX("button_click");
			if(dragState == DragState.Fresh){
				hand.gameObject.SetActive(true);
				
				Vector3 forceVector = Quaternion.Euler(0, 0, (swingRotationMin+progress+90f)) * Vector3.up;
				forceVector = new Vector3(forceVector.x, forceVector.y,0f);
				float rotation = (Input.GetAxis("Mouse X")*forceVector.x + Input.GetAxis("Mouse Y")*forceVector.y) * ChallengeFactor; // * Time.deltaTime
				if(progress+rotation > 0){
					progress+=rotation;
					armsProgress+=rotation;
				}
				
				//arms image
				changeArms(-130, -76-18, 0, 0); //-130 -76
				changeArms(-76-18, 8-40, -16, 1);//-76 8
				changeArms(8-40, 10, -40, 2);//8 10
				
				RotateZTo(gameObject.transform, swingRotationMin + progress);
				RotateZTo(arms, armsProgress);
				hand.position = gameManager.currentSwordBlade.position;
				
				
				if(progress >= swingRotationMax - swingRotationMin){ //-130 -> 10    winning condition
					hand.gameObject.SetActive(false);
					armsLevel=0;
					progress=0f;
					gameManager.challengeSuccess = true;
				}
			}
		}
    }
	void updateTutorialTextColor(Color val){
		tutorialText.color = val;
	}
	
	public void changeArms(float min, float max, float offset, int level){
		if(progress >= min - swingRotationMin && progress < max - swingRotationMin && armsLevel!=level){ //-75
			if(armsLevel>level) armsProgress = (max-min) + offset; else armsProgress = offset;
			armsLevel=level;
			armsImage.texture = armsTexture[armsLevel];
		}
	}
	
	public static Vector3 XY(Vector3 vector, float z=0f){ vector.z=z; return vector; }
	
	public static void RotateZTo(Transform obj, float z){
		Vector3 angles = obj.rotation.eulerAngles;
		obj.rotation = Identity;
		obj.Rotate(XY(angles, z));
	}
}
