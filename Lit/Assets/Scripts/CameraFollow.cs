using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera camera1;

    private CinemachineTransposer transposer;
    

    
    // Start is called before the first frame update

    void Start()
    {
        transposer = camera1.GetCinemachineComponent<CinemachineTransposer>();
        camera1.gameObject.SetActive(true);
        enabled = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
