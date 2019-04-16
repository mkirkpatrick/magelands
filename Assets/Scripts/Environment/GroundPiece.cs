using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class GroundPiece {

    private Land land;
    public Land Land
    {
        get { return land; }
    }

    public Chunk chunkParent;

    public enum GroundType { Empty, Surrounded, Dirt };
 
    private GroundType type;
    public GroundType Type
    {
        get { return type; }
        set { type = value; }
    }

    public bool[] neighbors;
    public bool isEdgePiece = false;

    public Vector3 position;

    public int rotation;

    public GroundPiece(Land _land, Vector3 _position, GroundType _groundType) {
        land = _land;
        position = _position;
        type = _groundType;
        neighbors = new bool[6];
    }
}
