using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollowTarget : MonoBehaviour {

	public GameObject target;
	
	// Update is called once per frame
	void Update () {
		transform.position = target.transform.position;
		transform.rotation = target.transform.rotation;
	}
}
