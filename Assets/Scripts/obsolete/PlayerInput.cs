using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

	public Dictionary<CharID, CharacterState> charactersDic = null;
	[SerializeField] CharStateManager charStateManager;

	[SerializeField] CharStateChanges[] charStateChangers;

	bool characterActiveA, characterActiveB, actionPressed;
	public bool CharacterActiveA {
		get {
			return characterActiveA;
		}
		set {
			characterActiveA = value;
			activeCharacter = CharID.charA;
			characterActiveB = !characterActiveA;
		}
	}
	public bool CharacterActiveB {
		get {
			return characterActiveB;
		}
		set {
			characterActiveB = value;
			activeCharacter = CharID.charB;
			characterActiveA = !characterActiveB;
		}
	}

	public delegate bool PlayerActionDelegate(CharID activeCharacter);
	public event PlayerActionDelegate playerActionEvent;

	

	[SerializeField] float inputDeadZone;
	float horizontal, vertical;
	CharID activeCharacter;

	public bool ActionPressed {
		get { 
			return actionPressed;
		}
		set {
			if(actionPressed == value)
				return;
			else {
				actionPressed = value;
			}
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
		InitializeCharacterDictionary();
		if(charStateChangers.Length <= 0){
			Debug.LogWarning("No character state changers set up");
			Debug.Break();
		} else {
			charStateManager = ScriptableObject.CreateInstance(typeof(CharStateManager)) as CharStateManager;
			foreach(CharStateChanges csc in charStateChangers)
				csc.SetStateManager(charStateManager);

		}
	}


	// Update is called once per frame
	void Update () {
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");
		
		if(Input.GetButtonDown("PlayerAction")){
			activeCharacter = (activeCharacter == CharID.charA) ? CharID.charB : CharID.charA;
			if(playerActionEvent != null) playerActionEvent(activeCharacter);
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
		if(charactersDic == null) InitializeCharacterDictionary();
		charactersDic[characterIdentifier] = newCharacterState;
	}

	void InitializeCharacterDictionary(){
		charactersDic = new Dictionary<CharID, CharacterState>();
		charactersDic.Add(CharID.charA, CharacterState.Initialize);
		charactersDic.Add(CharID.charB, CharacterState.Initialize);
	}
}
