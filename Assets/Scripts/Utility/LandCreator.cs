using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LandCreator
{
    public static Land CreateLand(Land.BiomeType _biomeType, Vector3 _position, int _xSize, int _zSize) {
        Land newLand = new Land(_position, _xSize, _zSize);

        newLand.Biome = _biomeType;
       
        //LandUtil.ElevateLand(newLand, 6, .025f);

        newLand.heightMap = HeightMapUtil.RaiseSquare(newLand.heightMap, new int[] { 0, 0 }, new int[] { newLand.XSize, newLand.ZSize }, newLand.levelHeight);
        newLand.heightMap = HeightMapUtil.RoundMapCorners(newLand.heightMap, 8, 6);
        newLand.heightMap = HeightMapUtil.RoughMapEdges(newLand.heightMap, 6, .04f, true);

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

        GenerateRocks(newLand, .005f);

        return newLand;
    }

    static void GenerateAreas() {

    }
    static void GenerateRocks(Land _land, float _rockiness = .1f) {

        foreach (GroundPiece ground in _land.groundPieces) {
            if (ground.Type == GroundPiece.GroundType.Empty)
                continue;

            if (ground.isEdgePiece == true && Random.Range(0f, 1f) <= (_rockiness * 5) ) //Sides are rockier than tops
            {

                List<int> openFaces = new List<int>();

                for (int i = 0; i < 4; i++)
                    if (ground.neighbors[i] == false)
                        openFaces.Add(i);

                int randomFace = openFaces[Random.Range(0, openFaces.Count)];

                Vector3 offset = new Vector3();

                switch (randomFace)
                {
                    case 0:
                        offset = new Vector3(Random.Range(.2f, .8f), Random.Range(-.5f, -1f), Random.Range(.8f, 1));
                        break;
                    case 1:
                        offset = new Vector3(Random.Range(.8f, 1), Random.Range(-.5f, -1f), Random.Range(.2f, .8f));
                        break;
                    case 2:
                        offset = new Vector3(Random.Range(.2f, .8f), Random.Range(-.5f, -1f), Random.Range(0, .2f));
                        break;
                    case 3:
                        offset = new Vector3(Random.Range(0, .2f), Random.Range(-.5f, -1f), Random.Range(.2f, .8f));
                        break;
                }

                int rotation = Random.Range(0, 4) * 90;
                ground.AddDecoration(new GroundDecoration(offset, rotation));
            }
            else {
                if (ground.neighbors[4] == false && Random.Range(0f, 1f) <= _rockiness) {
                    Vector3 offset = new Vector3(Random.Range(.1f, .9f), 0, Random.Range(.1f, .9f));
                    int rotation = Random.Range(0, 360);

                    ground.decorations.Add( new GroundDecoration(offset, rotation) );
                }
            }
        }

    }
}