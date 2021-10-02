using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakenedBehaviour : StateMachineBehaviour
{
    public float sloMoScale;
    Racer racer;
    GameObject damageEffect;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        racer = animator.gameObject.GetComponent<Racer>();
        switch (racer.currentRacerType)
        {
            case Racer.RacerType.Player:
                racer.StateMachine.ChangeState(racer.playerIdleState);
                break;
            case Racer.RacerType.Opponent:
                racer.StateMachine.ChangeState(racer.opponentIdleState);
                break;
            default:
                break;
        }
        //slo-mo effect
        Time.timeScale = sloMoScale;
        // damage surrounding runners

        // power up effect
        
        //start activated state timer and control in the cyclic progress bar in the game
    }
    
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Time.timeScale = sloMoScale;
     //   racer.movementVelocity = 0f;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch (racer.currentRacerType)
        {
            case Racer.RacerType.Player:
                racer.StateMachine.ChangeState(racer.playerMoveState);
                break;
            case Racer.RacerType.Opponent:
                racer.StateMachine.ChangeState(racer.opponentMoveState);
                break;
            default:
                break;
        }
        Time.timeScale = 1f;
        //use ability
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
