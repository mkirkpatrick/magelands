using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GroundDatabase : MonoBehaviour
{
    public Dictionary<string, GameObject> groundPieces;

    // Start is called before the first frame update
    void Awake()
    {
        groundPieces = new Dictionary<string, GameObject>();

        GameObject[] loadedGroundPieces = Resources.LoadAll( "Environment/GroundPieces", typeof(GameObject)).Cast<GameObject>().ToArray();
        for (int i = 0; i < loadedGroundPieces.Length; i++)
        {
            groundPieces.Add(loadedGroundPieces[i].name, loadedGroundPieces[i]);
        }
    }

    public GameObject GetGroundPiece_GO(string _name) {

        GameObject newGround = groundPieces[_name];
        return newGround;
    }
}
