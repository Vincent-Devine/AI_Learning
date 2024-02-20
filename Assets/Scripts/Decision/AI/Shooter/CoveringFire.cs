using FSMMono;
using UnityEngine;

public class CoveringFire : StateMachineBehaviour
{
    GameObject target = null;
    AIDecisionAgent DecisionAgent = null;
    public float firingRate = 1.0f;
    float firingTime = 0.0f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        firingTime = firingRate / 2f;
        if (!target)
            target = GameObject.Find("NPCTargetCursor(Clone)");
        if(!DecisionAgent)
            DecisionAgent = animator.GetComponent<AIDecisionAgent>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(DecisionAgent && target)
        {
            firingTime += Time.deltaTime;
            if(firingTime > firingRate)
            {
                DecisionAgent.ShootToPosition(target.transform.position);
                firingTime = 0.0f;
            }
        }
    }
}
