using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKatamari : MonoBehaviour
{
	public Transform CaptureAnchor;
	public List<Matter> CapturedObjects;
	public Rigidbody MasterRigidbody;

	public GravityWell Well;

	public float ConstantTorque = 5.0f;

	public float ChildGravitySize = 2.0f;
	public float ChildGravityStrengthMultiplier = 0.05f;


  // Start is called before the first frame update
  void Start()
  {
		if (CaptureAnchor == null)
			Debug.LogError($"Forgot to set {nameof(CaptureAnchor)} anchor!");

		if (MasterRigidbody == null)
			Debug.LogError($"Forgot to set {nameof(MasterRigidbody)}!");

		if (Well == null)
			Debug.LogError($"Forgot to set {nameof(Well)}!");

		Well.MasterKatamari = this;

		CapturedObjects = new List<Matter>();
	}

  // Update is called once per frame
  void Update()
  {
		MasterRigidbody.AddTorque(0, ConstantTorque * MasterRigidbody.mass * Time.deltaTime, 0);

  }

	public void OnMatterTouch(Matter matter)
	{
		matter.CaptureObject(this);
		Vector3 totalMomentum = (matter.Rigidbody.mass * matter.Rigidbody.velocity) + (MasterRigidbody.mass * MasterRigidbody.velocity);
		MasterRigidbody.mass += matter.Rigidbody.mass;
		MasterRigidbody.velocity = totalMomentum / MasterRigidbody.mass;

		CapturedObjects.Add(matter);
		var well = matter.gameObject.AddComponent<GravityWell>();
		well.Range = ChildGravitySize;
		well.GravityStrength = ChildGravityStrengthMultiplier * matter.Mass;
	}

	public void AttractObject(Matter matter)
	{
		
	}
}
