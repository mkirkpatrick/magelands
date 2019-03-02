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

    public GameObject LoadLand(Land _landData)
    {
        GameObject newLand = Instantiate(landPrefab, _landData.worldPosition, Quaternion.identity, this.transform);
        newLand.GetComponent<Land_gameobj>().landData = _landData;

        LoadGroundPieces(newLand);

        return newLand;
    }
    public void LoadGroundPieces(GameObject _newLand) {
        float xHalf = _newLand.GetComponent<Land_gameobj>().landData.XSize / 2;
        float yHalf = _newLand.GetComponent<Land_gameobj>().landData.YSize / 2;

        foreach (GroundPiece _groundData in _newLand.GetComponent<Land_gameobj>().landData.groundPieces) {
            GameObject newGround = Instantiate(groundPrefab, _newLand.transform);
            newGround.GetComponent<GroundPiece_gameobj>().groundPieceData = _groundData;
            newGround.transform.localPosition = new Vector3(_groundData.xPostion - xHalf, 0, _groundData.yPosition - yHalf);
        }
    }
}