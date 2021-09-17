using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydroScript : TowerScript {
    private float aps = 0.4f;
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
        Instantiate(this.bullet, this.transform);
        return true;
    }

}