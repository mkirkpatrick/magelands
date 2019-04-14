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
    private List<Vector2> newUV = new List<Vector2>();

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

        meshRend.sharedMaterial = Resources.Load("Environment/Materials/" + chunkData.landParent.Biome + "_Biome") as Material;

        float xHalf = chunkData.landParent.XSize / 2;
        float yHalf = chunkData.landParent.ZSize / 2;
        transform.localPosition = new Vector3((chunkData.xPosition * 32) - xHalf, 0, (chunkData.zPosition * 32) - yHalf);
    }

    private void Init() {

        GameObject groundEdgeDetail = new GameObject("Ground Edge Detail", typeof(MeshRenderer), typeof(MeshFilter));

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
                            CubeTop(x, y, z, chunkData.groundPieces[x, y, z]);
                        }

                        if (LandUtil.GetGroundPiece(chunkData.landParent, new int[3] { xOffset + x, y - 1, zOffset + z }).Type == GroundPiece.GroundType.Empty)
                        {
                            //Block below is air
                            CubeBottom(x, y, z, chunkData.groundPieces[x, y, z]);
                        }

                        if (LandUtil.GetGroundPiece(chunkData.landParent, new int[3] { xOffset + x + 1, y, zOffset + z }).Type == GroundPiece.GroundType.Empty)
                        {
                            //Block east is air
                            CubeEast(x, y, z, chunkData.groundPieces[x, y, z]);
                        }

                        if (LandUtil.GetGroundPiece(chunkData.landParent, new int[3] { xOffset + x - 1, y, zOffset + z }).Type == GroundPiece.GroundType.Empty)
                        {
                            //Block west is air
                            CubeWest(x, y, z, chunkData.groundPieces[x, y, z]);

                        }

                        if (LandUtil.GetGroundPiece(chunkData.landParent, new int[3] { xOffset + x, y, zOffset + z + 1 }).Type == GroundPiece.GroundType.Empty)
                        {
                            //Block north is air
                            CubeNorth(x, y, z, chunkData.groundPieces[x, y, z]);
                        }

                        if (LandUtil.GetGroundPiece(chunkData.landParent, new int[3] { xOffset + x, y, zOffset + z - 1 }).Type == GroundPiece.GroundType.Empty)
                        {
                            //Block south is air
                            CubeSouth(x, y, z, chunkData.groundPieces[x, y, z]);
                        }
                    }
                }
            }
        }
    }
    public void UpdateBlockMesh()
    {
        Mesh mesh = meshFilter.sharedMesh;

        mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.RecalculateNormals();

        col.sharedMesh = mesh;

        newVertices.Clear();
        newUV.Clear();
        newTriangles.Clear();

        faceCount = 0;
    }
    void CubeNorth(int x, int y, int z, GroundPiece _ground)
    {
        newVertices.Add(new Vector3(x + 1, y - 1, z + 1));
        newVertices.Add(new Vector3(x + 1, y, z + 1));
        newVertices.Add(new Vector3(x, y, z + 1));
        newVertices.Add(new Vector3(x, y - 1, z + 1));

        AssignTexture(_ground, 0);
    }
    void CubeEast(int x, int y, int z, GroundPiece _ground)
    {
        newVertices.Add(new Vector3(x + 1, y - 1, z));
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x + 1, y, z + 1));
        newVertices.Add(new Vector3(x + 1, y - 1, z + 1));

        AssignTexture(_ground, 1);
    }
    void CubeSouth(int x, int y, int z, GroundPiece _ground)
    {
        newVertices.Add(new Vector3(x, y - 1, z));
        newVertices.Add(new Vector3(x, y, z));
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x + 1, y - 1, z));

        AssignTexture( _ground, 2);
    }
    void CubeWest(int x, int y, int z, GroundPiece _ground)
    {
        newVertices.Add(new Vector3(x, y - 1, z + 1));
        newVertices.Add(new Vector3(x, y, z + 1));
        newVertices.Add(new Vector3(x, y, z));
        newVertices.Add(new Vector3(x, y - 1, z));

        AssignTexture( _ground, 3);
    }
    void CubeTop(int x, int y, int z, GroundPiece _ground)
    {
        newVertices.Add(new Vector3(x, y, z));
        newVertices.Add(new Vector3(x, y, z + 1));
        newVertices.Add(new Vector3(x + 1, y, z + 1));
        newVertices.Add(new Vector3(x + 1, y, z));

        AssignTexture(_ground, 4);
    }
    void CubeBottom(int x, int y, int z, GroundPiece _ground)
    {
        newVertices.Add(new Vector3(x, y - 1, z));
        newVertices.Add(new Vector3(x + 1, y - 1, z));
        newVertices.Add(new Vector3(x + 1, y - 1, z + 1));
        newVertices.Add(new Vector3(x, y - 1, z + 1));

        AssignTexture(_ground, 5); 
    }

    void AssignTexture(GroundPiece _ground, int _faceNum)
    {

        newTriangles.Add(faceCount * 4); //1
        newTriangles.Add(faceCount * 4 + 1); //2
        newTriangles.Add(faceCount * 4 + 2); //3
        newTriangles.Add(faceCount * 4); //1
        newTriangles.Add(faceCount * 4 + 2); //3
        newTriangles.Add(faceCount * 4 + 3); //4

        Vector2[] uvs = TextureUtil.GetGroundPieceTexture(_ground, _faceNum);
        newUV.AddRange(uvs);

        faceCount++;
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