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

        UpdateRocks();

        float xHalf = chunkData.landParent.XSize / 2;
        float yHalf = chunkData.landParent.ZSize / 2;
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

        meshRend.sharedMaterial = Resources.Load("Environment/Materials/" + chunkData.landParent.Biome + "_Biome") as Material;

        
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

    public void UpdateRocks() {

        Mesh rockMesh = GameController.instance.database.groundDatabase.GetRock_GO("Rock1").GetComponent<MeshFilter>().sharedMesh;
        List<CombineInstance> combineRocks = new List<CombineInstance>();

        for(int x = 0; x < 32; x++) {
            for (int z = 0; z < 32; z++) {
                for (int y = 0; y < 16; y++)
                {
                    if (chunkData.groundPieces[x, y, z].decorations.Count > 0)
                    {
                        GroundDecoration newDecor = chunkData.groundPieces[x, y, z].decorations[0];

                        combineRocks.Add( MeshUtil.AddCombineInstance(rockMesh, new Vector3(x,y,z) + newDecor.localPosition, newDecor.rotation ) );

                        /*
                        GameObject newRock = Instantiate( GameController.instance.database.groundDatabase.GetRock_GO("Rock1"), transform );
                        Vector3 offset = chunkData.groundPieces[x, y, z].decorations[0].localPosition;

                        newRock.transform.localPosition = new Vector3(x + offset.x, y + offset.y, z + offset.z);
                        newRock.transform.eulerAngles = new Vector3(0, chunkData.groundPieces[x, y, z].decorations[0].rotation, 0);
                        */
                    }
                }
            }
        }
        GameObject newRocks = MeshUtil.CreateCombinedMesh(combineRocks.ToArray(), transform, "Rock1");
        newRocks.GetComponent<MeshRenderer>().sharedMaterial = Resources.Load("Environment/Materials/" + newRocks.name) as Material;
        newRocks.AddComponent<MeshCollider>();
    }
    
}
                