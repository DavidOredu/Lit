using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    public static PlayerEvents instance;
    
    public static event Action OnBlackOverride;
    public static event Action OnGottenPowerup;
    public static event Action<int> OnAttackRunner;
    public void Awake()
    {
        instance = this;
    }

    public void BlackOverride()
    {
        OnBlackOverride?.Invoke();
    }
    public void GottenPowerup()
    {
        OnGottenPowerup?.Invoke();
    }
    public void AttackedRunner(int count)
    {
        OnAttackRunner.Invoke(count);
    }
}
