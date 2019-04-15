using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshUtil
{

    public static Vector3[] AddMeshFace(GroundPiece _ground, int _faceNum) {
        Vector3[] newVertices = new Vector3[4];

        Vector3 localPosition = new Vector3(_ground.position.x % 32, _ground.position.y % 16, _ground.position.z % 32);

        switch (_faceNum)
        {
            case 0:
                newVertices = FaceNorth(localPosition);
                break;
            case 1:
                newVertices = FaceEast(localPosition);
                break;
            case 2:
                newVertices = FaceSouth(localPosition);
                break;
            case 3:
                newVertices = FaceWest(localPosition);
                break;
            case 4:
                newVertices = FaceTop(localPosition);
                break;
            case 5:
                newVertices = FaceBottom(localPosition);
                break;
        }

        return newVertices;

        Vector3[] FaceNorth(Vector3 _position)
        {
            Vector3[] newVectors = new Vector3[4];
            Vector3 pos = _position;

            newVectors[0] = new Vector3(pos.x + 1, pos.y - 1, pos.z + 1);
            newVectors[1] = new Vector3(pos.x + 1, pos.y, pos.z + 1);
            newVectors[2] = new Vector3(pos.x, pos.y, pos.z + 1);
            newVectors[3] = new Vector3(pos.x, pos.y - 1, pos.z + 1);

            return newVectors;

        }
        Vector3[] FaceEast(Vector3 _position)
        {
            Vector3[] newVectors = new Vector3[4];
            Vector3 pos = _position;

            newVectors[0] = new Vector3(pos.x + 1, pos.y - 1, pos.z);
            newVectors[1] = new Vector3(pos.x + 1, pos.y, pos.z);
            newVectors[2] = new Vector3(pos.x + 1, pos.y, pos.z + 1);
            newVectors[3] = new Vector3(pos.x + 1, pos.y - 1, pos.z + 1);

            return newVectors;
        }
        Vector3[] FaceSouth(Vector3 _position)
        {
            Vector3[] newVectors = new Vector3[4];
            Vector3 pos = _position;

            newVectors[0] = new Vector3(pos.x, pos.y - 1, pos.z);
            newVectors[1] = new Vector3(pos.x, pos.y, pos.z);
            newVectors[2] = new Vector3(pos.x + 1, pos.y, pos.z);
            newVectors[3] = new Vector3(pos.x + 1, pos.y - 1, pos.z);

            return newVectors;

        }
        Vector3[] FaceWest(Vector3 _position)
        {
            Vector3[] newVectors = new Vector3[4];
            Vector3 pos = _position;

            newVectors[0] = new Vector3(pos.x, pos.y - 1, pos.z + 1);
            newVectors[1] = new Vector3(pos.x, pos.y, pos.z + 1);
            newVectors[2] = new Vector3(pos.x, pos.y, pos.z);
            newVectors[3] = new Vector3(pos.x, pos.y - 1, pos.z);

            return newVectors;
        }
        Vector3[] FaceTop(Vector3 _position)
        {
            Vector3[] newVectors = new Vector3[4];
            Vector3 pos = _position;

            newVectors[0] = new Vector3(pos.x, pos.y, pos.z);
            newVectors[1] = new Vector3(pos.x, pos.y, pos.z + 1);
            newVectors[2] = new Vector3(pos.x + 1, pos.y, pos.z + 1);
            newVectors[3] = new Vector3(pos.x + 1, pos.y, pos.z);

            return newVectors;
        }
        Vector3[] FaceBottom(Vector3 _position)
        {
            Vector3[] newVectors = new Vector3[4];
            Vector3 pos = _position;

            newVectors[0] = new Vector3(pos.x + 1, pos.y - 1, pos.z);
            newVectors[1] = new Vector3(pos.x + 1, pos.y - 1, pos.z + 1);
            newVectors[2] = new Vector3(pos.x, pos.y - 1, pos.z + 1);
            newVectors[3] = new Vector3(pos.x, pos.y - 1, pos.z);

            return newVectors;
        }
    }

    public static Mesh[] GenerateGrassEdges(GroundPiece[] _groundPieces) {

        List<Mesh> newMeshes = new List<Mesh>();
        int rotation = 0;

        foreach (GroundPiece ground in _groundPieces) {

            if (ground.neighbors[0] == false) {
                if (ground.neighbors[1] == false)
                    // Load corner grass
                else if (ground.neighbors[3] == false) {
                    // Load corner grass
                    rotation = 270;
                }
                else
                    // Load Edge Grass
            }
            else if (ground.neighbors[1] == false)
            {
                if (ground.neighbors[0] == false)
                    // Load corner grass
                else if (ground.neighbors[2] == false) {
                    // Load corner grass
                    rotation = 90;
                }
                else {
                    // Load Edge Grass
                    rotation = 90;
                }    
            }
            if (ground.neighbors[2] == false)
            {
                if (ground.neighbors[1] == false)
                {
                    rotation = 90;
                    // Load corner grass
                }
                else if (ground.neighbors[3] == false)
                {
                    // Load corner grass
                    rotation = 180;
                }
                else {
                    // Load Edge Grass
                    rotation = 180;
                }
            }
            if (ground.neighbors[3] == false)
            {
                if (ground.neighbors[0] == false) {
                    // Load corner grass
                    rotation = 270;
                }
                else if (ground.neighbors[2] == false)
                {
                    // Load corner grass
                    rotation = 180;
                }
                else
                {
                    // Load Edge Grass
                    rotation = 270;
                }
            }
        }

        return newMeshes.ToArray();
    }


    //Textures


}
