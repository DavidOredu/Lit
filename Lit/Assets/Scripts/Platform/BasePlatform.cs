using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BasePlatform : MonoBehaviour {
    [SerializeField]
    private float disappearTime,
    alpha;

    public GameObject platform;
    public Tilemap tilemap;

    private Color color;

    private void Start () {
        StartCoroutine (StartToDisappear ());
    }
    private void Update () {

    }
    IEnumerator StartToDisappear () {
        yield return new WaitForSeconds (disappearTime);
        color.b = 255;
        color.g = 255;
        color.r = 255;
        color.a -= alpha;

        tilemap.color = color;
        yield return null;

        StartCoroutine (StartToDisappear ());
    }
}