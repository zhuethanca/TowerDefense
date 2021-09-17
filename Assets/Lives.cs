using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lives : MonoBehaviour {
    public int lives;
    private Text text;
    public GameObject gameOver;
    private bool displayed = false;
    void Start() {
        text = GetComponent<Text>();
        lives = 20;
    }

    // Update is called once per frame
    void Update() {
        text.text = "Lives: " + lives;
        if (lives <= 0 && !displayed) {
            Instantiate(gameOver);
            displayed = true;
        }
    }
}