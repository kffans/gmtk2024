using UnityEngine;

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
	
	public Transform cameraRect;
	private Transform enemy;
	public GameObject enemyParent;
	public EnemyData[] enemyData;
	private Transform targetTransform;
	
	public Phase currentPhase;
	private float timeCount = 0f;
	public int stage = 0;
	
	public bool challengeSuccess = false; //whether we succeded in slaying the monster
	
	public float durationIntro = 3f;       bool doneOnceIntro = false;
	public float durationChallenge = 5f;   bool doneOnceChallenge = false;
	public float durationEnemyDeath = 3f;  bool doneOnceEnemyDeath = false;
	public float durationPlayerDeath = 0f; bool doneOncePlayerDeath = false;
	public float durationWin = 0f; 		   bool doneOnceWin = false;
	
    void Start() {
		currentPhase = Phase.Intro;
    }

    void Update() {
	Beginning:
        switch(currentPhase){
			case Phase.Intro:
				if(!doneOnceIntro) { //things here are called only once when entering this phase
					doneOnceIntro = true;
					
					enemy = Instantiate(enemyData[stage].prefab, enemyParent.transform).transform; //create enemy
					enemy.name = enemyData[stage].name;
					cameraRect.position = new Vector3(0f,0f,-10f); //reset camera position
					LeanTween.move(cameraRect.gameObject, new Vector3(enemy.position.x, enemy.position.y, -10), 1f).setEase(LeanTweenType.easeOutQuad);
					LeanTween.move(cameraRect.gameObject, new Vector3(0f, 0f, -10f), 0.3f).setDelay(3f).setEase(LeanTweenType.easeOutQuad);
					Camera cam = cameraRect.GetComponent<Camera>();
					LeanTween.value(cameraRect.gameObject, cam.orthographicSize, enemyData[stage].zoom, 1.6f) //camera zoom
								.setEase(LeanTweenType.easeOutExpo)
								.setLoopPingPong()
								.setRepeat(2)
								.setOnUpdate((float flt) => {cam.orthographicSize = flt;});
					durationChallenge = enemyData[stage].durationChallenge;
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
					
					Debug.Log("You've been defeated!");
				}
				
				break;
				
				
			case Phase.Win:
				if(!doneOnceWin) {
					doneOnceWin = true;
					
					Debug.Log("You've won!");
				}
				
				break;
		}
    }
}
