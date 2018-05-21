using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeCreator3D : MonoBehaviour {

	public GameObject linkPrefab;
	public GameObject character0, character1;
	public int numLinks = 5;
	public float playerDistance = 1.5f;

	Vector2 linkOffsets = new Vector3(0, -0.3f, 0f);
	Rigidbody c0RB, c1RB;

	// Use this for initialization
	void Start () {
		c0RB = character0.GetComponent<Rigidbody>();
		c1RB = character1.GetComponent<Rigidbody>();

		CreateRope();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void CreateRope(){
		//c0RB.constraints = c1RB.constraints = RigidbodyConstraints.FreezePosition;
		float linkDistance = playerDistance / (numLinks + 1);
		linkOffsets = new Vector3(0, -linkDistance, 0);
		Rigidbody connectTo = c0RB;
		
		GameObject link = null;
		HingeJoint hJoint;

		//link = Instantiate(linkPrefab, transform);
		//hJoint = link.GetComponent<HingeJoint>();
		//hJoint.connectedBody = connectTo;
		//hJoint.connectedAnchor = Vector3.zero;
		//connectTo = link.GetComponent<Rigidbody>();

		for(int i = 0; i < numLinks; i++){
			link = Instantiate(linkPrefab, transform);
			link.name = "Link " + i; 
			hJoint = link.GetComponent<HingeJoint>();
			hJoint.connectedBody = connectTo;
			hJoint.connectedAnchor = linkOffsets;
			connectTo = link.GetComponent<Rigidbody>();
		}

		hJoint = c1RB.gameObject.AddComponent<HingeJoint>();
		hJoint.autoConfigureConnectedAnchor = false;	
		hJoint.connectedBody = link.GetComponent<Rigidbody>();
		hJoint.connectedAnchor = linkOffsets;


		//c0RB.constraints = c1RB.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY;
	}
}
