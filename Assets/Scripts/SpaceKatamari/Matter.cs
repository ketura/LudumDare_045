using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Matter : MonoBehaviour
{
	public PlayerKatamari ParentKatamari;

	public int Mass = 10;
	public bool Attached;
	public bool Active = true;
	public bool Invincible = false;
	public float ExplosionMultiplier = 1.0f;

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

        Weapon weapon = GetComponent<Weapon>();
        if (weapon != null)
        {
            weapon.UpdateShip();
        }

        Collider.isTrigger = true;
	}

	public virtual void DetachObject(Vector3 explosionPos, float explosionSize)
	{
		if (ParentKatamari == null)
			return;

		Attached = false;
		transform.SetParent(null);

		Rigidbody.isKinematic = false;
		Rigidbody.AddExplosionForce(10.0f, explosionPos, explosionSize * ExplosionMultiplier);

		Collider.isTrigger = false;

		ParentKatamari = null;
	}

	public void OnTriggerEnter(Collider other)
	{
		Debug.Log("Matter Trigger Enter");
		Matter otherMatter = other.GetComponentInParent<Matter>();
		if (otherMatter == null)
			return;

		if (otherMatter.Attached)
			return;

		if(ParentKatamari != null)
		{
			ParentKatamari.OnMatterTouch(otherMatter, this);
		}
		
	}

	public void Damage(int amount)
	{
		if (Invincible)
			return;

		Mass -= amount;

		if (Mass <= 0)
		{
			if(ParentKatamari != null)
			{
				ParentKatamari.DestroyAttached(this);
			}
			Destroy(this.gameObject);
		}
		else
		{
			Rigidbody.mass = Mass;
		}
	}
    
}
