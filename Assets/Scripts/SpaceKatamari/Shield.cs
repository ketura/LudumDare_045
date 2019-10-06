using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
	private float size = 0;
	public float maxSize;
	public float regen;
	public float damageAmount;
	public Matter EmitterBase;
	public GameObject ShieldModel;

	// Start is called before the first frame update
	void Start()
	{
		if(EmitterBase == null)
		{
			EmitterBase = GetComponentInParent<Matter>();
		}
		
	}

// Update is called once per frame
	void Update()
	{
		if (EmitterBase.Attached)
		{
			ShieldModel.SetActive(true);
		}
		else
		{
			ShieldModel.SetActive(false);
			return;
		}
			

		if (size < maxSize)
		{
			size += regen * Time.deltaTime;
		}

		ShieldModel.transform.localScale = new Vector3(size, size, size);
		Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, size/2);
		foreach (Collider c in hitColliders)
		{
			var bullet = c.GetComponent<Bullet>();
			if(bullet != null)
			{
				OnBulletHit(bullet);
			} 
		}
	}

	private void OnBulletHit(Bullet bullet)
    {
        Ship ship = transform.root.GetComponent<Ship>();
        //get the bullet component and read damage from that
        if (bullet.gameObject.GetComponent<Bullet>() != null && bullet.gameObject.GetComponent<Bullet>().myTeam != ship.currentTeam)
		{
			Destroy(bullet.gameObject);
			size -= bullet.Damage;
		}
	}
}
