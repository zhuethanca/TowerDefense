using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour {
    public int level;
    private Text text;
    void Start() {
        text = GetComponent<Text>();
        level = 0;
    }

    // Update is called once per frame
    void Update() {
        text.text = "Level: " + level;
    }
}