using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Chunk
{
    public GroundPiece[,] groundPieces;

    public int xPosition;
    public int yPosition;

    public Chunk(int _xPos, int _yPos) {

        xPosition = _xPos;
        yPosition = _yPos;

        groundPieces = new GroundPiece[16, 16];

            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 16; y++)
                {
                    groundPieces[x, y] = new GroundPiece(x,y, GroundPiece.GroundType.Forest_Grass);
                }
            }
    }
}
