using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Vector3PathPosition))]
public class Vector3PathPositionInspector : Editor {
    
    Vector3PathPosition v3pp;

    void OnEnable(){
        v3pp = (Vector3PathPosition) target;
    }

    public override void OnInspectorGUI(){
        v3pp.father.posList = v3pp.father.transform.GetComponentsInChildren<Vector3PathPosition>();

        Color standardBGColor = GUI.backgroundColor;
        GUI.backgroundColor = Color.red;
        if(GUILayout.Button("Delete position")){
            v3pp.father.DeletePosition(v3pp.index);
        }
        GUI.backgroundColor = standardBGColor;
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Add position")){
            v3pp.father.AddPosition();
        }
        if(GUILayout.Button("Select parent")){
            Selection.activeObject = v3pp.father;
        }
        GUILayout.EndHorizontal();
        if(serializedObject != null && target != null) DrawDefaultInspector();
    }     

}