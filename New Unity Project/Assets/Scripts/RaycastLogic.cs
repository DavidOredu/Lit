using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum PlayerState
{
    Idle,
    Moving,
    AttackMoving,
}
public class RaycastLogic : MonoBehaviour
{
    NavMeshAgent navMeshAgent;

    [SerializeField]
    GameObject clickObj;

    float clickObjSize;

    [SerializeField]
    float meleeRange = 1.5f;

    SwordLogic swordLogic;

    PlayerState playerState = PlayerState.Idle;

    GameObject enemyTarget;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        swordLogic = GetComponentInChildren<SwordLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            RaycastCameraToMouse();
        }

        if(clickObjSize > 0)
        {
            clickObjSize -= Time.deltaTime;
            clickObj.transform.localScale = clickObjSize * Vector3.one;
        }

        if(enemyTarget && playerState == PlayerState.AttackMoving)
        {
            navMeshAgent.SetDestination(enemyTarget.transform.position);
            navMeshAgent.isStopped = false;
        }
        CheckAttackRange();
    }
    void RaycastCameraToMouse()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, 100f))
        {
            Debug.Log("We hit an object: " + hit.collider.name);
            Debug.Log("We hit an object at position: " + hit.point);

            navMeshAgent.destination = hit.point;
            navMeshAgent.isStopped = false;

            DisplayClickObj(hit.point);

            if (hit.collider.CompareTag("Enemy"))
            {
                enemyTarget = hit.collider.gameObject;
                playerState = PlayerState.AttackMoving;
            }
            else
            {
                enemyTarget = null;
                playerState = PlayerState.Moving;
            }
        }
    }
    void DisplayClickObj(Vector3 pos)
    {
        clickObjSize = 1f;
        clickObj.transform.localScale = Vector3.one;
        clickObj.transform.position = pos;
    }
    void CheckAttackRange()
    {
        if (!enemyTarget || playerState != PlayerState.AttackMoving) { return; }
 
        Debug.DrawRay(transform.position, transform.forward * meleeRange, Color.red);

        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        if(Physics.Raycast(ray, out hit, meleeRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                swordLogic.SetAttacking(true);
                navMeshAgent.isStopped = true;
            }
        }
    }
    public EnemyLogic GetTargetEnemyLogic()
    {
        if (enemyTarget)
        {
            EnemyLogic enemyLogic = enemyTarget.GetComponent<EnemyLogic>();
            if (enemyLogic)
            {
                return enemyLogic;
            }
        }
        return null;
    }
}