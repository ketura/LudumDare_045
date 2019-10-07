using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public enum Team { Neutral, Player, Enemy };

    public Team currentTeam = Team.Neutral;
    public bool Active = true;

	public float DeathForce = 10.0f;

	public Transform ModuleAnchor;
	public List<GameObject> ChildModules;

	public AudioClip DeathSound;
	public AudioClip HitSound;

	public List<GameObject> LootTable;


    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void DestroyShip()
	{
		foreach(var child in ChildModules)
		{
			child.transform.SetParent(null);
			var rb = child.GetComponent<Rigidbody>();
			if(rb != null)
			{
				rb.AddExplosionForce(DeathForce, this.transform.position, 2.0f);
			}
		}

		AudioManager.Instance.PlayClip(DeathSound);

		foreach(var go in LootTable)
		{
			var randomPoint = Random.insideUnitCircle;
			var point = new Vector3(randomPoint.x, 0, randomPoint.y);
			point.Normalize();
			point *= 3.0f;


			var loot = Instantiate(go, transform.position + point, Quaternion.identity);
			var rb = loot.GetComponent<Rigidbody>();

			if(rb!= null)
			{
				rb.AddForce(point * 10);
			}
		}
	}

	public void DestroyModule(Matter matter)
	{
		if(ChildModules.Contains(matter.gameObject))
		{
			ChildModules.Remove(matter.gameObject);

			AudioManager.Instance.PlayClip(HitSound);
		}
	}
}
