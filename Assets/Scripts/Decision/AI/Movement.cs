using FSMMono;
using UnityEngine;

public class Movement : StateMachineBehaviour
{
    Transform player;
    AIDecisionAgent DecisionAgent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!player)
            player = GameObject.Find("Player").transform;

        if (!DecisionAgent)
            DecisionAgent = animator.GetComponent<AIDecisionAgent>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!player || !DecisionAgent)
            return;

        float curAngle = (player.rotation.eulerAngles.y + DecisionAgent.OffsetAngle) * Mathf.Deg2Rad;
        DecisionAgent.MoveTo(player.position + DecisionAgent.OffsetDistance * new Vector3(Mathf.Sin(curAngle), 0f, Mathf.Cos(curAngle)).normalized);
    }
}
