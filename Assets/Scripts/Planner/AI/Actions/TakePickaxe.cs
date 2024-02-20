using System.Collections.Generic;

public class TakePickaxe : Action
{
    public TakePickaxe()
    {
        conditions.Add(CONDITION.HAS_PICKAXE_AVAILABLE_IN_WORLD, true);
        conditions.Add(CONDITION.NEAR_PICKAXE, true);
        conditions.Add(CONDITION.HAS_PICKAXE, false);
        effects.Add(CONDITION.HAS_PICKAXE, true);
    }

    public override void MakeAction(PlannerAgent PlannerAgent)
    {
        if (!CanMakeAction(WorldState.instance.GetWorldState(PlannerAgent)))
        {
            PlannerAgent.AbortAction(ABORT_CODE.CONDITION_NOT_RESPECTED);
            return;
        }

        Pickaxe pickaxe = World.Instance.GetNearPickaxe(PlannerAgent.transform.position);

        if (!pickaxe)
        {
            PlannerAgent.AbortAction(ABORT_CODE.PICKAXE_ALREADY_TAKEN);
            return;
        }

        PlannerAgent.TakePickaxe();
        pickaxe.TakePickaxe();
        PlannerAgent.CompleteAction();
    }

    public override int GetCost(Dictionary<CONDITION, bool> worldState)
    {
        return 1;
    }
}
