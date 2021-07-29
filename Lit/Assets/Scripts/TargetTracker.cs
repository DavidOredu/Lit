using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetTracker : MonoBehaviour
{
    [SerializeField] private Stickman stickman;
    [SerializeField] private RectTransform startPos;
    [SerializeField] private RectTransform endPos;
    [SerializeField] private Image image;

    private float mainPos = 0;
    private float startingPos;
    private Vector3 posOffset;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position.x;
        posOffset = endPos.position - startPos.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {     
        //        mainPos = ((player.currentPositionPercentage / 100) * posOffset.x) + startingPos;
                transform.position = new Vector3(mainPos, transform.position.y);
        image.material = Resources.Load<Material>($"{stickman.currentColor.colorID}");
    }
}
