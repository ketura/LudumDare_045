using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GravityWell))]
public class PlayerKatamari : MonoBehaviour
{
	public Transform CaptureAnchor;
	public List<Matter> CapturedObjects;
	public Rigidbody MasterRigidbody;

	public GravityWell Well;


  // Start is called before the first frame update
  void Start()
  {
		if (CaptureAnchor == null)
			Debug.LogError($"Forgot to set {nameof(CaptureAnchor)} anchor!");

		if (MasterRigidbody == null)
			Debug.LogError($"Forgot to set {nameof(MasterRigidbody)}!");

		Well = GetComponent<GravityWell>();

		Well.MasterKatamari = this;

		CapturedObjects = new List<Matter>();
	}

  // Update is called once per frame
  void Update()
  {


  }

	public void OnMatterTouch(Matter matter)
	{
		matter.CaptureObject(this);
		CapturedObjects.Add(matter);
	}

	public void AttractObject(Matter matter)
	{
		
	}
}
