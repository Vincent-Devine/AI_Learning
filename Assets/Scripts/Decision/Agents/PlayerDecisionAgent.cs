using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDecisionAgent : DecisionAgent
{
    [SerializeField]
    GameObject TargetCursorPrefab = null;
    [SerializeField]
    GameObject NPCTargetCursorPrefab = null;
    bool NPCShootTarget = false;

    Rigidbody rb;
    GameObject TargetCursor = null;
    GameObject NPCTargetCursor = null;

    GameObject deadCanva;

    [SerializeField] private float firingRate = 1.0f;
    private float firingTime = 0.0f;

    [SerializeField] private List<Animator> animatorsAIShooter;
    [SerializeField] private Animator animatorsAIHealer;
    [SerializeField] private Animator animatorsAITank;
    [SerializeField] private const string COVERING_FIRE_ANIMATOR = "CoveringFire";
    [SerializeField] private const string SUPPORTING_FIRE_ANIMATOR = "SupportingFire";

    private GameObject GetTargetCursor()
    {
        if (TargetCursor == null)
            TargetCursor = Instantiate(TargetCursorPrefab);
        return TargetCursor;
    }
    private GameObject GetNPCTargetCursor()
    {
        if (NPCTargetCursor == null)
        {
            NPCTargetCursor = Instantiate(NPCTargetCursorPrefab);
        }
        return NPCTargetCursor;
    }
    public void AimAtPosition(Vector3 pos)
    {
        GetTargetCursor().transform.position = pos;
        Vector3 direction = pos - transform.position;
        direction.y = 0f;
        transform.forward = direction.normalized;
    }
    public void ShootToPosition(Vector3 pos)
    {
        // instantiate bullet
        if (BulletPrefab && firingTime >= firingRate)
        {
            GameObject bullet = Instantiate<GameObject>(BulletPrefab, GunTransform.position + transform.forward * 0.5f, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * BulletPower);
            firingTime = 0;

            if(!NPCShootTarget)
            {
                foreach (Animator animator in animatorsAIShooter)
                    animator.SetBool(SUPPORTING_FIRE_ANIMATOR, true);
            }
            animatorsAIHealer.SetBool(SUPPORTING_FIRE_ANIMATOR, true);
            animatorsAITank.SetBool(SUPPORTING_FIRE_ANIMATOR, true);
        }
    }
    public void NPCShootToPosition(Vector3 pos)
    {
        NPCShootTarget = !NPCShootTarget;
        GetNPCTargetCursor().transform.position = pos;
        GetNPCTargetCursor().GetComponent<MeshRenderer>().enabled = NPCShootTarget;
        foreach (Animator animator in animatorsAIShooter)
            animator.SetBool(COVERING_FIRE_ANIMATOR, NPCShootTarget);
    }
    
    public void MoveToward(Vector3 velocity)
    {
        rb.MovePosition(rb.position + velocity * Time.deltaTime);
    }

    public override void AddDamage(int amount)
    {
        CurrentHP -= amount;
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            StartCoroutine("YouDiedUI");
        }

        if (CurrentHP > MaxHP)
            CurrentHP = MaxHP;

        if (healthBarUI)
            healthBarUI.value = CurrentHP;
    }

    #region MonoBehaviour Methods
    void Start()
    {
        deadCanva = GameObject.Find("/Canvas");
        deadCanva.SetActive(false);

        CurrentHP = MaxHP;
        firingTime = firingRate;
        GunTransform = transform.Find("Gun");
        rb = GetComponent<Rigidbody>();

        healthBarUI = transform.Find("Canvas/HealthBar").GetComponent<Slider>();
    }

    IEnumerator YouDiedUI()
    {
        deadCanva.SetActive(true);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(2);
        Time.timeScale = 1f;
        deadCanva.SetActive(false);
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (firingTime <= firingRate)
            firingTime += Time.deltaTime;
    }

    #endregion

}
