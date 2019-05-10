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
 
    public int id;
    public int[] subIDs;

    public Vector3 position;

    public bool[] neighbors;
    public List<string> attributes;

    public List<GroundDecoration> decorations;

    public GroundPiece(Land _land, Vector3 _position, int _id) {
        land = _land;
        position = _position;
        id = _id;
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
    public bool HasAttributes(string[] _attributes, bool allMatches = true) {
        bool hasAttributes = false;

        foreach (string s in _attributes) {
            if (attributes.Contains(s) == false && allMatches == true)
            {
                hasAttributes = false;
                break;
            }
            else if (attributes.Contains(s) == true && allMatches == false)
            {
                hasAttributes = true;
                break;
            }
            else
                hasAttributes = true;
        }
        return hasAttributes;
    }
    
}
