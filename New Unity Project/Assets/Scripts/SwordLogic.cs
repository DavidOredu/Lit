using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordLogic : MonoBehaviour
{
    Animator anim;
    RaycastLogic raycastLogic;

    [SerializeField]
    int damageAmount = 25;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        raycastLogic = GetComponentInParent<RaycastLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        SetAttacking(Input.GetButton("Fire1"));
    }
    public void WeaponHit()
    {
        Debug.Log("Weapon Hit!");
        EnemyLogic enemyLogic = raycastLogic.GetTargetEnemyLogic();

        if(enemyLogic)
            enemyLogic.TakeDamage(damageAmount);
    }
    public void SetAttacking(bool isAttacking)
    {
        anim.SetBool("isAttacking", isAttacking);
    }
}
