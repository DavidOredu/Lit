using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : Laser
{
    public override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        var racer = other.collider.GetComponent<Racer>();
        if ((other.gameObject.tag == "Player" || other.gameObject.tag == "Opponent") && racer.isAffectedByLaser)
                {
             
                    gravityScaleTemp = racer.RB.gravityScale;
            //        StartCoroutine(LaserDown(gravityScaleTemp, racer));
                }
        
    }

    public override void Start()
    {
        base.Start();
    }
}
