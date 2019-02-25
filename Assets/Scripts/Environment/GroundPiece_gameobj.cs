using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPiece_gameobj : MonoBehaviour
{
    public GroundPiece groundPieceData;


    void Start()
    {
        Debug.Log(groundPieceData.groundType);
        GameObject newTop = Instantiate(GameController.instance.database.groundDatabase.GetGroundTopper(groundPieceData.groundType), this.transform);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
