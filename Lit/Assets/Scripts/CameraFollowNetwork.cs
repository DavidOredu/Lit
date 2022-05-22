using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;

public class CameraFollowNetwork : NetworkBehaviour
{
    [SerializeField] private Racer racer;
    public bool attackMode;
    private CinemachineVirtualCamera _camera;
    private CinemachineTargetGroup targetGroup;
    
    private CinemachineTransposer transposer;

    // Start is called before the first frame update

    public override void OnStartAuthority()
    {
        _camera = GameObject.FindGameObjectWithTag("CMvcam").GetComponent<CinemachineVirtualCamera>();
        targetGroup = GameObject.FindGameObjectWithTag("TargetGroup").GetComponent<CinemachineTargetGroup>();
        _camera.gameObject.SetActive(true);
        transposer = _camera.GetCinemachineComponent<CinemachineTransposer>();
        enabled = true;
        _camera.Follow = transform;
        //if (targetGroup.FindMember(transform) == -1)
        //    targetGroup.AddMember(transform, 1, 10f);
      //  _camera.LookAt = gameObject.transform;
    }
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        if (attackMode)
            EnlargeViewForAttack();
        else
            CheckCloseObjects();

        if (targetGroup.FindMember(transform) != -1 && targetGroup.m_Targets.Length == 1)
            _camera.Follow = transform;
    }
    private void CheckCloseObjects()
    {
        var hit = Physics2D.OverlapCircleAll(transform.position, racer.playerData.proximitySearchRadius, racer.playerData.whatToSearch);

        // remove previous members of target group not within the search radius
        foreach (var target in targetGroup.m_Targets)
        {
            if (!Utils.HitContainsObject(hit, target.target.gameObject) && target.target != transform)
            {
                targetGroup.RemoveMember(target.target);
            }
        }

        // add new members to target group, including ourselves if we're not yet added
        foreach (var obj in hit)
        {
            if (targetGroup.FindMember(transform) == -1)
                targetGroup.AddMember(transform, 1, 6f);
            if (targetGroup.FindMember(obj.transform) == -1)
                targetGroup.AddMember(obj.transform, 1, 3f);

            _camera.Follow = targetGroup.transform;
        }
    }
    private void EnlargeViewForAttack()
    {
        var hit = Physics2D.OverlapCircleAll(transform.position, racer.playerData.attackSearchRadius, racer.playerData.whatToAttack);

        // remove previous members of target group not within the search radius
        foreach (var target in targetGroup.m_Targets)
        {
            if (!Utils.HitContainsObject(hit, target.target.gameObject) && target.target != transform)
            {
                targetGroup.RemoveMember(target.target);
            }
        }

        // add new members to target group, including ourselves if we're not yet added
        foreach (var obj in hit)
        {
            if (targetGroup.FindMember(transform) == -1)
                targetGroup.AddMember(transform, 1, 6f);
            if (targetGroup.FindMember(obj.transform) == -1)
                targetGroup.AddMember(obj.transform, 1, 3f);

            _camera.Follow = targetGroup.transform;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, racer.playerData.proximitySearchRadius);
    }
}
