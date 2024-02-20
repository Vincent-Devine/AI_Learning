using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToChest : Action
{
    public MoveToChest()
    {
        conditions.Add(CONDITION.NEAR_CHEST, false);
        conditions.Add(CONDITION.HAS_BAR, true);
        effects.Add(CONDITION.NEAR_CHEST, true);
        effects.Add(CONDITION.NEAR_ORE, false);
        effects.Add(CONDITION.NEAR_FURNACE_WITH_IRON, false);
        effects.Add(CONDITION.NEAR_AVAILABLE_FURNACE, false);
        effects.Add(CONDITION.NEAR_PICKAXE, false);
    }

    public override void MakeAction(PlannerAgent PlannerAgent)
    {
        if (!CanMakeAction(WorldState.instance.GetWorldState(PlannerAgent)) && (World.Instance.chest == null))
        {
            PlannerAgent.AbortAction(ABORT_CODE.CONDITION_NOT_RESPECTED);
            return;
        }

        PlannerAgent.StartCoroutine(MoveChest(World.Instance.chest.transform.position, PlannerAgent));
    }

    public override int GetCost(Dictionary<CONDITION, bool> worldState)
    {
        return 1;
    }

    private IEnumerator MoveChest(Vector3 chestPosition, PlannerAgent PlannerAgent)
    {
        PlannerAgent.NavMeshAgent.isStopped = false;
        PlannerAgent.NavMeshAgent.destination = chestPosition;
        PlannerAgent.StartWalking();

        while (Vector3.Distance(PlannerAgent.transform.position, chestPosition) > UTILS.NEAR)
            yield return null;

        PlannerAgent.StopWalking();
        PlannerAgent.NavMeshAgent.isStopped = true;
        PlannerAgent.CompleteAction();
    }
}
