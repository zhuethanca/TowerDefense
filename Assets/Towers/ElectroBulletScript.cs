using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroBulletScript : MonoBehaviour {
    private float time = 0;
    private float lifespan = 0.25f;
    private GameObject target;
    private Vector3 src;
    private LineRenderer lr;
    private float damage = 20;
    private bool damaged = false;
    
    private void Start() {
        this.lr = this.GetComponent<LineRenderer>();
        this.lr.SetPosition(0, Vector3.zero);
    }

    // Update is called once per frame
    private void Update() {
        this.time += Time.deltaTime;
        if (target != null) {
            var position = target.transform.position;
            this.lr.SetPosition(0, this.transform.parent.InverseTransformPoint(src));
            this.lr.SetPosition(1, this.transform.parent.InverseTransformPoint(new Vector3(position.x, position.y, 0)));
        }
        
        if (!damaged) {
            damaged = true;
            this.target.GetComponent<Enemy>().damage(damage, Element.Electro, 0);
        }

        if (this.time > this.lifespan) {
            Destroy(this.gameObject);
        }
    }

    public void setTarget(Vector3 src, GameObject target) {
        this.src = src;
        this.target = target;
    }
}