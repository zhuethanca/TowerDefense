using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {
    private LineRenderer path;
    public float speed = 2f;
    public bool stopped = false;
    public float pos = 0f;
    public int seg = 0;
    public bool log = false;
    private Vector3[] positions;
    private float[] segLengths;
    
    // Start is called before the first frame update
    private void Start() {
        LineRenderer[] objs = FindObjectsOfType<LineRenderer>();
        for (int i = 0; i < objs.Length; i++) {
            if (objs[i].CompareTag("Path")) {
                this.path = objs[i];
                break;
            }
        }
        var positionCount = path.positionCount;
        this.positions = new Vector3[positionCount];
        this.segLengths = new float[positionCount-1];
        this.path.GetPositions(this.positions);
        for (var i = 0; i < positionCount-1; i++) {
            this.segLengths[i] = (this.positions[i + 1] - this.positions[i]).magnitude;
        }
    }

    // Update is called once per frame
    private void Update() {
        if (this.seg >= this.segLengths.Length) {
            var Lvs = GameObject.Find("Lives").GetComponent<Lives>();
            Lvs.lives -= 1;
            Destroy(this.gameObject);
            return;
        }
        if (log) {
            Debug.Log(this.positions[this.seg]);
            Debug.Log(this.positions[this.seg+1]);
        }
        if (this.stopped) return;
        this.pos += (this.speed / this.segLengths[this.seg]) * Time.deltaTime;
        if (this.pos >= 1) {
            this.pos = 0;
            this.seg += 1;
        }
        if (this.seg >= this.segLengths.Length) return;
        Vector3 location = Vector3.LerpUnclamped(this.positions[this.seg], this.positions[this.seg+1], this.pos);
        location.z = 0;
        this.transform.localPosition = location;
    }

    public int getSeg() {
        return this.seg;
    }

    public float getPos() {
        return this.pos;
    }
}