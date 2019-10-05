using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Matter
{
  
	public float timer=1;
	public float explosionRadius;
	public int damage;

// Start is called before the first frame update
	void Start()
	{
        
	}
	public override void CaptureObject(PlayerKatamari katamari)
	{
		base.CaptureObject(katamari);
		StartCoroutine(Countdown(timer));
		Debug.Log("Counting down mine");
	}

	IEnumerator Countdown(float timer)
	{
		yield return new WaitForSeconds(timer);
		explode();
	}

	public void explode() 
	{
		Collider[] hitColliders=   Physics.OverlapSphere(this.transform.position, explosionRadius);
		foreach (Collider c in hitColliders)
		{
			var matter = c.GetComponentInParent<Matter>();

			if(matter == null)
			{
				matter = c.GetComponent<Matter>();
			}
           
			if (matter != null)
			{
				matter.Mass -= damage;
			}
		}
		Destroy(this.gameObject);

	}

}
