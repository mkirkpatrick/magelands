using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandController : MonoBehaviour
{

    public Land[] lands;

    //Prefabs
    public GameObject landPrefab;
    public GameObject groundPrefab;

    // Use this for initialization
    void Start() {
        foreach (Land land in lands) {
            LoadLand(land);
        }
    }

    public GameObject LoadLand(Land landData)
    {
        GameObject newLand = Instantiate(landPrefab, landData.worldPosition, Quaternion.identity, this.transform);
        newLand.GetComponent<Land_gameobj>().landData = landData;

        LoadGroundPieces(newLand);

        return newLand;
    }
    public void LoadGroundPieces(GameObject newLand) {

        foreach (GroundPiece ground in newLand.GetComponent<Land_gameobj>().landData.groundPieces) {
            GameObject newGround = Instantiate(groundPrefab, new Vector3(ground.xPostion, 0, ground.yPosition), Quaternion.identity, newLand.transform);
        }
    }
}