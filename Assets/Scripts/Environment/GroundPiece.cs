using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GroundPiece {

    public int xPostion;
    public int yPosition;
    public int[] materialIDs;

    public Vector3 location;
    public int rotation;

    public GroundPiece(int x, int y) {
        xPostion = x;
        yPosition = y;
    }
}
