using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydroBulletScript : MonoBehaviour {
    private float speed = 4f;
    private float range = 2f;
    private GameObject enemies;
    private float damage = 15;
    private HashSet<int> hit = new HashSet<int>();

    private void Start() {
        enemies = GameObject.Find("Enemies");
    }
    
    // Update is called once per frame
    private void Update() {
        this.transform.localScale += Vector3.one * this.speed * Time.deltaTime;
        if (this.transform.localScale.x/2 > range) {
            Destroy(this.gameObject);
        }
        for (int i = 0; i < enemies.transform.childCount; i++) {
            GameObject enemy = enemies.transform.GetChild(i).gameObject;
            if ((enemy.transform.position - this.transform.position).sqrMagnitude <= this.transform.localScale.x*this.transform.localScale.x) {
                Enemy es = enemy.GetComponent<Enemy>();
                if (!hit.Contains(es.id)) {
                    es.damage(damage, Element.Hydro, 0);
                    hit.Add(es.id);
                }
            }
        }
    }
}