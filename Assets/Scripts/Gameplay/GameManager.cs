using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum Phase {
		None,
		Intro,
		Challenge,
		EnemyDeath,
		PlayerDeath,
		Win
	}
	
	[System.Serializable]
	public struct EnemyData
	{
		public GameObject prefab;
		public string name;
		public float durationChallenge;
		public float zoom;
		public AnimationCurve curve;
	}
	
	//sword
	public GameObject swordHandle;
	private int swordLevel = 4;
	public GameObject[] swordBlades;
	public Transform currentSwordBlade;
	
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
	public TMPro.TextMeshProUGUI enemyText;
	public float durationIntro = 3f;       bool doneOnceIntro = false;
	public float durationChallenge = 5f;   bool doneOnceChallenge = false;
	public float durationEnemyDeath = 3f;  bool doneOnceEnemyDeath = false;
	public float durationPlayerDeath = 0f; bool doneOncePlayerDeath = false;
	public float durationWin = 0f; 		   bool doneOnceWin = false;
	
	public GameObject gameOverBlackout;
	public GameObject canvas;
	
	
	
    void Start() {
		currentPhase = Phase.Intro;
    }

    void Update() {
	Beginning:
        switch(currentPhase){
			case Phase.Intro:
				if(!doneOnceIntro) { //things here are called only once when entering this phase
					doneOnceIntro = true;
					
					currentSwordBlade = Instantiate(swordBlades[swordLevel], swordHandle.transform).transform; //create blade
					Sword.RotateZTo(swordHandle.transform, Sword.swingRotationMin);
					arms.texture = swordHandle.GetComponent<Sword>().armsTexture[0];
					//rotate arms too
					
					enemy = Instantiate(enemyData[stage].prefab, enemyParent.transform).transform; //create enemy
					enemy.name = enemyData[stage].name;
					cameraRect.position = new Vector3(0f,0f,-10f); //reset camera position
					LeanTween.move(cameraRect.gameObject, new Vector3(enemy.position.x, enemy.position.y, -10), 1f).setEase(LeanTweenType.easeOutQuad);
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
					
					if(enemyData.Length == stage+1){
						timeCount=0f; currentPhase = Phase.Win; doneOnceEnemyDeath = false; goto Beginning;
					}
					else stage++;
				}
				
				if(timeCount < durationEnemyDeath) { timeCount += Time.deltaTime; }
				else { timeCount=0f; currentPhase = Phase.Intro; doneOnceEnemyDeath = false; goto Beginning;}
				break;
				
				
			case Phase.PlayerDeath:
				if(!doneOncePlayerDeath) {
					doneOncePlayerDeath = true;
					
					Cursor.lockState = CursorLockMode.None;
					//bell sound and a scream after
					//music ends
					gameOverBlackout.SetActive(true);
					canvas.SetActive(false);
					Debug.Log("You've been defeated!");
				}
				
				break;
				
				
			case Phase.Win:
				if(!doneOnceWin) {
					doneOnceWin = true;
					
					//play cheer music on, load end credits scene
					Debug.Log("You've won!");
				}
				
				break;
		}
    }
	
	
	void updateEnemyTextColor(Color val){
		enemyText.color = val;
	}
}
