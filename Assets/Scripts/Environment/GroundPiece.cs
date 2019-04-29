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

    public enum GroundType { Empty, Surrounded, Dirt, Path };
 
    private GroundType type;
    public GroundType Type { get; set; }

    public Vector3 position;

    public bool[] neighbors;
    public bool isEdgePiece = false;

    public List<GroundDecoration> decorations;

    public GroundPiece(Land _land, Vector3 _position, GroundType _groundType) {
        land = _land;
        position = _position;
        type = _groundType;
        neighbors = new bool[6];

        decorations = new List<GroundDecoration>();
    }

    public void AddDecoration(GroundDecoration _decoration) {   
        decorations.Add(_decoration);
    }
}
