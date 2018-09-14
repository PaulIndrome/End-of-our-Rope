using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Vector3Path : ScriptableObject {
    [ReadOnly] public bool localCoordinates;
    [ReadOnly] public Vector3[] path;
}