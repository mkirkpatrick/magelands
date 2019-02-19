using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Land {

    public Vector3 worldPosition;
    public int xLength;
    public int yLength;

    public GroundPiece[,] groundPieces;

    public Land(Vector3 position, int xSize, int ySize) {

        worldPosition = position;
        xLength = xSize;
        yLength = ySize;

        groundPieces = new GroundPiece[xSize, ySize];

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                groundPieces[x, y] = new GroundPiece(x,y);
            }
        }
    }

}
