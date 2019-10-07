using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
//[RequireComponent(typeof(Rigidbody))]
public class KatamariCameraController : MonoBehaviour
{
	public PlayerKatamari Player;

	public float CameraSpeed = 10.0f;
	public float CameraSmooth = 0.28f;

	public Vector3 CameraOffset = new Vector3(0,7,-5);
   
	public Rigidbody Rigidbody;
    public float minY;
    public float maxY;
    public float mouseSpeed;
	private Camera Camera;
	private Vector3 velocity = Vector3.zero;

	// Start is called before the first frame update
	void Start()
  {
		if (Player == null)
			Debug.LogError($"Forgot to set {nameof(Player)}!");

		Camera = GetComponent<Camera>();
	}

  // Update is called once per frame
  void FixedUpdate()
  {
		float interpolation = CameraSpeed * Time.deltaTime;
        float OffsetY = Mathf.Clamp(Input.GetAxis("Mouse ScrollWheel") * mouseSpeed + CameraOffset.y, minY, maxY);
        CameraOffset += new Vector3(0, OffsetY, 0);

        Vector3 target = Player.transform.position + CameraOffset;
		Camera.transform.position = Vector3.SmoothDamp(Camera.transform.position, target, ref velocity, CameraSmooth);
        

    }
}


public class DampCamera2D : MonoBehaviour
{
	public Transform target;
	public float smoothTime = 0.3F;
	private Vector3 velocity = Vector3.zero;

	void Update()
	{
		// Define a target position above and behind the target transform
		Vector3 targetPosition = target.TransformPoint(new Vector3(0, 5, -10));

		// Smoothly move the camera towards that target position
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
	}
}