using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl2 : MonoBehaviour {/*

	delegate void SwitchCharacterToDelegate(CharID charID);
	event SwitchCharacterToDelegate SwitchCharacter;

	[SerializeField] CharID thisCharID;
	[SerializeField] LayerMask groundLayer;
	[SerializeField] float swingStrength = 2f;
	[SerializeField] bool controlled = false;
	[SerializeField] CharStateManager charStateManager;
	
	Rigidbody body;
	SphereCollider sphereCollider;
	MeshRenderer characterMesh;

	float horizontal, vertical;
	bool ActionPressed {
		get { 
			return ActionPressed;
		}
		set {
			if(ActionPressed == value)
				return;
			else 
				ActionPressed = value;
		}
	}

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody>();
		characterMesh = GetComponentInChildren<MeshRenderer>();
		sphereCollider = GetComponent<SphereCollider>();

		//input.playerActionEvent += PlayerAction;
		controlled = thisCharID == 0 ? false : true;
		SwitchCharacter += CharacterSwitch;
		SetCharacter(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(controlled){
			body.AddForce(horizontal * swingStrength * (charStateManager.IsGrounded(thisCharID) ? 10f : 1f), 0, 0);
		}
	}

	void PlayerAction(){
		if(controlled){
			switch(charStateManager.charactersDic[thisCharID]){
				case CharacterState.Grounded:
					if()
					//jump into rope
					charStateManager.Hang(thisCharID);
					break;
				case CharacterState.Hanging:
					//grapple
					charStateManager.Grapple(thisCharID);
					break;
			}
		} else {
			switch(charStateManager.charactersDic[thisCharID]){
				case CharacterState.Grappling:
					//let go of grapple
					charStateManager.Hang(thisCharID);
					break;
				case CharacterState.Grounded:
			}
		}
	}

	//void SetCharacter(bool switchChars){
	//	string s = "Character " + thisCharID + " switched from " + (controlled ? "controlled " : "uncontrolled ") + " to ";
	//	controlled = (switchChars ? !controlled : controlled);
	//	s += controlled ? "controlled " : "uncontrolled ";
	//	Debug.Log(s);
	//	if(controlled){
	//		Grappling = false;
	//		characterMesh.material.color = Color.red;
	//		body.drag = 0.33f;
	//		body.mass = 1f;
	//		body.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY;
	//	} else {
	//		characterMesh.material.color = Color.white;
	//		if(!Grounded) {
	//			Grappling = true;
	//			body.drag = 1;
	//			body.constraints = RigidbodyConstraints.FreezeAll;
	//		} else {
	//			Grappling = false;
	//			body.mass = 5f;
	//			body.drag = 1f;
	//			body.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY;
	//		}
	//	}
	//}

	void GetInput(){
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");
		actionPressed = Input.GetButton("PlayerAction");
	}

	public void OnCollisionEnter(Collision col){
		CharacterControl other = col.gameObject.GetComponent<CharacterControl>();
		if(other != null && other.Grounded && controlled){
			transform.position = other.transform.position + (Vector3.right * sphereCollider.radius * 2);
			SetCharacter(false);
		} else if(Physics.Raycast(transform.position, Vector3.down, sphereCollider.radius + 0.1f, groundLayer)){
			Grounded = true;
		}
		
	}

	IEnumerator DeactivateColliderFor(float time){
		sphereCollider.enabled = false;
		Grounded = false;
		yield return new WaitForSeconds(time);
		sphereCollider.enabled = true;
	}

	void CharacterSwitch(CharID charId){
		if(charId == thisCharID)
			controlled = true;
		else 
			controlled = false;
	}
	
	*/

}
