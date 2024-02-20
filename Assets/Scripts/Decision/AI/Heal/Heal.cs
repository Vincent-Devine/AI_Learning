using FSMMono;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heal : StateMachineBehaviour
{
    AIDecisionAgent healerDecisionAgent;
    public DecisionAgent healTarget;
    List<DecisionAgent> allyDecisionAgents = new List<DecisionAgent>();

    public int HealAmount;
    public int HealthPointThreshold;

    public float HealCooldown;
    public float HealingTime;

    public float HealDistanceThreshold;
    public float MaxHealFetchRange;

    Transform healerTransform;

    Slider actionBarSlider;
    Image actionBarImage;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        animator.SetFloat("curHealTime", HealingTime);

        if (!healerTransform)
            healerTransform = animator.transform;

        if (!actionBarSlider)
            actionBarSlider = animator.transform.Find("Canvas/ActionBar").GetComponent<Slider>();

        if (!actionBarImage)
            actionBarImage = animator.transform.Find("Canvas/ActionBar/Fill Area/Fill").GetComponent<Image>();

        if (allyDecisionAgents.Count > 0)
            return;

        healerDecisionAgent = animator.GetComponent<AIDecisionAgent>();

        Transform allies = GameObject.Find("Allies").transform;

        for (int i = 0; i < allies.childCount; ++i)
        {
            DecisionAgent curDecisionAgent = allies.GetChild(i).GetComponent<DecisionAgent>();

            if (curDecisionAgent != healerDecisionAgent)
                allyDecisionAgents.Add(curDecisionAgent);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!healTarget || !healTarget.gameObject.activeInHierarchy)
        {
            SelectHealTarget(animator);
            return;
        }

        if (Vector3.Distance(healerTransform.position, healTarget.transform.position) > HealDistanceThreshold)
        {
            healerDecisionAgent.MoveTo(healTarget.transform.position);
            return;
        }

        HealTarget(animator);
    }

    void SelectHealTarget(Animator animator)
    {
        int lowerHitpoints = int.MaxValue;
        foreach (DecisionAgent ally in allyDecisionAgents)
        {
            if (!ally.gameObject.activeInHierarchy)
                continue;

            if(
                Vector3.Distance(healerTransform.position, ally.transform.position) < MaxHealFetchRange
                && ally.CurrentHP <= HealthPointThreshold
                && ally.CurrentHP < lowerHitpoints
              )
            {
                lowerHitpoints = ally.CurrentHP;
                healTarget = ally;
            }
        }

        if(!healTarget)
        {
            animator.SetFloat("curHealTime", -0.1f);
            animator.SetFloat("curHealCooldown", .01f);
            return;
        }

        actionBarImage.color = Color.cyan;
    }

    void HealTarget(Animator animator)
    {
        float curHealTime = animator.GetFloat("curHealTime") - Time.deltaTime;

        animator.SetFloat("curHealTime", curHealTime);

        actionBarSlider.value = 1f - curHealTime / HealingTime;

        if (curHealTime > 0f)
            return;

        healTarget.AddDamage(-HealAmount);

        animator.SetFloat("curHealCooldown", HealCooldown);

        actionBarImage.color = Color.gray;

        healTarget = null;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
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
