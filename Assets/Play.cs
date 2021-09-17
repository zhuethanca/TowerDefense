using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Play : MonoBehaviour, IPointerClickHandler {
    private EnemySpawner spawner;
    private SpriteRenderer renderer;
    private SpriteRenderer speedUp;
    private SpriteRenderer speedDown;
    public bool speedUpBool = false;

    private void Start() {
        spawner = GameObject.Find("Enemies").GetComponent<EnemySpawner>();
        renderer = GetComponent<SpriteRenderer>();
        speedUp = GameObject.Find("SpeedUp").GetComponent<SpriteRenderer>();
        speedDown = GameObject.Find("SpeedDown").GetComponent<SpriteRenderer>();
    }

    private void Update() {
        renderer.enabled = !spawner.getRunning();
        speedUp.enabled = spawner.getRunning() && speedUpBool;
        speedDown.enabled = spawner.getRunning() && !speedUpBool;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (GameObject.Find("Lives").GetComponent<Lives>().lives > 0) {
            if (!spawner.getRunning()) {
                spawner.nextLevel();
            } else {
                speedUpBool = !speedUpBool;
                Time.timeScale = speedUpBool ? 2 : 1;
            }
        }
    }
}