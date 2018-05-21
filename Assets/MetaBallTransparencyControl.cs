using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MetaBallTransparencyControl : MonoBehaviour {

	[SerializeField] float maxDistance = 0.2f, minDistance = 0.6f, currentValue = 0f;
	[SerializeField] CharActions2D charA, charB;

	[SerializeField] Material AlphaCutMaterial;

	void Update(){
		if(AlphaCutMaterial.shader.name != "Custom/Transparent Cutout Edit"){
			enabled = false;
		}
		currentValue = Vector3.Distance(charA.transform.position, charB.transform.position);
		AlphaCutMaterial.SetFloat("_Transparency", Mathf.InverseLerp(minDistance, maxDistance, currentValue));
	}


}
