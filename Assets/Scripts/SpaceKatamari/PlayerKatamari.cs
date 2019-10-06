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
}

[RequireComponent(typeof(Matter))]
public class PlayerKatamari : MonoBehaviour
{
	public Transform CaptureAnchor;
	public List<Matter> CapturedObjects;
	public Rigidbody MasterRigidbody;
	public AttachNode Root;

	public GravityWell Well;

	public float ConstantTorque = 5.0f;
	public float MaxTorque = 50.0f;

	public float ChildGravitySize = 2.0f;
	public float ChildGravityStrengthMultiplier = 0.05f;


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

		Root = new AttachNode(GetComponent<Matter>());

		CapturedObjects = new List<Matter>();
	}

  // Update is called once per frame
  void Update()
  {
		

  }

	public void OnMatterTouch(Matter otherMatter, Matter hitter)
	{ 

		if (otherMatter == null)
			Debug.LogError("how the hell did we hit a null matter");

		otherMatter.CaptureObject(this);
		Vector3 totalMomentum = (otherMatter.Rigidbody.mass * otherMatter.Rigidbody.velocity) + (MasterRigidbody.mass * MasterRigidbody.velocity);
		MasterRigidbody.mass += otherMatter.Rigidbody.mass;
		MasterRigidbody.velocity = totalMomentum / MasterRigidbody.mass;

		CapturedObjects.Add(otherMatter);
		var well = otherMatter.gameObject.AddComponent<GravityWell>();
		well.Range = ChildGravitySize;
		well.GravityStrength = ChildGravityStrengthMultiplier * otherMatter.Mass;

		var node = Root.FindChild(hitter);
		if (node == null && Root.Node == hitter)
		{ 
				node = Root;
		}

		if (node == null)
			Debug.LogError("Somehow a null matter reported a hit??");

		node.AttachChild(otherMatter);
	}

	public void AttractObject(Matter matter)
	{
		
	}

	public void DestroyAttached(Matter matter)
	{
		if(!CapturedObjects.Contains(matter))
		{
			Debug.LogError($"Katamari does not contain {matter.name}!");
			return;
		}

		CapturedObjects.Remove(matter);

		var node = Root.FindChild(matter);
		var children = node.GetAllChildren();
		foreach(var child in children)
		{
			Destroy(child.Node.GetComponent<GravityWell>());
			Vector3 totalMomentum = MasterRigidbody.mass * MasterRigidbody.velocity;
			MasterRigidbody.mass -= child.Node.Rigidbody.mass;
			MasterRigidbody.velocity = totalMomentum / MasterRigidbody.mass;
		}
	}

	public static PlayerKatamari GetPlayer()
	{
		return GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerKatamari>();
	}
}
