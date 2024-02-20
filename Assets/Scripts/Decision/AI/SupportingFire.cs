using FSMMono;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportingFire : StateMachineBehaviour
{
    GameObject target = null;
    AIDecisionAgent DecisionAgent = null;

    [SerializeField] private const string SUPPORTING_FIRE_ANIMATOR = "SupportingFire";

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!target)
            target = GameObject.Find("TargetCursor(Clone)");
        if (!DecisionAgent)
            DecisionAgent = animator.GetComponent<AIDecisionAgent>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(DecisionAgent && target)
        {
            DecisionAgent.ShootToPosition(target.transform.position);
            animator.SetBool(SUPPORTING_FIRE_ANIMATOR, false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
