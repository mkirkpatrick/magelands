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

    public string orientationID;
    public bool[] topBottomNeighbors;

    public Vector3 position;

    public int rotation;

    public GroundPiece(Land _land, Vector3 _position, GroundType _groundType) {
        land = _land;
        position = _position;
        type = _groundType;
        orientationID = "Empty";
        topBottomNeighbors = new bool[2];
    }
}
