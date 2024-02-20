using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealCooldownScript : MonoBehaviour
{
    Animator animator;
    Heal healBehaviorScript;
    Slider actionBarUI;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        healBehaviorScript = animator.GetBehaviour<Heal>();
        actionBarUI = transform.Find("Canvas/ActionBar").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        float curHealCooldown = animator.GetFloat("curHealCooldown");
        if (curHealCooldown >= 0f)
        {
            animator.SetFloat("curHealCooldown", curHealCooldown - Time.deltaTime);
            actionBarUI.value = curHealCooldown / healBehaviorScript.HealCooldown;
        }
    }
}
