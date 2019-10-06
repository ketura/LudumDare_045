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
	}

	public void DestroyModule(Matter matter)
	{
		if(ChildModules.Contains(matter.gameObject))
		{
			ChildModules.Remove(matter.gameObject);
		}
	}
}
