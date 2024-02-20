using System.Collections.Generic;
public enum CONDITION : uint
{
    NEAR_ORE,
    HAS_ENOUGH_ORE,
    NEAR_AVAILABLE_FURNACE,
    NEAR_FURNACE_WITH_IRON,
    BAR_IS_READY_TO_PICK_UP,
    HAS_BAR,
    NEAR_CHEST,
    HAS_ENOUGH_BAR,
    NEAR_PICKAXE,
    HAS_PICKAXE,
    HAS_PICKAXE_AVAILABLE_IN_WORLD,
}

public static class UTILS
{
    public static float NEAR = 2f;
    public static int ENOUGHT_BAR = 5;
    public static int SECOND_WAIT = 5;
    public static int SECOND_ANIM_MINE_WITHOUT_PICKAXE = 1;
    public static float SECOND_ANIM_MINE_WITH_PICKAXE = .5f;
    public static float SECOND_ANIM_CRAFTING_BAR = 2f;
    public static float SECOND_ANIM_STORING_BAR = .5f;

    public static Dictionary<CONDITION, bool> Copy(Dictionary<CONDITION, bool> toCopy)
    {
        Dictionary<CONDITION, bool> copy = new Dictionary<CONDITION, bool>();
        
        foreach(CONDITION condition in toCopy.Keys)
            copy.Add(condition, toCopy[condition]);

        return copy;
    }

    public static List<Action> Copy(List<Action> toCopy)
    {
        List<Action> copy = new List<Action>();

        foreach(Action action in toCopy)
            copy.Add(action);

        return copy;
    }
}

public class ABORT_CODE
{
    private ABORT_CODE(string value) { this.value = value; }
    public string value { get; private set; }

    public static ABORT_CODE CONDITION_NOT_RESPECTED { get { return new ABORT_CODE("condition not respected"); } }
    public static ABORT_CODE CANT_RESERVE_ORE { get { return new ABORT_CODE("can't reserve ore"); } }
    public static ABORT_CODE NO_AVAILLABLE_FURNACE { get { return new ABORT_CODE("no available furnace"); } }
    public static ABORT_CODE TAKE_TOO_MANY_ORE { get { return new ABORT_CODE("take to many ore"); } }
    public static ABORT_CODE NO_FURNACE_WITH_BAR { get { return new ABORT_CODE("no furnace with bar"); } }
    public static ABORT_CODE BAR_ALREADY_TAKEN { get { return new ABORT_CODE("bar already taken"); } }
    public static ABORT_CODE PICKAXE_ALREADY_TAKEN { get { return new ABORT_CODE("pickaxe already taken"); } }
    public static ABORT_CODE NO_PICKAXE_AVAILABLE { get { return new ABORT_CODE("no pickaxe available"); } }

    public override string ToString()
    {
        return value;
    }
}

public class Node
{
    public Dictionary<CONDITION, bool> worldState { get; private set; }
    public List<Action> actions { get; private set; }

    private Node parent;
    public Node child { get; private set; }
    public int cost { get; private set; }

    public Node(Dictionary<CONDITION, bool> worldState)
    {
        this.worldState = UTILS.Copy(worldState);
        this.parent = null;
        this.actions = new List<Action>();
        this.cost = 0;
    }

    public Node(Dictionary<CONDITION, bool> worldState, Node parent, Action action)
    {
        this.worldState = UTILS.Copy(worldState);
        this.parent = parent;
        this.parent.child = this;
        this.actions = UTILS.Copy(parent.actions);
        this.actions.Add(action);
        this.cost = this.parent.cost + action.GetCost(worldState);
    }
}