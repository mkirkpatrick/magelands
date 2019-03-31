using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandController : MonoBehaviour
{

    public List<Land> lands;

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
        return newLand;
    }
}