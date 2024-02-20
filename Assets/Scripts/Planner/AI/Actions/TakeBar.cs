using System.Collections.Generic;

public class TakeBar : Action
{
    public TakeBar()
    {
        conditions.Add(CONDITION.NEAR_FURNACE_WITH_IRON, true);
        conditions.Add(CONDITION.BAR_IS_READY_TO_PICK_UP, true);
        conditions.Add(CONDITION.HAS_BAR, false);
        effects.Add(CONDITION.HAS_BAR, true);
        effects.Add(CONDITION.BAR_IS_READY_TO_PICK_UP, false);
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

        if(!furnaceWithIron.TryPickUp())
        {
            PlannerAgent.AbortAction(ABORT_CODE.NO_FURNACE_WITH_BAR);
            return;
        }

        PlannerAgent.TakeBar();
        PlannerAgent.CompleteAction();
    }

    public override int GetCost(Dictionary<CONDITION, bool> worldState)
    {
        return 1;
    }
}
