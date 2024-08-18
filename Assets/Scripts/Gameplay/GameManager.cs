using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Phase {
		None,
		Intro,
		Challenge,
		EnemyDeath,
		PlayerDeath
	}
	
	[System.Serializable]
	public struct EnemyData
	{
		public GameObject prefab;
		public string name;
		public float challengeDuration;
		public float zoom;
	}
	
	public Transform cameraRect;
	public Transform enemy;
	public GameObject enemyParent;
	
	public EnemyData[] enemyData;
	
	public Phase currentPhase;
	private float timeCount = 0f;
	public int stage = 0;
	
	public float durationIntro = 3f;       bool doneOnceIntro = false;
	public float durationChallenge = 5f;   bool doneOnceChallenge = false;
	public float durationEnemyDeath = 3f;  bool doneOnceEnemyDeath = false;
	public float durationPlayerDeath = 6f; bool doneOncePlayerDeath = false;
	
    void Start() {
		currentPhase = Phase.Intro;
    }

    void Update() {
	Beginning:
        switch(currentPhase){
			case Phase.Intro:
				if(!doneOnceIntro) {
					doneOnceIntro = true;
					enemy = Instantiate(enemyData[stage].prefab, enemyParent.transform).transform;
					//instantiate monster?
					cameraRect.position = new Vector3(0f,0f,-10f);
					//enemy = GameObject.FindGameObjectsWithTag("Enemy")[0].transform;
					LeanTween.move(cameraRect.gameObject, new Vector3(enemy.position.x, enemy.position.y, -10), 1f).setEase(LeanTweenType.easeOutQuad);
					LeanTween.move(cameraRect.gameObject, new Vector3(0f, 0f, -10f), 0.3f).setDelay(3f).setEase(LeanTweenType.easeOutQuad);
					Camera cam = cameraRect.GetComponent<Camera>();
					LeanTween.value(cameraRect.gameObject, cam.orthographicSize, 400f, 1f)
								.setLoopPingPong()
								.setRepeat(2)
								.setEase(LeanTweenType.easeOutQuad)
								.setOnUpdate((float flt) => {cam.orthographicSize = flt;});
					/*LeanTween.value(cameraRect.gameObject, cam.orthographicSize, 540f, 0.3f)
								.setDelay(2.7f)
								.setEase(LeanTweenType.easeOutQuad)
								.setOnUpdate((float flt) => {cam.orthographicSize = flt;});*/
				}
			
				
			
			
				if(timeCount < durationIntro) { timeCount += Time.deltaTime; }
				else { timeCount=0f; currentPhase = Phase.Challenge; doneOnceIntro = false; goto Beginning;}
				break;
				
				
			case Phase.Challenge:
				//targetTransform = GameObject.FindGameObjectsWithTag("Deathline")[0].transform;
				//LeanTween.moveX(gameObject, targetTransform.position.x-100, duration);
				break;
				
				
			case Phase.EnemyDeath:
				//stage++;
				break;
				
				
			case Phase.PlayerDeath:
				
				break;
		}
    }
}
