using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;

public class CameraFollowNetwork : NetworkBehaviour
{
    [SerializeField] private CinemachineVirtualCamera camera1;
    
    private CinemachineTransposer transposer;

    // Start is called before the first frame update

    public override void OnStartAuthority()
    {
        transposer = camera1.GetCinemachineComponent<CinemachineTransposer>();
        camera1.gameObject.SetActive(true);
        enabled = true;
    }
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
