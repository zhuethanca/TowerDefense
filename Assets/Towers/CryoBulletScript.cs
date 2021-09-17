using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CryoBulletScript : MonoBehaviour {
    private float speed = 10f;
    private float range = 5f;
    private Vector3 direction = new Vector3(1, 0, 0);
    private BoxCollider2D collider;
    private GameObject enemies;
    private float damage = 50;
    private int mastery = 0;

    private void Start() {
        collider = GetComponent<BoxCollider2D>();
        enemies = GameObject.Find("Enemies");
    }

    // Update is called once per frame
    private void Update() {
        this.transform.localPosition += direction * Time.deltaTime;
        if (this.transform.localPosition.sqrMagnitude > range * range) {
            Destroy(this.gameObject);
        }
        for (int i = 0; i < enemies.transform.childCount; i++) {
            GameObject enemy = enemies.transform.GetChild(i).gameObject;
            if ((enemy.transform.position - this.transform.position).sqrMagnitude <= 1) {
                enemy.GetComponent<Enemy>().damage(damage, Element.Cryo, mastery);
                Destroy(this.gameObject);
                break;
            }
        }
    }

    public void setDamage(float damage) {
        this.damage = damage;
    }
    
    public void setMastery(int mastery) {
        this.mastery = mastery;
    }

    public void setDirection(Vector3 direction) {
        this.direction = direction.normalized*this.speed;
        float angle = Vector3.Angle(Vector3.right, this.direction);
        if (this.direction.y < 0) {
            angle *= -1;
        }
        this.transform.Rotate(0, 0, angle);
    }
}