using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBar : Action
{
    public CraftingBar()
    {
        conditions.Add(CONDITION.NEAR_AVAILABLE_FURNACE, true);
        conditions.Add(CONDITION.HAS_ENOUGH_ORE, true);
        effects.Add(CONDITION.HAS_ENOUGH_ORE, false);
        effects.Add(CONDITION.BAR_IS_READY_TO_PICK_UP, true);
    }

    public override void MakeAction(PlannerAgent PlannerAgent)
    {
        if(!CanMakeAction(WorldState.instance.GetWorldState(PlannerAgent)))
        {
            PlannerAgent.AbortAction(ABORT_CODE.CONDITION_NOT_RESPECTED);
            return;
        }

        Furnace availableFurnace = World.Instance.GetNearAvailableFurnace(PlannerAgent.transform.position ,PlannerAgent.oreCurrentCapacity);

        if (!availableFurnace)
        {
            PlannerAgent.AbortAction(ABORT_CODE.NO_AVAILLABLE_FURNACE);
            return;
        }

        PlannerAgent.StartCoroutine(Crafting(PlannerAgent, availableFurnace));
    }

    public override int GetCost(Dictionary<CONDITION, bool> worldState)
    {
        return 1;
    }

    private IEnumerator Crafting(PlannerAgent PlannerAgent, Furnace furnace)
    {
        PlannerAgent.StartCraftingBar();
        yield return new WaitForSeconds(UTILS.SECOND_ANIM_CRAFTING_BAR);
        PlannerAgent.StopCraftingBar();
        PlannerAgent.oreCurrentCapacity = furnace.TryCraft(PlannerAgent.oreCurrentCapacity);
        PlannerAgent.CompleteAction();
    }
}
