using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Land {

    public Vector3 worldPosition;
    public int xSize;
    public int ySize;

    public List<GroundPiece> groundPieces;

}
