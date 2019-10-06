using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachNode
{
	public Matter Node;
	public List<AttachNode> Children;

	public AttachNode(Matter node)
	{
		Node = node;
		Children = new List<AttachNode>();
	}

	public AttachNode AttachChild(Matter node)
	{
		AttachNode newNode = new AttachNode(node);
		Children.Add(newNode);
		return newNode;
	}

	public AttachNode FindChild(Matter node)
	{
		if (node == Node)
			return this;

		foreach(var child in Children)
		{
			if (child.Node == node)
				return child;

			var result = child.FindChild(node);
			if (result != null)
				return result;
		}

		return null;
	}

	public List<AttachNode> GetAllChildren()
	{
		List<AttachNode> result = new List<AttachNode>();
		foreach(var child in Children)
		{
			result.Add(child);
			result.AddRange(child.GetAllChildren());
		}

		return result;
	}

	public void RemoveReferencesToChild(Matter node)
	{
		var childs = Children.ToArray();
		foreach (var child in childs)
		{
			if (child.Node == node)
			{
				Children.Remove(child);
			}
			else
			{
				child.RemoveReferencesToChild(node);
			}
		}
	}
}

public enum PlayerState
{
	Oblivion,
	Existing,
	Killed
}

[RequireComponent(typeof(Matter))]
[RequireComponent(typeof(Ship))]
public class PlayerKatamari : MonoBehaviour
{
	public GameObject ExistingModel;
	public GameObject DeadModel;

	public Transform CaptureAnchor;
	public List<Matter> CapturedObjects;
	public Rigidbody MasterRigidbody;
	public AttachNode Root;

	public GravityWell Well;
	public Ship Ship;
	public Matter Matter;

	public int StartingHealth = 10;
	public int HealthDecrement = 1;

	public float ConstantTorque = 5.0f;
	public float MaxTorque = 50.0f;

	public float ChildGravitySize = 2.0f;
	public float ChildGravityStrengthMultiplier = 0.05f;

	public PlayerState CurrentState;
	private int SpamCount;
	public int SpamRequiredToExist = 10;

	public float MomentumCap = 500.0f;


  // Start is called before the first frame update
  void Start()
  {
		if (CaptureAnchor == null)
			Debug.LogError($"Forgot to set {nameof(CaptureAnchor)} anchor!");

		if (MasterRigidbody == null)
			Debug.LogError($"Forgot to set {nameof(MasterRigidbody)}!");

		if (Well == null)
			Debug.LogError($"Forgot to set {nameof(Well)}!");

		Well.MasterKatamari = this;
		Ship = GetComponent<Ship>();
		Matter = GetComponent<Matter>();

		Root = new AttachNode(GetComponent<Matter>());

		CapturedObjects = new List<Matter>();

		CurrentState = PlayerState.Oblivion;

		Well.enabled = false;
		ExistingModel.SetActive(false);
		DeadModel.SetActive(true);
		Ship.currentTeam = Ship.Team.Neutral;
		Ship.enabled = false;
		GetComponent<SphereCollider>().enabled = false;
		GetComponent<Matter>().enabled = false;
	}

  // Update is called once per frame
  void Update()
  {
		

  }

	public void OnMatterTouch(Matter otherMatter, Matter hitter)
	{
		if (CurrentState != PlayerState.Existing)
			return;

		if (otherMatter == null)
			Debug.LogError("how the hell did we hit a null matter");

		otherMatter.CaptureObject(this);
		Vector3 currentMomentum = MasterRigidbody.mass * MasterRigidbody.velocity;
		Vector3 totalMomentum = otherMatter.Rigidbody.mass * otherMatter.Rigidbody.velocity + currentMomentum;
		if(totalMomentum.magnitude < MomentumCap)
		{
			MasterRigidbody.velocity = totalMomentum / MasterRigidbody.mass;
		}
		MasterRigidbody.mass += otherMatter.Rigidbody.mass;
		

		CapturedObjects.Add(otherMatter);
		var well = otherMatter.gameObject.AddComponent<GravityWell>();
		well.Range = ChildGravitySize;
		well.GravityStrength = ChildGravityStrengthMultiplier * otherMatter.Mass;
        UpdateSpeed();

        var node = Root.FindChild(hitter);
		if (node == null && Root.Node == hitter)
		{ 
				node = Root;
		}

		if (node == null)
			Debug.LogError("Somehow a null matter reported a hit??");

		node.AttachChild(otherMatter);
	}

	public void DestroyAttached(Matter matter, bool destroyMatter=true)
	{
		if(!CapturedObjects.Contains(matter) && Root.Node != matter)
		{
			Debug.LogError($"Katamari does not contain {matter.name}!");
			return;
		}

		CapturedObjects.Remove(matter);

		var node = Root.FindChild(matter);
		var children = node.GetAllChildren();
		foreach(var child in children)
		{
			if (child.Node.gameObject == null || !CapturedObjects.Contains(child.Node))
				continue;

			Destroy(child.Node.GetComponent<GravityWell>());
			Vector3 totalMomentum = MasterRigidbody.mass * MasterRigidbody.velocity;
			MasterRigidbody.mass -= child.Node.Rigidbody.mass;
			MasterRigidbody.velocity = totalMomentum / MasterRigidbody.mass;

			child.Node.DetachObject(node.Node.transform.position, 1.0f);

			Root.RemoveReferencesToChild(child.Node);
		}

		Root.RemoveReferencesToChild(node.Node);
        UpdateSpeed();

        if (matter != Root.Node && destroyMatter)
		{
			node.Node.DestroyMatter();
		}
		
	}

	public void ChangeState(PlayerState newState)
	{
		if (CurrentState == newState)
			return;

		CurrentState = newState;
		switch (newState)
		{
			default:
			case PlayerState.Oblivion:
				Debug.LogError("Cannot transition into Oblivion state while the game is running!");
				break;

			case PlayerState.Existing:
				SpamCount = 0;
				Matter.Mass = StartingHealth;

				Well.enabled = true;
				ExistingModel.SetActive(true);
				DeadModel.SetActive(false);
				Ship.currentTeam = Ship.Team.Player;
				Ship.enabled = true;
				GetComponent<SphereCollider>().enabled = true;
				GetComponent<Matter>().enabled = true;

				break;

			case PlayerState.Killed:
				DestroyAttached(Root.Node);
				GameController.Instance.ShowLossTutorial();
				StartingHealth -= HealthDecrement;
				StartingHealth = Mathf.Max(3, StartingHealth);

				Well.enabled = false;
				ExistingModel.SetActive(false);
				DeadModel.SetActive(true);
				Ship.currentTeam = Ship.Team.Neutral;					
				Ship.enabled = false;
				GetComponent<SphereCollider>().enabled = false;
				GetComponent<Matter>().enabled = false;

				break;
		}
	}
    public void UpdateSpeed() {

        Thruster[] thrusterlist = GetComponentsInChildren<Thruster>();
        PlayerController pc = this.GetComponent<PlayerController>();
        pc.MovementSpeed = pc.MovementSpeedBase;
        foreach (Thruster t in thrusterlist)
        {
            pc.MovementSpeed += t.SpeedIncrease;
        }
    }

	public void SpamExist()
	{
		SpamCount++;
		if (CurrentState != PlayerState.Existing && SpamCount >= SpamRequiredToExist)
		{
			ChangeState(PlayerState.Existing);
		}
	}

	public static PlayerKatamari GetPlayer()
	{
		return GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerKatamari>();
	}

}
