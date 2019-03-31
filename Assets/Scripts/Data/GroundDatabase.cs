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
    }

    public GameObject GetGroundPiece_GO(GroundPiece _ground) {
        if (_ground.Type == GroundPiece.GroundType.Empty || _ground.Type == GroundPiece.GroundType.Surrounded)
            return groundPieces["Dirt"];

        GameObject newGround = groundPieces[_ground.orientationID];
        return newGround;
    }
}
