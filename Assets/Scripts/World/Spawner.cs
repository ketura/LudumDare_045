using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

[System.Serializable]
public struct ObjectSpawn
{
	public GameObject PrefabToSpawn;
	public float SpawnChance;
}

[System.Serializable]
public struct SpawnBand
{
	public ObjectSpawn[] Entities;
	public float DistanceFromEdge;
}

[ExecuteInEditMode]
public class Spawner : MonoBehaviour
{
	public bool Active = false;
	public float SpawnRadius = 30.0f;
	public float SpawnInterval = 1.0f;

	public MapBounds Map;
	public PlayerKatamari Player;

	public SpawnBand[] Bands;


	[SerializeField]
	private Dictionary<float, List<ObjectSpawn>> SpawnRates;
	[SerializeField]
	private Dictionary<int, float> SpawnBands;

	private float MaxBand = 0;
	private float MinBand = 0;

	private void Awake()
	{
		SpawnRates = new Dictionary<float, List<ObjectSpawn>>();
		SpawnBands = new Dictionary<int, float>();

		ReadSpawnInfo();
	}

	// Start is called before the first frame update
	void Start()
  {
		Map = MapBounds.Instance;
		Player = PlayerKatamari.GetPlayer();

		
  }

	private void ReadSpawnInfo()
	{
		SpawnBands = new Dictionary<int, float>();
		var bands = Bands.Select(x => x.DistanceFromEdge).OrderBy(x => x).ToList();
		for(int i = 0; i < bands.Count; i++)
		{
			SpawnBands[i] = bands[i];
		}

		SpawnRates = Bands.ToDictionary(key => key.DistanceFromEdge, value => value.Entities.ToList());

		MaxBand = SpawnBands[SpawnBands.Keys.Last()];
		MinBand = SpawnBands[SpawnBands.Keys.First()];
	}

  // Update is called once per frame
  void Update()
  {
		if(Active && Application.IsPlaying(gameObject))
		{
			StartCoroutine(SpawnTimer(SpawnInterval));
		}
  }

	bool isSpawning = false;
	IEnumerator SpawnTimer(float interval)
	{
		if (!isSpawning)
		{
			isSpawning = true;

			yield return new WaitForSeconds(interval);
			if (Active)
			{
				SpawnEntities();
			}

			isSpawning = false;
		}
	}

	public void SpawnEntities()
	{
		float dist = Vector3.Distance(Player.transform.position, Map.transform.position);

		int bandnum = 0;
		for(int i = 0; i < SpawnBands.Count; i++)
		{
			float boundary = SpawnBands[i];
			if (dist > boundary)
			{
				bandnum = i;
			}
			else
				break;
		}

		var spawnInfo = SpawnRates[SpawnBands[bandnum]];

		foreach(var spawnItem in spawnInfo)
		{
			float chance = Random.Range(0, 1.0f);
			if(chance < spawnItem.SpawnChance)
			{
				var randomPoint = Random.insideUnitCircle;
				var point = new Vector3(randomPoint.x, 0, randomPoint.y);
				point.Normalize();
				point *= SpawnRadius;


				Map.SpawnPrefab(spawnItem.PrefabToSpawn, point + Player.transform.position, bandnum);
			}
		}

	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;

		if (SpawnBands == null)
			return;
		foreach(float boundary in SpawnBands.Values)
		{
			Gizmos.DrawWireSphere(transform.position, boundary);
		}
		
	}
}
