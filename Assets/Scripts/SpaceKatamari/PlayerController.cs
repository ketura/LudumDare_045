using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
	public PlayerKatamari Player;

	public float MovementSpeed = 0.1f;

	public float TractorSpeed = 10.0f;

	public float RotateSpeed = 3.0f;


	private Vector3 OriginalWellPosition = Vector3.zero;

	private MapBounds Map;

  // Start is called before the first frame update
  void Start()
  {
		Map = MapBounds.Instance;
  }

  // Update is called once per frame
  void Update()
  {
		Vector3 movement = Vector2.zero;

		movement.z += Input.GetAxis("Vertical");
		movement.x += Input.GetAxis("Horizontal");

		movement *= MovementSpeed * Time.deltaTime;

		Player.MasterRigidbody.velocity += movement;

		var overshot = Vector3.Distance(Player.transform.position, Map.transform.position);
		if (overshot > Map.WorldRadius)
		{
			float delta = 1 - (Map.WorldRadius / overshot);
			var deltaVector = Vector3.Lerp(Player.transform.position, Map.transform.position, delta);
			Player.MasterRigidbody.velocity -= (deltaVector * Time.deltaTime);
		}


		if(Input.GetButtonDown("Tractor"))
		{
			OriginalWellPosition = Player.Well.transform.position;
			Player.Well.Tractoring = true;
		}

		if(Input.GetButton("Tractor"))
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

		if(Input.GetButtonUp("Tractor"))
		{
			Player.Well.Tractoring = false;
		}

		if (Input.GetButtonDown("Repel"))
		{
			Player.Well.TractorSign = -1.0f;
		}

		if (Input.GetButtonUp("Repel"))
		{
			Player.Well.TractorSign = 1.0f;
		}

	}


}
