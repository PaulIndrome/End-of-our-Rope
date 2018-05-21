using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeCreator : MonoBehaviour {

	public GameObject linkPrefab;
	public GameObject character0, character1;
	public int numLinks = 5;
	public float playerDistance = 1;
	
	Vector2 linkOffsets = new Vector2(-0.3f, 0f);
	Rigidbody2D c0RB, c1RB;

	// Use this for initialization
	void Start () {
		c0RB = character0.GetComponent<Rigidbody2D>();
		c1RB = character1.GetComponent<Rigidbody2D>();

		CreateRope();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void CreateRope(){
		c0RB.constraints = c1RB.constraints = RigidbodyConstraints2D.FreezePosition;
		float linkDistance = playerDistance / (numLinks + 1);
		linkOffsets = new Vector2(0, -linkDistance);
		Rigidbody2D connectTo = c0RB;
		
		GameObject link = null;
		HingeJoint2D hJoint;

		link = Instantiate(linkPrefab, transform);
		hJoint = link.GetComponent<HingeJoint2D>();
		hJoint.enabled = true;
		hJoint.connectedBody = connectTo;
		hJoint.connectedAnchor = Vector2.zero;
		connectTo = link.GetComponent<Rigidbody2D>();

		for(int i = 1; i < numLinks; i++){
			link = Instantiate(linkPrefab, transform);
			hJoint = link.GetComponent<HingeJoint2D>();
			hJoint.enabled = true;
			hJoint.connectedBody = connectTo;
			hJoint.connectedAnchor = linkOffsets;
			connectTo = link.GetComponent<Rigidbody2D>();
		}

		//distanceJoint2D last link to other character
		hJoint = link.AddComponent<HingeJoint2D>();
		hJoint.autoConfigureConnectedAnchor = false;	
		hJoint.enabled = true;
		hJoint.connectedBody = c1RB;
		hJoint.connectedAnchor = new Vector2(0.1f, 0);


		c0RB.constraints = c1RB.constraints = RigidbodyConstraints2D.None;
	}
}
