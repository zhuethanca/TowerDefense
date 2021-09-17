using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyroBulletScript : MonoBehaviour {
    private float speed = 10f;
    private float lifespan = 0.7f;
    private float time = 0;
    private float aps = 10f;
    private float attackCD = 0;
    private Vector3 direction = new Vector3(1, 0, 0);
    private GameObject enemies;
    private float damage = 0.33f;

    private void Start() {
        enemies = GameObject.Find("Enemies");
    }


    // Update is called once per frame
    private void Update() {
        this.time += Time.deltaTime;
        float decayedSpeed = (float) (this.speed * Math.Exp(-6 * time));
        this.transform.localPosition += decayedSpeed * direction * Time.deltaTime;
        if (this.time > this.lifespan) {
            Destroy(this.gameObject);
        }

        this.attackCD -= Time.deltaTime;
        if (attackCD < 0) {
            this.Attack();
            this.attackCD = 1 / aps;
        }
    }

    private void Attack() {
        for (int i = 0; i < enemies.transform.childCount; i++) {
            GameObject enemy = enemies.transform.GetChild(i).gameObject;
            if ((enemy.transform.position - this.transform.position).sqrMagnitude <= 1) {
                enemy.GetComponent<Enemy>().damage(damage, Element.Pyro, 0);
            }
        }
    }

    public void setDirection(Vector3 direction) {
        this.direction = direction.normalized;
        this.transform.Rotate(0, 0, Vector3.Angle(Vector3.right, this.direction));
    }
}