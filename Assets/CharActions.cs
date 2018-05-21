using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharActions : MonoBehaviour {
/*
	public bool startActiveDebugCoroutine = false, startStatesDebugCoroutine = false;
	[SerializeField] bool startActive;
	[SerializeField] CharActiveHolder charActiveHolder;
	[SerializeField] float swingStrength, walkSpeed, climbSpeed;
	[SerializeField] CharID charID;
	[SerializeField] LayerMask groundLayers, obstacleLayers;
	float horizontal, vertical;
	static float currentRopeLength = 1.5f;
	static bool actionStarted = false, passThroughActive = false;
	static ConfigurableJoint ropeJoint;
	static SoftJointLimit ropeJointLimit;
	LayerMask collidingLayer, passingLayer;
	CharID otherID;
	MeshRenderer meshRenderer;
	RaycastHit hit;
	Rigidbody body;

	void Start(){
		charActiveHolder.SwitchStateOfTo(charID, CharacterState.Initialize);
		charActiveHolder.SetStartActive(charID, startActive);

		body = GetComponent<Rigidbody>();
		if(GetComponent<ConfigurableJoint>() != null){
			ropeJoint = GetComponent<ConfigurableJoint>();
			ropeJointLimit = ropeJoint.linearLimit;
			currentRopeLength = ropeJointLimit.limit;
		}
		meshRenderer = GetComponentInChildren<MeshRenderer>();

		charActiveHolder.switchCharactersEvent += SwitchActiveState;

		if(charID == CharID.charA){
			 if(startActiveDebugCoroutine){
				StartCoroutine(LogActiveStates());
			 } else if(startStatesDebugCoroutine){
				 StartCoroutine(LogStates());
			 }
		}
			
		charActiveHolder.RegisterCharacter(charID, this);

		otherID = charID == CharID.charA ? CharID.charB : CharID.charA;

		collidingLayer = LayerMask.NameToLayer("CharactersColliding");
		passingLayer = LayerMask.NameToLayer("CharactersPassing");

		CheckCurrentState();
	}

	void Update(){
		if(body.velocity.y >= 0.1f || passThroughActive){
			gameObject.layer = passingLayer;
		} else {
			gameObject.layer = collidingLayer;
		}

		if(charActiveHolder.IsActive(charID)){
			switch(charActiveHolder.State(charID)){
				case CharacterState.Grappling:
					Debug.Log(charID + " started activity in grapple");
					ToggleGrapple(false);
					break;
				case CharacterState.Hanging:
					GetAxisInputHorizontal();
					GetAxisInputVertical();
					GetGrappleAction();
					break;
				case CharacterState.Grounded:
					GetAxisInputHorizontal();
					if(charActiveHolder.BothGrounded()){
						//GetThrowAction
					} else {
						GetJumpThroughAction();
					}
					break;
				case CharacterState.Throwing:
					//ignore inputs
					break;
			}
		} else {
			GetReleaseGrappleButton();
		}
	}

	void GetThrowAction(){
		if(Input.GetButtonDown("PlayerAction") && !actionStarted){
			actionStarted = true;
			StartCoroutine(SwitchHolder());
			charActiveHolder.ThrowInitiated(charID);
		}
	}

	void GetJumpThroughAction(){
		if(Input.GetButtonDown("PlayerAction") && !actionStarted){
			Debug.Log("Jump through action on " + charID);
			actionStarted = true;
			passThroughActive = true;
			StartCoroutine(SwitchHolder());
			StartCoroutine(PassThroughTime(1f));
		}
	}

	void GetGrappleAction(){
		if(Input.GetButtonDown("PlayerAction") && !actionStarted){
			actionStarted = true;
			StartCoroutine(SwitchHolder());
			ToggleGrapple(true);
			//Debug.Log("Buttondown called from " + gameObject.name + " at " + Time.time);
			//Debug.Log(charID + " is " + (active[charID] ? "active" : "inactive"));
			charActiveHolder.SwitchActionBy(charID);
		}
	}

	void ToggleGrapple(bool grapple){
		if(grapple){
			body.constraints = RigidbodyConstraints.FreezeAll;
			charActiveHolder.SwitchStateOfTo(charID, CharacterState.Grappling);
		} else {
			body.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY;
			charActiveHolder.SwitchStateOfTo(charID, CharacterState.Hanging);
		}
	}

	void GetReleaseGrappleButton(){
		if(Input.GetButtonDown("ReleaseGrapple")){
			ToggleGrapple(false);
		}
	}

	void GetActionButtonDown(){
		

		if(Input.GetButtonDown("PlayerAction") && !actionStarted){
			actionStarted = true;
			StartCoroutine(SwitchHolder());
			//Debug.Log("Buttondown called from " + gameObject.name + " at " + Time.time);
			//Debug.Log(charID + " is " + (active[charID] ? "active" : "inactive"));
			charActiveHolder.SwitchActionBy(charID);
		}
	}

	void GetAxisInputHorizontal(){
		horizontal = Input.GetAxis("Horizontal");
		if(Mathf.Abs(horizontal) > 0.05f){
			if(charActiveHolder.State(charID) == CharacterState.Grounded){
				body.AddForce(horizontal * walkSpeed * Time.deltaTime, 0, 0);
			}
			else if(charActiveHolder.State(charID) == CharacterState.Hanging){
				body.AddForce(horizontal * swingStrength * Time.deltaTime, 0, 0);
			}
		}
	}

	void GetAxisInputVertical(){
		vertical = Input.GetAxis("Vertical");
		if(Mathf.Abs(vertical) > 0.1f && body.velocity.sqrMagnitude < 0.15f){
			if(charActiveHolder.State(charID) == CharacterState.Hanging){
				SoftJointLimit sjl = new SoftJointLimit();
				sjl.limit = currentRopeLength = Mathf.Clamp(currentRopeLength + vertical * climbSpeed * Time.deltaTime, 0.2f, 2f);
				ropeJoint.linearLimit = sjl;
			}
		}
	}

	public void OnCollisionEnter(Collision collision){
		int collisionLayer = collision.gameObject.layer;
		if(groundLayers.Contains(collisionLayer)){
			if(Physics.Raycast(transform.position, Vector3.down, out hit, 2f, groundLayers)){
				charActiveHolder.SwitchStateOfTo(charID, CharacterState.Grounded);
			}
		} else if(obstacleLayers.Contains(collisionLayer)){

		}
	}

	public void OnCollisionExit(Collision collision){
		if(charActiveHolder.State(charID) == CharacterState.Grounded){
			charActiveHolder.SwitchStateOfTo(charID, CharacterState.Hanging);
		}
	}

	void CheckCurrentState(){
		CharacterState currentState = charActiveHolder.State(charID);
		if(currentState == CharacterState.Initialize || currentState != CharacterState.Grounded){
			charActiveHolder.SwitchStateOfTo(charID, CharacterState.Hanging);
		}
	}

	void SwitchActiveState(CharID sentCharID){
		meshRenderer.material.color = sentCharID == charID ? Color.red : Color.white;
	}

	void DebugState(){
		string s = "Character " + charID + " switched to " + charActiveHolder.IsActive(charID) + " at " + Time.time;
		Debug.Log(s);
	}


	void OnDestroy(){
		charActiveHolder.switchCharactersEvent -= SwitchActiveState;
		StopAllCoroutines();
	}

	IEnumerator LogActiveStates(){
		while(gameObject.activeSelf){
			Debug.Log(CharID.charA + " is " + charActiveHolder.IsActive(CharID.charA));
			Debug.Log(CharID.charB + " is " + charActiveHolder.IsActive(CharID.charB));
			yield return new WaitForSeconds(3f);
		}
	}

	IEnumerator LogStates(){
		while(gameObject.activeSelf){
			Debug.Log(CharID.charA + " is " + charActiveHolder.State(CharID.charA));
			Debug.Log(CharID.charB + " is " + charActiveHolder.State(CharID.charB));
			yield return new WaitForSeconds(3f);
		}
	}

	IEnumerator SwitchHolder(){
		yield return new WaitUntil(() => Input.GetButtonUp("PlayerAction"));
		actionStarted = false;
	}

	IEnumerator PassThroughTime(float passTime){
		gameObject.layer = passingLayer;
		yield return new WaitForSeconds(passTime);
		passThroughActive = false;
	}

	IEnumerator ThrowAction(){
		while(Input.GetButton("PlayerAction")){
			yield return null;
		}
	}
*/
}
