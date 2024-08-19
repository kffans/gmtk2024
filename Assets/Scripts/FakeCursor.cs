using UnityEngine;

public class FakeCursor : MonoBehaviour
{
    public static Transform instance;
	public GameObject objParent;
	public GameObject fakeCursor;
	
	void Awake(){
        instance = Instantiate(fakeCursor, objParent.transform).transform;
	}
	
    void Update(){
        instance.position = Input.mousePosition;
    }
}
