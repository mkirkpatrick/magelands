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
        
    }

    public GameObject LoadLand(Land landData)
    {
        GameObject newLand = Instantiate(landPrefab, landData.worldPosition, Quaternion.identity);
       

        return newLand;
    }

}
