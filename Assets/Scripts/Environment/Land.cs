using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Land {

    public Vector3 worldPosition;

    [SerializeField]
    private int xSize;
    public int XSize { get { return xSize; } private set { xSize = value; } }
    [SerializeField]
    private int ySize;
    public int YSize { get { return ySize; } private set { ySize = value; } }

    public GroundPiece[,] groundPieces;

    public Land(Vector3 _worldPosition, int _xSize, int _ySize) {

        worldPosition = _worldPosition;
        xSize = _xSize;
        ySize = _ySize;

        groundPieces = new GroundPiece[xSize, ySize];

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                groundPieces[x, y] = new GroundPiece(x,y, GroundPiece.GroundType.Forest_Grass);
            }
        }
    }

}
