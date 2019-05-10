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

        if (_ground.HasAttributes("Slant") == true)
        {
            int x, y;
            x = (int)_ground.position.x;
            y = (int)_ground.position.y;

            int[] heightNeighbors = HeightMapUtil.GetMapNeighbors(_ground.Land.heightMap, new int[2] { x, y });
            float yVal = 23f / 128f;

            uvVectors[0] = new Vector2(tUnit * texturePos.x, yVal * texturePos.y);
            uvVectors[1] = new Vector2(tUnit * texturePos.x, yVal * texturePos.y + yVal);
            uvVectors[2] = new Vector2(tUnit * texturePos.x + tUnit, yVal * texturePos.y + yVal);
            uvVectors[3] = new Vector2(tUnit * texturePos.x + tUnit, yVal * texturePos.y);
            uvVectors = AlignTexture(uvVectors, _ground, _faceNum);
        }
        else {
            uvVectors[0] = new Vector2(tUnit * texturePos.x, tUnit * texturePos.y);
            uvVectors[1] = new Vector2(tUnit * texturePos.x, tUnit * texturePos.y + tUnit);
            uvVectors[2] = new Vector2(tUnit * texturePos.x + tUnit, tUnit * texturePos.y + tUnit);
            uvVectors[3] = new Vector2(tUnit * texturePos.x + tUnit, tUnit * texturePos.y);
            uvVectors = AlignTexture(uvVectors, _ground, _faceNum);
        }

        //uvVectors = RotateTexture( uvVectors, _ground, _faceNum);

        return uvVectors;
    }

    static Vector2 GetTexturePosition(GroundPiece _ground, int _face) {
        Vector2 texturePosition = new Vector2();

        GroundPiece[] neighbors = LandUtil.GetNeighborGroundPieces(_ground);

        switch (_ground.id)
        {
            case 1:

                if (_face == 4)
                    texturePosition = new Vector2(1, 0);
                else if (_face == 0) {
                    if (_ground.neighbors[1] == false || _ground.neighbors[3] == false )
                    {
                        if (_ground.neighbors[5] == true)
                            texturePosition = new Vector2(0, 3);
                        else
                            texturePosition = new Vector2(0, 5);
                    }
                    else if (_ground.neighbors[5] == false)
                        texturePosition = new Vector2(0, 1);
                }
                else if (_face == 1)
                {
                    if (_ground.neighbors[0] == false || _ground.neighbors[2] == false )
                    {
                        if (_ground.neighbors[5] == true)
                            texturePosition = new Vector2(0, 4);
                        else
                            texturePosition = new Vector2(0, 6);
                    }
                    else if (_ground.neighbors[5] == false)
                        texturePosition = new Vector2(0, 1);
                }
                else if (_face == 2)
                {
                    if (_ground.neighbors[1] == false || _ground.neighbors[3] == false )
                    {
                        if (_ground.neighbors[5] == true)
                            texturePosition = new Vector2(0, 3);
                        else
                            texturePosition = new Vector2(0, 5);
                    }
                    else if (_ground.neighbors[5] == false)
                        texturePosition = new Vector2(0, 1);
                }
                else if (_face == 3)
                {
                    if (_ground.neighbors[0] == false || _ground.neighbors[2] == false )
                    {
                        if (_ground.neighbors[5] == true)
                            texturePosition = new Vector2(0, 4);
                        else
                            texturePosition = new Vector2(0, 6);
                    }
                    else if (_ground.neighbors[5] == false)
                        texturePosition = new Vector2(0, 1);
                }

                break;

            case 2:

                if (_ground.HasAttributes("Slant") == true) {
                    int counter = 0;
                    for(int i = 0; i < 4; i++)
                        if (neighbors[i].id != 0 && neighbors[i].HasAttributes("Slant") == false)
                            counter++;

                    if(counter == 2)
                        texturePosition = new Vector2(2, 1);
                    else
                        texturePosition = new Vector2(2, 0);
                }   
                else
                    texturePosition = new Vector2(0, 0);
                break;

            case 3:
                if (_face == 4)
                    texturePosition = new Vector2(1, 0);
                else
                    texturePosition = new Vector2(1, 1);
                break;

        }
        return texturePosition;
    }
    static Vector2[] AlignTexture(Vector2[] _vectors, GroundPiece _ground, int _face) {
        Vector2[] newVectors = _vectors;
        bool[] neighbors = _ground.neighbors;

        if (_ground.HasAttributes("Slant") == true)
        {
            if (_ground.HasAttributes("Facing_E") == true)
            {
                if (_ground.neighbors[2] == true)
                    newVectors = RotateTexture(newVectors, 3);
                else
                    newVectors = RotateTexture(newVectors, 1);
            }
            else if (_ground.HasAttributes("Facing_S") == true) {
                if (_ground.neighbors[3] == true)
                    newVectors = RotateTexture(newVectors, 0);
                else
                    newVectors = RotateTexture(newVectors, 2);
            }
            else if (_ground.HasAttributes("Facing_W") == true) {
                if (_ground.neighbors[2] == true)
                    newVectors = RotateTexture(newVectors, 3);
                else
                    newVectors = RotateTexture(newVectors, 1);
            }
            else if (_ground.HasAttributes("Facing_N") == true) {
                if (_ground.neighbors[1] == true)
                    newVectors = RotateTexture(newVectors, 2);
            }

        }
        else {
            switch (_face)
            {
                case 0:
                    if (neighbors[3] == false)
                        newVectors = FlipTexture(newVectors, 1);
                    break;
                case 1:
                    if (neighbors[2] == false)
                        newVectors = FlipTexture(newVectors, 1);
                    break;
                case 2:
                    if (neighbors[1] == false)
                        newVectors = FlipTexture(newVectors, 1);
                    break;
                case 3:
                    if (neighbors[0] == false)
                        newVectors = FlipTexture(newVectors, 1);
                    break;
                case 4:
                    newVectors = RotateTexture(newVectors, Random.Range(0, 4));
                    break;
            }
        }

        return newVectors;
    }
    static Vector2[] RotateTexture(Vector2[] _vectors, int _rotation) {
        Vector2[] newVectors = new Vector2[4];

        switch (_rotation)
        {
            case 1:
                newVectors[0] = _vectors[3];
                newVectors[1] = _vectors[0];
                newVectors[2] = _vectors[1];
                newVectors[3] = _vectors[2];
                break;
            case 2:
                newVectors[0] = _vectors[2];
                newVectors[1] = _vectors[3];
                newVectors[2] = _vectors[0];
                newVectors[3] = _vectors[1];
                break;
            case 3:
                newVectors[0] = _vectors[1];
                newVectors[1] = _vectors[2];
                newVectors[2] = _vectors[3];
                newVectors[3] = _vectors[0];
                break;

            default:
                newVectors = _vectors;
                break;
        }

        return newVectors;
    }
    static Vector2[] FlipTexture(Vector2[] vectors, int flipAxis = 0) {
        Vector2[] newVectors = new Vector2[4];

        //Vertical
        if (flipAxis == 0) {
            newVectors[0] = vectors[1];
            newVectors[1] = vectors[0];
            newVectors[2] = vectors[3];
            newVectors[3] = vectors[2];
        }
        //Horizontal
        else {
            newVectors[0] = vectors[3];
            newVectors[1] = vectors[2];
            newVectors[2] = vectors[1];
            newVectors[3] = vectors[0];
        }

        return newVectors;
    }
}
