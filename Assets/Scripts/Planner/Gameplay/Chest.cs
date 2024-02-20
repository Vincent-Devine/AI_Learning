using TMPro;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public int amountBar { get; private set; } = 0;
    
    private Animator animator;

    [Header("References")]
    [SerializeField] private TMP_Text amountBarText = null;

    private void Start()
    {
        World.Instance.RegisterChest(this);
        animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        World.Instance.UnregisterChest();
    }

    public void OpenChest()
    {
        animator.SetBool("OpenChest", true);
    }

    public void DepositIron()
    {
        amountBar++;
        amountBarText.text = amountBar.ToString();
        animator.SetBool("OpenChest", false);
    }
}
