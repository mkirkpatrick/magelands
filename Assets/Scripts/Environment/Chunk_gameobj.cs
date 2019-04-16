using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Chunk_gameobj : MonoBehaviour
{

    public Chunk chunkData;
    MeshFilter meshFilter;
    MeshRenderer meshRend;

    private List<Vector3> newVertices = new List<Vector3>();
    private List<int> newTriangles = new List<int>();
    private List<Vector2> newUVs = new List<Vector2>();

    private MeshCollider col;

    private int faceCount;

    // Child Objects
    GameObject groundEdgeDetail;

    void Awake()
    {
        meshFilter = this.gameObject.AddComponent<MeshFilter>();
        meshRend = this.gameObject.AddComponent<MeshRenderer>();
        col = this.gameObject.AddComponent<MeshCollider>();

        meshFilter.mesh = new Mesh();
    }

    void Start()
    {
        Init();
        GenerateBlockMesh();
        UpdateBlockMesh();

        GenerateGroundEdges();

        meshRend.sharedMaterial = Resources.Load("Environment/Materials/" + chunkData.landParent.Biome + "_Biome") as Material;

        float xHalf = chunkData.landParent.XSize / 2;
        float yHalf = chunkData.landParent.ZSize / 2;
        transform.localPosition = new Vector3((chunkData.xPosition * 32) - xHalf, 0, (chunkData.zPosition * 32) - yHalf);
    }

    private void Init() {

        groundEdgeDetail = new GameObject("Ground Edge Detail", typeof(MeshRenderer), typeof(MeshFilter));

        for (int x = 0; x < 32; x++) {
            for (int z = 0; z < 32; z++) {
                for (int y = 0; y < 16; y++) {
                    chunkData.groundPieces[x, y, z] = chunkData.landParent.groundPieces[(chunkData.xPosition * 32) + x, y, (chunkData.zPosition * 32) + z];
                }
            }
        }  
    }
    public void GenerateBlockMesh()
    {
        int xOffset = chunkData.xPosition * 32;
        int zOffset = chunkData.zPosition * 32;

        for (int x = 0; x < 32; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 32; z++)
                {
                    if (chunkData.groundPieces[x, y, z].Type != GroundPiece.GroundType.Empty)
                    {
                        if (LandUtil.GetGroundPiece(chunkData.landParent, new int[3] {xOffset + x, y + 1, zOffset + z }).Type == GroundPiece.GroundType.Empty)
                        {
                            //Block above is air
                            newVertices.AddRange( MeshUtil.AddMeshFace(chunkData.groundPieces[x, y, z], 4) );
                            GenerateTris();
                            newUVs.AddRange( TextureUtil.GetGroundPieceTexture(chunkData.groundPieces[x, y, z], 4) );
                        }

                        if (LandUtil.GetGroundPiece(chunkData.landParent, new int[3] { xOffset + x, y - 1, zOffset + z }).Type == GroundPiece.GroundType.Empty)
                        {
                            //Block below is air
                            newVertices.AddRange(MeshUtil.AddMeshFace(chunkData.groundPieces[x, y, z], 5));
                            GenerateTris();
                            newUVs.AddRange( TextureUtil.GetGroundPieceTexture(chunkData.groundPieces[x, y, z], 5) );
                        }

                        if (LandUtil.GetGroundPiece(chunkData.landParent, new int[3] { xOffset + x + 1, y, zOffset + z }).Type == GroundPiece.GroundType.Empty)
                        {
                            //Block east is air
                            newVertices.AddRange(MeshUtil.AddMeshFace(chunkData.groundPieces[x, y, z], 1));
                            GenerateTris();
                            newUVs.AddRange( TextureUtil.GetGroundPieceTexture(chunkData.groundPieces[x, y, z], 1) );
                        }

                        if (LandUtil.GetGroundPiece(chunkData.landParent, new int[3] { xOffset + x - 1, y, zOffset + z }).Type == GroundPiece.GroundType.Empty)
                        {
                            //Block west is air
                            newVertices.AddRange(MeshUtil.AddMeshFace(chunkData.groundPieces[x, y, z], 3));
                            GenerateTris();
                            newUVs.AddRange( TextureUtil.GetGroundPieceTexture(chunkData.groundPieces[x, y, z], 3) );

                        }

                        if (LandUtil.GetGroundPiece(chunkData.landParent, new int[3] { xOffset + x, y, zOffset + z + 1 }).Type == GroundPiece.GroundType.Empty)
                        {
                            //Block north is air
                            newVertices.AddRange(MeshUtil.AddMeshFace(chunkData.groundPieces[x, y, z], 0));
                            GenerateTris();
                            newUVs.AddRange( TextureUtil.GetGroundPieceTexture(chunkData.groundPieces[x, y, z], 0) );
                        }

                        if (LandUtil.GetGroundPiece(chunkData.landParent, new int[3] { xOffset + x, y, zOffset + z - 1 }).Type == GroundPiece.GroundType.Empty)
                        {
                            //Block south is air
                            newVertices.AddRange(MeshUtil.AddMeshFace(chunkData.groundPieces[x, y, z], 2));
                            GenerateTris();
                            newUVs.AddRange( TextureUtil.GetGroundPieceTexture(chunkData.groundPieces[x, y, z], 2) );
                        }
                    }
                }
            }
        }

        void GenerateTris() {
            newTriangles.Add(faceCount * 4); //1
            newTriangles.Add(faceCount * 4 + 1); //2
            newTriangles.Add(faceCount * 4 + 2); //3
            newTriangles.Add(faceCount * 4); //1
            newTriangles.Add(faceCount * 4 + 2); //3
            newTriangles.Add(faceCount * 4 + 3); //4

            faceCount++;
        }
    }
    public void UpdateBlockMesh()
    {
        Mesh mesh = meshFilter.sharedMesh;

        mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.uv = newUVs.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.RecalculateNormals();

        col.sharedMesh = mesh;

        newVertices.Clear();
        newUVs.Clear();
        newTriangles.Clear();

        faceCount = 0;
    }

    public void GenerateGroundEdges() {

        List<GroundPiece> groundGrassEdges = new List<GroundPiece>();

        foreach (GroundPiece ground in chunkData.groundPieces) {
            if (ground.isEdgePiece == true && ground.neighbors[4] == false)
                groundGrassEdges.Add(ground);
        }

        GameObject[] newEdgeDetails = MeshUtil.GenerateGrassEdges(groundGrassEdges.ToArray(), gameObject);

        foreach (GameObject newObj in newEdgeDetails) {
            newObj.transform.parent = transform;
            newObj.GetComponent<MeshRenderer>().sharedMaterial = Resources.Load("Environment/Materials/" + newObj.name) as Material;
        }
            
    }

    
}
/*
 * 
 *  Dictionary<string, List<GroundPiece>> groundPieceCollection = new Dictionary<string, List<GroundPiece>>();

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
        */