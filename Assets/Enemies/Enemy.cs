using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour {
    private static int ids = 0;
    public readonly int id = ids++;
    private float health;
    private Element element;
    private float freezeTimer = 0;

    public float maxHealth;
    private float resistance = 1;
    private float overloadRange = 3;
    private float overloadDamage = 50;
    private float electrocuteRange = 5;
    private GameObject healthObject;
    public GameObject eBullet;
    private GameObject elementContainer;
    private GameObject otherBullets;
    private Path path;
    private Gold gold;
    private Level level;
    public int worth;
    public GameObject reactionText;
    public GameObject goldText;

    public GameObject pyro, cryo, electro, hydro;

    // Start is called before the first frame update
    void Start() {
        this.healthObject = this.transform.Find("HealthBar").transform.Find("Health").gameObject;
        this.elementContainer = this.transform.Find("Element").gameObject;
        this.path = GetComponent<Path>();
        this.health = maxHealth;
        this.gold = GameObject.Find("Gold").GetComponent<Gold>();
        this.otherBullets = GameObject.Find("OtherBullets");
        this.level = GameObject.Find("Level").GetComponent<Level>();
        this.maxHealth *= 1+0.1f*this.level.level;
    }

    // Update is called once per frame
    void Update() {
        freezeTimer -= Time.deltaTime;
        if (freezeTimer < 0) {
            this.path.stopped = false;
        }

        float pct = health / maxHealth;
        this.healthObject.transform.localScale = new Vector3(pct, this.healthObject.transform.localScale.y, 1);
        this.healthObject.transform.localPosition = new Vector3((pct - 1) / 2, 0, 0);
    }

    private float react(Element e1, Element e2) {
        if (e1 == Element.None || e2 == Element.None || e1 == e2) {
            if (e1 == Element.None) element = e2;
            return 1f;
        }

        if (e1 == Element.Pyro && e2 == Element.Hydro) {
            spawnText(Reaction.VaporizeHydro);
            element = Element.None;
            return 4f;
        }

        if (e1 == Element.Hydro && e2 == Element.Pyro) {
            spawnText(Reaction.VaporizePyro);
            element = Element.None;
            return 10f;
        }

        if (e1 == Element.Pyro && e2 == Element.Cryo) {
            spawnText(Reaction.MeltCryo);
            element = Element.None;
            return 8.0f;
        }

        if (e1 == Element.Cryo && e2 == Element.Pyro) {
            spawnText(Reaction.MeltCryo);
            element = Element.None;
            return 15.0f;
        }

        if ((e1 == Element.Cryo && e2 == Element.Hydro) || (e1 == Element.Hydro && e2 == Element.Cryo)) {
            spawnText(Reaction.Freeze);
            freeze();
            element = Element.None;
            return 1f;
        }

        if ((e1 == Element.Pyro && e2 == Element.Electro) || (e1 == Element.Electro && e2 == Element.Pyro)) {
            spawnText(Reaction.Overload);
            overload();
            element = Element.None;
            return 1f;
        }

        if ((e1 == Element.Hydro && e2 == Element.Electro) || (e1 == Element.Hydro && e2 == Element.Pyro)) {
            spawnText(Reaction.Electrocute);
            element = Element.None;
            electrocute();
            return 1f;
        }

        if ((e1 == Element.Cryo && e2 == Element.Electro) || (e1 == Element.Cryo && e2 == Element.Pyro)) {
            spawnText(Reaction.Superconduct);
            superconduct();
            element = Element.None;
            return 1f;
        }

        return 1f;
    }

    private void superconduct() {
        this.resistance *= 0.7f;
    }

    private void electrocute() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> targets = new List<GameObject>();
        foreach (GameObject obj in enemies) {
            if ((obj.transform.position - this.transform.position).sqrMagnitude < this.electrocuteRange*this.electrocuteRange) {
                if (obj.GetComponent<Enemy>().element == Element.Hydro)
                    targets.Add(obj);
            }
        }
        targets.Sort((obj1, obj2) => {
            Path p1 = obj1.GetComponent<Path>();
            Path p2 = obj2.GetComponent<Path>();
            if (p1.getSeg() != p2.getSeg()) return p1.getSeg() > p2.getSeg() ? 1 : -1;
            return p1.getPos() > p2.getPos() ? 1 : -1;
        });
        if (targets.Count == 0) return;
        GameObject blt = Instantiate(eBullet, this.otherBullets.transform);
        blt.GetComponent<ElectroBulletScript>().setTarget(this.transform.position, targets[targets.Count-1]);
    }

    private void freeze() {
        this.freezeTimer = 2f;
        this.path.stopped = true;
    }

    private void overload() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject obj in enemies) {
            if ((obj.transform.position - this.transform.position).sqrMagnitude < this.overloadRange*this.overloadRange) {
                obj.GetComponent<Enemy>().damage(overloadDamage, Element.None, 0);
            }
        }
    }

    private void spawnText(Reaction reaction) {
        var text = Instantiate(reactionText);
        text.transform.position = this.transform.position + new Vector3(0.1f, 0.1f, 0f);
        var rt = text.GetComponent<ReactionText>();
        rt.setReaction(reaction);
    }
    
    private void spawnGold(int gold) {
        var text = Instantiate(goldText);
        text.transform.position = this.transform.position + new Vector3(-0.1f, -0.1f, 0f);
        var rt = text.GetComponent<GoldText>();
        rt.setGold(gold);
    }

    public void damage(float damage, Element element, int mastery) {
        if (this == null) return;
        float multiplier = react(this.element, element);
        this.health -= damage * multiplier / resistance;
        updateElement();
        if (this.health <= 0) {
            gold.gold += worth;
            spawnGold(worth);
            Destroy(this.gameObject);
        }
    }

    private void updateElement() {
        if (this.elementContainer != null && this.elementContainer.transform.childCount > 0)
            Destroy(this.elementContainer.transform.GetChild(0).gameObject);
        switch (element) {
            case Element.None:
                break;
            case Element.Pyro:
                Instantiate(pyro, this.elementContainer.transform);
                break;
            case Element.Cryo:
                Instantiate(cryo, this.elementContainer.transform);
                break;
            case Element.Electro:
                Instantiate(electro, this.elementContainer.transform);
                break;
            case Element.Hydro:
                Instantiate(hydro, this.elementContainer.transform);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}