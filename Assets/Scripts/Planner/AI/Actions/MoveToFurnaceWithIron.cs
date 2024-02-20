using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToFurnaceWithIron : Action
{
    public MoveToFurnaceWithIron()
    {
        conditions.Add(CONDITION.NEAR_FURNACE_WITH_IRON, false);
        conditions.Add(CONDITION.BAR_IS_READY_TO_PICK_UP, true);
        effects.Add(CONDITION.NEAR_AVAILABLE_FURNACE, true);
        effects.Add(CONDITION.NEAR_FURNACE_WITH_IRON, true);
        effects.Add(CONDITION.NEAR_CHEST, false);
        effects.Add(CONDITION.NEAR_ORE, false);
        effects.Add(CONDITION.NEAR_PICKAXE, false);
    }

    public override void MakeAction(PlannerAgent PlannerAgent)
    {
        if (!CanMakeAction(WorldState.instance.GetWorldState(PlannerAgent)))
        {
            PlannerAgent.AbortAction(ABORT_CODE.CONDITION_NOT_RESPECTED);
            return;
        }

        Furnace furnaceWithIron = World.Instance.GetNearFurnaceWithIron(PlannerAgent.transform.position);

        if (!furnaceWithIron)
        {
            PlannerAgent.AbortAction(ABORT_CODE.NO_FURNACE_WITH_BAR);
            return;
        }

        PlannerAgent.StartCoroutine(MoveFurnace(furnaceWithIron.transform.position, PlannerAgent));
    }

    public override int GetCost(Dictionary<CONDITION, bool> worldState)
    {
        return 1;
    }

    private IEnumerator MoveFurnace(Vector3 furnacePosition, PlannerAgent PlannerAgent)
    {
        PlannerAgent.NavMeshAgent.isStopped = false;
        PlannerAgent.NavMeshAgent.destination = furnacePosition;
        PlannerAgent.StartWalking();

        while (Vector3.Distance(PlannerAgent.transform.position, furnacePosition) > UTILS.NEAR)
            yield return null;

        PlannerAgent.StopWalking();
        PlannerAgent.NavMeshAgent.isStopped = true;
        PlannerAgent.CompleteAction();
    }
}
