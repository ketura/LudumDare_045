using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float lifetime = 5f;
	public Ship.Team myTeam = Ship.Team.Neutral;
	public float speed = 10f;
	public int Damage = 1;
    public bool Piercing = false;

    private List<Matter> hitRecord = new List<Matter>();

	public AudioClip HitSound;

	private float lifetimeTimer = 0f;

	// Start is called before the first frame update
	void Start()
	{
        
	}

	// Update is called once per frame
	void Update()
	{
			lifetimeTimer += Time.deltaTime;
			if (lifetimeTimer >= lifetime)
			{
					Destroy(this.gameObject);
			}

			Vector3 velocity = new Vector3(0, 0, speed * Time.deltaTime);
			transform.position = transform.position + (transform.rotation * velocity);
	}

	void OnTriggerEnter(Collider other)
	{
		Ship ship = other.transform.root.GetComponent<Ship>();
		Matter matter = other.GetComponentInParent<Matter>();
		if (matter != null)
		{		
		    if (!hitRecord.Contains(matter) && (ship == null || ship.currentTeam == Ship.Team.Neutral || ship.currentTeam != myTeam))
		    {
			    AudioManager.Instance.PlayClip(HitSound);
                matter.Damage(Damage);

                if (!Piercing)
                {
                    Destroy(this.gameObject);
                }
							
		    }

            hitRecord.Add(matter);
        }
	}
}
