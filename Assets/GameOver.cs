using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    private void Start() {
        GetComponent<MeshRenderer>().sortingOrder = 100;
    }
}
