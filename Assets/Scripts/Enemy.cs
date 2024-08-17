using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float duration = 5f;
	public Transform targetTransform;
	
	// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LeanTween.moveX(gameObject, targetTransform.position.x-100, duration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
