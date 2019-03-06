﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk_gameobj : MonoBehaviour
{

    public Chunk chunkData;
    MeshFilter meshFilter;
    MeshRenderer meshRend;

    void Awake()
    {
        meshFilter = this.gameObject.AddComponent<MeshFilter>();
        meshRend = this.gameObject.AddComponent<MeshRenderer>();
    }

    public void UpdateChunkVisuals()
    {

        //GameObject newGround = Instantiate(GameController.instance.database.groundDatabase.GetGroundPiece_GO(groundPieceData.groundObjectID), this.transform);
        // GameObject newTopper = Instantiate(GameController.instance.database.groundDatabase.GetTopper_GO(groundPieceData.topperObjectID), this.transform);
    }

    public void CombineMesh()
    {

        List<CombineInstance> groundList = new List<CombineInstance>();
        List<CombineInstance> topperList = new List<CombineInstance>();

        foreach (GroundPiece ground in chunkData.groundPieces)
        {
            GameObject obj = Instantiate(GameController.instance.database.groundDatabase.GetTopper_GO(ground.topperObjectID), this.transform);
            obj.transform.localPosition += new Vector3(ground.xPostion, 0, ground.yPosition);

            //Deactivate the tree 
            obj.SetActive(false);

            CombineInstance combine = new CombineInstance();
            combine.mesh = meshFilter.mesh;
            combine.transform = meshFilter.transform.localToWorldMatrix;

            //Add it to the list of leaf mesh data
            topperList.Add(combine);
        }

        meshFilter.sharedMesh.CombineMeshes(topperList.ToArray(), false);

    }
}
