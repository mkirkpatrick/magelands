using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GroundDecoration
{
    public string type;
    public Vector3 localPosition;
    public int rotation;

    public GroundDecoration(Vector3 _localPosition, int _rotation = 0) {
        type = "Rock";
        localPosition = _localPosition;
        rotation = _rotation;
    }
}
