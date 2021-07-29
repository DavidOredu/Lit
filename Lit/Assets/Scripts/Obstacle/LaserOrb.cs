using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserOrb : Laser
{
    public override void Update()
    {
        base.Update();

        if (hasElapsed)
        {
            hasElapsed = false;
            
        }
    }
    public override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        Destroy(gameObject);
        
    }

    public override void Start()
    {
        base.Start();
    }
}
