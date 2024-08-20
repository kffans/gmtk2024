using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public RawImage whiteOut;
	
	// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LeanTween.value(whiteOut.gameObject, updateColor, new Color(1f,1f,1f,1f), new Color(1f,1f,1f,0f), 0.5f);
					
    }
	
	void updateColor(Color val){
		whiteOut.color = val;
	}
	
}
