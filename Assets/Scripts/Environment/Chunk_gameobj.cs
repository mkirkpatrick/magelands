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
        meshRend.sharedMaterial = GameController.instance.database.groundDatabase.GetGroundPiece_GO("Forest_Grass").GetComponent<MeshRenderer>().sharedMaterial;
    }

    void Start()
    {
        float xHalf = chunkData.landParent.XSize * 8;
        float yHalf = chunkData.landParent.YSize * 8;
        transform.localPosition = new Vector3((chunkData.xPosition * 16) - xHalf, 0, (chunkData.yPosition * 16) - yHalf);
    }

    public void UpdateChunkVisuals()
    {
    
    }

    public void CombineMesh()
    {

        List<CombineInstance> groundList = new List<CombineInstance>();

        for (int x = 0; x < 16; x++) {
            for (int y = 0; y < 16; y++)
            {
                GroundPiece ground = chunkData.groundPieces[x, y];

                if (GameController.instance.database.groundDatabase.GetGroundPiece_GO(ground.Type.ToString()) != null)
                {
                    GameObject obj = Instantiate(GameController.instance.database.groundDatabase.GetGroundPiece_GO(ground.Type.ToString()), this.transform);
                    obj.transform.localPosition += new Vector3(x, 0, y);

                    CombineInstance combine = new CombineInstance();
                    combine.mesh = obj.GetComponent<MeshFilter>().sharedMesh;
                    combine.transform = obj.transform.localToWorldMatrix;

                    //Add it to the list of leaf mesh data
                    groundList.Add(combine);
                    obj.SetActive(false);
                }
            }
        }
        meshFilter.sharedMesh.CombineMeshes(groundList.ToArray(), true);

    }
}
