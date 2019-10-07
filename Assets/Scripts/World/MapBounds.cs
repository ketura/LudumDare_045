using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class MapBounds : Singleton<MapBounds>
{
	public float WorldRadius = 100.0f;

	public int MaxObjectsPerBand = 10;

	public Dictionary<int, List<Matter>> SpawnedObjects;
	public List<GameObject> ObjectCache;

	// Start is called before the first frame update
	void Start()
  {
		SpawnedObjects = new Dictionary<int, List<Matter>>();
		ObjectCache = new List<GameObject>();
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

	public void SpawnPrefab(GameObject prefab, Vector3 pos, int band)
	{
		if (SpawnedObjects.ContainsKey(band) && SpawnedObjects[band].Count > MaxObjectsPerBand)
			return;

		if (Vector3.Distance(transform.position, pos) > WorldRadius)
			return;

		if(!SpawnedObjects.ContainsKey(band))
		{
			SpawnedObjects[band] = new List<Matter>();
		}


		var go = GameObject.Instantiate(prefab, pos, Quaternion.identity);
		SpawnedObjects[band].Add(go.GetComponent<Matter>());
	}

	public void RemoveMatterFromSpawnList(Matter matter)
	{
		if (matter == null)
			return;

		foreach(var pair in SpawnedObjects)
		{
			if(pair.Value.Contains(matter))
			{
				pair.Value.Remove(matter);
				return;
			}
		}
	}
}
