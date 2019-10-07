using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedExit : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		ExitHandler.Instance.GracefulExit(5.0f);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
