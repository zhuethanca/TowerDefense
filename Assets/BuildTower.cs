using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildTower : MonoBehaviour, IPointerDownHandler {
    private GameObject nullGO;

    private GameObject texture;
    private GameObject tower;
    public GameObject range;

    private GameObject hoveredTexture;
    private GameObject hoveredRange;
    private Camera camera;
    private BoxCollider2D shopCollider;
    private GameObject towers;
    private Gold gold;
    public GameObject goldText;

    private bool invalid = false;
    private Lives lives;
    
    private void Start() {
        nullGO = new GameObject();
        texture = nullGO;
        tower = nullGO;
        hoveredTexture = nullGO;
        hoveredRange = nullGO;
        camera = FindObjectOfType<Camera>();
        shopCollider = GameObject.FindGameObjectWithTag("Shop").GetComponent<BoxCollider2D>();
        towers = GameObject.FindGameObjectWithTag("Towers");
        gold = GameObject.Find("Gold").GetComponent<Gold>();
        lives = GameObject.Find("Lives").GetComponent<Lives>();
    }

    // Update is called once per frame
    private void Update() {
        Vector3 pos = camera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        this.transform.position = pos;
        bool ni = shopCollider.bounds.Contains(pos);
        if (this.hoveredRange != nullGO) {
            if (ni != invalid) {
                if (!ni) {
                    this.hoveredRange.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0f, 0.5f);
                } else {
                    this.hoveredRange.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 0.5f);
                }
            }
        }
        invalid = ni;
        if (lives.lives <= 0) {
            clearHover();
        }
    }

    private void clearHover() {
        if (this.hoveredTexture != nullGO) {
            Destroy(this.hoveredTexture);
            this.hoveredTexture = nullGO;
        }

        if (this.hoveredRange != nullGO) {
            Destroy(this.hoveredRange);
            this.hoveredRange = nullGO;
        }
    }

    public void setHover(GameObject texture, GameObject tower) {
        this.clearHover();
        this.hoveredTexture = Instantiate(texture, this.transform);
        this.hoveredRange = Instantiate(range, this.transform);
        this.hoveredRange.transform.localScale = Vector3.one * tower.GetComponent<TowerScript>().getAttackRange()*2;
        this.texture = texture;
        this.tower = tower;
    }
    
    private void spawnGold(int gold) {
        var text = Instantiate(goldText);
        text.transform.position = this.transform.position + new Vector3(0.25f, 0.25f, 0f);
        var rt = text.GetComponent<GoldText>();
        rt.setGold(gold);
        rt.setNeg(true);
    }
    
    public void OnPointerDown(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            if (shopCollider.bounds.Contains(this.transform.position)) return;
            if (this.hoveredTexture == nullGO) return;
            if (gold.gold >= 100) {
                GameObject nt = Instantiate(tower, towers.transform);
                nt.transform.position = this.transform.position;
                gold.gold -= 100;
                spawnGold(100);
            }
            clearHover();
        } else if (eventData.button == PointerEventData.InputButton.Right) {
             clearHover();
        }
    }
}