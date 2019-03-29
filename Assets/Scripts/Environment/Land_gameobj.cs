using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Land_gameobj : MonoBehaviour
{
    public Land landData;
    public Chunk_gameobj[,] chunks;
    public GameObject underPieces;

    private void Awake()
    {
        underPieces = new GameObject();
        underPieces.name = "UnderPieces";
        underPieces.transform.parent = this.transform;
        underPieces.AddComponent<MeshFilter>();
        underPieces.AddComponent<MeshRenderer>();
        underPieces.GetComponent<MeshFilter>().mesh = new Mesh();
    }
    // Start is called before the first frame update
    void Start()
    {
        LoadUnderPieces(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadUnderPieces()
    {
        Dictionary<string, List<UnderPiece>> underPieceCollection = new Dictionary<string, List<UnderPiece>>();
        
        foreach (UnderPiece underPiece in landData.underPieces) {
            if (underPieceCollection.ContainsKey(underPiece.orientationType) == true)
            {
                underPieceCollection[underPiece.orientationType].Add(underPiece);
            }
            else {
                underPieceCollection.Add(underPiece.orientationType, new List<UnderPiece>() );
                underPieceCollection[underPiece.orientationType].Add(underPiece);
            }
        }

        Mesh[] finalMesh = new Mesh[underPieceCollection.Count];
        Material[] materials = new Material[underPieceCollection.Count];
        int counter = 0;
         
        foreach (KeyValuePair< string, List<UnderPiece> > combineList in underPieceCollection) {
            CombineInstance[] combineArray = new CombineInstance[combineList.Value.Count];

            for (int i = 0; i < combineList.Value.Count; i++) {
                GameObject obj = Instantiate(GameController.instance.database.groundDatabase.GetUnderPiece_GO(combineList.Value[i].orientationType), this.transform);
                obj.transform.localPosition += ( combineList.Value[i].position );
                obj.transform.localRotation = Quaternion.Euler(new Vector3(0, combineList.Value[i].rotation, 0));

                CombineInstance combine = new CombineInstance();
                combine.mesh = obj.GetComponent<MeshFilter>().sharedMesh;
                combine.transform = obj.transform.localToWorldMatrix;

                combineArray[i] = combine;
                obj.SetActive(false);
            }
            finalMesh[counter] = new Mesh();

            finalMesh[counter].CombineMeshes(combineArray);
            materials[counter] = Resources.Load("Environment/Materials/" + underPieceCollection.Keys.ElementAt<string>(counter) ) as Material;
            counter++;
        }

        CombineInstance[] finalCombineArray = new CombineInstance[finalMesh.Length];
        
        for(int i = 0; i < finalMesh.Length; i++) {
            CombineInstance combine = new CombineInstance();
            combine.mesh = finalMesh[i];
            combine.transform = this.transform.localToWorldMatrix;
            finalCombineArray[i] = combine;
        }

        underPieces.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(finalCombineArray, false);
        underPieces.transform.position = new Vector3( (-landData.XSize * 8) + .5f, 0, (-landData.ZSize * 8) + .5f);

        //Add Materials
        MeshRenderer rend = underPieces.GetComponent<MeshRenderer>();
        rend.materials = materials;
    }
}