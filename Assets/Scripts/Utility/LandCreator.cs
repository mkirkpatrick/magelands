using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LandCreator
{
    public static Land CreateLand(Land.BiomeType _biomeType, Vector3 _position, int _xSize, int _zSize) {
        Land newLand = new Land(_position, _xSize, _zSize);

        newLand.Biome = _biomeType;

        //Initial land shape
        newLand.heightMap = HeightMapUtil.RaiseSquare(newLand.heightMap, new int[] { 0, 0 }, new int[] { newLand.XSize, newLand.ZSize }, newLand.levelHeight);
        newLand.heightMap = HeightMapUtil.RoundMapCorners(newLand.heightMap, 8, 6);
        newLand.heightMap = HeightMapUtil.RoughMapEdges(newLand.heightMap, 6, .04f, true);

        //Land detailing
        GenerateAreas(newLand, new int[] { 0, 30, 58, newLand.XSize }, new int[] { 0, 30, 72, newLand.ZSize });
        

        newLand.heightMap = HeightMapUtil.SmoothMap(newLand.heightMap);

        GeneratePaths(newLand);

        // Generate ground from heightmap
        for (int x = 0; x < newLand.XSize; x++) {
            for (int z = 0; z < newLand.ZSize; z++) {
                int height = newLand.heightMap[x, z];

                if (height <= 0)
                    continue;

                for (int h = 0; h <= height; h++) {
                    if(newLand.groundPieces[x, h, z].Type == GroundPiece.GroundType.Empty)
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

    static void GenerateAreas(Land _land, int[] _horizontalDivisions, int[] _verticalDivisions) {

        _land.areas = new Area[ _horizontalDivisions.Length - 1, _verticalDivisions.Length - 1];

        for (int h = 0; h < _horizontalDivisions.Length - 1; h++) {
            for (int v = 0; v < _verticalDivisions.Length - 1; v++) {

                int hSize = _horizontalDivisions[h + 1] - _horizontalDivisions[h];
                int vSize = _verticalDivisions[v + 1] - _verticalDivisions[v];

               _land.areas[h,v] = new Area( new int[2] { _horizontalDivisions[h], _verticalDivisions[v] }, new int[2]{ hSize, vSize } , Random.Range(_land.levelHeight - 1, _land.levelHeight + 5 ) );
            }
        }

        //Shift each area size a bit
        for(int x = 0; x < _land.areas.GetLength(0); x++)
        {
            for (int z = 0; z < _land.areas.GetLength(1); z++)
            {
                int xRand = Random.Range(-4, 5);
                int zRand = Random.Range(-4, 5);

                _land.areas[x,z].size[0] += xRand;
                _land.areas[x,z].size[1] += zRand;

                //Shift neighboring position according to size change
                if (x == _land.areas.GetLength(0) - 1) {
                    _land.areas[x, z].size[0] = _land.XSize - _land.areas[x, z].position[0];
                }   
                else
                    _land.areas[x + 1, z].position[0] += (xRand * -1);

                if (z == _land.areas.GetLength(1) - 1)
                    _land.areas[x, z].size[1] = _land.ZSize - _land.areas[x, z].position[1];
                else
                    _land.areas[x, z + 1].position[1] += (zRand * -1);

            }
        }

        foreach (Area area in _land.areas) {
            _land.heightMap = HeightMapUtil.RaiseSquare(_land.heightMap, area.position, area.size, area.height, true);

            int[,] areaMap = HeightMapUtil.ExtractMapArea(_land.heightMap, area.position, area.size);
            areaMap = HeightMapUtil.RoughMapEdges(areaMap, 2, .1f);
            _land.heightMap = HeightMapUtil.InsertMapIntoMap(_land.heightMap, areaMap, area.position);

        }
    }

    static void GeneratePaths(Land _land) {
        _land.pathSystem = new PathSystem( _land, new Vector2[3] { new Vector2(0, Random.Range(0, _land.ZSize)), new Vector2(Random.Range(0, _land.XSize), 0), new Vector2( _land.XSize, Random.Range( 0, _land.ZSize)) }, new Vector2(_land.XSize / 2, _land.ZSize / 2));
        for (int x = 0; x < _land.XSize; x++) {
            for (int z = 0; z < _land.ZSize; z++) {

                if (_land.pathSystem.pathMap[x, z] == 1 && _land.heightMap[x,z] != 0 )  
                    _land.groundPieces[x, _land.heightMap[x, z], z].Type = GroundPiece.GroundType.Path;
            }
        }
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