using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Chunk
{
    public Land landParent;
    public GroundPiece[,,] groundPieces;

    public int xPosition;
    public int zPosition;

    public Chunk(Land _land, int _xPos, int _zPos) {

        landParent = _land;
        xPosition = _xPos;
        zPosition = _zPos;

        groundPieces = new GroundPiece[32, 16, 32];
    }
}
