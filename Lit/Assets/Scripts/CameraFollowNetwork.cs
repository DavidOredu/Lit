using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;

public class CameraFollowNetwork : NetworkBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camera;
    
    private CinemachineTransposer transposer;

    // Start is called before the first frame update

    public override void OnStartAuthority()
    {
        _camera = GameObject.FindGameObjectWithTag("CMvcam").GetComponent<CinemachineVirtualCamera>();
        _camera.gameObject.SetActive(true);
        transposer = _camera.GetCinemachineComponent<CinemachineTransposer>();
        enabled = true;
        _camera.Follow = gameObject.transform;
      //  _camera.LookAt = gameObject.transform;
    }
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
