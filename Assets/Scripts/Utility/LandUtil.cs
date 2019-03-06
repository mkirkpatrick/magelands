using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LandUtil
{
    public static Land currentLand;
    public static GroundPiece[,] groundPieces;

    
    public static void Initialize(Land _land) {
        currentLand = _land;
        groundPieces = new GroundPiece[_land.XSize * 16, _land.YSize * 16];

        foreach (Chunk _chunk in _land.chunks) {
            foreach (GroundPiece _ground in _chunk.groundPieces) {
                groundPieces[(_chunk.xPosition * 16) + _ground.xPosition, (_chunk.yPosition * 16) + _ground.yPosition] = _ground;
            }
        }
    }

}
