using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GroundDatabase : MonoBehaviour
{
    public Dictionary<string, GameObject> groundPieces;
    public Dictionary<string, GameObject> rocks;

    // Start is called before the first frame update
    void Awake()
    {
        groundPieces = new Dictionary<string, GameObject>();

        GameObject[] loadedGroundPieces = Resources.LoadAll( "Environment/GroundPieces", typeof(GameObject)).Cast<GameObject>().ToArray();
        for (int i = 0; i < loadedGroundPieces.Length; i++)
        {
            groundPieces.Add(loadedGroundPieces[i].name, loadedGroundPieces[i]);
        }


        rocks = new Dictionary<string, GameObject>();

        GameObject[] loadedRocks = Resources.LoadAll("Environment/Rocks", typeof(GameObject)).Cast<GameObject>().ToArray();
        for (int i = 0; i < loadedRocks.Length; i++)
        {
            rocks.Add(loadedRocks[i].name, loadedRocks[i]);
        }
    }

    public GameObject GetGroundPiece_GO(string _name) {
        GameObject newGround = groundPieces[_name];
        return newGround;
    }
    public GameObject GetRock_GO(string _name) {
        GameObject newRock = rocks[_name];
        return newRock;
    }
}
