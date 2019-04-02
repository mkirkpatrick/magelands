﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LandCreator
{
    public static Land CreateLand(Land.BiomeType _biomeType, Vector3 _position, int _xSize, int _zSize) {
        Land newLand = new Land(_position, _xSize, _zSize);

        newLand.Biome = _biomeType;
        LandUtil.CreateRectangle(newLand);
        //LandUtil.RoundLandCorners(newLand, 18);
        LandUtil.RoughEdges(newLand, 12, 10, .6f);
        //LandUtil.RemoveJuts(newLand);
        //LandUtil.RemoveOrphanFloaters(newLand);

        foreach (GroundPiece ground in newLand.groundPieces) {
            LandUtil.AssignGroundPieceID(ground);
        }

        return newLand;
    }
}