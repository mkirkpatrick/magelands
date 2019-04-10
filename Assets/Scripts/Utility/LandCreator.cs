using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LandCreator
{
    public static Land CreateLand(Land.BiomeType _biomeType, Vector3 _position, int _xSize, int _zSize) {
        Land newLand = new Land(_position, _xSize, _zSize);

        newLand.Biome = _biomeType;
        LandUtil.CreateRectangle(newLand);
        LandUtil.RoundLandCorners(newLand, 8, 6);
        LandUtil.RoughEdges(newLand, 6, .04f);
        LandUtil.ElevateLand(newLand, 4, .015f);

        foreach (GroundPiece ground in newLand.groundPieces) {
            LandUtil.AssignGroundPieceNeighbors(ground);
        }

        LandUtil.RemoveJutsAndHoles(newLand);

        return newLand;
    }
}