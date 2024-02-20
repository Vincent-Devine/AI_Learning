using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineOre : Action
{
    public MineOre()
    {
        conditions.Add(CONDITION.NEAR_ORE, true);
        conditions.Add(CONDITION.HAS_ENOUGH_ORE, false);
        effects.Add(CONDITION.HAS_ENOUGH_ORE, true);
    }

    public override void MakeAction(PlannerAgent PlannerAgent)
    {
        if (!CanMakeAction(WorldState.instance.GetWorldState(PlannerAgent)))
        {
            PlannerAgent.AbortAction(ABORT_CODE.CONDITION_NOT_RESPECTED);
            return;
        }

        float timePickUpOre = PlannerAgent.hasPickaxe ? UTILS.SECOND_ANIM_MINE_WITH_PICKAXE : UTILS.SECOND_ANIM_MINE_WITHOUT_PICKAXE;
        int oreMine = PlannerAgent.ore.PickUpOre(PlannerAgent.GetAmountOreWanted(), timePickUpOre);
        PlannerAgent.ore = null;
        PlannerAgent.oreCurrentCapacity += oreMine;

        PlannerAgent.StartCoroutine(Mine(PlannerAgent, oreMine, PlannerAgent.GetAmountOreWanted(), timePickUpOre));
    }

    public override int GetCost(Dictionary<CONDITION, bool> worldState)
    {
        if (worldState[CONDITION.HAS_PICKAXE])
            return 1;
        return 10;
    }

    private IEnumerator Mine(PlannerAgent PlannerAgent, int oreMine, int oreAmountWantToPick, float timePickUpOre)
    {
        PlannerAgent.StartMining();
        yield return new WaitForSeconds(timePickUpOre * oreMine);
        PlannerAgent.StopMining();

        if (oreMine != oreAmountWantToPick)
            PlannerAgent.AbortAction(ABORT_CODE.TAKE_TOO_MANY_ORE);
        else
            PlannerAgent.CompleteAction();
    }
}
