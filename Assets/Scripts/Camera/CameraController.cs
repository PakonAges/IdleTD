using UnityEngine;

public class CameraController : MonoBehaviour {

    public float panSpeed = 1.2f;
	
	void Update () {

        if (Input.GetKey("w"))
        {
            transform.Translate(Vector3.up * panSpeed * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey("s"))
        {
            transform.Translate(Vector3.down * panSpeed * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey("a"))
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey("d"))
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.Self);
        }
    }
}
