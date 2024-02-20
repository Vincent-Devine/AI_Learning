using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.XR;

public class CheckEnemy : MonoBehaviour
{
    public List<GameObject> enemyInZone = new List<GameObject>();
    public GameObject target = null;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(target && !CheckEnemyInView(target))
            target = null;

        if (!target)
            GetNewTarget();
    }

    private void GetNewTarget()
    {
        if (enemyInZone.Count == 0)
            return;

        List<GameObject> enemyToDelete = new List<GameObject>();
        foreach (GameObject enemy in enemyInZone)
        {
            if (!enemy.activeSelf)
            {
                enemyToDelete.Add(enemy);
                continue;
            }

            if (CheckEnemyInView(enemy))
            {
                target = enemy;
                animator.SetBool("Shield", true);
                break;
            }
        }

        foreach (GameObject enemy in enemyToDelete)
            enemyInZone.Remove(enemy);
    }

    private bool CheckEnemyInView(GameObject _target)
    {
        if (!_target)
            return false;

        Ray ray = new Ray();
        ray.origin = transform.position;
        ray.direction = _target.transform.position - transform.position;
        RaycastHit hit;
        // Ignore allies, enemy bullet, allies bullet
        int ignoreSelf = ~(1 << LayerMask.NameToLayer("Allies") | 1 << LayerMask.NameToLayer("BulletEnemy") | 1 << LayerMask.NameToLayer("BulletAllies"));
        if (Physics.Raycast(ray, out hit, 1000f, ignoreSelf, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.transform.parent.gameObject == _target)
                return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemyInZone.Add(other.transform.parent.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemyInZone.Remove(other.transform.parent.gameObject);
            if(target == other.transform.parent.gameObject)
                target = null;
        }
    }
}
