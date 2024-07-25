using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogKnight : Player
{
    private int comboIndex = 0;
    private float attackTime = 0;

    protected override void StartChildren()
    {
        this.walkingSpeed = 90f;
        this.runningSpeed = 150f;
        this.jumpSpeed = 35f;
        this.maxHp = 600f;
        this.hp = maxHp;
        this.basicDamage = 25f;
        this.UpdateHealth();
    }

    protected override void UpdateChildren()
    {
        if (comboIndex > 0 && animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 1.5f)
        {
            animator.SetBool("basic1", false);
            animator.SetBool("basic2", false);
            animator.SetBool("basic3", false);
            weapon.SetDamage(0f);
        }

        if (animator.GetCurrentAnimatorStateInfo(1).IsName("Heavy1") && animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.85f)
        {
            animator.SetBool("heavy1", false);
        }
    }

    public override void AttackSound(int variation)
    {
        audioManager.Attack("heavySword" + variation.ToString());
    }

    protected override void FixedUpdateChildren()
    {

    }

    protected override void BasicAttack()
    {
        if (Time.time - attackTime > 0.6f)
        {
            attackTime = Time.time + 0.6f;
            comboIndex++;
        }
        weapon.SetDamage(25f);

        if (comboIndex > 3) comboIndex = 1;

        if (comboIndex == 1)
        {
            animator.SetBool("basic1", true);
            animator.SetBool("basic3", false);
        }
        if (comboIndex == 2 && animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.65f)
        {
            animator.SetBool("basic1", false);
            animator.SetBool("basic2", true);
        }
        if (comboIndex == 3 && animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.55f)
        {
            animator.SetBool("basic2", false);
            animator.SetBool("basic3", true);
        }
    }

    protected override void HeavyAttack()
    { 
        if (Time.time - attackTime < 0.15f) return;
        attackTime = Time.time + 0.15f;

        comboIndex = 0;
        weapon.SetDamage(35f);
        animator.SetBool("heavy1", true);
        animator.SetBool("basic1", false);
        animator.SetBool("basic2", false);
        animator.SetBool("basic3", false);
    }

    protected override void SpecialAttack()
    {
        if (!DeductMana(5f))
        {
            return;
        }

        var bullet = Instantiate(specialProjectile, projectileSpawnpoint.position, Camera.main.transform.rotation);
        Vector3 direction = Camera.main.transform.forward;

        AudioManager.Instance.Attack("projectile");

        bullet.GetComponent<PlayerProjectile>().explosion = explosion;
        bullet.GetComponent<PlayerProjectile>().damage = 45f;
        bullet.GetComponent<Rigidbody>().AddForce(direction * 140000 * Time.deltaTime);
    }
}
