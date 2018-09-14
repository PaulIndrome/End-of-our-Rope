using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Vector3PathFromChildTransforms))]
public class Vector3PathFromChildTransformsInspector : Editor {

    Vector3PathFromChildTransforms v3pfct;

    protected static bool showPositions = true;
    void OnEnable(){
        v3pfct = (Vector3PathFromChildTransforms) target;
    }

    public override void OnInspectorGUI(){
        serializedObject.Update();

        Vector3PathPosition[] posList = v3pfct.posList = v3pfct.transform.GetComponentsInChildren<Vector3PathPosition>();

        Color standardBGColor = GUI.backgroundColor;
        GUI.backgroundColor = Color.red;
        EditorGUI.BeginDisabledGroup(!(posList.Length > 0));
        if(GUILayout.Button("Create Path")){
            v3pfct.CreatePathAssetFromChildren();
        }
        EditorGUI.EndDisabledGroup();
        GUI.backgroundColor = standardBGColor;
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Add Position")){
            v3pfct.AddPosition();
        }
        EditorGUI.BeginDisabledGroup(!(posList.Length > 0));
            if(GUILayout.Button("Toggle gizmos")){
                v3pfct.ToggleGizmos();
                SceneView.RepaintAll();
            }
            if(GUILayout.Button("Remove last")){
                v3pfct.DeleteLastPosition();
            }
            if(GUILayout.Button("Delete all")){
                v3pfct.DeleteAllPositions();
            }
        EditorGUI.EndDisabledGroup();
        GUILayout.EndHorizontal();
        v3pfct.nameOfPath = EditorGUILayout.DelayedTextField("name of path", v3pfct.nameOfPath);
        v3pfct.localCoordinates = EditorGUILayout.Toggle("use local space", v3pfct.localCoordinates);
        if(v3pfct.localCoordinates) v3pfct.localToTransform = (Transform) EditorGUILayout.ObjectField("local space transform", v3pfct.localToTransform, typeof(Transform), true);
        showPositions = EditorGUILayout.Foldout(showPositions, "positions", true);
        if(showPositions){
            EditorGUI.BeginDisabledGroup(true);
            foreach(Vector3PathPosition v3pp in v3pfct.posList){
                EditorGUILayout.Vector3Field(v3pp.gameObject.name, v3pp.transform.position);
            }
            EditorGUI.EndDisabledGroup();
        }
        //DrawDefaultInspector();
    }

}