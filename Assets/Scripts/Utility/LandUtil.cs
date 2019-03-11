using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LandUtil
{
    public static Land currentLand;

    
    public static void Initialize(Land _land) {
        currentLand = _land;
    }

    public static void RoundIslandCorners(int _radius) {

        int xLength = currentLand.groundPieces.GetLength(0);
        int yLength = currentLand.groundPieces.GetLength(1);

        foreach (GroundPiece ground in currentLand.groundPieces) {
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
}
