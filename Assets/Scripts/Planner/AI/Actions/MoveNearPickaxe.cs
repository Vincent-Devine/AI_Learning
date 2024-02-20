using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNearPickaxe : Action
{
    public MoveNearPickaxe()
    {
        conditions.Add(CONDITION.HAS_PICKAXE, false);
        conditions.Add(CONDITION.NEAR_PICKAXE, false);
        conditions.Add(CONDITION.HAS_PICKAXE_AVAILABLE_IN_WORLD, true);
        effects.Add(CONDITION.NEAR_PICKAXE, true);
        effects.Add(CONDITION.NEAR_ORE, false);
        effects.Add(CONDITION.NEAR_CHEST, false);
        effects.Add(CONDITION.NEAR_AVAILABLE_FURNACE, false);
        effects.Add(CONDITION.NEAR_FURNACE_WITH_IRON, false);
    }

    public override void MakeAction(PlannerAgent PlannerAgent)
    {
        if (!CanMakeAction(WorldState.instance.GetWorldState(PlannerAgent)))
        {
            PlannerAgent.AbortAction(ABORT_CODE.CONDITION_NOT_RESPECTED);
            return;
        }

        Pickaxe pickaxe = World.Instance.GetNearPickaxe(PlannerAgent.transform.position);

        if(!pickaxe)
        {
            PlannerAgent.AbortAction(ABORT_CODE.NO_PICKAXE_AVAILABLE);
            return;
        }

        PlannerAgent.StartCoroutine(MovePickaxe(pickaxe.transform.position, PlannerAgent));
    }

    public override int GetCost(Dictionary<CONDITION, bool> worldState)
    {
        return 1;
    }

    private IEnumerator MovePickaxe(Vector3 pickaxePosition, PlannerAgent PlannerAgent)
    {
        PlannerAgent.NavMeshAgent.isStopped = false;
        PlannerAgent.NavMeshAgent.destination = pickaxePosition;
        PlannerAgent.StartWalking();

        while (Vector3.Distance(PlannerAgent.transform.position, pickaxePosition) > UTILS.NEAR)
        {
            if (!CanMakeAction(WorldState.instance.GetWorldState(PlannerAgent)))
            {
                PlannerAgent.AbortAction(ABORT_CODE.CONDITION_NOT_RESPECTED);
                yield break;
            }
            yield return null;
        }

        PlannerAgent.StopWalking();
        PlannerAgent.NavMeshAgent.isStopped = true;
        PlannerAgent.CompleteAction();
    }
}
