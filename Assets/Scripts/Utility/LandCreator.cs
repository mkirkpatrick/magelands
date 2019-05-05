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
        GenerateAdvancedAreas(newLand);

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

        int hNum = _horizontalDivisions.Length - 1;
        int vNum = _verticalDivisions.Length - 1;

        _land.areas = new Area[ hNum * vNum ];

        for (int h = 0; h < hNum; h++) {
            for (int v = 0; v < vNum; v++) {
  
                int hSize = _horizontalDivisions[h + 1] - _horizontalDivisions[h];
                int vSize = _verticalDivisions[v + 1] - _verticalDivisions[v];

               _land.areas[ (h*hNum) + v] = new Area( new int[2] { _horizontalDivisions[h], _verticalDivisions[v] }, new int[2]{ hSize, vSize } , Random.Range(_land.levelHeight - 1, _land.levelHeight + 5 ) );
            }
        }
        //foreach (Area a in _land.areas)
            //Debug.Log(a.position[0] + "," + a.position[1] + "-" + a.size[0] + "," + a.size[1]);

        //Shift each area size a bit
        for(int x = 0; x < hNum; x++)
        {
            for (int z = 0; z < vNum; z++)
            {
                int xRand = Random.Range(-4, 5);
                int zRand = Random.Range(-4, 5);
                int landIndex = (x * hNum) + z;
                _land.areas[landIndex].size[0] += xRand;
                _land.areas[landIndex].size[1] += zRand;

                //Shift neighboring position according to size change
                if (x == hNum - 1) {
                    _land.areas[landIndex].size[0] = _land.XSize - _land.areas[landIndex].position[0];
                }
                else
                    _land.areas[landIndex + vNum].position[0] += (xRand * -1);

                if (z == vNum - 1)
                    _land.areas[landIndex].size[1] = _land.ZSize - _land.areas[landIndex].position[1];
                else
                    _land.areas[landIndex + 1].position[1] += (zRand * -1);
            }
        }

        foreach (Area area in _land.areas) {
            _land.heightMap = HeightMapUtil.RaiseSquare(_land.heightMap, area.position, area.size, area.height, true);

            /* Rough edges
            int[,] areaMap = HeightMapUtil.ExtractMapArea(_land.heightMap, area.position, area.size);
            areaMap = HeightMapUtil.RoughMapEdges(areaMap, 2, .1f);
            _land.heightMap = HeightMapUtil.InsertMapIntoMap(_land.heightMap, areaMap, area.position);
            */
        }
    }
    static void GenerateAdvancedAreas(Land _land) {

        int[,] areaMap = new int[_land.XSize, _land.ZSize];
        List<Area> newAreas = new List<Area>();
        List<GroundPiece> currentAreaGround;
        int currentAreaNum = 0;
        int currentAreaHeight = 0;

        for (int x = 0; x < _land.XSize; x++)
        {
            for (int z = 0; z < _land.ZSize; z++)
            {
                if (_land.heightMap[x, z] == 0 || areaMap[x,z] > 0 || _land.pathSystem.pathMap[x,z] == 1)
                    continue;

                GroundPiece ground = _land.groundPieces[x, _land.heightMap[x,z], z ];
                currentAreaNum++;
                currentAreaHeight = _land.heightMap[x, z];
                currentAreaGround = new List<GroundPiece>();

                AddAreaNeighbors(ground, currentAreaNum, currentAreaHeight);      
            }
        }

        int areaNum = 0; //Get Area Count
        foreach (int i in areaMap)
            if (i > areaNum)
                areaNum = i;

        for (int i = 0; i < areaNum; i++) {
            int[] postion = new int[2] { _land.XSize, _land.ZSize };
            int[] size = new int[2];
            int height = 0;

            for (int x = 0; x < _land.XSize; x++)
            {
                for (int z = 0; z < _land.ZSize; z++)
                {
                    if (areaMap[x, z] != i + 1)
                        continue;

                    height = _land.heightMap[x, z];

                    if (x < postion[0])
                        postion[0] = x;
                    if (z < postion[1])
                        postion[1] = z;

                    if (x - postion[0] > size[0])
                        size[0] = x - postion[0];
                    if (z - postion[1] > size[1])
                        size[1] = z - postion[1];
                }
            }
            newAreas.Add( new Area(postion, size, height) );

            int[,] newMap = HeightMapUtil.ExtractMapArea(areaMap, postion, size);
            for (int x = 0; x < newMap.GetLength(0); x++) {
                for (int z = 0; z < newMap.GetLength(1); z++) {
                    if (newMap[x, z] != i + 1)
                        newMap[x, z] = 0;
                    else
                        newMap[x, z] = 1;
                }
            }
            newAreas[i].areaMap = newMap;

        }

        _land.areas = newAreas.ToArray();

        void AddAreaNeighbors(GroundPiece _ground, int _currentArea, int _currentHeight) {
            int x = (int)_ground.position.x;
            int z = (int)_ground.position.z;

            if (areaMap[x, z] != 0 || _land.heightMap[x,z] != _currentHeight || _land.pathSystem.pathMap[x, z] == 1)
                return;
                
            areaMap[x, z] = _currentArea;
            GroundPiece[] neighbors = LandUtil.GetNeighborGroundPieces(_ground, false);

            foreach (GroundPiece neighbor in neighbors)
                AddAreaNeighbors(neighbor, _currentArea, _currentHeight);
            
        }

    }

    static void GeneratePaths(Land _land) {
        _land.pathSystem = new PathSystem( _land, new Vector2[3] { new Vector2(0, Random.Range(12, _land.ZSize - 12)), new Vector2(Random.Range(12, _land.XSize - 12), 0), new Vector2( _land.XSize - 1, Random.Range( 12, _land.ZSize - 12)) }, new Vector2(_land.XSize / 2, _land.ZSize / 2));
        for (int x = 0; x < _land.XSize; x++) {
            for (int z = 0; z < _land.ZSize; z++) {

                if (_land.pathSystem.pathMap[x, z] == 1 && _land.heightMap[x, z] != 0) {
                    _land.groundPieces[x, _land.heightMap[x, z], z].Type = GroundPiece.GroundType.Path;

                    //Check for slants
                    int[] heightNeighbors = HeightMapUtil.GetMapNeighbors(_land.heightMap, new int[2] { x, z } );
                    foreach (int i in heightNeighbors)
                        if (i < _land.heightMap[x, z])
                            _land.groundPieces[x, _land.heightMap[x, z], z].attributes.Add("Slant");
                   
                }
            }
        }
    }
       
    static void GenerateRocks(Land _land, float _rockiness = .1f) {

        foreach (GroundPiece ground in _land.groundPieces) {
            if (ground.Type == GroundPiece.GroundType.Empty)
                continue;

            if (ground.HasAttributes(new string[1] { "Edge" }) == true && Random.Range(0f, 1f) <= (_rockiness * 5) ) //Sides are rockier than tops
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