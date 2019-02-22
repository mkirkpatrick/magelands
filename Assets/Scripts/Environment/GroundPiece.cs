using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GroundPiece {

    public enum GroundType { Empty, Forest_Grass, Dirt };

    public int xPostion;
    public int yPosition;

    public GroundType groundType;
    public int topperType;

    public Vector3 location;
    public int rotation;

    public GroundPiece(int _xPosition, int _yPosition, GroundType _groundType) {
        xPostion = _xPosition;
        yPosition = _yPosition;
        groundType = _groundType;
    }
}
