using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStateChanges : CharIdentifier {

	[SerializeField] CharStateManager charStateManager;
	[SerializeField] LayerMask groundLayer;

	RaycastHit hit;
	SphereCollider sphereCollider;

	void Start(){
		sphereCollider = GetComponent<SphereCollider>();
	}

	public void OnCollisionEnter(Collision collision){
		string s = gameObject.name + " collided with " + collision.gameObject.name;
		if(groundLayer.Contains(collision.gameObject.layer)){
			s = gameObject.name + " collided with ground layer object " + collision.gameObject.name + " ";
			Debug.DrawRay(transform.position, Vector3.down * 0.5f, Color.red, 10f, false);
			if(Physics.Raycast(transform.position, Vector3.down, out hit, 2f, groundLayer)){
				s += "at a point below the sphere";
				charStateManager.Ground(charID);
			}
			//Debug.Log("Contact point 0 y = " + collision.contacts[0].point.y);
			//Debug.Log("Sphere collider bounds min y = " + sphereCollider.bounds.min.y);
			//if(collision.contacts[0].point.y - 0.01f <= sphereCollider.bounds.min.y + 0.01f){
			//	s += "at a point below the sphere";
			//	charStateManager.Ground(charID);
			//}
		}
		Debug.Log(s);
	}

	public void OnCollisionExit(Collision collision){
		//if(Active){
		//	charStateManager.Hang(charID);
		//}
	}

	public void SetStateManager(CharStateManager csm){
		charStateManager = csm;
	}

}
