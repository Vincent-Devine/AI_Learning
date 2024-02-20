using UnityEngine;

public class ShieldProtection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("BulletEnemy"))
            other.gameObject.GetComponent<Bullet>().damage = 1;
    }
}
