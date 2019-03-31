using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Land_gameobj : MonoBehaviour
{
    public Land landData;
    public GameObject[,] chunks;

    private void Start()
    {
        
        LoadChunks();
    }

    public void LoadChunks()
    {
        chunks = new GameObject[landData.XSize, landData.ZSize];

        for (int x = 0; x < landData.XSize; x++)
        {
            for (int z = 0; z < landData.ZSize; z++)
            {
                GameObject newChunk = new GameObject();
                newChunk.name = "Chunk (" + x + ", " + z + ")";

                newChunk.AddComponent<Chunk_gameobj>();
                newChunk.GetComponent<Chunk_gameobj>().chunkData = new Chunk(landData, x, z);
                newChunk.transform.parent = transform;

                newChunk.tag = "Ground";
                newChunk.layer = 8;
            }
        }
    }
}