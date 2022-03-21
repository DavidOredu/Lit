using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkTest : MonoBehaviour
{
    public Perk testFloatPerk;
    public Perk testBoolPerk;
    public float floatValueToChange;
    public bool boolValueToChange;
    public PerkData floatPerkData;
    public PerkData boolPerkData;

    private float originalFloatValue;
    private bool originalBoolValue;
    // Start is called before the first frame update
    void Start()
    {
        originalFloatValue = floatValueToChange;
        originalBoolValue = boolValueToChange;

        testFloatPerk = new Perk(floatPerkData, originalFloatValue);
        testBoolPerk = new Perk(boolPerkData, originalBoolValue);
        testFloatPerk.ActivatePerk();
        testBoolPerk.ActivatePerk();
    }
    [ContextMenu("Float Perk Activator")]
    public void ActivateFloatPerk()
    {
        testFloatPerk.ActivatePerk();
    }
    [ContextMenu("Bool Perk Activator")]
    public void ActivateBoolPerk()
    {
        testBoolPerk.ActivatePerk();
    }
    [ContextMenu("Float Perk Remover")]
    public void RemoveFloatPerk()
    {
        floatValueToChange = testFloatPerk.RemovePerk(floatValueToChange);
    }
    [ContextMenu("Bool Perk Remover")]
    public void RemoveBoolPerk()
    {
        boolValueToChange = testBoolPerk.RemovePerk(boolValueToChange);
    }
    // Update is called once per frame
    void Update()
    {
        floatValueToChange = testFloatPerk.HandleActivePerk(floatValueToChange);
        boolValueToChange = testBoolPerk.HandleActivePerk(boolValueToChange);
    }
}
