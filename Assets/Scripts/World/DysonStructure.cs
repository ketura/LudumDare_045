using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DysonStructure : MonoBehaviour
{
	public GameObject Structure;

	public float RotationRate;
  // Start is called before the first frame update
  void Start()
  {
        
  }

  // Update is called once per frame
  void Update()
  {
		Structure.transform.Rotate(0, RotationRate * Time.deltaTime, 0, Space.Self);
  }
}
