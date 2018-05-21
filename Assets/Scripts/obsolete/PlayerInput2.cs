using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput2 : MonoBehaviour {

	public Dictionary<CharID, CharacterState> charactersDic;
	public bool CharacterActiveA {
		get {
			return CharacterActiveA;
		}
		set {
			CharacterActiveA = value;
			activeCharacter = CharID.charA;
			CharacterActiveB = !CharacterActiveA;
		}
	}
	public bool CharacterActiveB {
		get {
			return CharacterActiveB;
		}
		set {
			CharacterActiveB = value;
			activeCharacter = CharID.charB;
			CharacterActiveA = !CharacterActiveB;
		}
	}

	public delegate bool PlayerActionDelegate();
	public event PlayerActionDelegate playerActionEvent;

	

	[SerializeField] float inputDeadZone;
	float horizontal, vertical;
	CharID activeCharacter;

	public bool ActionPressed {
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

	public float Horizontal {
		get  
		{ 
			if(Mathf.Abs(horizontal) < inputDeadZone) 
				{ 
					return 0f;
				}
			return horizontal;
		}
	}

	public float Vertical {
		get  
		{ 
			if(Mathf.Abs(vertical) < inputDeadZone) 
				{ 
					return 0f;
				}
			return vertical;
		}
	}

	void Start(){
		CharacterControl.characterStateChange += ChangeCharacterState;
	}

	// Update is called once per frame
	void Update () {
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");
		
		if(Input.GetButtonDown("PlayerAction")){
			if(charactersDic[CharID.charA] == CharacterState.Grounded && charactersDic[CharID.charB] == CharacterState.Grounded){
				ActionPressed = Input.GetButton("PlayerAction");
			} else {
				
				HandleSingleAction();
			}
		}

		
	}

	void HandleSingleAction(){
		if (charactersDic[activeCharacter] == CharacterState.Hanging){	
			
		} else if (charactersDic[activeCharacter] == CharacterState.Grounded){

		}
		
	}

	void HandleThrowingAction(){

	}

	void ChangeCharacterState(CharID characterIdentifier, CharacterState newCharacterState){
		charactersDic[characterIdentifier] = newCharacterState;
	}
}
