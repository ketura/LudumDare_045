using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetChunk : Matter
{
	public Collider DetectionCollider;

	public int MassThreshold = 100;

	// Start is called before the first frame update
	void Start()
	{
		DetectionCollider.isTrigger = true;
		Active = false;
	}

	// Update is called once per frame
	void Update()
	{

	}

	private new void OnTriggerEnter(Collider other)
	{
		if(Active)
		{
			base.OnTriggerEnter(other);
			return;
		}

		PlayerKatamari player = other.GetComponentInParent<PlayerKatamari>();
		if (player == null)
			return;


		if(player.MasterRigidbody.mass < MassThreshold)
		{
			GameController.Instance.ShowText("What a wonderful planet!  We must absorb it, we must have it, but we must first grow larger...", 7);
		}
		else
		{
			GameController.Instance.ShowText("What a wonderful planet!  We must absorb it, we must have it!", 7);
			Active = true;
		}
	}

	public override void CaptureObject(PlayerKatamari katamari)
	{
		base.CaptureObject(katamari);

		DetectionCollider.enabled = false;
		DetectionCollider.gameObject.SetActive(false);

		GameController.Instance.ShowText("What opulence!  The cataclysmic terror really brings out the flavor of halted civilization.", 3.0f);
	}
}
