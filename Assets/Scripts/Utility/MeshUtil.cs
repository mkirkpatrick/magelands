using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshUtil
{

    public static Vector3[] AddMeshFace(GroundPiece _ground, int _faceNum) {
        Vector3[] newVertices = new Vector3[4];

        Vector3 localPosition = new Vector3(_ground.position.x % 32, _ground.position.y % 16, _ground.position.z % 32);

        if (_ground.Type == GroundPiece.GroundType.Path)

        {
            GroundPiece[] neighbors = LandUtil.GetNeighborGroundPieces(_ground, false);
            string slantCode = "";
            int x = (int)_ground.position.x;
            int z = (int)_ground.position.z;

            if ( (_ground.Land.heightMap[x, z + 1] < _ground.Land.heightMap[x, z]))
                slantCode += "1";
            else
                slantCode += "0";

            if ( (_ground.Land.heightMap[x + 1, z] < _ground.Land.heightMap[x, z]))
                slantCode += "1";
            else
                slantCode += "0";

            if ( (_ground.Land.heightMap[x, z - 1] < _ground.Land.heightMap[x, z]))
                slantCode += "1";
            else
                slantCode += "0";

            if ( (_ground.Land.heightMap[x - 1, z] < _ground.Land.heightMap[x, z]))
                        slantCode += "1";
            else
                slantCode += "0";


            if (slantCode == "1000")
                newVertices = FaceDiagonal(localPosition, 0);
            else if (slantCode == "0100")
                newVertices = FaceDiagonal(localPosition, 1);
            else if (slantCode == "0010")
                newVertices = FaceDiagonal(localPosition, 2);
            else if (slantCode == "0001")
                newVertices = FaceDiagonal(localPosition, 3);
            else if (slantCode == "1100")
                newVertices = FaceDiagonal(localPosition, 4);
            else if (slantCode == "0110")
                newVertices = FaceDiagonal(localPosition, 5);
            else if (slantCode == "0011")
                newVertices = FaceDiagonal(localPosition, 6);
            else if (slantCode == "1001")
                newVertices = FaceDiagonal(localPosition, 7);
            else
                newVertices = FaceTop(localPosition);
            
            if (slantCode == "0000") {
                if ( neighbors[0].HasAttributes("Slant") == true && neighbors[1].HasAttributes("Slant") == true )
                    newVertices = FaceDiagonal(localPosition, 8);
                if (neighbors[1].HasAttributes("Slant") == true && neighbors[2].HasAttributes("Slant") == true )
                    newVertices = FaceDiagonal(localPosition, 9);
                if (neighbors[2].HasAttributes("Slant") == true && neighbors[3].HasAttributes("Slant") == true )
                    newVertices = FaceDiagonal(localPosition, 10);
                if (neighbors[3].HasAttributes("Slant") == true && neighbors[0].HasAttributes("Slant") == true )
                    newVertices = FaceDiagonal(localPosition, 11);
            }
            

        }
        else {
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
            float offsetUnit = 0;

            if (_ground.Type == GroundPiece.GroundType.Dirt)
                offsetUnit = .0625f;

            newVectors[0] = new Vector3(pos.x, pos.y + offsetUnit, pos.z);
            newVectors[1] = new Vector3(pos.x, pos.y + offsetUnit, pos.z + 1);
            newVectors[2] = new Vector3(pos.x + 1, pos.y + offsetUnit, pos.z + 1);
            newVectors[3] = new Vector3(pos.x + 1, pos.y + offsetUnit, pos.z);

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
        Vector3[] FaceDiagonal(Vector3 _position, int _direction) {
            
            Vector3[] newVectors = new Vector3[4];
            Vector3 pos = _position;

            int[] yOffset = new int[4];

            if (_direction == 0 || _direction == 4) {
                yOffset[1] = -1;
                yOffset[2] = -1;
            }
            else if (_direction == 1 || _direction == 5) {
                yOffset[2] = -1;
                yOffset[3] = -1;
            }
            else if (_direction == 2 || _direction == 6) {
                yOffset[3] = -1;
                yOffset[0] = -1;
            }
            else if (_direction == 3 || _direction == 7) {
                yOffset[0] = -1;
                yOffset[1] = -1;
            }
            
            if (_direction == 4)
                yOffset[3] = -1;
            else if (_direction == 5)
                yOffset[0] = -1;
            else if (_direction == 6)
                yOffset[1] = -1;
            else if (_direction == 7)
                yOffset[2] = -1;

            if (_direction == 8)
                yOffset[2] = -1;
            else if (_direction == 9)
                yOffset[3] = -1;
            else if (_direction == 10)
                yOffset[0] = -1;
            else if (_direction == 11)
                yOffset[1] = -1;
                
            newVectors[0] = new Vector3(pos.x, pos.y + yOffset[0], pos.z);
            newVectors[1] = new Vector3(pos.x, pos.y + yOffset[1], pos.z + 1);
            newVectors[2] = new Vector3(pos.x + 1, pos.y + yOffset[2], pos.z + 1);
            newVectors[3] = new Vector3(pos.x + 1, pos.y + yOffset[3], pos.z);

            return newVectors;
        }
    }

    public static GameObject[] GenerateGrassEdges(GroundPiece[] _groundPieces, GameObject _chunkParent) {

        List<GameObject> newMeshes = new List<GameObject>();
        List<CombineInstance> cornerCombines = new List<CombineInstance>();
        List<CombineInstance> edgeCombines = new List<CombineInstance>();

        Mesh grassEdge = GameController.instance.database.groundDatabase.GetGroundPiece_GO("Grass_Edge").GetComponent<MeshFilter>().sharedMesh;
        Mesh grassCorner = GameController.instance.database.groundDatabase.GetGroundPiece_GO("Grass_Corner").GetComponent<MeshFilter>().sharedMesh;

        float offsetUnit = .0625f;

        foreach (GroundPiece ground in _groundPieces) {

            Vector3 localPosition = new Vector3((ground.position.x % 32) + .5f, (ground.position.y % 16) + offsetUnit, (ground.position.z % 32) + .5f);
            
            if (ground.neighbors[0] == false) {
                if (ground.neighbors[1] == false)
                    cornerCombines.Add( AddCombineInstance( grassCorner, localPosition, 0 ) );
                else if (ground.neighbors[3] == false) {
                    cornerCombines.Add(AddCombineInstance( grassCorner, localPosition, 270));
                }
                else
                    edgeCombines.Add(AddCombineInstance( grassEdge, localPosition, 0));
            }
            else if (ground.neighbors[1] == false)
            {
                if (ground.neighbors[0] == false)
                    cornerCombines.Add(AddCombineInstance( grassCorner, localPosition, 0));
                else if (ground.neighbors[2] == false) {
                    cornerCombines.Add(AddCombineInstance( grassCorner, localPosition, 90));
                }
                else {
                    edgeCombines.Add(AddCombineInstance( grassEdge, localPosition, 90));
                }    
            }
            if (ground.neighbors[2] == false)
            {
                if (ground.neighbors[1] == false)
                {
                    cornerCombines.Add(AddCombineInstance( grassCorner, localPosition, 90));
                }
                else if (ground.neighbors[3] == false)
                {
                    cornerCombines.Add(AddCombineInstance( grassCorner, localPosition, 180));
                }
                else {
                    edgeCombines.Add(AddCombineInstance( grassEdge, localPosition, 180));
                }
            }
            if (ground.neighbors[3] == false)
            {
                if (ground.neighbors[0] == false) {
                    cornerCombines.Add(AddCombineInstance( grassCorner, localPosition, 270));
                }
                else if (ground.neighbors[2] == false)
                {
                    cornerCombines.Add(AddCombineInstance( grassCorner, localPosition, 180));
                }
                else
                {
                    edgeCombines.Add(AddCombineInstance( grassEdge, localPosition, 270));
                }
            }
        }

        newMeshes.Add(CreateCombinedMesh(cornerCombines.ToArray(), _chunkParent.transform, "Grass_Corner"));
        newMeshes.Add(CreateCombinedMesh(edgeCombines.ToArray(), _chunkParent.transform, "Grass_Edge"));
        return newMeshes.ToArray();
    }
    public static GameObject[] GeneratePathEdges(GroundPiece[] _groundPieces, Land _land, GameObject _chunkParent)
    {

        List<GameObject> newMeshes = new List<GameObject>();
        List<CombineInstance> cornerCombines = new List<CombineInstance>();
        List<CombineInstance> innerCornerCombines = new List<CombineInstance>();
        List<CombineInstance> edgeCombines = new List<CombineInstance>();

        int[,] pathMap = _land.pathSystem.pathMap;
        int[,] heightMap = _land.heightMap;

        Mesh pathEdge = GameController.instance.database.groundDatabase.GetGroundPiece_GO("Path_Edge").GetComponent<MeshFilter>().sharedMesh;
        Mesh pathCorner = GameController.instance.database.groundDatabase.GetGroundPiece_GO("Path_Corner").GetComponent<MeshFilter>().sharedMesh;
        Mesh pathInnerCorner = GameController.instance.database.groundDatabase.GetGroundPiece_GO("Grass_Corner").GetComponent<MeshFilter>().sharedMesh;

        float offsetUnit = .0625f;

        foreach (GroundPiece ground in _groundPieces)
        {

            Vector3 localPosition = new Vector3((ground.position.x % 32) + .5f, (ground.position.y % 16) + offsetUnit, (ground.position.z % 32) + .5f);
            int x = (int)ground.position.x;
            int y = (int)ground.position.z;

            int[] edgesNeeded = new int[4];

            if (ground.neighbors[0] == true) 
                if (pathMap[x, y + 1] == 0 && heightMap[x, y + 1] == heightMap[x, y]) 
                    edgesNeeded[0] = 1;
            if (ground.neighbors[1] == true)
                if (pathMap[x + 1, y] == 0 && heightMap[x + 1, y] == heightMap[x, y])
                    edgesNeeded[1] = 1;
            if (ground.neighbors[2] == true)
                if (pathMap[x, y - 1] == 0 && heightMap[x, y - 1] == heightMap[x, y])
                    edgesNeeded[2] = 1;
            if (ground.neighbors[3] == true)
                if (pathMap[x - 1, y] == 0 && heightMap[x - 1, y] == heightMap[x, y])
                    edgesNeeded[3] = 1;



            string edgeCode = "";
            foreach (int i in edgesNeeded)
                edgeCode += i.ToString();



            if (edgeCode == "1000")
                edgeCombines.Add(AddCombineInstance(pathEdge, localPosition, 0));
            else if (edgeCode == "0100")
                edgeCombines.Add(AddCombineInstance(pathEdge, localPosition, 90));
            else if (edgeCode == "0010")
                edgeCombines.Add(AddCombineInstance(pathEdge, localPosition, 180));
            else if (edgeCode == "0001")
                edgeCombines.Add(AddCombineInstance(pathEdge, localPosition, 270));
            else if (edgeCode == "1100")
                cornerCombines.Add(AddCombineInstance(pathCorner, localPosition, 0));
            else if (edgeCode == "0110")
                cornerCombines.Add(AddCombineInstance(pathCorner, localPosition, 90));
            else if (edgeCode == "0011")
                cornerCombines.Add(AddCombineInstance(pathCorner, localPosition, 180));
            else if (edgeCode == "1001")
                cornerCombines.Add(AddCombineInstance(pathCorner, localPosition, 270));
            else if (edgeCode == "1110" || edgeCode == "1101" || edgeCode == "1011" || edgeCode == "0111") {
                Debug.Log(ground.position);
                ground.Type = GroundPiece.GroundType.Dirt;
                //_chunkParent.GetComponent<Chunk_gameobj>().UpdateBlockMesh();
            }
                

        }

        newMeshes.Add(CreateCombinedMesh(cornerCombines.ToArray(), _chunkParent.transform, "Path_Corner"));
       // newMeshes.Add(CreateCombinedMesh(innerCornerCombines.ToArray(), _chunkParent.transform, "Path_Inner_Corner"));
        newMeshes.Add(CreateCombinedMesh(edgeCombines.ToArray(), _chunkParent.transform, "Path_Edge"));
        return newMeshes.ToArray();
    }

    public static CombineInstance AddCombineInstance(Mesh _newMesh, Vector3 _position, float _rotation) {

        CombineInstance newCombine = new CombineInstance();
        newCombine.mesh = _newMesh;
        newCombine.transform = Matrix4x4.TRS(_position, Quaternion.Euler(0, _rotation, 0), Vector3.one);

        return newCombine;
    }
    public static GameObject CreateCombinedMesh(CombineInstance[] _meshDataList, Transform _parentTransform, string _objectName = "Combined Mesh")
    {
        GameObject combinedMesh = new GameObject(_objectName, typeof(MeshFilter), typeof(MeshRenderer));

        combinedMesh.transform.parent = _parentTransform;

        Mesh newMesh = new Mesh();
        newMesh.CombineMeshes(_meshDataList);
        combinedMesh.GetComponent<MeshFilter>().mesh = newMesh;

        return combinedMesh;
    }

}
