using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController instance = null;

    public DatabaseController database;

    public LandController landController;

    // Use this for initialization
    void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);

        landController.lands = new List<Land>();
        landController.lands.Add( LandCreator.CreateLand( Land.BiomeType.Forest, new Vector3(0, 0, 0), 3, 3) );
        landController.lands.Add(LandCreator.CreateLand(Land.BiomeType.Forest, new Vector3(-96, 0, 0), 3, 3));
        landController.lands.Add(LandCreator.CreateLand(Land.BiomeType.Forest, new Vector3(96, 0, 0), 3, 3));
        landController.lands.Add(LandCreator.CreateLand(Land.BiomeType.Forest, new Vector3(0, 0, 96), 6, 3));
    }
    void Start() {

    }
}
