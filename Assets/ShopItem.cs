using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour, IPointerDownHandler {
    public GameObject texture;
    public GameObject tower;
    private BuildTower hover;
    private Gold gold;

    private void Start() {
        this.hover = GameObject.FindGameObjectWithTag("Hover").GetComponent<BuildTower>();
        this.gold = GameObject.Find("Gold").GetComponent<Gold>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (gold.gold >= 100 && GameObject.Find("Lives").GetComponent<Lives>().lives > 0) {
            hover.setHover(texture, tower);
        }
    }
}