using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LandUtil
{

    // Getters and Setters
    public static GroundPiece[] GetNeighborGroundPieces(GroundPiece _ground, bool diagonals = false)
    {
        GroundPiece[] groundPieces = new GroundPiece[6];

        // Adjacents
        if (_ground.position.z != _ground.Land.groundPieces.GetLength(2) - 1)
            groundPieces[0] = _ground.Land.groundPieces[(int)_ground.position.x, (int)_ground.position.y, (int)_ground.position.z + 1];
        else
            groundPieces[0] = new GroundPiece(null, new Vector3(0,0,0), GroundPiece.GroundType.Empty);
        if (_ground.position.x != _ground.Land.groundPieces.GetLength(0) - 1)
            groundPieces[1] = _ground.Land.groundPieces[(int)_ground.position.x + 1, (int)_ground.position.y, (int)_ground.position.z];
        else
            groundPieces[1] = new GroundPiece(null, new Vector3(0, 0, 0), GroundPiece.GroundType.Empty);
        if (_ground.position.z != 0)
            groundPieces[2] = _ground.Land.groundPieces[(int)_ground.position.x, (int)_ground.position.y, (int)_ground.position.z - 1];
        else
            groundPieces[2] = new GroundPiece(null, new Vector3(0, 0, 0), GroundPiece.GroundType.Empty);
        if (_ground.position.x != 0)
            groundPieces[3] = _ground.Land.groundPieces[(int)_ground.position.x - 1, (int)_ground.position.y, (int)_ground.position.z];
        else
            groundPieces[3] = new GroundPiece(null, new Vector3(0, 0, 0), GroundPiece.GroundType.Empty);

        //Top and Bottom
        if (_ground.position.y != _ground.Land.groundPieces.GetLength(1) - 1)
            groundPieces[4] = _ground.Land.groundPieces[(int)_ground.position.x, (int)_ground.position.y + 1, (int)_ground.position.z];
        else
            groundPieces[4] = new GroundPiece(null, new Vector3(0, 0, 0), GroundPiece.GroundType.Empty);
        if (_ground.position.y != 0)
            groundPieces[5] = _ground.Land.groundPieces[(int)_ground.position.x, (int)_ground.position.y - 1, (int)_ground.position.z];
        else
            groundPieces[5] = new GroundPiece(null, new Vector3(0, 0, 0), GroundPiece.GroundType.Empty);

        // Diagonals     TODO: Add diagonals if needed.

        return groundPieces;
    }
    public static void AssignGroundPieceID(GroundPiece _groundPiece)
    {
        if (_groundPiece.Type == GroundPiece.GroundType.Empty)
            return;

        string adjacentPieceID = "";
        string[] adjacentsFound = new string[4];
        for (int i = 0; i < 4; i++)
            adjacentsFound[i] = "";

        string verticalPieceID = "";
        string[] verticalsFound = new string[2];
        for (int i = 0; i < 2; i++)
            verticalsFound[i] = "";

        GroundPiece[] neighborGroundPieces = LandUtil.GetNeighborGroundPieces(_groundPiece);

        foreach (GroundPiece neighbor in neighborGroundPieces)
        {
            if (neighbor.Type != GroundPiece.GroundType.Empty)
            {
                if (neighbor.position.z > _groundPiece.position.z)
                    adjacentsFound[0] = "N";
                if (neighbor.position.x > _groundPiece.position.x)
                    adjacentsFound[1] = "E";
                if (neighbor.position.z < _groundPiece.position.z)
                    adjacentsFound[2] = "S";
                if (neighbor.position.x < _groundPiece.position.x)
                    adjacentsFound[3] = "W";
                if (neighbor.position.y > _groundPiece.position.y)
                    verticalsFound[0] = "T";
                if (neighbor.position.y < _groundPiece.position.y)
                    verticalsFound[1] = "B";
            }
        }
        for (int i = 0; i < 4; i++)
            adjacentPieceID += adjacentsFound[i];
        for (int i = 0; i < 2; i++)
            verticalPieceID += verticalsFound[i];

        if (adjacentPieceID == "NESW" && verticalPieceID == "TB") {
            _groundPiece.Type = GroundPiece.GroundType.Surrounded;
            _groundPiece.orientationID = _groundPiece.Type.ToString();
            return;
        }

        _groundPiece.orientationID = _groundPiece.Type.ToString();

        switch (adjacentPieceID)
        {

            case "N":
            case "E":
            case "S":
            case "W":
                _groundPiece.orientationID += "_Jut";
                break;
            case "NE":
            case "ES":
            case "SW":
            case "NW":
                _groundPiece.orientationID += "_Corner";
                break;
            case "NES":
            case "ESW":
            case "NSW":
            case "NEW":
                _groundPiece.orientationID += "_Straight";
                break;
        }
        switch (adjacentPieceID)
        {
            case "NW":
            case "NSW":
            case "W":
                _groundPiece.rotation = 90;
                break;
            case "NE":
            case "NEW":
            case "N":
                _groundPiece.rotation = 180;
                break;
            case "ES":
            case "NES":
            case "E":
                _groundPiece.rotation = 270;
                break;
        }
        switch (verticalPieceID)
        {
            case "":
                _groundPiece.orientationID += "_Bottom";
                break;
            case "T":
                _groundPiece.orientationID += "_Bottom";
                _groundPiece.topBottomNeighbors[0] = true;
                break;
            case "B":
                _groundPiece.topBottomNeighbors[1] = true;
                break;
        }
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
            else if (ground.position.z == 0 || ground.position.z == (_land.groundPieces.GetLength(1) - 1))
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

    //Ground Formation
    public static void CreateRectangle(Land _land, int height) {
        int xSize = _land.groundPieces.GetLength(0);
        int zSize = _land.groundPieces.GetLength(2);

        for (int x = 0; x < xSize; x++) {
            for (int z = 0; z < zSize; z++) {
                for (int y = 0; y < height; y++) {
                    _land.groundPieces[x, y, z].Type = GroundPiece.GroundType.Dirt;
                }
            }
        }
    }
    public static void RoundLandCorners(Land _land, int _radius) {

        int xLength = _land.groundPieces.GetLength(0);
        int zLength = _land.groundPieces.GetLength(2);

        for (int x = 0; x < xLength; x++) {
            for (int z = 0; z < zLength; z++) {
                if (_land.groundPieces[x,0,z].position.x < _radius)
                {
                    if (_radius > z)
                    {
                        if (Vector2.Distance(new Vector2(x, z), new Vector2(_radius, _radius)) > _radius)
                            for (int y = 0; y < 32; y++)
                                _land.groundPieces[x, 0, z].Type = GroundPiece.GroundType.Empty;
                    }
                    else if (z > zLength - _radius - 1)
                    {
                        if (Vector2.Distance(new Vector2(x, z), new Vector2(_radius, zLength - _radius - 1)) > _radius)
                            for(int y = 0; y < 32; y++)
                                _land.groundPieces[x, y, z].Type = GroundPiece.GroundType.Empty;
                    }
                }
                else if (x > (xLength - _radius - 1))
                {
                    if (_radius > z)
                    {
                        if (Vector2.Distance(new Vector2(x, z), new Vector2((xLength - _radius - 1), _radius)) > _radius)
                            for (int y = 0; y < 32; y++)
                                _land.groundPieces[x, y, z].Type = GroundPiece.GroundType.Empty;
                    }
                    else if (z > zLength - _radius - 1)
                    {
                        if (Vector2.Distance(new Vector2(x, z), new Vector2(xLength - _radius - 1, zLength - _radius - 1)) > _radius)
                            for (int y = 0; y < 32; y++)
                                _land.groundPieces[x, y, z].Type = GroundPiece.GroundType.Empty;
                    }
                }
            }
        }
    }

    public static void RoughEdges(Land _land, int iterations = 1, int minDist = 5, int maxDist = 10) {

        for (int i = 0; i < iterations; i++) {
            GroundPiece[] edges = GetEdgePieces(_land);

            int counter = Random.Range(minDist, maxDist+1);

            while (counter < edges.Length) {
                edges[counter].Type = GroundPiece.GroundType.Empty;

                GroundPiece[] neighbors = GetNeighborGroundPieces(edges[counter]);
                foreach (GroundPiece neighbor in neighbors)
                    neighbor.Type = GroundPiece.GroundType.Empty;

                counter += Random.Range(minDist, maxDist + 1);
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

    public static void RemoveJuts(Land _land) {
        GroundPiece[] groundPieces = GetEdgePieces(_land);

        foreach (GroundPiece ground in groundPieces)
        {
            GroundPiece[] neighbors = GetNeighborGroundPieces(ground);

            int counter = 0;
            foreach (GroundPiece neighbor in neighbors) {
                if (neighbor.Type != GroundPiece.GroundType.Empty)
                    counter++;
            }
            if (counter == 1)
                ground.Type = GroundPiece.GroundType.Empty;
        }
    }
}
