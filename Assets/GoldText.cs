using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldText : MonoBehaviour {
    private float lifeSpan = 0.5f;
    private TextMesh text;
    private int gold;
    private bool neg;
    
    // Start is called before the first frame update
    private void Start() {
        GetComponent<MeshRenderer>().sortingOrder = 30;
        text = GetComponent<TextMesh>();
        text.text = (neg ? "-" : "+") + gold + "g";
    }

    private void Update() {
        lifeSpan -= Time.deltaTime;
        if (lifeSpan < 0) {
            Destroy(this.gameObject);
        }
    }

    public void setGold(int gold) {
        this.gold = gold;
    }

    public void setNeg(bool neg) {
        this.neg = neg;
    }
}