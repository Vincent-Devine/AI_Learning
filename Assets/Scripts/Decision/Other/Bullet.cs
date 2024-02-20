using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Duration = 2f;
    public int damage = 2;

    void Start()
    {
        Destroy(gameObject, Duration);
    }
    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damagedDecisionAgent = collision.gameObject.GetComponentInParent<IDamageable>();
        if (damagedDecisionAgent == null)
            damagedDecisionAgent = collision.gameObject.GetComponent<IDamageable>();
        damagedDecisionAgent?.AddDamage(damage);

        Destroy(gameObject);
    }
}
