using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
	public PlayerKatamari Player;

	public float MovementSpeed = 0.1f;

	public float TractorSpeed = 10.0f;


	private Vector3 OriginalWellPosition = Vector3.zero;

  // Start is called before the first frame update
  void Start()
  {
        
  }

  // Update is called once per frame
  void Update()
  {
		Vector3 movement = Vector2.zero;

		if (Input.GetKey(KeyCode.W))
		{
			movement.z += 1;
		}

		if (Input.GetKey(KeyCode.S))
		{
			movement.z -= 1;
		}

		if (Input.GetKey(KeyCode.A))
		{
			movement.x -= 1;
		}

		if (Input.GetKey(KeyCode.D))
		{
			movement.x += 1;
		}

		movement *= MovementSpeed * Time.deltaTime;

		Player.MasterRigidbody.velocity += movement;

		if(Input.GetMouseButtonDown(0))
		{
			OriginalWellPosition = Player.Well.transform.position;
			Player.Well.Tractoring = true;
		}
		

		if(Input.GetMouseButton(0))
		{
			Vector3 point;

			Plane plane = new Plane(Vector3.up, Vector3.zero);
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float dist;
			if (plane.Raycast(ray, out dist))
			{
				point = ray.GetPoint(dist);
			}
			else
			{
				point = Vector3.zero;
			}

			Player.Well.transform.position = Vector3.Lerp(Player.Well.transform.position, point, TractorSpeed * Time.deltaTime);
		}
		else
		{
			Player.Well.transform.position = Vector3.Lerp(Player.Well.transform.position, Player.transform.position, TractorSpeed * Time.deltaTime);
		}

		if(Input.GetMouseButtonUp(0))
		{
			Player.Well.Tractoring = false;
		}

	}
}
