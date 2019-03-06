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

    public Land(Vector3 _worldPosition, int _xSize, int _ySize) {

        worldPosition = _worldPosition;
        xSize = _xSize;
        ySize = _ySize;

        chunks = new Chunk[xSize, ySize];

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                chunks[x, y] = new Chunk(this, x, y);
            }
        }
    }

}
