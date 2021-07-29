using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Runner
{
    public GameObject runner;
    public Stickman stickman;
    public StickmanNet stickmanNet;
    public SpriteRenderer spriteRendererOfRunner;
    public Racer player;

    public Runner(Stickman stickman, StickmanNet stickmanNet, SpriteRenderer spriteRenderer, Racer player)
    {
        this.stickman = stickman;
        spriteRendererOfRunner = spriteRenderer;
        this.player = player;
        this.stickmanNet = stickmanNet;
    }


    public SpriteRenderer SetRenderer(Collision2D other)
    {
        spriteRendererOfRunner = other.gameObject.GetComponentInChildren<SpriteRenderer>();
        return spriteRendererOfRunner;
    }

    public Racer SetPlayer(Collision2D other)
    {
        player = other.gameObject.GetComponent<Racer>();
        return player;
    }

    public Stickman SetStickman(Collision2D other)
    {
        stickman = other.gameObject.GetComponent<Stickman>();
        return stickman;
    }
}
