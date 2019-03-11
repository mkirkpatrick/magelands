using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GroundDatabase : MonoBehaviour
{
    public Dictionary<string, GameObject> groundPieces;
    public Dictionary<string, GameObject> groundToppers;

    // Start is called before the first frame update
    void Awake()
    {
        groundPieces = new Dictionary<string, GameObject>();
        groundToppers = new Dictionary<string, GameObject>();

        GameObject[] loadedGroundPieces = Resources.LoadAll( "Environment/GroundPieces", typeof(GameObject)).Cast<GameObject>().ToArray();
        for (int i = 0; i < loadedGroundPieces.Length; i++)
        {
            groundPieces.Add(loadedGroundPieces[i].name, loadedGroundPieces[i]);
        }

        GameObject[] loadedGroundToppers = Resources.LoadAll("Environment/Toppers", typeof(GameObject)).Cast<GameObject>().ToArray();
        for (int i = 0; i < loadedGroundToppers.Length; i++)
        {
            groundToppers.Add(loadedGroundToppers[i].name, loadedGroundToppers[i]);
        }
    }

    public GameObject GetGroundPiece_GO(string _groundID) {
        GameObject newGround = groundPieces[_groundID];
        return newGround;
    }

    public GameObject GetTopper_GO(string _topperID) {
        if (_topperID == "Empty")
            return null;

        GameObject newTopper = groundToppers[_topperID];
        return newTopper;
    }
}
