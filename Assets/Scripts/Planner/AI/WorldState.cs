using System.Collections.Generic;
using UnityEngine;

public class WorldState : MonoBehaviour
{
    public static WorldState instance { get; private set; }

    private Dictionary<PlannerAgent, Dictionary<CONDITION, bool>> worldStates = new Dictionary<PlannerAgent, Dictionary<CONDITION, bool>>();

    [Header("References")]
    [SerializeField] private List<PlannerAgent> PlannerAgents = new List<PlannerAgent>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        worldStates.Clear();
        foreach (PlannerAgent PlannerAgent in PlannerAgents)
            worldStates.Add(PlannerAgent, new Dictionary<CONDITION, bool>());
    }

    private void FixedUpdate()
    {
        foreach(PlannerAgent PlannerAgent in PlannerAgents)
            ComputeWorldState(PlannerAgent);
    }

    private void ComputeWorldState(PlannerAgent PlannerAgent)
    {
        Dictionary<CONDITION, bool> worldState = new Dictionary<CONDITION, bool>();
        worldState.Clear();
        worldState.Add(CONDITION.NEAR_ORE, PlannerAgentIsNearOre(PlannerAgent));
        worldState.Add(CONDITION.HAS_ENOUGH_ORE, CheckPlannerAgentEnoughtOre(PlannerAgent));
        worldState.Add(CONDITION.NEAR_AVAILABLE_FURNACE, PlannerAgentIsNearAvailableFurnace(PlannerAgent));
        worldState.Add(CONDITION.NEAR_FURNACE_WITH_IRON, PlannerAgentIsNearFurnaceWithIron(PlannerAgent));
        worldState.Add(CONDITION.BAR_IS_READY_TO_PICK_UP, CheckBarReadyToPickUp());
        worldState.Add(CONDITION.HAS_BAR, CheckPlannerAgentHasBar(PlannerAgent));
        worldState.Add(CONDITION.NEAR_CHEST, PlannerAgentIsNearChest(PlannerAgent));
        worldState.Add(CONDITION.HAS_ENOUGH_BAR, CheckEnoughtBar());
        worldState.Add(CONDITION.NEAR_PICKAXE, PlannerAgentIsNearPickaxe(PlannerAgent));
        worldState.Add(CONDITION.HAS_PICKAXE, CheckPlannerAgentHasPickaxe(PlannerAgent));
        worldState.Add(CONDITION.HAS_PICKAXE_AVAILABLE_IN_WORLD, CheckPickaxeAvailableInWorld());

        if (GOAP.instance.MainGoalAchieved(worldState))
            GameManagerPlanner.Instance.EndGame();

        worldStates[PlannerAgent] = worldState;
    }

    public Dictionary<CONDITION, bool> GetWorldState(PlannerAgent PlannerAgent)
    {
        return worldStates[PlannerAgent];
    }

    private bool PlannerAgentIsNearOre(PlannerAgent PlannerAgent)
    {
        OreChunk oreChunk = World.Instance.GetNearOre(PlannerAgent.transform.position);
        if(!oreChunk)
            return false;

        return Vector3.Distance(oreChunk.transform.position, PlannerAgent.transform.position) <= UTILS.NEAR;
    }

    private bool CheckPlannerAgentEnoughtOre(PlannerAgent PlannerAgent)
    {
        return PlannerAgent.oreMaxCapacity == PlannerAgent.oreCurrentCapacity;
    }

    private bool PlannerAgentIsNearAvailableFurnace(PlannerAgent PlannerAgent)
    {
        List<Furnace> availableFurnace = World.Instance.GetAvailableFurnaces(PlannerAgent.oreCurrentCapacity);
        foreach(Furnace furnace in availableFurnace)
        {
            if(Vector3.Distance(furnace.transform.position, PlannerAgent.transform.position) <= UTILS.NEAR)
                    return true;
        }
        return false;
    }

    private bool PlannerAgentIsNearFurnaceWithIron(PlannerAgent PlannerAgent)
    {
        List<Furnace> availableFurnace = World.Instance.GetFurnacesWithIron();
        foreach (Furnace furnace in availableFurnace)
        {
            if (Vector3.Distance(furnace.transform.position, PlannerAgent.transform.position) <= UTILS.NEAR)
                return true;
        }
        return false;
    }

    private bool CheckBarReadyToPickUp()
    {
        return World.Instance.GetFurnacesWithIron().Count > 0;
    }

    private bool CheckPlannerAgentHasBar(PlannerAgent PlannerAgent)
    {
        return PlannerAgent.hasBar;
    }

    private bool PlannerAgentIsNearChest(PlannerAgent PlannerAgent)
    {
        if(World.Instance.chest == null)
            return false;

        return Vector3.Distance(World.Instance.chest.transform.position, PlannerAgent.transform.position) <= UTILS.NEAR;
    }

    private bool CheckEnoughtBar()
    {
        return World.Instance.chest != null && World.Instance.chest.amountBar >= UTILS.ENOUGHT_BAR;
    }

    private bool PlannerAgentIsNearPickaxe(PlannerAgent PlannerAgent)
    {
        if (World.Instance.pickaxe == null)
            return false;

        return Vector3.Distance(World.Instance.pickaxe.transform.position, PlannerAgent.transform.position) <= UTILS.NEAR;
    }

    private bool CheckPlannerAgentHasPickaxe(PlannerAgent PlannerAgent)
    {
        return PlannerAgent.hasPickaxe;
    }

    private bool CheckPickaxeAvailableInWorld()
    {
        return World.Instance.HasPickaxeAvailble();
    }
}

