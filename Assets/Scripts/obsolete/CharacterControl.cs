using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour {

	public delegate void CharacterStateChangeDelegate(CharID cI, CharacterState cS);
	public static event CharacterStateChangeDelegate characterStateChange;

	[SerializeField] private CharID characterIdentifier;
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private float swingStrength = 2f;
	[SerializeField] private bool controlled;

	bool grounded, grappling;
	
	public bool Grounded {
		get { return grounded;}
		set {
			if(grounded == value) return;
			grounded = value;
			grappling = !grounded;
			if(characterStateChange != null) characterStateChange(characterIdentifier, CharacterState.Grounded);
		}
	}

	public bool Grappling{
		get { return grappling; }
		set {
			if(grappling == value) return;
			grappling = value;
			grounded = !grappling;
			if(characterStateChange != null) characterStateChange(characterIdentifier, CharacterState.Grappling);
		}
	}

	private PlayerInput input;
	private Rigidbody body;
	private SphereCollider sphereCollider;
	MeshRenderer characterMesh;

	// Use this for initialization
	void Start () {
		input = GameObject.Find("PlayerControl").GetComponent<PlayerInput>();
		body = GetComponent<Rigidbody>();
		characterMesh = GetComponentInChildren<MeshRenderer>();
		sphereCollider = GetComponent<SphereCollider>();

		input.playerActionEvent += PlayerAction;
		SetCharacter(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(controlled && !Grounded){
			body.AddForce(input.Horizontal * (Grounded ? swingStrength*10f : swingStrength), 0, 0);
		}
	}

	bool PlayerAction(CharID charID){
		controlled = (charID == characterIdentifier) ? true : false;
		characterMesh.material.color = controlled ? Color.red : Color.white;
		if(controlled && Grounded){
			DeactivateColliderFor(1f);
			return false;
		} else {
			SetCharacter(true);
			return true;
		}
	}

	int characterSetCount = 1;
	void SetCharacter(bool switchChars){
		//string s = "Character " + characterIdentifier + " switched from " + (controlled ? "controlled " : "uncontrolled ") + " to ";
		//controlled = switchChars ? !controlled : controlled;
		//s += controlled ? "controlled " : "uncontrolled ";
		//Debug.Log(s);
		Debug.Log("charsetcount " + characterSetCount++ + " from character " + gameObject.name);
		if(controlled){
			Grappling = false;
			characterMesh.material.color = Color.red;
			body.drag = 0.33f;
			body.mass = 1f;
			body.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY;
		} else {
			characterMesh.material.color = Color.white;
			if(!Grounded) {
				Grappling = true;
				body.drag = 1;
				body.constraints = RigidbodyConstraints.FreezeAll;
			} else {
				Grappling = false;
				body.mass = 5f;
				body.drag = 1f;
				body.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY;
			}
		}
	}

	public void OnCollisionEnter(Collision col){
		CharacterControl other = col.gameObject.GetComponent<CharacterControl>();
		if(other != null && other.Grounded && controlled){
			transform.position = other.transform.position + (Vector3.right * sphereCollider.radius * 2);
			SetCharacter(true);
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



}
