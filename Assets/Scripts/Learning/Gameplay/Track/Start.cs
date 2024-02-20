using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GeneticManager.Instance.SaveCurrentGenome();
    }
}
