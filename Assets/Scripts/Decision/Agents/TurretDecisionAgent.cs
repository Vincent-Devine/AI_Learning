using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretDecisionAgent : DecisionAgent
{

    [SerializeField]
    float ShootFrequency = 1f;

    float NextShootDate = 0f;

    public GameObject Target = null;

    void ShootToPosition(Vector3 pos)
    {
        Vector3 direction = pos - transform.position;
        direction.y = 0f;
        transform.forward = direction.normalized;

        // instantiate bullet
        if (BulletPrefab)
        {
            GameObject bullet = Instantiate<GameObject>(BulletPrefab, GunTransform.position + transform.forward * 0.5f, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * BulletPower);
        }
    }
    void Start()
    {
        GunTransform = transform.Find("Body/Gun");
        if (GunTransform == null)
            Debug.Log("could not find gun transform");

        CurrentHP = MaxHP;

        healthBarUI = transform.Find("Canvas/HealthBar").GetComponent<Slider>();
    }

    void Update()
    {
        if (Target && Target.activeInHierarchy && Time.time >= NextShootDate)
        {
            NextShootDate = Time.time + ShootFrequency;
            ShootToPosition(Target.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((Target == null || !Target.activeInHierarchy) && other.gameObject.layer == LayerMask.NameToLayer("Allies"))
        {
            Target = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (Target != null && other.gameObject == Target)
        {
            Target = null;
        }
    }
}
