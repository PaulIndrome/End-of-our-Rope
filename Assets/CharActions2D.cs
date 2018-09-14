using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharActions2D : MonoBehaviour {

	public bool startActiveDebugCoroutine = false, startStatesDebugCoroutine = false;
	[SerializeField] bool startActive;
	[SerializeField] CharActiveHolder2D charActiveHolder;
	[SerializeField] float swingStrength = 300, walkSpeed = 300, climbSpeed = 1, aimSpeed = 100f, throwForce = 10f, maxRopeLength = 2f;
	[SerializeField] CharID charID;
	[SerializeField] LayerMask groundLayers, obstacleLayers, damagingLayers;
	[SerializeField] SpriteRenderer throwArrow;
	float horizontal, vertical;
	static float currentRopeLength = 1.5f;
	static bool actionStarted = false, passThroughActive = false;
	public static DistanceJoint2D distanceJoint;
	LayerMask collidingLayer, passingLayer;
	CharID otherID;
	
	RaycastHit2D hit;
	Rigidbody2D body;

	CircleCollider2D circleCollider;

	void Start(){
		StartCoroutine(StartRoutine());
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
					ToggleGrapple(false);
					break;
				case CharacterState.Hanging:
					GetAxisInputHorizontal();
					GetAxisInputVertical();
					GetGrappleAction();
					break;
				case CharacterState.Grounded:
					GetAxisInputHorizontal();
					GetAxisInputVertical();
					if(charActiveHolder.State(otherID) == CharacterState.Grappling){
						GetJumpThroughAction();
					}
					break;
				case CharacterState.Braced:
					ToggleBraced(false);
					break;
				case CharacterState.Throwing:
					//ignore inputs
					break;
				case CharacterState.Combined:
					GetAxisInputHorizontal();
					GetThrowAction();
					break;
				case CharacterState.Thrown:
				charActiveHolder.SwitchActiveTo(charID);
					break;
			}
		} else {
			GetReleaseGrappleButton();
			if(charActiveHolder.Combined){
				transform.position = charActiveHolder.GetChar(otherID).transform.position;
				if(circleCollider.enabled) circleCollider.enabled = false;
			} 
		}
	}

	void GetThrowAction(){
		if(Input.GetButtonDown("PlayerAction") && !actionStarted){
			charActiveHolder.ThrowInitiated(charID);
			actionStarted = true;
			transform.rotation = Quaternion.identity;
			body.constraints = RigidbodyConstraints2D.FreezePosition;
			body.velocity = Vector2.zero;
			body.angularVelocity = 0f;
			StartCoroutine(ThrowAction());
		}
	}

	public void ThrowInDirection(Vector2 direction){
		distanceJoint.distance = currentRopeLength = maxRopeLength;
		charActiveHolder.SwitchStateOfTo(charID, CharacterState.Hanging);
		body.constraints = RigidbodyConstraints2D.None;
		body.AddForce(direction * throwForce, ForceMode2D.Impulse);
	}

	void GetJumpThroughAction(){
		if(Input.GetButtonDown("PlayerAction") && !actionStarted){
			//Debug.Log("Jump through action on " + charID);
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
			charActiveHolder.SwitchActionBy(charID);
		}
	}

	void ToggleGrapple(bool grapple){
		if(grapple){
			charActiveHolder.SwitchStateOfTo(charID, CharacterState.Grappling);
			transform.rotation = Quaternion.identity;
			body.constraints = RigidbodyConstraints2D.FreezePosition;
		} else {
			charActiveHolder.SwitchStateOfTo(charID, CharacterState.Hanging);
			body.constraints = RigidbodyConstraints2D.FreezeRotation;
		}
	}

	void ToggleBraced(bool braced){
		if(braced){
			charActiveHolder.SwitchStateOfTo(charID, CharacterState.Braced);
			transform.rotation = Quaternion.identity;
			body.constraints = RigidbodyConstraints2D.FreezePosition;
			body.velocity = Vector2.zero;
			body.angularVelocity = 0f;
			charActiveHolder.SwitchActionBy(charID);
		} else {
			charActiveHolder.SwitchStateOfTo(charID, CharacterState.Grounded);
			body.constraints = RigidbodyConstraints2D.None;
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
			if(charActiveHolder.State(charID) == CharacterState.Hanging){
				body.AddForce(new Vector2(horizontal * swingStrength * Time.deltaTime, 0));
			} else if(charActiveHolder.State(charID) == CharacterState.Grounded || charActiveHolder.State(charID) == CharacterState.Combined) {
				body.AddForce(new Vector2(horizontal * walkSpeed * Time.deltaTime, 0));
			}

		}
	}

	void GetAxisInputVertical(){
		vertical = Input.GetAxis("Vertical");
		if(Mathf.Abs(vertical) > 0.5f){
			distanceJoint.distance = currentRopeLength = Mathf.Clamp(currentRopeLength + vertical * climbSpeed * Time.deltaTime, 0.2f, maxRopeLength);
		}
	}

	public void OnCollisionEnter2D(Collision2D collision){
		int collisionLayer = collision.gameObject.layer;
		if(groundLayers.Contains(collisionLayer)){
			hit = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, groundLayers, -0.5f, 0.5f);
			if(hit.collider != null){
				Debug.DrawRay(transform.position, Vector2.down * 0.5f, Color.red, 5f, false);
				if(charActiveHolder.Combined){
					return;
				} else if(charActiveHolder.State(otherID) == CharacterState.Braced && !charActiveHolder.Combined)
					CombineCharacters();
				else {
					ToggleBraced(true);
				}
			}
		} else if(obstacleLayers.Contains(collisionLayer)){

		} else if(damagingLayers.Contains(collisionLayer)){
			Debug.LogWarning("Character " + charID.ToString() + " damaged");
		}
	}

	public void OnCollisionExit2D(Collision2D collision){
		if(charActiveHolder.State(charID) == CharacterState.Grounded){
			charActiveHolder.SwitchStateOfTo(charID, CharacterState.Hanging);
		}
	}

	public void CombineCharacters(){
		Debug.Log("Combine! " + charID);
		charActiveHolder.SwitchActiveTo(charID);
		charActiveHolder.SwitchStateOfBothTo(CharacterState.Combined);
		charActiveHolder.GetChar(otherID).TogglePhysics(false);
	}

	public void TogglePhysics(bool onOff){
		DebugState();
		circleCollider.enabled = onOff;
		body.simulated = onOff;
		distanceJoint.enabled = onOff;
	}

	void CheckCurrentState(){
		CharacterState currentState = charActiveHolder.State(charID);
		if(currentState == CharacterState.Initialize || currentState != CharacterState.Grounded){
			charActiveHolder.SwitchStateOfTo(charID, CharacterState.Hanging);
		}
	}

	public void CheckForGround(){
		Debug.Log(charID + " checked for ground");
		if(Physics2D.Raycast(transform.position, Vector2.down, 0.5f, groundLayers, -0.5f, 0.5f).collider == null){
			charActiveHolder.SwitchStateOfTo(charID, CharacterState.Hanging);
		} else {
			charActiveHolder.SwitchStateOfTo(charID, CharacterState.Grounded);
		}
	}

	void DebugState(){
		string s = "Character " + charID + " switched to " + charActiveHolder.State(charID) + " at " + Time.time;
		Debug.Log(s);
	}


	void OnDestroy(){
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
		CharActions2D other = charActiveHolder.GetChar(otherID);
		throwArrow.enabled = true;
		while(Input.GetButton("PlayerAction")){
			throwArrow.transform.Rotate(-Vector3.forward, Input.GetAxis("Horizontal") * Time.deltaTime * aimSpeed);
			yield return null;
		}
		throwArrow.enabled = false;
		other.TogglePhysics(true);
		other.ThrowInDirection(throwArrow.transform.right);
		ToggleBraced(true);
		actionStarted = false;
	}

	IEnumerator StartRoutine(){
		body = GetComponent<Rigidbody2D>();
		circleCollider = GetComponent<CircleCollider2D>();
		
		charActiveHolder.RegisterCharacter2D(charID, this);

		if(charID == CharID.charA){
			distanceJoint = GetComponent<DistanceJoint2D>();
		}

		while(distanceJoint == null){
			Debug.Log(charID + "still waiting 1");
			if(charID == CharID.charA){
				distanceJoint = GetComponent<DistanceJoint2D>();
			} 
			yield return new WaitForSecondsRealtime(0.25f);
		}

		distanceJoint.distance = maxRopeLength;

		if(charID == CharID.charA){
			 if(startActiveDebugCoroutine){
				StartCoroutine(LogActiveStates());
			 } else if(startStatesDebugCoroutine){
				 StartCoroutine(LogStates());
			 }
		}

		otherID = charID == CharID.charA ? CharID.charB : CharID.charA;

		collidingLayer = LayerMask.NameToLayer("CharactersColliding");
		passingLayer = LayerMask.NameToLayer("CharactersPassing");
		
		charActiveHolder.SwitchStateOfTo(charID, CharacterState.Initialize);
		charActiveHolder.SetStartActive(charID, startActive);

		CheckCurrentState();
	}

}
