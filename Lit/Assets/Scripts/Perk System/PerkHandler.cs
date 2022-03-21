using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkHandler : MonoBehaviour
{
    public Racer racer;

    [Header("PERKS")]
    public Perk damageResistance;
    // Start is called before the first frame update
    void Start()
    {
       // damageResistance = new Perk(Resources.Load<PerkData>($"{racer.runner.stickmanNet.currentColor.colorID}/Perks/Damage Resistance"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
