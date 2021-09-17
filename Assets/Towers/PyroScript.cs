using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyroScript : TowerScript {
    private float aps = 10f;
    private float attackCD = 0;

    public GameObject bullet;

    private void Start() {
        this.attackRange = 2.5f;
    }

    // Update is called once per frame
    private void Update() {
        this.attackCD -= Time.deltaTime;
        if (attackCD < 0) {
            if (this.Attack()) {
                this.attackCD = 1 / aps;
            }
        }
    }

    private bool Attack() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> targets = new List<GameObject>();
        foreach (GameObject obj in enemies) {
            if ((obj.transform.position - this.transform.position).sqrMagnitude < this.attackRange*this.attackRange) {
                targets.Add(obj);
            }
        }
        if (targets.Count == 0) return false;
        targets.Sort((obj1, obj2) => {
            Path p1 = obj1.GetComponent<Path>();
            Path p2 = obj2.GetComponent<Path>();
            if (p1.getSeg() != p2.getSeg()) return p1.getSeg() > p2.getSeg() ? 1 : -1;
            return p1.getPos() > p2.getPos() ? 1 : -1;
        });
        GameObject target = targets[targets.Count-1];
        GameObject blt = Instantiate(this.bullet, this.transform);
        blt.GetComponent<PyroBulletScript>().setDirection(target.transform.position - this.transform.position);
        return true;
    }
}