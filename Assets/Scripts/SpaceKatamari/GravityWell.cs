using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWell : MonoBehaviour
{
	public PlayerKatamari MasterKatamari;

	public bool Attracting;

	public float GravityStrength = 1.0f;
	public float Range = 20.0f;

	// Start is called before the first frame update
	void Start()
  {

	}

  // Update is called once per frame
  void Update()
  {
		Collider[] hits = Physics.OverlapSphere(this.transform.position, Range);

		foreach (var hit in hits)
		{

			Matter matter = hit.gameObject.GetComponentInParent<Matter>();
			if (matter == null)
				continue;

			if (matter.Attached)
				continue;

			if (Vector3.Distance(transform.position, hit.transform.position) < Range)
			{
				var vector = transform.position - matter.transform.position;

				vector *= GravityStrength;

				matter.Rigidbody.AddForce(vector);
			}
		}
	}

	//private void OnTriggerStay(Collider other)
	//{
	//	Debug.Log("Gravity Trigger Stay");

	//	if (!Attracting)
	//		return;

	//	Matter matter = other.gameObject.GetComponentInParent<Matter>();
	//	if (matter == null)
	//		return;

	//	if (matter.Attached)
	//		return;

	//	var vector = transform.position - matter.transform.position;

	//	vector *= GravityStrength;

	//	matter.Rigidbody.AddForce(vector);

	//}


	//public void OnTriggerEnter(Collider other)
	//{
	//	//This is only to make sure the katamari's Matter object doesn't make stuff stick to this sphere
	//}

}
