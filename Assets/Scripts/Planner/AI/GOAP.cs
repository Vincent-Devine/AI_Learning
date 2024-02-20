using System.Collections.Generic;
using UnityEngine;

public class GOAP : MonoBehaviour
{
    public static GOAP instance { get; private set; }

    private List<Action> actions = new List<Action>();
    private List<Dictionary<CONDITION, bool>> goals = new List<Dictionary<CONDITION, bool>>();

    private void Awake()
    {
        instance = this;
        InitGoals();
        InitActions();
    }
    public List<Action> RunGOAP(PlannerAgent PlannerAgent)
    {
        Node currentState = new Node(WorldState.instance.GetWorldState(PlannerAgent));
        Node bestLeave = null;

        GOAPStats.instance.StartBuildGraph();
        foreach (Dictionary<CONDITION, bool> goal in goals)
        {
            BuildGraph(currentState, ref bestLeave, actions, goal);
            if (bestLeave != null)
            {
                PrintGoalObject(PlannerAgent.name, goal);
                GOAPStats.instance.FinishBuildGraph();
                return bestLeave.actions;
            }
        }

        GOAPStats.instance.FinishBuildGraph();
        return new List<Action> { new Wait() };
    }

    public bool MainGoalAchieved(Dictionary<CONDITION, bool> worldState)
    {
        return GoalAchieved(worldState, goals[0]);
    }

    private void BuildGraph(Node parentNode, ref Node bestLeave, List<Action> availableActions, Dictionary<CONDITION, bool> goal)
    {
        foreach(Action action in availableActions)
        {
            GOAPStats.instance.NewLeaveTest();

            if(action.CanMakeAction(parentNode.worldState))
            {
                Dictionary<CONDITION, bool> newWorldState = action.ApplyEffects(parentNode.worldState);
                Node node = new Node(newWorldState, parentNode, action);

                if (bestLeave != null && node.cost > bestLeave.cost)
                    continue;

                if(GoalAchieved(newWorldState, goal))
                {
                    bestLeave = node;
                }
                else
                {
                    List<Action> newAvailableActions = CreateActionAvailableList(availableActions, action);
                    BuildGraph(node, ref bestLeave, newAvailableActions, goal);
                }
            }
        }
    }

    private bool GoalAchieved(Dictionary<CONDITION, bool> worldState, Dictionary<CONDITION, bool> goals)
    {
        foreach(KeyValuePair<CONDITION, bool> goal in goals)
        {
            if (worldState.TryGetValue(goal.Key, out bool value) && value != goal.Value)
                return false;
        }
        return true;
    }

    private List<Action> CreateActionAvailableList(List<Action> availableActions, Action actionMade)
    {
        List<Action> newAvailableActions = UTILS.Copy(availableActions);
        newAvailableActions.Remove(actionMade);

        return newAvailableActions;
    }

    private void InitGoals()
    {
        goals.Clear();
        Dictionary<CONDITION, bool> mainGoal = new Dictionary<CONDITION, bool>
        {
            { CONDITION.HAS_ENOUGH_BAR, true }
        };
        goals.Add(mainGoal);
        // Dictionary<CONDITION, bool> subGoal = new Dictionary<CONDITION, bool>
        // {
        //     { CONDITION.HAS_ENOUGH_ORE, true }
        // };
        // goals.Add(subGoal);
    }

    private void InitActions()
    {
        actions.Add(new TakeBar());
        actions.Add(new MoveToChest());
        actions.Add(new StoreBar());
        actions.Add(new MoveNearPickaxe());
        actions.Add(new TakePickaxe());
        actions.Add(new MoveNearOre());
        actions.Add(new MineOre());
        actions.Add(new MoveToAvailableFurnace());
        actions.Add(new CraftingBar());
        actions.Add(new MoveToFurnaceWithIron());
    }

    private void PrintGoalObject(string PlannerAgentName, Dictionary<CONDITION, bool> goal)
    {
        if (goal.ContainsKey(CONDITION.HAS_ENOUGH_BAR))
            Debug.Log(PlannerAgentName + " - Maingoal: has enought bar");
        else
            Debug.Log(PlannerAgentName + " - Subgoal: has enought ore");
    }
}
