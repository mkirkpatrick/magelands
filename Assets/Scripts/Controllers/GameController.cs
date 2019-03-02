﻿using System.Collections;
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

        landController.lands = new Land[2];
        landController.lands[0] = new Land(new Vector3(0, 0, 0), 10, 20);
        landController.lands[1] = new Land(new Vector3(0, 0, 22), 20, 20);

    }
    void Start() {
        
    }
}
