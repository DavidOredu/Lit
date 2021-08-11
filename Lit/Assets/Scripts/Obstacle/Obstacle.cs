using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speedReduction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.CompareTag("Player") || other.collider.CompareTag("Opponent"))
        {
            Racer racer = other.collider.GetComponent<Racer>();
            racer.movementVelocity -= speedReduction;

            Destroy(gameObject);
            // place a camera shake effect
            // tell the player to turn the move state bool to false
            // tell the player to play a stumble animation
            // when the animation is done check if is grounded or in air, then turn the appropriate bool true. This should be done in the stumble animation
            // a better way should be used, like an animation then destroyed afterwards, this is just for testing
        }
    }
}
