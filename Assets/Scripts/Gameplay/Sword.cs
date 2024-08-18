using UnityEngine;

public class Sword : MonoBehaviour
{
    public GameManager gameManager;
	public bool lol=true;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //LeanTween.rotateAround(gameObject, Vector3.forward, 200f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.currentPhase == GameManager.Phase.Challenge && lol){
			lol=false;
			//gameManager.challengeSuccess = false;
			LeanTween.rotateAround(gameObject, Vector3.forward, 200f, 2f);
			//transform.Rotate(new Vector3(0, 0, rotation));
		}
    }
}
