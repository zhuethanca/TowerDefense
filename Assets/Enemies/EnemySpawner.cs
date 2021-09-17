using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    private enum Type {
        Normal, Slow, Fast, Strong
    }
    public GameObject normal, slow, fast, strong;
    private List<List<(Type, int)>> levels = new List<List<(Type, int)>> {
        new List<(Type, int)>() {
            (Type.Normal, 10)
        },
        new List<(Type, int)>() {
            (Type.Normal, 20)
        },
        new List<(Type, int)>() {
            (Type.Strong, 10)
        },
        new List<(Type, int)>() {
            (Type.Normal, 10),
            (Type.Strong, 10)
        },
        new List<(Type, int)>() {
            (Type.Strong, 10),
            (Type.Normal, 10),
            (Type.Strong, 10)
        },
        new List<(Type, int)>() {
            (Type.Fast, 5)
        },
        new List<(Type, int)>() {
            (Type.Strong, 10),
            (Type.Normal, 20),
            (Type.Fast, 20),
            (Type.Strong, 10)
        },
        new List<(Type, int)>() {
            (Type.Slow, 5)
        },
        new List<(Type, int)>() {
            (Type.Strong, 10),
            (Type.Slow, 10),
            (Type.Strong, 10)
        },
        new List<(Type, int)>() {
            (Type.Strong, 10),
            (Type.Normal, 30),
            (Type.Slow, 5),
            (Type.Fast, 30),
            (Type.Slow, 5),
            (Type.Strong, 10)
        },
    };

    private List<(Type, int)> queue = new List<(Type, int)>();
    private float eps = 3f;
    private float spawnCD = 0;
    private bool running = false;
    private int level = 0;
    private Level levelText;

    private void Start() {
        levelText = GameObject.Find("Level").GetComponent<Level>();
    }

    // Update is called once per frame
    void Update() {
        if (running) {
            this.spawnCD -= Time.deltaTime;
            if (spawnCD < 0) {
                this.spawn();
                this.spawnCD += 1 / eps;
            }

            if (this.queue.Count == 0 && this.transform.childCount == 0)
                this.running = false;
        }
    }

    public void nextLevel() {
        if (!running) {
            if (level < levels.Count) {
                queue = new List<(Type, int)>(levels[level]);
            } else {
                queue = new List<(Type, int)>(levels[levels.Count-1]);
                for (int i = 0; i < queue.Count; i++) {
                    queue[i] = (queue[i].Item1, queue[i].Item2 * (level-levels.Count+2));
                }
            }
            eps *= 1.1f;
            level += 1;
            levelText.level = level;
            running = true;
        }
    }

    private void spawn() {
        if (queue.Count <= 0) return;
        (Type, int) entry = queue[0];

        switch (entry.Item1) {
            case Type.Normal:
                Instantiate(normal, this.transform);
                break;
            case Type.Slow:
                Instantiate(slow, this.transform);
                break;
            case Type.Fast:
                Instantiate(fast, this.transform);
                break;
            case Type.Strong:
                Instantiate(strong, this.transform);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        entry.Item2 -= 1;
        if (entry.Item2 == 0)
            queue.RemoveAt(0);
        else
            queue[0] = entry;
    }

    public bool getRunning() {
        return running;
    }

    public int getLevel() {
        return level;
    }
}