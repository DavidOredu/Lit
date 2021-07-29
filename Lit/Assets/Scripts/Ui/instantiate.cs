using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instantiate : MonoBehaviour
{
    public float waitTime;
    // Start is called before the first frame update

    private void Awake()
    {
        
    }
    void Start()
    {

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("instan", waitTime);
    }

    void instan()
    {
        

        gameObject.SetActive(true);
    }
}
