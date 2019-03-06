using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPiece_gameobj : MonoBehaviour
{
    public GroundPiece groundPieceData;

    void Start()
    {
        //UpdateGroundPieceObject();
    }

    public void UpdateGroundPieceObject() {
       // GameObject newGround = Instantiate(GameController.instance.database.groundDatabase.GetGroundPiece_GO(groundPieceData.groundObjectID), this.transform);
       // GameObject newTopper = Instantiate( GameController.instance.database.groundDatabase.GetTopper_GO( groundPieceData.topperObjectID ), this.transform);
    }
}
