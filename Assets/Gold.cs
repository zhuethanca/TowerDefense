using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gold : MonoBehaviour {
    public int gold;
    private Text text;
    void Start() {
        text = GetComponent<Text>();
        gold = 500;
    }

    // Update is called once per frame
    void Update() {
        text.text = "Gold: " + gold + "g";
    }
}