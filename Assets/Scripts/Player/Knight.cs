using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Player
{
    private int comboIndex = 0;
    private float attackTime = 0f;

    protected override void StartChildren()
    {
        this.walkingSpeed = 120f;
        this.runningSpeed = 180f;
        this.jumpSpeed = 50f;
        this.maxHp = 350f;
        this.hp = maxHp;
        this.SetPlayerActive(true);
        this.basicDamage = 15f;
        UpdateHealth();
    }

    protected override void UpdateChildren()
    {
        if (comboIndex > 0 && animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 1.00f)
        {
            animator.SetBool("basic1", false);
            animator.SetBool("basic2", false);
            animator.SetBool("basic3", false);
            weapon.SetDamage(0f);
        }

        if (comboIndex > 0 && animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 1.4f)
        {
            animator.SetBool("basic1", false);
            animator.SetBool("basic2", false);
            animator.SetBool("basic3", false);
            weapon.SetDamage(0f);
            comboIndex = 0;
        }
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("Heavy1") && animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.5f)
        {
            animator.SetBool("heavy1", false);
        }
    }

    public override void AttackSound(int variation)
    {
        audioManager.Attack("lightSword" + variation.ToString());
    }

    protected override void FixedUpdateChildren()
    {

    }

    protected override void BasicAttack()
    {
        if (Time.time - attackTime > 0.15f)
        {
            attackTime = Time.time + 0.15f;
            comboIndex++;
        }
        weapon.SetDamage(basicDamage);

        if (comboIndex > 3) comboIndex = 1;

        if (comboIndex == 1)
        {
            animator.SetBool("basic1", true);
            animator.SetBool("basic3", false);
        }
        if (comboIndex == 2 && animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.3f)
        {
            animator.SetBool("basic1", false);
            animator.SetBool("basic2", true);
        }
        if (comboIndex == 3 && animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.3f)
        {
            animator.SetBool("basic2", false);
            animator.SetBool("basic3", true);
        }
    }

    protected override void HeavyAttack()
    {
        if (Time.time - attackTime < 0.25f) return;
        attackTime = Time.time + 0.25f;

        comboIndex = 0;
        weapon.SetDamage(25f);
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

        var bullet = Instantiate(specialProjectile, projectileSpawnpoint.position, transform.rotation);
        Vector3 direction = Camera.main.transform.forward;

        AudioManager.Instance.Attack("projectile");

        bullet.GetComponent<PlayerProjectile>().explosion = explosion;
        bullet.GetComponent<PlayerProjectile>().damage = 40f;
        bullet.GetComponent<Rigidbody>().AddForce(direction * 140000 * Time.deltaTime);
    }
}
