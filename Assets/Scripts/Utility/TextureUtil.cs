using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureUtil
{
    public static Vector2[] GetGroundPieceTexture(GroundPiece _ground, int _faceNum = 4)
    {
        Vector2[] uvVectors = new Vector2[4];
        Vector2 texturePos = GetTexturePosition(_ground, _faceNum);
        float tUnit = 1f / 8f;

        uvVectors[0] = new Vector2(tUnit * texturePos.x, tUnit * texturePos.y);
        uvVectors[1] = new Vector2(tUnit * texturePos.x, tUnit * texturePos.y + tUnit);
        uvVectors[2] = new Vector2(tUnit * texturePos.x + tUnit, tUnit * texturePos.y + tUnit);
        uvVectors[3] = new Vector2(tUnit * texturePos.x + tUnit, tUnit * texturePos.y);

        uvVectors = RotateTexture( uvVectors, _ground, _faceNum);

        return uvVectors;
    }

    static Vector2 GetTexturePosition(GroundPiece _ground, int _face) {
        Vector2 texturePosition = new Vector2();

        switch (_ground.Type)
        {
            case GroundPiece.GroundType.Dirt:

                if (_face == 4)
                    texturePosition = new Vector2(1, 0);
                else if (_face == 0) {
                    if (_ground.neighbors[1] == false || _ground.neighbors[3] == false)
                    {
                        if (_ground.neighbors[5] == true)
                            texturePosition = new Vector2(0, 1);
                        else
                            texturePosition = new Vector2(0, 3);
                    }
                    else if (_ground.neighbors[5] == false)
                        texturePosition = new Vector2(0, 1);
                }
                else if (_face == 1)
                {
                    if (_ground.neighbors[0] == false || _ground.neighbors[2] == false)
                    {
                        if (_ground.neighbors[5] == true)
                            texturePosition = new Vector2(0, 1);
                        else
                            texturePosition = new Vector2(0, 3);
                    }
                    else if (_ground.neighbors[5] == false)
                        texturePosition = new Vector2(0, 1);
                }
                else if (_face == 2)
                {
                    if (_ground.neighbors[1] == false || _ground.neighbors[3] == false)
                    {
                        if (_ground.neighbors[5] == true)
                            texturePosition = new Vector2(0, 1);
                        else
                            texturePosition = new Vector2(0, 3);
                    }
                    else if (_ground.neighbors[5] == false)
                        texturePosition = new Vector2(0, 1);
                }
                else if (_face == 3)
                {
                    if (_ground.neighbors[0] == false || _ground.neighbors[2] == false)
                    {
                        if (_ground.neighbors[5] == true)
                            texturePosition = new Vector2(0, 1);
                        else
                            texturePosition = new Vector2(0, 3);
                    }
                    else if (_ground.neighbors[5] == false)
                        texturePosition = new Vector2(0, 1);
                }

                break;
        }
        return texturePosition;
    }
    static Vector2[] RotateTexture(Vector2[] vectors, GroundPiece _ground, int _face) {
        Vector2[] newVectors = new Vector2[4];
        int rotation = 0;

        if (_face == 0) {
            if (_ground.neighbors[1] == false)
                rotation = 3;
            else if (_ground.neighbors[3] == false)
                rotation = 1;
            else
                rotation = 2;
        }
        else if (_face == 1)
        {
            if (_ground.neighbors[0] == false)
                rotation = 1;
            else if (_ground.neighbors[2] == false)
                rotation = 3;
            else
                rotation = 2;
        }
        else if (_face == 2)
        {
            if (_ground.neighbors[1] == false)
                rotation = 1;
            else if (_ground.neighbors[3] == false)
                rotation = 3;
            else
                rotation = 2;
        }
        else if (_face == 3)
        {
            if (_ground.neighbors[0] == false)
                rotation = 3;
            else if (_ground.neighbors[2] == false)
                rotation = 1;
            else
                rotation = 2;
        }


        switch (rotation)
        {
            case 1:
                newVectors[0] = vectors[3];
                newVectors[1] = vectors[0];
                newVectors[2] = vectors[1];
                newVectors[3] = vectors[2];
                break;
            case 2:
                newVectors[0] = vectors[2];
                newVectors[1] = vectors[3];
                newVectors[2] = vectors[0];
                newVectors[3] = vectors[1];
                break;
            case 3:
                newVectors[0] = vectors[1];
                newVectors[1] = vectors[2];
                newVectors[2] = vectors[3];
                newVectors[3] = vectors[0];
                newVectors = FlipTexture(newVectors);
                break;

            default:
                newVectors = vectors;
                break;
        }

        return newVectors;
    }
    static Vector2[] FlipTexture(Vector2[] vectors, int flipAxis = 0) {
        Vector2[] newVectors = new Vector2[4];

        if (flipAxis == 0) {
            newVectors[0] = vectors[1];
            newVectors[1] = vectors[0];
            newVectors[2] = vectors[3];
            newVectors[3] = vectors[2];
        }
        else {
            newVectors[0] = vectors[3];
            newVectors[1] = vectors[2];
            newVectors[2] = vectors[1];
            newVectors[3] = vectors[0];
        }

        return newVectors;
    }
}
