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

    public static GameObject[] GenerateGrassEdges(GroundPiece[] _groundPieces, GameObject _chunkParent) {

        List<GameObject> newMeshes = new List<GameObject>();
        List<CombineInstance> cornerCombines = new List<CombineInstance>();
        List<CombineInstance> edgeCombines = new List<CombineInstance>();

        foreach (GroundPiece ground in _groundPieces) {

            Vector3 localPosition = new Vector3((ground.position.x % 32) + .5f, ground.position.y % 16, (ground.position.z % 32) + .5f);
            Mesh grassEdge = GameController.instance.database.groundDatabase.GetGroundPiece_GO("Grass_Edge").GetComponent<MeshFilter>().sharedMesh;
            Mesh grassCorner = GameController.instance.database.groundDatabase.GetGroundPiece_GO("Grass_Corner").GetComponent<MeshFilter>().sharedMesh;

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

    //Textures
    /*
    Dictionary<string, List<GroundPiece>> groundPieceCollection = new Dictionary<string, List<GroundPiece>>();

        foreach (GroundPiece groundPiece in chunkData.groundPieces)
        {
            if (groundPiece.Type == GroundPiece.GroundType.Empty)
                continue;

            if (groundPieceCollection.ContainsKey(groundPiece.orientationID) == true)
            {
                groundPieceCollection[groundPiece.orientationID].Add(groundPiece);
}
            else
            {
                groundPieceCollection.Add(groundPiece.orientationID, new List<GroundPiece>());
                groundPieceCollection[groundPiece.orientationID].Add(groundPiece);
            }
        }

        Mesh[] finalMesh = new Mesh[groundPieceCollection.Count];
Material[] materials = new Material[groundPieceCollection.Count];
int counter = 0;

        foreach (KeyValuePair<string, List<GroundPiece>> combineList in groundPieceCollection)
        {
            CombineInstance[] combineArray = new CombineInstance[combineList.Value.Count];

            for (int i = 0; i<combineList.Value.Count; i++)
            {
                GameObject obj = Instantiate(GameController.instance.database.groundDatabase.GetGroundPiece_GO(combineList.Value[i]), this.transform);
obj.transform.localPosition += (combineList.Value[i].position);
                obj.transform.localRotation = Quaternion.Euler(new Vector3(0, combineList.Value[i].rotation, 0));

                CombineInstance combine = new CombineInstance();
combine.mesh = obj.GetComponent<MeshFilter>().sharedMesh;
                combine.transform = obj.transform.localToWorldMatrix;

                combineArray[i] = combine;
                obj.SetActive(false);
            }
            finalMesh[counter] = new Mesh();

finalMesh[counter].CombineMeshes(combineArray);
materials[counter] = Resources.Load("Environment/Materials/" + groundPieceCollection.Keys.ElementAt<string>(counter)) as Material;
            counter++;
        }

        CombineInstance[] finalCombineArray = new CombineInstance[finalMesh.Length];

        for (int i = 0; i<finalMesh.Length; i++)
        {
            CombineInstance combine = new CombineInstance();
combine.mesh = finalMesh[i];
            combine.transform = this.transform.localToWorldMatrix;
            finalCombineArray[i] = combine;
        }

meshFilter.sharedMesh.CombineMeshes(finalCombineArray, false);

        //Add Materials
        meshRend.materials = materials;
    */

}
