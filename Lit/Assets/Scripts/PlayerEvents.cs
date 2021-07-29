using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    public static PlayerEvents instance;
   
    public static event Action OnGottenPowerup;
    public static event Action<int> OnAttackRunner;
    public void Awake()
    {
        instance = this;
    }
    public void GottenPowerup()
    {
        OnGottenPowerup?.Invoke();
    }
    public void AttackedRunner(int count)
    {
        OnAttackRunner.Invoke(count);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
