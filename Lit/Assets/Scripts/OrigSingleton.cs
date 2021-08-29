﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrigSingleton : MonoBehaviour
{

    public static OrigSingleton instance;
   
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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