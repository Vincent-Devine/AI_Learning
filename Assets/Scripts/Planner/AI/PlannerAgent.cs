using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlannerAgent : MonoBehaviour
{
    protected NavMeshAgent _navMeshAgent;
    public NavMeshAgent NavMeshAgent { get { return _navMeshAgent; } }

    [Header("References")]
    [SerializeField] private GameObject pickaxe = null;
    [SerializeField] private GameObject pickaxePrefab = null;
    [SerializeField] private GameObject bar = null;
    [SerializeField] private GameObject hands = null;
    [SerializeField] private GameObject oreInHand = null;
    [SerializeField] private TMP_Text UIName = null;

    // Action data
    public int oreMaxCapacity { get; private set; } = 2;
    public int oreCurrentCapacity = 0;
    public bool hasBar;
    public bool hasPickaxe { get; private set; } = false;
    public OreChunk ore = null;

    // Action
    private List<Action> actions = new List<Action>();
    private bool doingAction = false;

    // Animation
    private Animator animator = null;

    protected virtual void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        actions.Clear();
        UIName.text = gameObject.name;
    }

    protected virtual void FixedUpdate()
    {
        if (actions.Count == 0)
        {
            actions = GOAP.instance.RunGOAP(this);
            PrintActionsList();
        }

        if(!doingAction)
            StartAction();
    }

    private void StartAction()
    {
        doingAction = true;
        Debug.Log(transform.name + " - Start action: " + actions.First().GetType().Name);
        actions.First().MakeAction(this);
    }

    public void CompleteAction()
    {
        doingAction = false;
        Debug.Log(transform.name + " - Complete action: " + actions.First().GetType().Name);
        actions.Remove(actions.First());
    }

    public void AbortAction(ABORT_CODE abortCode)
    {
        doingAction = false;
        Debug.Log(transform.name + " - Abort action: " + actions.First().GetType().Name + ". Reason: " + abortCode.value);
        actions.Clear();
    }

    public string GetCurrentActionName()
    {
        if (actions.Count == 0)
            return "";
        return actions.First().GetType().Name;
    }

    public void TakePickaxe()
    {
        hasPickaxe = true;
        pickaxe.SetActive(true);
        animator.SetBool("hasPickaxe", true);
    }

    public void DropPickaxe()
    {
        hasPickaxe = false;
        pickaxe.SetActive(false);
        animator.SetBool("hasPickaxe", false);
        Instantiate(pickaxePrefab, new Vector3(transform.position.x, -.45f, transform.position.z), Quaternion.Euler(0f, 85f, 90f));
    }

    public void TakeBar()
    {
        hasBar = true;
        bar.SetActive(true);
        hands.SetActive(true);
        animator.SetBool("hasBar", true);
        if (hasPickaxe)
            DropPickaxe();
    }

    public void StartStoringBar()
    {
        hands.SetActive(true);
        oreInHand.SetActive(true);
        animator.SetBool("StoringBar", true);
    }

    public void DropBar()
    {
        hasBar = false;
        bar.SetActive(false);
        hands.SetActive(false);
        animator.SetBool("hasBar", false);
        animator.SetBool("StoringBar", false);
    }

    public void StartWalking()
    {
        animator.SetBool("isWalking", true);
    }

    public void StopWalking()
    {
        animator.SetBool("isWalking", false);
    }

    public void StartMining()
    {
        animator.SetBool("isMining", true);
        if (!hasPickaxe)
            hands.SetActive(true);
    }

    public void StopMining()
    {
        animator.SetBool("isMining", false);
        if (!hasPickaxe)
            hands.SetActive(false);
    }

    public void StartCraftingBar()
    {
        hands.SetActive(true);
        oreInHand.SetActive(true);
        animator.SetBool("isCraftingBar", true);
    }

    public void StopCraftingBar()
    {
        hands.SetActive(false);
        oreInHand.SetActive(false);
        animator.SetBool("isCraftingBar", false);
    }

    public int GetAmountOreWanted()
    {
        return oreMaxCapacity - oreCurrentCapacity;
    }

    private void PrintActionsList()
    {
        string actionsList = "";
        foreach (Action action in actions)
        {
            if (action == actions.Last())
                actionsList += action.GetType().Name;
            else
                actionsList += action.GetType().Name + " -> ";
        }
        Debug.Log(transform.name + " - Action list: " + actionsList);
    }
}
