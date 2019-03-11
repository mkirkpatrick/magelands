using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandController : MonoBehaviour
{

    public Land[] lands;

    //Prefabs
    public GameObject landPrefab;
    public GameObject groundPrefab;

    // Use this for initialization
    void Start() {
        foreach (Land land in lands) {
            LandUtil.Initialize(land);
            LandUtil.RoundIslandCorners(12);
            LoadLand(land);

            foreach (GroundPiece g in land.groundPieces) {
                if (g.Type == GroundPiece.GroundType.Empty)
                    Debug.Log(g.xPosition + "-" + g.yPosition);
            }
        }
    }

    public GameObject LoadLand(Land _landData)
    {
        GameObject newLand = Instantiate(landPrefab, _landData.worldPosition, Quaternion.identity, this.transform);
        newLand.GetComponent<Land_gameobj>().landData = _landData;

        LoadChunks(newLand);

        return newLand;
    }
    public void LoadChunks(GameObject _newLand) {

        Land newLandData = _newLand.GetComponent<Land_gameobj>().landData;
        

        foreach (Chunk _chunk in newLandData.chunks) {
            GameObject newChunk = new GameObject();
            newChunk.name = "Chunk (" + _chunk.xPosition + ", " + _chunk.yPosition + ")";

            newChunk.AddComponent<Chunk_gameobj>();
            newChunk.GetComponent<Chunk_gameobj>().chunkData = _chunk;
            newChunk.transform.parent = _newLand.transform;

            newChunk.GetComponent<Chunk_gameobj>().CombineMesh();
            newChunk.AddComponent<MeshCollider>();
            newChunk.tag = "Ground";
            newChunk.layer = 8;
        }
    }

}