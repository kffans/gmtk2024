using UnityEngine;

public class FakeCursor : MonoBehaviour
{
    public static Transform instance;
	public GameObject objParent;
	public GameObject fakeCursor;
	public Camera cam;
	
	void Awake(){
        instance = Instantiate(fakeCursor, objParent.transform).transform;
	}
	
    void Update(){
		//Debug.Log(cam.ScreenToWorldPoint(Input.mousePosition));
		Vector3 point = new Vector3();
        Vector2 mousePos = new Vector2();
		mousePos.x = Input.mousePosition.x;
        mousePos.y =  2*Input.mousePosition.y - cam.pixelHeight;

        point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
		Debug.Log(point);
        instance.position = point;
    }
}
