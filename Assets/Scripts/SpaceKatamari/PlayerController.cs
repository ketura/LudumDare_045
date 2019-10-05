using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
	public PlayerKatamari Player;

	public float MovementSpeed = 0.1f;

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
	}
}
