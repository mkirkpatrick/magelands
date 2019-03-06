using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class GroundPiece {

    Chunk chunkParent;

    public enum GroundType { Empty, Forest_Grass, Dirt };
 
    private GroundType type;
    public GroundType Type
    {
        get { return type; }
        set { type = value; }
    }

    public string groundObjectID = "Dirt";
    public string topperObjectID = "Grass";

    public int xPosition;
    public int yPosition;

    public int topperType;

    public int rotation;

    public GroundPiece(Chunk _chunk, int _xPosition, int _yPosition, GroundType _groundType) {
        chunkParent = _chunk;
        xPosition = _xPosition;
        yPosition = _yPosition;
        type = _groundType;
    }
}
