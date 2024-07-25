using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurEnemy : Enemy
{
    private Transform crystal;

    protected override void StartChildren()
    {
        findCrystal = true;
        weapon = transform.Find("root/pelvis/spine_01/spine_02/spine_03/clavicle_r/upperarm_r/lowerarm_r/hand_r/mino_weapon_axe").gameObject.GetComponent<EnemyWeapon>();
    }

    protected override void UpdateChildren()
    {
    }

    protected override void Attack()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        animator.SetBool("basic1", true);
        animator.SetBool("isRunning", false);
        weapon.SetDamage(20f);
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
