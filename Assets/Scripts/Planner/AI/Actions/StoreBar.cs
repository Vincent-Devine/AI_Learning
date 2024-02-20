using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreBar : Action
{
    public StoreBar()
    {
        conditions.Add(CONDITION.HAS_BAR, true);
        conditions.Add(CONDITION.NEAR_CHEST, true);
        effects.Add(CONDITION.HAS_BAR, false);
        effects.Add(CONDITION.HAS_ENOUGH_BAR, true);
    }

    public override void MakeAction(PlannerAgent PlannerAgent)
    {
        if (!CanMakeAction(WorldState.instance.GetWorldState(PlannerAgent)))
        {
            PlannerAgent.AbortAction(ABORT_CODE.CONDITION_NOT_RESPECTED);
            return;
        }

        PlannerAgent.StartCoroutine(StoringBar(PlannerAgent));
    }

    public override int GetCost(Dictionary<CONDITION, bool> worldState)
    {
        return 1;
    }

    private IEnumerator StoringBar(PlannerAgent PlannerAgent)
    {
        PlannerAgent.StartStoringBar();
        World.Instance.chest.OpenChest();
        yield return new WaitForSeconds(UTILS.SECOND_ANIM_STORING_BAR);
        World.Instance.chest.DepositIron();
        PlannerAgent.DropBar();
        PlannerAgent.CompleteAction();
    }
}
