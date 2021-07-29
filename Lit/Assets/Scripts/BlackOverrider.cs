using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class BlackOverrider : MonoBehaviour
{
    public static BlackOverrider instance;
    //  public delegate void OnBlackOverride();
    //public event OnBlackOverride HandleOnBlackOverride;

    

    public static event Action OnBlackOverride;


    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;

    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Override()
    {
        OnBlackOverride?.Invoke();
    }

}
