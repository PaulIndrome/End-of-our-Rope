using UnityEngine;
[ExecuteInEditMode]
public class Vector3PathPosition : MonoBehaviour {

    [HideInInspector] public Vector3PathFromChildTransforms father = null;
    [HideInInspector] public Color gizmoColor = Color.white;
    [ReadOnly] public int index;
    bool drawGizmos = true;

    public void OnDrawGizmos(){
        if(drawGizmos){
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireCube(transform.position, Vector3.one * 0.1f);
        }
    }

    public void OnDrawGizmosSelected(){
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.2f);
    }

    public void ToggleGizmos(bool onOff){
        drawGizmos = onOff;
    }

}
