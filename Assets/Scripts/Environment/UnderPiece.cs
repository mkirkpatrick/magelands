using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnderPiece
{
    public enum UnderType { Dirt };

    private UnderType type;
    public UnderType Type
    {
        get { return type; }
        set { type = value; }
    }

    public int rotation;
    public string orientationType;

    public string neighborIDString;
    public Vector3 position;

    public UnderPiece(UnderType _type, Vector3 _position) {
        type = _type;
        position = _position;
    }
}
