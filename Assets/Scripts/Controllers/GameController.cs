using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController instance = null;
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
    }
    void Start() {

    }
}
