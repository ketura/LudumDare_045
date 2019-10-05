using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Matter : MonoBehaviour
{
	public PlayerKatamari ParentKatamari;

	public int Mass;
	public bool Attached;
    public bool Active = true;

	public Rigidbody Rigidbody;
	public Collider Collider;

  // Start is called before the first frame update
  void Start()
  {
		Rigidbody = GetComponent<Rigidbody>();

		if(Collider == null)
			Debug.LogError($"Forgot to set {nameof(Collider)} on {this.gameObject.name}!");
	}

  // Update is called once per frame
  void Update()
  {
        if (Mass <= 0) {
            Destroy(this.gameObject);
        }
  }

	public virtual void CaptureObject(PlayerKatamari katamari)
	{
		Attached = true;
		ParentKatamari = katamari;
		transform.SetParent(katamari.CaptureAnchor, true);
		Rigidbody.isKinematic = true;
		Rigidbody.velocity = Vector3.zero;

        Ship ship = GetComponent<Ship>();
        if (ship != null)
        {
            ship.Active = false;
        }

		Collider.isTrigger = true;
	}

	public void OnTriggerEnter(Collider other)
	{
		Debug.Log("Matter Trigger Enter");
		Matter otherMatter = other.GetComponentInParent<Matter>();
		if (otherMatter == null)
			return;

		if (otherMatter.Attached)
			return;

		ParentKatamari.OnMatterTouch(otherMatter);
	}
    
}
