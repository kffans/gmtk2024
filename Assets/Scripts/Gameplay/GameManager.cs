using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum Phase {
		None,
		Intro,
		Challenge,
		EnemyDeath,
		PlayerDeath,
		SwordGrow,
		Win
	}
	
	[System.Serializable]
	public struct EnemyData
	{
		public GameObject prefab;
		public string name;
		public string sfxName;
		public float durationChallenge;
		public float challengeFactor;
		public int swordLevelUp;
		public Vector2 sway;  //X - range of sway rotation, Y - duration of sway
		public float zoom;
		public Vector2 zoomDisplace;
		public AnimationCurve curve;
	}
	
	//cursor
	public Texture2D cursorPointer;
	
	//sword
	public GameObject swordHandle;
	private int swordLevel = 0;
	public GameObject[] swordBlades;
	public Transform currentSwordBlade;
	public AnimationCurve swordCurve;
	
	//arms
	public RawImage arms;
	
	//enemy and enemy targets
	public Transform cameraRect;
	private Transform enemy;
	public GameObject enemyParent;
	public EnemyData[] enemyData;
	private Transform targetTransform;
	
	//phase, stage and time
	public Phase currentPhase;
	private float timeCount = 0f;
	public int stage = 0;
	
	public bool challengeSuccess = false; //whether we succeded in slaying the monster
	
	public AnimationCurve introCurve;
	public AnimationCurve enemySway;
	public TMPro.TextMeshProUGUI enemyText;
	public float durationIntro = 3f;       bool doneOnceIntro = false;
	public float durationChallenge = 5f;   bool doneOnceChallenge = false;
	public float durationEnemyDeath = 4f;  bool doneOnceEnemyDeath = false;
	public float durationPlayerDeath = 5f; bool doneOncePlayerDeath = false;
	public float durationSwordGrow = 3f;   bool doneOnceSwordGrow = false;
	public float durationWin = 5f; 		   bool doneOnceWin = false;
	
	public GameObject canvas;
	
	public GameObject gameOverBlackout;
	public TMPro.TextMeshProUGUI gameOverText;
	public Color gameOverTextColor;
	
	public GameObject gameWonBlackout;
	public TMPro.TextMeshProUGUI gameWonText;
	public Color gameWonTextColor;
	
	
	
	
    void Start() {
		currentPhase = Phase.Intro;
		durationIntro = 3f;
		durationEnemyDeath = 4f;
		durationPlayerDeath = 5f;
		durationSwordGrow = 0f;
		durationWin = 6f;
		AudioManager.instance.PlayMusic("fight");
    }

    void Update() {
		if(Input.GetKeyDown(KeyCode.Q)){
			Cursor.lockState = CursorLockMode.None;
			SceneManager.LoadScene(0); 
		}
		
	Beginning:
        switch(currentPhase){
			case Phase.Intro:
				if(!doneOnceIntro) { //things here are called only once when entering this phase
					doneOnceIntro = true;
					
					AudioManager.instance.PlaySFX(enemyData[stage].sfxName);
					Sword.ChallengeFactor = enemyData[stage].challengeFactor;
					currentSwordBlade = Instantiate(swordBlades[swordLevel], swordHandle.transform).transform; //create blade
					Sword.RotateZTo(swordHandle.transform, Sword.swingRotationMin);
					arms.texture = swordHandle.GetComponent<Sword>().armsTexture[0];
					Sword.RotateZTo(arms.GetComponent<Transform>(),0);
					
					enemy = Instantiate(enemyData[stage].prefab, enemyParent.transform).transform; //create enemy
					enemy.name = enemyData[stage].name;
					cameraRect.position = new Vector3(0f,0f,-10f); //reset camera position
					LeanTween.move(cameraRect.gameObject, new Vector3(enemy.position.x+enemyData[stage].zoomDisplace.x, enemy.position.y+enemyData[stage].zoomDisplace.y, -10), 1f).setEase(LeanTweenType.easeOutQuad);
					LeanTween.move(cameraRect.gameObject, new Vector3(0f, 0f, -10f), 0.3f).setDelay(3f).setEase(LeanTweenType.easeOutQuad);
					Camera cam = cameraRect.GetComponent<Camera>();
					LeanTween.value(cameraRect.gameObject, cam.orthographicSize, enemyData[stage].zoom, 3.3f) //camera zoom
								.setEase(introCurve)
								.setOnUpdate((float flt) => {cam.orthographicSize = flt;});
					durationChallenge = enemyData[stage].durationChallenge;
					LeanTween.value(enemyText.gameObject, updateEnemyTextColor, new Color(1f,1f,1f,0f), new Color(1f,1f,1f,1f), 2.5f).setEase(introCurve);
					enemyText.text = enemyData[stage].name;
				}
			
				
			
			
				if(timeCount < durationIntro) { timeCount += Time.deltaTime; }
				else { timeCount=0f; currentPhase = Phase.Challenge; doneOnceIntro = false; goto Beginning;}
				break;
				
				
			case Phase.Challenge:
				if(!doneOnceChallenge) {
					doneOnceChallenge = true;
					
					LeanTween.rotateZ(enemy.gameObject,enemyData[stage].sway.x,enemyData[stage].sway.y).setEase(enemySway).setLoopPingPong().setRepeat(-1);
					targetTransform = GameObject.FindGameObjectsWithTag("Deathline")[0].transform;
					LeanTween.moveX(enemy.gameObject, targetTransform.position.x, durationChallenge).setEase(enemyData[stage].curve);
				}

				if(challengeSuccess){
					challengeSuccess = false;
					timeCount=0f; currentPhase = Phase.EnemyDeath; doneOnceChallenge = false; goto Beginning;
				}
					
				if(timeCount < durationChallenge) { timeCount += Time.deltaTime; }
				else { timeCount=0f; currentPhase = Phase.PlayerDeath; doneOnceChallenge = false; goto Beginning;}
				break;
				
				
			case Phase.EnemyDeath:
				if(!doneOnceEnemyDeath) {
					doneOnceEnemyDeath = true;
					
					if(enemyData[stage].swordLevelUp>0){ //level up miecza
						swordLevel += enemyData[stage].swordLevelUp;
					}
					Camera cam = cameraRect.GetComponent<Camera>();
					
					//destroy enemy
					LeanTween.scaleY(enemy.gameObject, 0f, 0f);
					//make new one in designated place
					Transform enemyTemp = Instantiate(enemyData[stage].prefab, enemyParent.transform).transform;
					//set position of enemy
					enemyTemp.position = new Vector3(10-8*stage,enemyTemp.position.y,0);
					//zoom the camera
					cam.orthographicSize = 300+20*swordLevel;
					//leantween sword & arms rotation, change arms midway
					Transform armsTr = arms.GetComponent<Transform>();
					Sword.RotateZTo(swordHandle.transform, Sword.swingRotationMax);
					Sword.RotateZTo(armsTr,1);
					
					LeanTween.rotateZ(swordHandle, 130f, 0.4f).setEase(swordCurve).setDelay(0.2f);
					LeanTween.rotateZ(armsTr.gameObject, 119f, 0.4f).setEase(swordCurve).setDelay(0.2f);
					//play sound
					if(stage<6)
						AudioManager.instance.PlaySFX("slay-soft");
					else
						AudioManager.instance.PlaySFX("slay-hard");
					//death anim with scale y
					LeanTween.scaleY(enemyTemp.gameObject, 0f, 0.6f).setDelay(1.6f);
					//shake camera
					//leantween camera zoom out,
					
					
					
					if(enemyData.Length == stage+1){
						timeCount=0f; currentPhase = Phase.Win; doneOnceEnemyDeath = false; goto Beginning;
					}
					else stage++;
				}
				cameraRect.position = new Vector3(currentSwordBlade.position.x, currentSwordBlade.position.y+50*swordLevel, -10);
				
				
				if(timeCount < durationEnemyDeath) { timeCount += Time.deltaTime; }
				else { 
					cameraRect.GetComponent<Camera>().orthographicSize = 540;
					timeCount=0f; currentPhase = Phase.Intro;
					if(enemyData[stage-1].swordLevelUp>0) currentPhase = Phase.SwordGrow;
					doneOnceEnemyDeath = false; goto Beginning;
				}
				break;
				
				
			case Phase.PlayerDeath:
				if(!doneOncePlayerDeath) {
					doneOncePlayerDeath = true;
					Debug.Log("You've been defeated!");
					
					
					//bell sound and a scream after
					AudioManager.instance.PlayMusic("silence");
					AudioManager.instance.PlaySFX("death_with_bells");
					gameOverBlackout.SetActive(true);
					LeanTween.value(gameOverText.gameObject, updateGameOverTextColor, new Color(0f,0f,0f,0f), gameOverTextColor, 3.5f).setEase(introCurve).setDelay(1.5f);
					canvas.SetActive(false);
				}
				
				
				if(timeCount < durationPlayerDeath) { timeCount += Time.deltaTime; }
				else { SceneManager.LoadScene(0); }
				break;
				
				
			case Phase.SwordGrow:
				if(!doneOnceSwordGrow) {
					doneOnceSwordGrow = true;
					Debug.Log("aa");
					//camera to different place to show blade
					//cameraRect.position = new Vector3(0f,0f,-10f); //reset camera position
					//"The blade has visibly grown in size...!"
				}
				
				if(timeCount < durationSwordGrow) { timeCount += Time.deltaTime; }
				else { timeCount=0f; currentPhase = Phase.Intro; doneOnceSwordGrow = false; goto Beginning; }
				break;
				
				
			case Phase.Win:
				if(!doneOnceWin) {
					doneOnceWin = true;
					Debug.Log("You've won!");
					
					//play cheer music on, load end credits scene
					AudioManager.instance.PlayMusic("credits");
					gameWonBlackout.SetActive(true);
					LeanTween.value(gameWonText.gameObject, updateGameWonTextColor, new Color(1f,1f,1f,0f), gameWonTextColor, 4.5f).setEase(introCurve).setDelay(1.1f);
					canvas.SetActive(false);
				}
				
				if(timeCount < durationWin) { timeCount += Time.deltaTime; }
				else { SceneManager.LoadScene(2); }
				break;
		}
    }
	
	
	void updateEnemyTextColor(Color val){
		enemyText.color = val;
	}
	void updateGameOverTextColor(Color val){
		gameOverText.color = val;
	}
	void updateGameWonTextColor(Color val){
		gameWonText.color = val;
	}
}
