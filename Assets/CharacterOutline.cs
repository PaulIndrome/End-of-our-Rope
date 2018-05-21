using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOutline : MonoBehaviour {

	[SerializeField] CharID charID;
	[SerializeField] CharActiveHolder2D charActiveHolder2D;
	[SerializeField] Color characterColor, combineColor;

	[SerializeField] SpriteRenderer outlineRenderer, activeLightRenderer;

	// Use this for initialization
	void Start () {
		outlineRenderer = GetComponent<SpriteRenderer>();
		charActiveHolder2D.switchCharacterStateEvent += EvaluateColor;
	}
	
	public void ChangeColor(Color newColor){
		outlineRenderer.color = newColor;
	}

	public void ToggleCombine(){

	}

	public void EvaluateColor(CharID charID, CharacterState newState){
		if(this.charID != charID) return;
		else {
			if(charActiveHolder2D.IsActive(charID))
				activeLightRenderer.enabled = true;
			else 
				activeLightRenderer.enabled = false;

			if(newState == CharacterState.Combined)
				ChangeColor(combineColor);
			else 
				ChangeColor(characterColor);
		}
	}

	void OnDestroy(){
		charActiveHolder2D.switchCharacterStateEvent -= EvaluateColor;
	}
}
