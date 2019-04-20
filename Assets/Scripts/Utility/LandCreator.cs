using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LandCreator
{
    public static Land CreateLand(Land.BiomeType _biomeType, Vector3 _position, int _xSize, int _zSize) {
        Land newLand = new Land(_position, _xSize, _zSize);

        newLand.Biome = _biomeType;

       
        //LandUtil.ElevateLand(newLand, 6, .025f);

        HeightMapUtil.RaiseSquare(newLand, new int[] { 0, 0 }, new int[] { newLand.XSize, newLand.ZSize }, newLand.levelHeight);
        HeightMapUtil.RoundMapCorners(newLand, 8, 6);
        newLand.heightMap = HeightMapUtil.RoughMapEdges(newLand.heightMap, 6, .04f, true);

        HeightMapUtil.RaiseMountain(newLand, new int[] { newLand.XSize / 2, newLand.ZSize / 2 }, new int[] { Mathf.RoundToInt( newLand.XSize * .75f ), Mathf.RoundToInt(newLand.ZSize * .75f) }, 4);
        HeightMapUtil.RaiseMountain(newLand, new int[] { 48, 48 }, new int[] { 40, 40 }, 12);

        HeightMapUtil.RaiseMountain(newLand, new int[] { 48, 64 }, new int[] { 40, 40 }, 12);

        newLand.heightMap = HeightMapUtil.SmoothMap(newLand.heightMap);

        // Generate ground from heightmap
        for (int x = 0; x < newLand.XSize; x++) {
            for (int z = 0; z < newLand.ZSize; z++) {
                int height = newLand.heightMap[x, z];

                if (height == 0)
                    continue;

                for (int h = 0; h < height; h++) {
                    newLand.groundPieces[x, h, z].Type = GroundPiece.GroundType.Dirt;
                }
            }
        }
        foreach (GroundPiece ground in newLand.groundPieces)
        {
            LandUtil.AssignGroundPieceNeighbors(ground);
        }

        GenerateRocks(newLand);

        return newLand;
    }

    static void GenerateRocks(Land _land, float _rockiness = .1f) {

        foreach (GroundPiece ground in _land.groundPieces) {

            if (ground.isEdgePiece == true && Random.Range(0f, 1f) <= _rockiness)
            {

                List<int> openFaces = new List<int>();

                for (int i = 0; i < 4; i++)
                    if (ground.neighbors[i] == true)
                        openFaces.Add(i);

                int randomFace = openFaces[ Random.Range(0, openFaces.Count) ];

                Vector3 offset = new Vector3();

                switch (randomFace)
                {
                    case 0:
                        offset = new Vector3(Random.Range(0f, 1f), Random.Range(-1f, 0f), 1f);
                        break;
                    case 1:
                        offset = new Vector3(1f, Random.Range(-1f, 0f), Random.Range(0f, 1f) );
                        break;
                    case 2:
                        offset = new Vector3(Random.Range(0f, 1f), Random.Range(-1f, 0f), 1f);
                        break;
                    case 3:
                        offset = new Vector3( 1f, Random.Range(-1f, 0f), Random.Range(0f, 1f) );
                        break;
                }

            
                ground.AddDecoration( new GroundDecoration( offset ) );
            }
        }

    }
}