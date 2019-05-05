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
    public List<string> attributes;

    public List<GroundDecoration> decorations;

    public GroundPiece(Land _land, Vector3 _position, GroundType _groundType) {
        land = _land;
        position = _position;
        type = _groundType;
        neighbors = new bool[6];

        decorations = new List<GroundDecoration>();
        attributes = new List<string>();
    }

    public void AddDecoration(GroundDecoration _decoration) {   
        decorations.Add(_decoration);
    }

    public bool HasAttributes(string _attribute)
    {
        if (attributes.Contains(_attribute))
            return true;
        else
            return false;

    }
    public bool HasAttributes(string[] _attributes) {
        bool hasAttributes = true;

        foreach (string s in _attributes) {
            if (attributes.Contains(s) == false) {
                hasAttributes = false;
                break;
            }     
        }
        return hasAttributes;
    }
    
}
