using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GroundPiece {

    public int index;
    public int[] materialIDs;

    public Vector3 location;
    public int rotation;

    public GroundPiece(int _index) {
        index = _index;
    }
}
