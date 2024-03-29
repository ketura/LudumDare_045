﻿using System.Collections;
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

	public float MomentumDamageMultiplier = 0.05f;

	public Rigidbody Rigidbody;
	public Collider CaptureCollider;
	public Collider PhysicsCollider;

	public AudioClip HitSound;
	public AudioClip DeathSound;

    // Start is called before the first frame update
    void Start()
  {
		Rigidbody = GetComponent<Rigidbody>();

		if (CaptureCollider == null)
			Debug.LogError($"Forgot to set {nameof(CaptureCollider)} on {this.gameObject.name}!");

		if (PhysicsCollider == null)
			Debug.LogError($"Forgot to set {nameof(PhysicsCollider)} on {this.gameObject.name}!");

		PhysicsCollider.enabled = false;
		CaptureCollider.enabled = true;
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

        Weapon[] weapons = GetComponentsInChildren<Weapon>();
        foreach (Weapon weapon in weapons)
        {
            weapon.UpdateShip();
        }

        CaptureCollider.isTrigger = true;
		PhysicsCollider.enabled = true;
	}

	public virtual void DetachObject(Vector3 explosionPos, float explosionSize)
	{
		if (ParentKatamari == null)
			return;

		Attached = false;
		transform.SetParent(null);

		Rigidbody.isKinematic = false;
		Rigidbody.AddExplosionForce(10.0f, explosionPos, explosionSize * ExplosionMultiplier);

		CaptureCollider.isTrigger = false;
		PhysicsCollider.enabled = false;

		Weapon[] weapons = GetComponentsInChildren<Weapon>();
		foreach (Weapon weapon in weapons)
		{
			weapon.UpdateShip();
		}


		ParentKatamari = null;
	}

	public void OnTriggerEnter(Collider other)
	{
		if (!Active || ParentKatamari == null)
			return;
		//Debug.Log("Matter Trigger Enter");
		Matter otherMatter = other.GetComponentInParent<Matter>();
		if (otherMatter == null || other.isTrigger)
			return;

		if (otherMatter.Attached || !otherMatter.Active)
			return;

		Ship ship = other.GetComponentInParent<Ship>();
		if (ship != null)
		{
			float momentum = (ParentKatamari.MasterRigidbody.mass * ParentKatamari.MasterRigidbody.velocity).magnitude * MomentumDamageMultiplier;
			Debug.Log($"{gameObject.name} hitting {otherMatter.name} with momentum {momentum}");

			otherMatter.Damage((int)(momentum));

			return;
		}
			

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
		if(!Invincible)
		{
			StartCoroutine(DamageBlink());
		}
		

		if (Mass <= 0)
		{
			DestroyMatter();
		}
		else
		{
			AudioManager.Instance.PlayClip(HitSound);
			//Rigidbody.mass = Mass;
		}
	}

	IEnumerator DamageBlink()
	{
		float blinktime = 0.1f;

		Renderer[] childrenRenderers = gameObject.GetComponentsInChildren<Renderer>();
		for (float t = 0; t < 4; t++)
		{
			foreach (Renderer r in childrenRenderers)
			{
				if(r != null)
				{
					r.enabled = !r.enabled;
				}
                
			}
			yield return new WaitForSeconds(blinktime);
		}

		foreach (Renderer r in childrenRenderers)
		{
			if (r != null)
			{
				r.enabled = true;
			}
		}
	}

    public void DestroyMatter(bool destroy = true)
	{
		AudioManager.Instance.PlayClip(DeathSound);
		if (gameObject.tag == "Player")
		{
			ParentKatamari.ChangeState(PlayerState.Killed);
		}
		else if (ParentKatamari != null)
		{
			ParentKatamari.DestroyAttached(this, false);

			if (destroy && this.gameObject != null)
			{
				Destroy(this.gameObject);
			}
		}
		else
		{
			var ship = GetComponent<Ship>();
			if (ship != null)
			{
				ship.DestroyShip();
			}

			var parentShip = GetComponentInParent<Ship>();
			if(parentShip != null)
			{
				parentShip.DestroyModule(this);
			}

			if (destroy && this.gameObject != null)
			{
				Destroy(this.gameObject);
				MapBounds.Instance.RemoveMatterFromSpawnList(this);
			}

			
		}
	}
    
}
