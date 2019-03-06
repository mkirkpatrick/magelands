using System.Collections;
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

        meshFilter.mesh = new Mesh();

        meshRend.sharedMaterial = GameController.instance.database.groundDatabase.GetTopper_GO("Grass").GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void UpdateChunkVisuals()
    {

    }

    public void CombineMesh()
    {

        List<CombineInstance> topperList = new List<CombineInstance>();

        foreach (GroundPiece ground in chunkData.groundPieces)
        {
            GameObject obj = Instantiate(GameController.instance.database.groundDatabase.GetTopper_GO(ground.topperObjectID), this.transform);
            obj.transform.localPosition += new Vector3(ground.xPosition, 0, ground.yPosition);
            

            CombineInstance combine = new CombineInstance();
            combine.mesh = obj.GetComponent<MeshFilter>().sharedMesh;
            combine.transform = obj.transform.localToWorldMatrix;
 
            //Add it to the list of leaf mesh data
            topperList.Add(combine);
            obj.SetActive(false);

        }

        meshFilter.sharedMesh.CombineMeshes(topperList.ToArray(), true);

    }
}
