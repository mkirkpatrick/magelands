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
        HeightMapUtil.RaiseMountain(newLand, new int[] { Random.Range(12,newLand.XSize - 16) , Random.Range(16, newLand.ZSize - 16) }, new int[] { Random.Range(20, 33), Random.Range(16, 25) }, Random.Range(4, 8));
        HeightMapUtil.RaiseMountain(newLand, new int[] { Random.Range(12, newLand.XSize - 16), Random.Range(16, newLand.ZSize - 16) }, new int[] { Random.Range(16, 25), Random.Range(16, 25) }, Random.Range(4, 8));
        HeightMapUtil.RaiseMountain(newLand, new int[] { Random.Range(12, newLand.XSize - 16), Random.Range(16, newLand.ZSize - 16) }, new int[] { Random.Range(16, 25), Random.Range(16, 25) }, Random.Range(8, 10));

        newLand.heightMap = HeightMapUtil.SmoothMap(newLand.heightMap);
        newLand.heightMap = HeightMapUtil.SmoothMap(newLand.heightMap);

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

        //LandUtil.RemoveJutsAndHoles(newLand);

        return newLand;
    }
}