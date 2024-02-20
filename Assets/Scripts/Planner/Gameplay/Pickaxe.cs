using UnityEngine;

public class Pickaxe : MonoBehaviour
{
    private void Start()
    {
        World.Instance.RegisterPickaxe(this);
    }

    private void OnDestroy()
    {
        World.Instance.UnregisterPickaxe();
    }

    public void TakePickaxe()
    {
        Destroy(gameObject);
    }
}
