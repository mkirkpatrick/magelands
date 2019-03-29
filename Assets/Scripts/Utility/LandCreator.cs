using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LandCreator
{
    public static Land CreateLand(Land.BiomeType _biomeType, Vector3 _position, int _xSize, int _ySize) {
        Land newLand = new Land(_position, _xSize, _ySize);

        newLand.Biome = _biomeType;
        LandUtil.RoundLandCorners(newLand, 12);
        LandUtil.RoughEdges(newLand, 2, 4, 8);
        LandUtil.RemoveJuts(newLand);
        LandUtil.RemoveOrphanFloaters(newLand);

        foreach (GroundPiece ground in newLand.groundPieces) {
            LandUtil.SetGroundOrientationID(ground);
        }
            

        BuildUnderPieces(newLand);

        return newLand;
    }

    public static void AssignGroundPieceID( GroundPiece _groundPiece ) {

        UnderPiece[] _underPieces = _groundPiece.underPieces.ToArray();
        foreach (UnderPiece newUnderPiece in _underPieces)
        {
            List<UnderPiece> sameHeightUnderPieces = new List<UnderPiece>();
            string underPieceID = "";
            string[] neighborsFound = new string[4];
            for (int i = 0; i < 4; i++)
                neighborsFound[i] = "";

            GroundPiece[] neighborGroundPieces = LandUtil.GetNeighborGroundPieces(_groundPiece);

            foreach (GroundPiece ground in neighborGroundPieces)
            {
                if (ground.underPieces.Count > 0)
                    sameHeightUnderPieces.Add(ground.underPieces[ground.underPieces.Count - 1]); 
            }

            foreach (UnderPiece underPiece in sameHeightUnderPieces)
            {
                if (underPiece.position.y <= newUnderPiece.position.y) {
                    if (underPiece.position.z > newUnderPiece.position.z)
                        neighborsFound[0] = "N";
                    if (underPiece.position.x > newUnderPiece.position.x)
                        neighborsFound[1] = "E";
                    if (underPiece.position.z < newUnderPiece.position.z)
                        neighborsFound[2] = "S";
                    if (underPiece.position.x < newUnderPiece.position.x)
                        neighborsFound[3] = "W";
                }
                
            }
            for (int i = 0; i < 4; i++)
                underPieceID += neighborsFound[i];

            newUnderPiece.orientationType = newUnderPiece.Type.ToString();

            switch (underPieceID)
            {
                case "N":
                case "E":
                case "S":
                case "W":
                    newUnderPiece.orientationType += "_Jut";
                    break;
                case "NE":
                case "ES":
                case "SW":
                case "NW":
                    newUnderPiece.orientationType += "_Corner";
                    break;
                case "NES":
                case "ESW":
                case "NSW":
                case "NEW":
                    newUnderPiece.orientationType += "_Straight";
                    break;
            }
            switch (underPieceID)
            {
                case "NW":
                case "NSW":
                case "W":
                    newUnderPiece.rotation = 90;
                    break;
                case "NE":
                case "NEW":
                case "N":
                    newUnderPiece.rotation = 180;
                    break;
                case "ES":
                case "NES":
                case "E":
                    newUnderPiece.rotation = 270;
                    break;
            }

            bool bottomNeighbor = false;
            foreach (UnderPiece u in _underPieces) {
                if (u.position.y < newUnderPiece.position.y) {
                    bottomNeighbor = true;
                    break;
                }
            }
            if (bottomNeighbor == false)
                newUnderPiece.orientationType += "_Bottom";
        }
    }
}