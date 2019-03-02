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
            LoadLand(land);
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
        float xHalf = _newLand.GetComponent<Land_gameobj>().landData.XSize * 8;
        float yHalf = _newLand.GetComponent<Land_gameobj>().landData.YSize * 8;

        foreach (Chunk _chunk in _newLand.GetComponent<Land_gameobj>().landData.chunks) {
            GameObject newChunk = new GameObject();
            newChunk.name = "Chunk (" + _chunk.xPosition + ", " + _chunk.yPosition + ")";
            newChunk.AddComponent<Chunk_gameobj>();
            newChunk.transform.parent = _newLand.transform;

            newChunk.GetComponent<Chunk_gameobj>().chunkData = _chunk;
            newChunk.transform.localPosition = new Vector3((_chunk.xPosition * 16) - xHalf, 0, (_chunk.yPosition * 16) - yHalf);

            LoadGroundPieces(newChunk);
        }
    }
    public void LoadGroundPieces(GameObject _newChunk) {
        Chunk chunkData = _newChunk.GetComponent<Chunk_gameobj>().chunkData;
        foreach (GroundPiece _ground in chunkData.groundPieces)
        {
            GameObject newGround = Instantiate(groundPrefab, _newChunk.transform);

            newGround.GetComponent<GroundPiece_gameobj>().groundPieceData = _ground;
            newGround.transform.localPosition = new Vector3(_ground.xPostion, 0, _ground.yPosition);
        }
    }
}