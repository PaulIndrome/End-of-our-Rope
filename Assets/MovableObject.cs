using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour {

	[SerializeField] bool moveToPath = false;
	[SerializeField] float timeForMovement = 5f;
	[SerializeField] Vector3Path movementPath;
	[SerializeField] iTween.LoopType loopType;
	[SerializeField] iTween.EaseType easeType;
	Vector3[] path;

	// Use this for initialization
	void Awake () {
		if(movementPath == null) Debug.LogError("No movement path set on" + gameObject.name);
		path = movementPath.path;
	}
	
	void Start(){
		iTween.MoveTo(gameObject, iTween.Hash("path", path, "time", timeForMovement, "easetype", easeType, "islocal", movementPath.localCoordinates, "looptype", loopType, "movetopath", moveToPath));
	}
}
