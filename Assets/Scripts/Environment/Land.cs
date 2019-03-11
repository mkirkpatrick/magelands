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

    public Chunk[,] chunks;
    public GroundPiece[,] groundPieces;

    public Land(Vector3 _worldPosition, int _xSize, int _ySize) {

        worldPosition = _worldPosition;
        xSize = _xSize;
        ySize = _ySize;

        groundPieces = new GroundPiece[XSize * 16, ySize * 16];
        chunks = new Chunk[xSize, ySize];

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                chunks[x, y] = new Chunk(this, x, y);
            }
        }

        for (int x = 0; x < xSize * 16; x++)
        {
            for (int y = 0; y < ySize * 16; y++)
            {
                groundPieces[x, y] = new GroundPiece(chunks[x / 16, y / 16], x, y, GroundPiece.GroundType.Forest_Grass);
                chunks[x / 16, y / 16].groundPieces[x % 16, y % 16] = groundPieces[x, y];
            }
        }
        

    }

}
