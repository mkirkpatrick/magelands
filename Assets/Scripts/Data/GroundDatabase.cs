using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GroundDatabase : MonoBehaviour
{
    public Dictionary<string, GameObject> groundPieces;
    public Dictionary<string, GameObject> groundToppers;
    public Dictionary<string, GameObject> underPieces;

    // Start is called before the first frame update
    void Awake()
    {
        groundPieces = new Dictionary<string, GameObject>();
        underPieces = new Dictionary<string, GameObject>();

        GameObject[] loadedGroundPieces = Resources.LoadAll( "Environment/GroundPieces", typeof(GameObject)).Cast<GameObject>().ToArray();
        for (int i = 0; i < loadedGroundPieces.Length; i++)
        {
            groundPieces.Add(loadedGroundPieces[i].name, loadedGroundPieces[i]);
        }

        GameObject[] loadedUnderPieces = Resources.LoadAll("Environment/UnderPieces", typeof(GameObject)).Cast<GameObject>().ToArray();
        for (int i = 0; i < loadedUnderPieces.Length; i++)
        {
            underPieces.Add(loadedUnderPieces[i].name, loadedUnderPieces[i]);
        }
    }

    public GameObject GetGroundPiece_GO(string _groundID) {
        if (_groundID == "Empty")
            return null;

        GameObject newGround = groundPieces[_groundID];
        return newGround;
    }

    public GameObject GetUnderPiece_GO(string _underPieceID)
    {
        if (_underPieceID == "Empty")
            return null;

        GameObject newUnderPiece;
        if (underPieces.ContainsKey(_underPieceID))
            newUnderPiece = underPieces[_underPieceID];
        else
            newUnderPiece = underPieces["Dirt_Bottom"];

        return newUnderPiece;
    }
}
