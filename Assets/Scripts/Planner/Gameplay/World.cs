using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    private static World instance;
    public static World Instance { get { return instance; } }
    
    public List<OreChunk> OreChunks { get; private set; } = new List<OreChunk>();
    public List<Furnace> Furnaces { get; private set; } = new List<Furnace>();
    public Chest chest { get; private set; } = null;
    public Pickaxe pickaxe { get; private set; } = null;

    private void Awake()
    {
        instance = this;
    }

    public void RegisterOre(OreChunk ore)
    {
        if (!OreChunks.Contains(ore))
        {
            OreChunks.Add(ore);
        }
    }

    public void UnregisterOre(OreChunk ore)
    {
        if (OreChunks.Contains(ore))
        {
            OreChunks.Remove(ore);
        }
    }

    public void RegisterFurnace(Furnace furnace)
    {
        if (!Furnaces.Contains(furnace))
        {
            Furnaces.Add(furnace);
        }
    }

    public void UnregisterFurnace(Furnace furnace)
    {
        if (Furnaces.Contains(furnace))
        {
            Furnaces.Remove(furnace);
        }
    }

    public void RegisterChest(Chest chest)
    {
        this.chest = chest;
    }

    public void UnregisterChest()
    {
        chest = null;
    }

    public void RegisterPickaxe(Pickaxe pickaxe)
    {
        this.pickaxe = pickaxe;
    }

    public void UnregisterPickaxe()
    {
        pickaxe = null;
    }

    public List<Furnace> GetAvailableFurnaces(int oreAmount)
    {
        List<Furnace> availableFurnaces = new List<Furnace>();
        foreach (Furnace furnace in Furnaces)
        {
            if (furnace.CanCraft(oreAmount))
                availableFurnaces.Add(furnace);
        }
        return availableFurnaces;
    }

    public List<Furnace> GetFurnacesWithIron()
    {
        List<Furnace> availableFurnaces = new List<Furnace>();
        foreach (Furnace furnace in Furnaces)
        {
            if (furnace.CanPickUp())
                availableFurnaces.Add(furnace);
        }
        return availableFurnaces;
    }

    public OreChunk GetNearOre(Vector3 PlannerAgentPosition)
    {
        if(OreChunks.Count == 0)
            return null;

        float distanceNearOre = float.MaxValue;
        OreChunk nearOre = null;

        foreach(OreChunk oreChunk in OreChunks)
        {
            if (distanceNearOre > Vector3.Distance(PlannerAgentPosition, oreChunk.transform.position))
            {
                distanceNearOre = Vector3.Distance(PlannerAgentPosition, oreChunk.transform.position);
                nearOre = oreChunk;
            }
        }

        return nearOre;
    }

    public OreChunk GetNearOreAvailable(Vector3 PlannerAgentPosition)
    {
        if (OreChunks.Count == 0)
            return null;

        float distanceNearOre = float.MaxValue;
        OreChunk nearOre = null;

        foreach (OreChunk oreChunk in OreChunks)
        {
            if (oreChunk.HasOreAvailable() &&
                distanceNearOre > Vector3.Distance(PlannerAgentPosition, oreChunk.transform.position))
            {
                distanceNearOre = Vector3.Distance(PlannerAgentPosition, oreChunk.transform.position);
                nearOre = oreChunk;
            }
        }

        return nearOre;
    }

    public Furnace GetNearAvailableFurnace(Vector3 PlannerAgentPosition, int oreAmount)
    {
        List<Furnace> availableFurnaces = GetAvailableFurnaces(oreAmount);

        if (availableFurnaces.Count == 0)
            return null;

        float distanceFurnace = float.MaxValue;
        Furnace nearFurnace = null;

        foreach (Furnace furnace in availableFurnaces)
        {
            if (distanceFurnace > Vector3.Distance(PlannerAgentPosition, furnace.transform.position))
            {
                distanceFurnace = Vector3.Distance(PlannerAgentPosition, furnace.transform.position);
                nearFurnace = furnace;
            }
        }

        return nearFurnace;
    }

    public Furnace GetNearFurnaceWithIron(Vector3 PlannerAgentPosition)
    {
        List<Furnace> furnacesWithIron = GetFurnacesWithIron();

        if (furnacesWithIron.Count == 0)
            return null;

        float distanceFurnace = float.MaxValue;
        Furnace nearFurnace = null;

        foreach (Furnace furnace in furnacesWithIron)
        {
            if (distanceFurnace > Vector3.Distance(PlannerAgentPosition, furnace.transform.position))
            {
                distanceFurnace = Vector3.Distance(PlannerAgentPosition, furnace.transform.position);
                nearFurnace = furnace;
            }
        }

        return nearFurnace;
    }

    public Pickaxe GetNearPickaxe(Vector3 PlannerAgentPosition)
    {
        return pickaxe;
    }

    public bool HasPickaxeAvailble()
    {
        return pickaxe;
    }
}
