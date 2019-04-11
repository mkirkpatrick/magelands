using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LandUtil
{

    // Getters and Setters
    public static GroundPiece GetGroundPiece(Land _land, int[] position) {
        if (position[0] < 0 || position[0] > _land.groundPieces.GetLength(0)-1 || position[1] < 0 || position[1] > _land.groundPieces.GetLength(1)-1 || position[2] < 0 || position[2] > _land.groundPieces.GetLength(2)-1)
            return new GroundPiece(_land, new Vector3(position[0], position[1], position[2]), GroundPiece.GroundType.Empty);
        else {
            return _land.groundPieces[position[0], position[1], position[2]];
        }
            
    }
    public static GroundPiece[] GetNeighborGroundPieces(GroundPiece _ground, bool diagonals = false)
    {
        GroundPiece[] groundPieces = new GroundPiece[6];
        int x = (int)_ground.position.x;
        int y = (int)_ground.position.y;
        int z = (int)_ground.position.z;

        // Adjacents

        groundPieces[0] = GetGroundPiece(_ground.Land, new int[] { x, y, z + 1 });
        groundPieces[1] = GetGroundPiece(_ground.Land, new int[] { x + 1, y, z });
        groundPieces[2] = GetGroundPiece(_ground.Land, new int[] { x, y, z - 1 });
        groundPieces[3] = GetGroundPiece(_ground.Land, new int[] { x - 1, y, z });
        groundPieces[4] = GetGroundPiece(_ground.Land, new int[] { x, y + 1, z });
        groundPieces[5] = GetGroundPiece(_ground.Land, new int[] { x, y - 1, z });

        // Diagonals     TODO: Add diagonals if needed.

        return groundPieces;
    }
    public static void AssignGroundPieceNeighbors(GroundPiece _groundPiece)
    {
        int x = (int)_groundPiece.position.x;
        int y = (int)_groundPiece.position.y;
        int z = (int)_groundPiece.position.z;

        if (GetGroundPiece(_groundPiece.Land, new int[3] { x, y, z + 1 }).Type != GroundPiece.GroundType.Empty)
            _groundPiece.neighbors[0] = true;
        if (GetGroundPiece(_groundPiece.Land, new int[3] { x + 1, y, z }).Type != GroundPiece.GroundType.Empty)
            _groundPiece.neighbors[1] = true;
        if (GetGroundPiece(_groundPiece.Land, new int[3] { x, y, z - 1 }).Type != GroundPiece.GroundType.Empty)
            _groundPiece.neighbors[2] = true;
        if (GetGroundPiece(_groundPiece.Land, new int[3] { x - 1, y, z }).Type != GroundPiece.GroundType.Empty)
            _groundPiece.neighbors[3] = true;
        if (GetGroundPiece(_groundPiece.Land, new int[3] { x, y + 1, z }).Type != GroundPiece.GroundType.Empty)
            _groundPiece.neighbors[4] = true;
        if (GetGroundPiece(_groundPiece.Land, new int[3] { x, y - 1, z }).Type != GroundPiece.GroundType.Empty)
            _groundPiece.neighbors[5] = true;
    }
    public static GroundPiece[] GetEdgePieces(Land _land)
    {
        List<GroundPiece> groundPieces = new List<GroundPiece>();

        foreach (GroundPiece ground in _land.groundPieces)
        {
            if (ground.Type == GroundPiece.GroundType.Empty)
                continue;

            if (ground.position.x == 0 || ground.position.x == (_land.groundPieces.GetLength(0) - 1))
                groundPieces.Add(ground);
            else if (ground.position.z == 0 || ground.position.z == (_land.groundPieces.GetLength(2) - 1))
                groundPieces.Add(ground);
            else
            {
                GroundPiece[] neighbors = GetNeighborGroundPieces(ground);

                foreach (GroundPiece neighborGround in neighbors)
                {
                    if (neighborGround.Type == GroundPiece.GroundType.Empty)
                    {
                        groundPieces.Add(ground);
                        break;
                    }
                }
            }
        }
        return groundPieces.ToArray();
    }
    public static void RemoveArea(Land _land, int xStart, int zStart, int xEnd, int zEnd) {
        for (int x = xStart; x < xEnd; x++) {
            for (int z = zStart; z < zEnd; x++)
            {
                _land.groundPieces[x, _land.levelHeight, z].Type = GroundPiece.GroundType.Empty;
            }
        }
    }
    
    //Ground Formation
    public static void CreateBox(Land _land, Vector3 _position, Vector3 _size) {

        for (int x = 0; x < _size.x; x++) {
            for (int z = 0; z < _size.z; z++) {
                for (int y = 0; y < _size.y; y++) {
                    _land.groundPieces[(int)_position.x + x, (int)_position.y + y, (int)_position.z + z].Type = GroundPiece.GroundType.Dirt;
                }
            }
        }
    }
    public static void RoundLandCorners(Land _land, int insetValue, int iterations = 1) {

        int xCount = 0;
        int zCount = 0;

        Vector2 area;

        //Bottom Left
        for (int i = 0; i < iterations; i++) {
            if (i > 0)
            {
                int smallerInset = insetValue - (i * 2);
                area = new Vector2(Random.Range( smallerInset - 1, smallerInset + 2), Random.Range(smallerInset - 1, smallerInset + 2));

                for (int x = xCount; x < xCount + area.x; x++)
                {
                    for (int z = 0; z < area.y; z++)
                    {
                        _land.groundPieces[x, _land.levelHeight, z].Type = GroundPiece.GroundType.Empty;
                    }
                }
                xCount += (int)area.x;

                area = new Vector2(Random.Range(smallerInset - 1, smallerInset + 2), Random.Range(smallerInset - 1, smallerInset + 2));

                for (int x = 0; x < area.x; x++)
                {
                    for (int z = zCount; z < zCount + area.y; z++)
                    {
                        _land.groundPieces[x, _land.levelHeight, z].Type = GroundPiece.GroundType.Empty;
                    }
                }
                zCount += (int)area.y;
            }
            else {
                area = new Vector2(Random.Range(insetValue - 1, insetValue + 2), Random.Range(insetValue - 1, insetValue + 2));

                for (int x = 0; x < area.x; x++)
                {
                    for (int z = 0; z < area.y; z++)
                    {
                        _land.groundPieces[x, _land.levelHeight, z].Type = GroundPiece.GroundType.Empty;
                    }
                }
                xCount = (int)area.x;
                zCount = (int)area.y;
            }
        }

        //Top Left
        for (int i = 0; i < iterations; i++)
        {
            if (i > 0)
            {
                int smallerInset = insetValue - (i * 2);
                area = new Vector2(Random.Range(smallerInset - 1, smallerInset + 2), Random.Range(smallerInset - 1, smallerInset + 2));

                for (int x = xCount; x < xCount + area.x; x++)
                {
                    for (int z = _land.ZSize - (int)area.y; z < _land.ZSize; z++)
                    {
                        _land.groundPieces[x, _land.levelHeight, z].Type = GroundPiece.GroundType.Empty;
                    }
                }
                xCount += (int)area.x;

                area = new Vector2(Random.Range(smallerInset - 1, smallerInset + 2), Random.Range(smallerInset - 1, smallerInset + 2));

                for (int x = 0; x < area.x; x++)
                {
                    for (int z = _land.ZSize - ((int)area.y + zCount); z < _land.ZSize; z++)
                    {
                        _land.groundPieces[x, _land.levelHeight, z].Type = GroundPiece.GroundType.Empty;
                    }
                }
                zCount += (int)area.y;
            }
            else
            {
                area = new Vector2(Random.Range(insetValue - 1, insetValue + 2), Random.Range(insetValue - 1, insetValue + 2));

                for (int x = 0; x < area.x; x++)
                {
                    for (int z = _land.ZSize - (int)area.y; z < _land.ZSize; z++)
                    {
                        _land.groundPieces[x, _land.levelHeight, z].Type = GroundPiece.GroundType.Empty;
                    }
                }
                xCount = (int)area.x;
                zCount = (int)area.y;
            }
        }

        //Top Right
        for (int i = 0; i < iterations; i++)
        {
            if (i > 0)
            {
                int smallerInset = insetValue - (i * 2);
                area = new Vector2(Random.Range(smallerInset - 1, smallerInset + 2), Random.Range(smallerInset - 1, smallerInset + 2));

                for (int x = _land.XSize - ((int)area.x + xCount); x < _land.XSize; x++)
                {
                    for (int z = _land.ZSize - (int)area.y; z < _land.ZSize; z++)
                    {
                        _land.groundPieces[x, _land.levelHeight, z].Type = GroundPiece.GroundType.Empty;
                    }
                }
                xCount += (int)area.x;

                area = new Vector2(Random.Range(smallerInset - 1, smallerInset + 2), Random.Range(smallerInset - 1, smallerInset + 2));

                for (int x = _land.XSize - (int)area.x; x < _land.XSize; x++)
                {
                    for (int z = _land.ZSize - ((int)area.y + zCount); z < _land.ZSize; z++)
                    {
                        _land.groundPieces[x, _land.levelHeight, z].Type = GroundPiece.GroundType.Empty;
                    }
                }
                zCount += (int)area.y;
            }
            else
            {
                area = new Vector2(Random.Range(insetValue - 1, insetValue + 2), Random.Range(insetValue - 1, insetValue + 2));

                for (int x = _land.XSize - (int)area.x; x < _land.XSize; x++)
                {
                    for (int z = _land.ZSize - (int)area.y; z < _land.ZSize; z++)
                    {
                        _land.groundPieces[x, _land.levelHeight, z].Type = GroundPiece.GroundType.Empty;
                    }
                }
                xCount = (int)area.x;
                zCount = (int)area.y;
            }
        }

        //Bottom Right
        for (int i = 0; i < iterations; i++)
        {
            if (i > 0)
            {
                int smallerInset = insetValue - (i * 2);
                area = new Vector2(Random.Range(smallerInset - 1, smallerInset + 2), Random.Range(smallerInset - 1, smallerInset + 2));

                for (int x = _land.XSize - ((int)area.x + xCount); x < _land.XSize; x++)
                {
                    for (int z = 0; z < area.y; z++)
                    {
                        _land.groundPieces[x, _land.levelHeight, z].Type = GroundPiece.GroundType.Empty;
                    }
                }
                xCount += (int)area.x;

                area = new Vector2(Random.Range(smallerInset - 1, smallerInset + 2), Random.Range(smallerInset - 1, smallerInset + 2));

                for (int x = _land.XSize - (int)area.x; x < _land.XSize; x++)
                {
                    for (int z = zCount; z < area.y + zCount; z++)
                    {
                        _land.groundPieces[x, _land.levelHeight, z].Type = GroundPiece.GroundType.Empty;
                    }
                }
                zCount += (int)area.y;
            }
            else
            {
                area = new Vector2(Random.Range(insetValue - 1, insetValue + 2), Random.Range(insetValue - 1, insetValue + 2));

                for (int x = _land.XSize - (int)area.x; x < _land.XSize; x++)
                {
                    for (int z = 0; z < area.y; z++)
                    {
                        _land.groundPieces[x, _land.levelHeight, z].Type = GroundPiece.GroundType.Empty;
                    }
                }
                xCount = (int)area.x;
                zCount = (int)area.y;
            }
        }
        
    }

    public static void RoughEdges(Land _land, int _cutDepth = 4, float _scale = .05f) {

        float scale = _scale;
        float randomX = Random.Range(0f, 1f);
        float randomY = Random.Range(0f, 1f);


        for (int x = 0; x < _land.XSize; x++) {
            int cutDepth = Mathf.FloorToInt( Mathf.PerlinNoise((x * scale) + randomX, randomY) * (_cutDepth + 1) );

            for (int depth = 0; depth < cutDepth; depth++) {
                _land.groundPieces[x, _land.levelHeight, _land.ZSize - (depth + 1)].Type = GroundPiece.GroundType.Empty;
            }
        }
        randomX = Random.Range(0f, 1f);
        randomY = Random.Range(0f, 1f);
        for (int z = 0; z < _land.ZSize; z++)
        {
            int cutDepth = Mathf.FloorToInt(Mathf.PerlinNoise(randomX, (z * scale) + randomY) * (_cutDepth + 1));

            for (int depth = 0; depth < cutDepth; depth++)
            {
                _land.groundPieces[_land.XSize - (depth + 1), _land.levelHeight, z].Type = GroundPiece.GroundType.Empty;
            }
        }
        randomX = Random.Range(0f, 1f);
        randomY = Random.Range(0f, 1f);
        for (int x = 0; x < _land.XSize; x++)
        {
            int cutDepth = Mathf.FloorToInt(Mathf.PerlinNoise((x * scale) + randomX, randomY) * (_cutDepth + 1));

            for (int depth = 0; depth < cutDepth; depth++)
            {
                _land.groundPieces[x, _land.levelHeight, depth].Type = GroundPiece.GroundType.Empty;
            }
        }
        randomX = Random.Range(0f, 1f);
        randomY = Random.Range(0f, 1f);
        for (int z = 0; z < _land.ZSize; z++)
        {
            int cutDepth = Mathf.FloorToInt(Mathf.PerlinNoise(randomX, (z * scale) + randomY) * (_cutDepth + 1));

            for (int depth = 0; depth < cutDepth; depth++)
            {
                _land.groundPieces[depth, _land.levelHeight, z].Type = GroundPiece.GroundType.Empty;
            }
        }
    }

    public static void RemoveOrphanFloaters(Land _land) {
        GroundPiece[] groundPieces = GetEdgePieces(_land);

        foreach (GroundPiece ground in groundPieces) {
            GroundPiece[] neighbors = GetNeighborGroundPieces(ground);
            bool neighborFound = false;

            foreach (GroundPiece neighborGround in neighbors) {
                if (neighborGround.Type != GroundPiece.GroundType.Empty) {
                    neighborFound = true;
                    break;
                }
            }
            if (neighborFound == false)
                ground.Type = GroundPiece.GroundType.Empty;

        }
    }
    public static void RemoveJutsAndHoles(Land _land) {

        foreach (GroundPiece ground in _land.groundPieces)
        {

            int counter = 0;
            for (int i = 0; i < 4; i++) {
                if (ground.neighbors[i] == true)
                    counter++;
            }
            if (counter <= 1)
                ground.Type = GroundPiece.GroundType.Empty;
            else if (counter == 4)
                ground.Type = GroundPiece.GroundType.Dirt;
        }
    }
    public static void ElevateLand(Land _land, int elevationMax, float _scale = .04f) {
        float scale = _scale;
        float randomX = Random.Range(0f, 1f);
        float randomY = Random.Range(0f, 1f);

        for (int x = 0; x < _land.XSize; x++) {
            for (int z = 0; z < _land.ZSize; z++) {
                if (_land.groundPieces[x, _land.levelHeight, z].Type == GroundPiece.GroundType.Empty)
                    continue;

                int heightValue = Mathf.FloorToInt( Mathf.PerlinNoise( (x * scale) + randomX, (z * scale) + randomY) * (elevationMax + 1) );

                for (int i = _land.levelHeight + 1; i < _land.levelHeight + heightValue; i++)
                    _land.groundPieces[x, i, z].Type = GroundPiece.GroundType.Dirt;
            }
        }
    }
}
