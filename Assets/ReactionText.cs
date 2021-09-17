using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReactionText : MonoBehaviour {
    private float lifeSpan = 0.5f;
    private TextMesh text;
    private Reaction reaction;
    
    // Start is called before the first frame update
    private void Start() {
        GetComponent<MeshRenderer>().sortingOrder = 30;
        text = GetComponent<TextMesh>();
        updateReaction();
    }

    private void Update() {
        lifeSpan -= Time.deltaTime;
        if (lifeSpan < 0) {
            Destroy(this.gameObject);
        }
    }

    public void setReaction(Reaction reaction) {
        this.reaction = reaction;
    }
    private void updateReaction(){
        switch (reaction) {
            case Reaction.VaporizePyro:
                text.text = "Vaporize";
                text.color = Color.red;
                break;
            case Reaction.VaporizeHydro:
                text.text = "Vaporize";
                text.color = Color.blue;
                break;
            case Reaction.Freeze:
                text.text = "Freeze";
                text.color = Color.cyan;
                break;
            case Reaction.MeltPyro:
                text.text = "Melt";
                text.color = Color.red;
                break;
            case Reaction.MeltCryo:
                text.text = "Melt";
                text.color = Color.cyan;
                break;
            case Reaction.Overload:
                text.text = "Overload";
                text.color = Color.magenta;
                break;
            case Reaction.Superconduct:
                text.text = "Superconduct";
                text.color = Color.cyan;
                break;
            case Reaction.Electrocute:
                text.text = "Electrocute";
                text.color = Color.magenta;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(reaction), reaction, null);
        }

        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5f);
    }
}