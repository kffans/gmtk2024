using UnityEngine;

public class Sword : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LeanTween.rotateAround(gameObject, Vector3.forward, 200f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
