using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Matter
{
  
	public float timer=1;
	public float explosionRadius;
	public int damage;
    public Color explodingColor;
    // Start is called before the first frame update
    void Start()
	{
        

    }
	public override void CaptureObject(PlayerKatamari katamari)
	{
		base.CaptureObject(katamari);
        StartCoroutine(Blink());
        StartCoroutine(Countdown(timer));
		Debug.Log("Counting down mine");
	}
    IEnumerator Blink()
    {
        float blinktime=0.1f;
        Renderer mineRenderer = base.CaptureCollider.gameObject.GetComponent<Renderer>();
        for (float t=0;t<timer*2/3;t=+blinktime)
        {
            if (mineRenderer.material.color == Color.white)
            {
                mineRenderer.material.color = explodingColor;
            }
            else {
                mineRenderer.material.color = Color.white;
            }
            yield return new WaitForSeconds(blinktime);
        }
        
        
        for (float i = 0; i < timer/ 3; i = +blinktime/10)
        {
            if (mineRenderer.material.color == Color.white)
            {
                mineRenderer.material.color = explodingColor;
            }
            else
            {
                mineRenderer.material.color = Color.white;
            }
            yield return new WaitForSeconds(blinktime/10);
        }
    }

    IEnumerator Countdown(float timer)
	{
		yield return new WaitForSeconds(timer);
		explode();
	}

	public void explode() 
	{
		Collider[] hitColliders=   Physics.OverlapSphere(this.transform.position, explosionRadius);
		List<Matter> affectedMatter = new List<Matter>();
		Matter katamari = null;
		
		foreach (Collider c in hitColliders)
		{
			var matter = c.GetComponentInParent<Matter>();

			if(matter == null)
			{
				matter = c.GetComponent<Matter>();
			}
           
			if (matter != null && !affectedMatter.Contains(matter) && matter != this)
			{
				if(matter.gameObject.tag == "Player")
				{
					katamari = matter;
				}
				else
				{
					affectedMatter.Add(matter);
				}
				
			}
		}

		foreach(var matter in affectedMatter)
		{
			matter.Damage(damage);
		}

		if(katamari != null)
		{
			katamari.Damage(damage);
		}

		if (this.gameObject.GetComponent<ParticleSystem>()!=null) 
		{
			this.gameObject.GetComponent<ParticleSystem>().Play();
		}
		base.CaptureCollider.gameObject.GetComponent<Renderer>().enabled = false ;

		this.DestroyMatter(false);

		Destroy(this.gameObject,1);

	}

}
