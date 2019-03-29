using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LandUtil
{

    // Getters
    public static GroundPiece[] GetNeighborGroundPieces(GroundPiece _ground, bool diagonals = false)
    {
        List<GroundPiece> groundPieces = new List<GroundPiece>();

        // Adjacents
        if (_ground.xPosition != 0)
            groundPieces.Add(_ground.Land.groundPieces[_ground.xPosition - 1, _ground.yPosition]);
        if (_ground.xPosition != _ground.Land.groundPieces.GetLength(0) - 1)
            groundPieces.Add(_ground.Land.groundPieces[_ground.xPosition + 1, _ground.yPosition]);
        if (_ground.yPosition != 0)
            groundPieces.Add(_ground.Land.groundPieces[_ground.xPosition, _ground.yPosition - 1]);
        if (_ground.yPosition != _ground.Land.groundPieces.GetLength(1) - 1)
            groundPieces.Add(_ground.Land.groundPieces[_ground.xPosition, _ground.yPosition + 1]);

        // Diagonals     TODO: Add diagonals if needed.

        return groundPieces.ToArray();
    }
    public static void SetGroundOrientationID(GroundPiece _groundPiece) {
        string neighborCode = "";

        string[] neighborsFound = new string[4];
        for (int i = 0; i < 4; i++)
            neighborsFound[i] = "";

        GroundPiece[] neighborGroundPieces = LandUtil.GetNeighborGroundPieces(_groundPiece);

        foreach (GroundPiece ground in neighborGroundPieces) {

            if (ground.yPosition > _groundPiece.yPosition)
                neighborsFound[0] = "N";
            if (ground.xPosition > _groundPiece.xPosition)
                neighborsFound[1] = "E";
            if (ground.yPosition < _groundPiece.yPosition)
                neighborsFound[2] = "S";
            if (ground.xPosition < _groundPiece.xPosition)
                neighborsFound[3] = "W";
        }
        
        for (int i = 0; i < 4; i++)
            neighborCode += neighborsFound[i];

        _groundPiece.orientationID = _groundPiece.Type.ToString();

        switch (neighborCode)
        {
            case "NESW":
                _groundPiece.orientationID += "_Top";
                break;
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
        switch (neighborCode)
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
    }
    public static GroundPiece[] GetEdgePieces(Land _land)
    {
        List<GroundPiece> groundPieces = new List<GroundPiece>();

        foreach (GroundPiece ground in _land.groundPieces)
        {

            if (ground.Type == GroundPiece.GroundType.Empty)
                continue;

            if (ground.xPosition == 0 || ground.xPosition == (_land.groundPieces.GetLength(0) - 1))
                groundPieces.Add(ground);
            else if (ground.yPosition == 0 || ground.yPosition == (_land.groundPieces.GetLength(1) - 1))
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
    public static void RoundLandCorners(Land _land, int _radius) {

        int xLength = _land.groundPieces.GetLength(0);
        int yLength = _land.groundPieces.GetLength(1);

        foreach (GroundPiece ground in _land.groundPieces) {
            if (ground.xPosition < _radius)
            {
                if (_radius > ground.yPosition)
                {
                    if (Vector2.Distance(new Vector2(ground.xPosition, ground.yPosition), new Vector2(_radius, _radius)) > _radius)
                        ground.Type = GroundPiece.GroundType.Empty;
                }
                else if (ground.yPosition > yLength - _radius - 1) {
                    if (Vector2.Distance(new Vector2(ground.xPosition, ground.yPosition), new Vector2(_radius, yLength - _radius - 1)) > _radius)
                        ground.Type = GroundPiece.GroundType.Empty;
                }
            }
            else if (ground.xPosition > (xLength - _radius - 1)) {
                if (_radius > ground.yPosition) {
                    if (Vector2.Distance(new Vector2(ground.xPosition, ground.yPosition), new Vector2((xLength - _radius - 1), _radius)) > _radius)
                        ground.Type = GroundPiece.GroundType.Empty;
                }
                else if (ground.yPosition > yLength - _radius - 1)
                {
                    if (Vector2.Distance(new Vector2(ground.xPosition, ground.yPosition), new Vector2(xLength - _radius - 1, yLength - _radius - 1)) > _radius)
                        ground.Type = GroundPiece.GroundType.Empty;
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
