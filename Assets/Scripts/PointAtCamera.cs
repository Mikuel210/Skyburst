using UnityEngine;

public class PointAtCamera : MonoBehaviour {

	private Camera camera;

	void Start() => camera = GameObject.Find("RenderingCamera").GetComponent<Camera>();
	
	void Update()
	{
		Transform cameraTransform = camera.transform;
		transform.LookAt(cameraTransform.position);
		transform.forward *= -1;
		transform.eulerAngles = new(transform.eulerAngles.x, transform.eulerAngles.y, 0);
	}
}