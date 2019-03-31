using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Chunk_gameobj : MonoBehaviour
{

    public Chunk chunkData;
    MeshFilter meshFilter;
    MeshRenderer meshRend;

    void Awake()
    {
        meshFilter = this.gameObject.AddComponent<MeshFilter>();
        meshRend = this.gameObject.AddComponent<MeshRenderer>();

        meshFilter.mesh = new Mesh();    }

    void Start()
    {
        Init();
        float xHalf = chunkData.landParent.XSize * 16;
        float yHalf = chunkData.landParent.ZSize * 16;
        transform.localPosition = new Vector3((chunkData.xPosition * 32) - xHalf, 0, (chunkData.zPosition * 32) - yHalf);
    }

    private void Init() {
        for (int x = 0; x < 32; x++) {
            for (int z = 0; z < 32; z++) {
                for (int y = 0; y < 16; y++) {
                    chunkData.groundPieces[x, y, z] = chunkData.landParent.groundPieces[(chunkData.xPosition * 32) + x, y, (chunkData.zPosition * 32) + z];
                }
            }
        }
        CombineMesh();
        gameObject.AddComponent<MeshCollider>();
    }

    public void CombineMesh()
    {
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

            for (int i = 0; i < combineList.Value.Count; i++)
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

        for (int i = 0; i < finalMesh.Length; i++)
        {
            CombineInstance combine = new CombineInstance();
            combine.mesh = finalMesh[i];
            combine.transform = this.transform.localToWorldMatrix;
            finalCombineArray[i] = combine;
        }

        meshFilter.sharedMesh.CombineMeshes(finalCombineArray, false);

        //Add Materials
        meshRend.materials = materials;
    }
}
