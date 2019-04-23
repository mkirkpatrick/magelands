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

    public int levelHeight;
    public int[,] heightMap;

    public GroundPiece[,,] groundPieces;
    public Area[] areas;
    public Vector2[] linkageLocations;

    public Land(Vector3 _worldPosition, int _xSize, int _zSize) {

        worldPosition = _worldPosition;
        xSize = _xSize * 32;
        zSize = _zSize * 32;
        levelHeight = 3;
        heightMap = new int[xSize, zSize];

        groundPieces = new GroundPiece[xSize, 16, zSize];

        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {
                for (int y = 0; y < 16; y++)
                {
                    groundPieces[x, y, z] = new GroundPiece(this, new Vector3(x, y, z), GroundPiece.GroundType.Empty);
                }
            }
        }

        //TODO: Make dynamic. West, North, and South for now.
        linkageLocations = new Vector2[3] { new Vector2(4, ZSize / 2), new Vector2(XSize / 2, ZSize - 4), new Vector2(XSize / 2, 4)};
    }
}
