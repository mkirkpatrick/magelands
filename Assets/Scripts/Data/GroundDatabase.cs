using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDatabase : MonoBehaviour
{
    public GameObject[] groundPieces;
    public GameObject[] groundToppers;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetGroundTopper(GroundPiece.GroundType _groundType) {

        GameObject newTopper = null;

        switch (_groundType)
        {
            case GroundPiece.GroundType.Empty:
                break;
            case GroundPiece.GroundType.Forest_Grass:
                newTopper = groundToppers[0];
                break;
            case GroundPiece.GroundType.Dirt:
                newTopper = groundToppers[0];
                break;
            default:
                newTopper = groundToppers[0];
                break;
        }

        return newTopper;
    }
}
