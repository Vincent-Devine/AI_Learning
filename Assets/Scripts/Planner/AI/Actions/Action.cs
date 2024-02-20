using System.Collections.Generic;

public abstract class Action
{
    protected Dictionary<CONDITION, bool> conditions = new Dictionary<CONDITION, bool>();
    protected Dictionary<CONDITION, bool> effects = new Dictionary<CONDITION, bool>();

    public bool CanMakeAction(Dictionary<CONDITION, bool> worldState)
    {
        foreach(KeyValuePair<CONDITION, bool> condition in conditions)
        {
            if (worldState.TryGetValue(condition.Key, out bool value) && value != condition.Value)
                return false;
        }
        return true;
    }

    public Dictionary<CONDITION, bool> ApplyEffects(Dictionary<CONDITION, bool> worldState)
    {
        Dictionary<CONDITION, bool> newWorldSpace = UTILS.Copy(worldState);
        
        foreach (KeyValuePair<CONDITION, bool> effect in effects)
            newWorldSpace[effect.Key] = effect.Value;
        
        return newWorldSpace;
    }

    public abstract void MakeAction(PlannerAgent PlannerAgent);

    public abstract int GetCost(Dictionary<CONDITION, bool> worldState);
}
