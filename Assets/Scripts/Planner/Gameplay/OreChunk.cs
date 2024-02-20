using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreChunk : MonoBehaviour
{
    [SerializeField] private int _amount;
    public int Amount { get { return _amount; } }

    [SerializeField] private int _reserved = 0;

    private int coroutineCount = 0;

    [Header("References")]
    [SerializeField] private GameObject particuleSystem;
    [SerializeField] private List<GameObject> ores;

    private void Start()
    {
        World.Instance.RegisterOre(this);
    }

    private void OnDestroy()
    {
        World.Instance.UnregisterOre(this);
    }

    public bool HasOreAvailable()
    {
        return _amount > 0;
    }

    public bool ReserveOre(int amount)
    {
        if (_amount <= 0)
            return false;

        if(amount > _amount)
        {
            _reserved += _amount;
            _amount = 0;
        }
        else
        {
            _reserved += amount;
            _amount -= amount;
        }

        return true;
    }

    public int PickUpOre(int amount, float timePickUpOre)
    {
        int amountMine = amount;
        if(amount > _reserved)
            amountMine = _reserved;

        _reserved -= amountMine;

        coroutineCount++;
        StartCoroutine(CheckDestroy(amountMine, timePickUpOre));
        return amountMine;
    }

    private IEnumerator CheckDestroy(int amountMine, float timePickUpOre)
    {
        particuleSystem.SetActive(true);
        for(int i = 0; i < amountMine; i++)
        {
            yield return new WaitForSeconds(timePickUpOre);
            int index = Random.Range(0, ores.Count);
            Destroy(ores[index]);
            ores.RemoveAt(index);
        }
        coroutineCount--;

        if(coroutineCount == 0)
        {
            particuleSystem.SetActive(false);
            if (_amount <= 0 && _reserved <= 0)
                Destroy(gameObject);
        }
    }
}
