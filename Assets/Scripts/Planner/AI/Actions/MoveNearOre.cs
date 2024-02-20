using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNearOre : Action
{
    public MoveNearOre()
    {
        conditions.Add(CONDITION.NEAR_ORE, false);
        conditions.Add(CONDITION.HAS_ENOUGH_ORE, false);
        effects.Add(CONDITION.NEAR_ORE, true);
        effects.Add(CONDITION.NEAR_CHEST, false);
        effects.Add(CONDITION.NEAR_AVAILABLE_FURNACE, false);
        effects.Add(CONDITION.NEAR_FURNACE_WITH_IRON, false);
        effects.Add(CONDITION.NEAR_PICKAXE, false);
    }

    public override void MakeAction(PlannerAgent PlannerAgent)
    {
        if (!CanMakeAction(WorldState.instance.GetWorldState(PlannerAgent)))
        {
            PlannerAgent.AbortAction(ABORT_CODE.CONDITION_NOT_RESPECTED);
            return;
        }

        OreChunk nearOre = World.Instance.GetNearOreAvailable(PlannerAgent.transform.position);

        if(!nearOre.ReserveOre(PlannerAgent.GetAmountOreWanted()))
        {
            PlannerAgent.AbortAction(ABORT_CODE.CANT_RESERVE_ORE);
            return;
        }

        PlannerAgent.ore = nearOre;
        PlannerAgent.StartCoroutine(MoveOre(nearOre.transform.position, PlannerAgent));
    }

    public override int GetCost(Dictionary<CONDITION, bool> worldState)
    {
        return 1;
    }

    private IEnumerator MoveOre(Vector3 orePosition, PlannerAgent PlannerAgent)
    {
        PlannerAgent.NavMeshAgent.isStopped = false;
        PlannerAgent.NavMeshAgent.destination = orePosition;
        PlannerAgent.StartWalking();

        while (Vector3.Distance(PlannerAgent.transform.position, orePosition) > UTILS.NEAR)
            yield return null;

        PlannerAgent.StopWalking();
        PlannerAgent.NavMeshAgent.isStopped = true;
        PlannerAgent.CompleteAction();
    }
}
