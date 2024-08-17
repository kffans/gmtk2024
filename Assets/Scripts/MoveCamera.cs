using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform targetTransform;
	
	// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LeanTween.moveX(gameObject, 500f,6f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
