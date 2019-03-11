using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Chunk
{
    public Land landParent;
    public GroundPiece[,] groundPieces;

    public int xPosition;
    public int yPosition;

    public Chunk(Land _land, int _xPos, int _yPos) {

        landParent = _land;
        xPosition = _xPos;
        yPosition = _yPos;

        groundPieces = new GroundPiece[16, 16];
    }
}
