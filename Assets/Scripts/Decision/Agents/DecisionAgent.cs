using UnityEngine;
using UnityEngine.UI;

public class DecisionAgent : MonoBehaviour, IDamageable
{
    //[HideInInspector]
    public int CurrentHP;

    [SerializeField]
    protected int MaxHP;

    [SerializeField]
    protected float BulletPower;

    [SerializeField]
    protected GameObject BulletPrefab;

    protected Transform GunTransform;

    protected Slider healthBarUI;

    void Start()
    {
        healthBarUI = transform.Find("Canvas/HealthBar").GetComponent<Slider>();
    }

    public virtual void AddDamage(int amount)
    {
        CurrentHP -= amount;
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            gameObject.SetActive(false);
        }

        if(CurrentHP > MaxHP)
            CurrentHP= MaxHP;

        if (healthBarUI)
            healthBarUI.value = CurrentHP;
    }
}
