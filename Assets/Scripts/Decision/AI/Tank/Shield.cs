using FSMMono;
using UnityEngine;
using UnityEngine.AI;

public class Shield : StateMachineBehaviour
{
    GameObject shield = null;
    Transform player = null;
    AIDecisionAgent DecisionAgent = null;
    NavMeshAgent NavMeshAgent = null;
    CheckEnemy checkEnemy = null;

    public float moveSpeedShield;
    float moveSpeedNoShield;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!shield)
            shield = animator.transform.GetChild(1).gameObject;
        if (!player)
            player = GameObject.Find("Player").transform;
        if (!DecisionAgent)
            DecisionAgent = animator.GetComponent<AIDecisionAgent>();
        if (!checkEnemy)
            checkEnemy = animator.GetComponent<CheckEnemy>();
        if (!NavMeshAgent)
        {
            NavMeshAgent = animator.GetComponent<NavMeshAgent>();
            moveSpeedNoShield = NavMeshAgent.speed;
        }

        shield.SetActive(true);
        NavMeshAgent.speed = moveSpeedShield;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!player || !DecisionAgent)
            return;

        if(checkEnemy.target)
        {
            // Rotation
            animator.transform.forward = checkEnemy.target.transform.position - animator.transform.position;
            animator.transform.forward = new Vector3(animator.transform.forward.x, 0f, animator.transform.forward.z);

            // Position
            DecisionAgent.MoveTo(player.position + DecisionAgent.OffsetDistance * animator.transform.forward);
        }
        else
        {
            animator.SetBool("Shield", false);
            animator.SetBool("SupportingFire", false);
        }

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        shield.SetActive(false);
        NavMeshAgent.speed = moveSpeedNoShield;
    }
}
