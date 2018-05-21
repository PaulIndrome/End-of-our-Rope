using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="CharActiveHolder")]
public class CharActiveHolder : ScriptableObject {

	
	public delegate void SwitchCharactersDelegate(CharID charID);
	public event SwitchCharactersDelegate switchCharactersEvent;

	public bool v_charActiveA = false, v_charActiveB = false;
	public CharacterState v_charStateA = CharacterState.Initialize, v_charStateB = CharacterState.Initialize;

	Dictionary<CharID, CharActions> charActionsDic;
	Dictionary<CharID, CharActions2D> charActionsDic2D;

	public CharacterState CharStateA {
		get { return v_charStateA; }
		set {
			v_charStateA = value;
			//CharActions.states[CharID.charA] = value;
		}
	}
	public CharacterState CharStateB {
		get { return v_charStateB; }
		set {
			v_charStateB = value;
			//CharActions.states[CharID.charB] = value;
		}
	}
	public bool CharActiveA {
		get { return v_charActiveA; }
		set {
			v_charActiveA = value;
			//CharActions.active[CharID.charA] = value;
			if(switchCharactersEvent != null && value == true) {
				switchCharactersEvent(CharID.charA);
			}
		}
	}
	public bool CharActiveB {
		get { return v_charActiveB; }
		set {
			v_charActiveB = value;
			//CharActions.active[CharID.charB] = value;
			if(switchCharactersEvent != null && value == true) {
				switchCharactersEvent(CharID.charB);
			}
		}
	}

	public void RegisterCharacter(CharID charID, CharActions charActions){
		if(charActionsDic == null) charActionsDic = new Dictionary<CharID, CharActions>();
		charActionsDic.Add(charID, charActions);
	}

	public void RegisterCharacter2D(CharID charID, CharActions2D charActions){
		if(charActionsDic == null) charActionsDic2D = new Dictionary<CharID, CharActions2D>();
		charActionsDic2D.Add(charID, charActions);
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

	public void SwitchStateOfTo(CharID sentCharID, CharacterState newState){
		if(sentCharID == CharID.charA){
			CharStateA = newState;
		} else {
			CharStateB = newState;
		}
	}

	public void ThrowInitiated(CharID initiatedBy){
		if(initiatedBy == CharID.charA){
			CharStateA = CharacterState.Throwing;
			CharStateB = CharacterState.Thrown;
		} else {
			CharStateA = CharacterState.Thrown;
			CharStateB = CharacterState.Throwing;
		}
	}

	public bool BothGrounded(){
		return (CharStateA == CharacterState.Grounded && CharStateB == CharacterState.Grounded);
	}

	public CharActions GetCharActions(CharID charID){
		return charActionsDic[charID];
	}

	public CharActions2D GetCharActions2D(CharID charID){
		return charActionsDic2D[charID];
	}
}
