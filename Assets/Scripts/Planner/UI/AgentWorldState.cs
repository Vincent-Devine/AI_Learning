using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlannerAgentWorldState : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlannerAgent PlannerAgent;
    [SerializeField] private TMP_Text PlannerAgentName;
    [SerializeField] private TMP_Text actionName;
    [SerializeField] private RawImage nearOre;
    [SerializeField] private RawImage nearFurnacAvailable;
    [SerializeField] private RawImage nearFurnaceWithIron;
    [SerializeField] private RawImage hasEnoughOre;
    [SerializeField] private RawImage hasBar;
    [SerializeField] private RawImage barIsReadyToPickUp;
    [SerializeField] private RawImage nearChest;
    [SerializeField] private RawImage nearPickaxe;
    [SerializeField] private RawImage hasPickaxe;
    [SerializeField] private RawImage hasPickaxeAvailable;

    [Header("Bool")]
    [SerializeField] private Color trueColor;
    [SerializeField] private Color falseColor;

    private void FixedUpdate()
    {
        PlannerAgentName.text = PlannerAgent.name;
        actionName.text = PlannerAgent.GetCurrentActionName();
        
        Dictionary<CONDITION, bool> worldState = WorldState.instance.GetWorldState(PlannerAgent);
        ApplyColor(nearOre, worldState[CONDITION.NEAR_ORE]);
        ApplyColor(nearFurnacAvailable, worldState[CONDITION.NEAR_AVAILABLE_FURNACE]);
        ApplyColor(nearFurnaceWithIron, worldState[CONDITION.NEAR_FURNACE_WITH_IRON]);
        ApplyColor(hasEnoughOre, worldState[CONDITION.HAS_ENOUGH_ORE]);
        ApplyColor(hasBar, worldState[CONDITION.HAS_BAR]);
        ApplyColor(barIsReadyToPickUp, worldState[CONDITION.BAR_IS_READY_TO_PICK_UP]);
        ApplyColor(nearChest, worldState[CONDITION.NEAR_CHEST]);
        ApplyColor(nearPickaxe, worldState[CONDITION.NEAR_PICKAXE]);
        ApplyColor(hasPickaxe, worldState[CONDITION.HAS_PICKAXE]);
        ApplyColor(hasPickaxeAvailable, worldState[CONDITION.HAS_PICKAXE_AVAILABLE_IN_WORLD]);
    }

    private void ApplyColor(RawImage boolImage, bool value)
    {
        if(value)
            boolImage.color = trueColor;
        else
            boolImage.color = falseColor;
    }
}
