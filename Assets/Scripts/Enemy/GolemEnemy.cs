using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemEnemy : Enemy
{
    private Transform crystal;

    protected override void StartChildren()
    {
        findCrystal = true;
        weapon = transform.Find("Hips/Spine/Upper_Arm_R/Lower_Arm_R/Hand_R").gameObject.GetComponent<EnemyWeapon>();
    }

    protected override void Attack()
    {
        if (Vector3.Distance(transform.position, target.position) > 0.05f)
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        animator.SetBool("basic1", true);
        animator.SetBool("isRunning", false);
        weapon.SetDamage(20f);
    }

    protected override void UpdateChildren()
    {

    }

    // Get crystal pos
    protected override void GetTarget()
    {
        target = crystal;
    }

    public void SetCrystal(Transform crystal)
    {
        this.crystal = crystal;
    }
}
