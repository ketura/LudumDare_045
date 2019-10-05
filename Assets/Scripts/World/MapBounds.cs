using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class MapBounds : Singleton<MapBounds>
{
	public float WorldRadius = 100.0f;

	public List<Matter> SpawnedDebris;
	public List<GameObject> DebrisCache;
	public List<GameObject> SpawnedEnemies;
	public List<GameObject> EnemyCache;

	// Start is called before the first frame update
	void Start()
  {
		SpawnedDebris = new List<Matter>();
		DebrisCache = new List<GameObject>();

		SpawnedEnemies = new List<GameObject>();
		EnemyCache = new List<GameObject>();
  }

  // Update is called once per frame
  void Update()
  {
        
  }

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.black;
			
		Gizmos.DrawWireSphere(transform.position, WorldRadius);
	}
}
