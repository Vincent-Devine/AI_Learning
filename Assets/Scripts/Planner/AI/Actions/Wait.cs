using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : Action
{
    public Wait()
    {
    }

    public override void MakeAction(PlannerAgent PlannerAgent)
    {
        PlannerAgent.StartCoroutine(WaitForSecond(PlannerAgent));
    }

    public override int GetCost(Dictionary<CONDITION, bool> worldState)
    {
        return 100;
    }

    private IEnumerator WaitForSecond(PlannerAgent PlannerAgent)
    {
        yield return new WaitForSeconds(UTILS.SECOND_WAIT);
        PlannerAgent.CompleteAction();
    }
}
