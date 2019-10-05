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
	public float CameraTetherDistance = 10.0f;
	public Vector3 CameraOffset = new Vector3(0,7,-5);
	public float CameraY = 7;

	public Rigidbody Rigidbody;

	private Camera Camera;
	private Vector3 velocity = Vector3.zero;

	// Start is called before the first frame update
	void Start()
  {
		if (Player == null)
			Debug.LogError($"Forgot to set {nameof(Player)}!");

		Camera = GetComponent<Camera>();
		//Rigidbody = GetComponent<Rigidbody>();
	}

  // Update is called once per frame
  void Update()
  {
		float interpolation = CameraSpeed * Time.deltaTime;

		Vector3 target = Player.transform.position + CameraOffset;

		//Vector3 position = Camera.transform.position;
		//position.z = Mathf.Lerp(Camera.transform.position.z, Player.transform.position.z - CameraOffset, interpolation);
		//position.x = Mathf.Lerp(Camera.transform.position.x, Player.transform.position.x, interpolation);
		//position.y = CameraY;

		//float smooth = CameraSmooth;
		//float dist = Vector3.Distance(position, Camera.transform.position);

		//smooth = Mathf.Lerp(0.1f, 0.3f, (1 / dist) - 1);

		//Camera.transform.position = Vector3.SmoothDamp(Camera.transform.position, position, ref velocity, smooth);

		Camera.transform.position = target;
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