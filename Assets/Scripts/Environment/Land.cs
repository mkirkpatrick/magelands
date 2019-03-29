using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Land {

    public enum BiomeType { Forest, Desert, Mountain, Tundra }
    private BiomeType biome;
    public BiomeType Biome
    {
        get { return biome; }
        set { biome = value; }
    }

    public Vector3 worldPosition;

    [SerializeField]
    private int xSize;
    public int XSize { get { return xSize; } private set { xSize = value; } }
    [SerializeField]
    private int zSize;
    public int ZSize { get { return zSize; } private set { zSize = value; } }

    public GroundPiece[,,] groundPieces;
    public UnderPiece[] underPieces;

    public Land(Vector3 _worldPosition, int _xSize, int _ZSize) {

        worldPosition = _worldPosition;
        xSize = _xSize;
        zSize = _ZSize;

        groundPieces = new GroundPiece[XSize * 16, 32, zSize * 16];

        for (int x = 0; x < xSize * 16; x++)
        {
            for (int z = 0; z < zSize * 16; z++)
            {
                for (int y = 0; y < 32; y++)
                {
                    groundPieces[x, y, z] = new GroundPiece(this, new Vector3(x, y, z), GroundPiece.GroundType.Forest_Grass);
                }
            }
        }
    }
}
