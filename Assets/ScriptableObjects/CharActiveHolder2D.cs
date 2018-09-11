using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="CharActiveHolder2D")]
public class CharActiveHolder2D : ScriptableObject {
	
	public delegate void SwitchCharacterStateDelegate(CharID charID, CharacterState newState);
	public event SwitchCharacterStateDelegate switchCharacterStateEvent;

	public bool v_charActiveA = false, v_charActiveB = false;
	[SerializeField] CharacterState v_charStateA = CharacterState.Initialize, v_charStateB = CharacterState.Initialize;

	Dictionary<CharID, CharActions2D> charActionsDic2D;

	public CharacterState CharStateA {
		get { return v_charStateA; }
		set {
			v_charStateA = value;
			switchCharacterStateEvent(CharID.charA, CharStateA);
		}
	}
	public CharacterState CharStateB {
		get { return v_charStateB; }
		set {
			v_charStateB = value;
			switchCharacterStateEvent(CharID.charB, CharStateB);
		}
	}
	public bool CharActiveA {
		get { return v_charActiveA; }
		set {
			v_charActiveA = value;
			v_charActiveB = !value;
			//CharActions.active[CharID.charA] = value;
			if(switchCharacterStateEvent != null && value == true) {
				switchCharacterStateEvent(CharID.charA, CharStateA);
				charActionsDic2D[CharID.charA].CheckForGround();
			}
		}
	}
	public bool CharActiveB {
		get { return v_charActiveB; }
		set {
			v_charActiveB = value;
			v_charActiveA = !value;
			//CharActions.active[CharID.charB] = value;
			if(switchCharacterStateEvent != null && value == true) {
				switchCharacterStateEvent(CharID.charB, CharStateB);
				charActionsDic2D[CharID.charB].CheckForGround();
			}
		}
	}

	public bool Combined {
		get { return (CharStateA == CharacterState.Combined && CharStateB == CharacterState.Combined); }
	}
	public CharID ActiveChar{
		get{ return CharActiveA ? CharID.charA : CharID.charB; }
	}
	public void RegisterCharacter2D(CharID charID, CharActions2D charActions){
		if(charActionsDic2D == null) charActionsDic2D = new Dictionary<CharID, CharActions2D>();
		charActionsDic2D.Add(charID, charActions);
	}

	public CharActions2D GetChar(CharID charID){
		return charActionsDic2D[charID];
	}

	

	public bool IsActive(CharID charID){
		if(charID == CharID.charA){
			return CharActiveA;
		} else {
			return CharActiveB;
		}
	}

	public CharacterState State(CharID charID){
		if(charID == CharID.charA){
			return CharStateA;
		} else {
			return CharStateB;
		}
	}

	public void SetStartActive(CharID charID, bool activeOnStart){
		if(charID == CharID.charA)
			CharActiveA = activeOnStart;
		else 
			CharActiveB = activeOnStart;
	}

	public void SwitchActionBy(CharID sentCharID){
		//Debug.Log("Switchaction by " + sentCharID + " at " + Time.time);
		if(sentCharID == CharID.charA){
			CharActiveA = false;
			CharActiveB = true;
		} else {
			CharActiveA = true;
			CharActiveB = false;
		}
	}

	public void SwitchActiveTo(CharID sentCharID){
		if(sentCharID == CharID.charA){
			CharActiveA = true;
		} else {
			CharActiveB = true;
		}
	}

	public void SwitchStateOfTo(CharID sentCharID, CharacterState newState){
		if(sentCharID == CharID.charA){
			CharStateA = newState;
		} else {
			CharStateB = newState;
		}
	}

	public void SwitchStateOfBothTo(CharacterState newState){
		CharStateA = newState;
		CharStateB = newState;
	}

	public void ThrowInitiated(CharID initiatedBy){
		if(initiatedBy == CharID.charA){
			CharStateA = CharacterState.Throwing;
			CharStateB = CharacterState.Thrown;
			charActionsDic2D[CharID.charB].transform.position = charActionsDic2D[CharID.charA].transform.position + Vector3.up;
		} else {
			CharStateA = CharacterState.Thrown;
			CharStateB = CharacterState.Throwing;
			charActionsDic2D[CharID.charA].transform.position = charActionsDic2D[CharID.charB].transform.position + Vector3.up;
		}
	}

	public void Throw(CharID initiatedBy){
		if(initiatedBy == CharID.charA){
			CharStateA = CharacterState.Braced;
			CharStateB = CharacterState.Hanging;
			CharActiveA = false;
		} else {
			CharStateA = CharacterState.Hanging;
			CharStateB = CharacterState.Braced;
			CharActiveB = false;
		}
	}

	public bool BothGrounded(){
		return ((CharStateA == CharacterState.Grounded || CharStateA == CharacterState.Braced) && (CharStateB == CharacterState.Grounded || CharStateB == CharacterState.Braced));
	}

}
