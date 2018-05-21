using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Custom/CharacterStatesManager")]
public class CharStateManager : ScriptableObject {

#region editor visible characterstates (do not alter)	
[SerializeField] private CharacterState characterStateA, characterStateB;
#endregion
	
	public Dictionary<CharID, CharacterState> charactersDic = new Dictionary<CharID, CharacterState> {
		{CharID.charA, CharacterState.Initialize},
		{CharID.charB, CharacterState.Initialize}
	};

	public void AddCharacter(CharID charID){
		charactersDic.Add(charID, CharacterState.Initialize);
	}
	
	public void ChangeState(CharID charID, CharacterState newState){
		charactersDic[charID] = newState;
		UpdateSerializedStates();
	}

	public void Ground(CharID charID){
		charactersDic[charID] = CharacterState.Grounded;
		UpdateSerializedStates();
	}

	public void Grapple(CharID charID){
		charactersDic[charID] = CharacterState.Grappling;
		UpdateSerializedStates();
	}

	public void Hang(CharID charID){
		charactersDic[charID] = CharacterState.Hanging;
		UpdateSerializedStates();
	}

	void UpdateSerializedStates(){
		characterStateA = charactersDic[CharID.charA] != characterStateA ? charactersDic[CharID.charA] : characterStateA;
		characterStateB = charactersDic[CharID.charB] != characterStateB ? charactersDic[CharID.charB] : characterStateB;
	}

	public bool BothGrounded(){
		return (	charactersDic[CharID.charA] == CharacterState.Grounded && 
					charactersDic[CharID.charB] == CharacterState.Grounded);
	}

	public bool IsGrounded(CharID charID){
		return charactersDic[charID] == CharacterState.Grounded;
	}

	public bool IsHanging(CharID charID){
		return charactersDic[charID] == CharacterState.Hanging;
	}

	public void OnValidate(){
		charactersDic[CharID.charA] = characterStateA;
		charactersDic[CharID.charB] = characterStateB;
	}

}
