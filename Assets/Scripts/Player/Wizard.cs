using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cinemachine;

public class Wizard : Player
{
    [Header("FPS Attributes")]
    [SerializeField]
    public CinemachineVirtualCamera fpsCam;
    [SerializeField]
    public GameObject crosshair;

    [Header("Projectile")]
    [SerializeField]
    protected GameObject basicOrb;
    [SerializeField]
    protected GameObject specialExplosion;

    private float attackTime = float.PositiveInfinity;
    private int comboIndex = 0;

    // Fps Attributes
    private float fpsXrotate = 0;
    private float fpsYrotate = 0;
    private float fpsTime = 0;

    protected override void StartChildren()
    {
        this.walkingSpeed = 65f;
        this.runningSpeed = 125f;
        this.jumpSpeed = 0f;
        this.maxHp = 250f;
        this.hp = maxHp;
        this.basicDamage = 20f;
        this.UpdateHealth();
    }

    public override void AttackSound(int variation)
    {

    }

    public override void SetPlayerActive(bool x)
    {
        base.SetPlayerActive(x);
        SetFps(false);
        crosshair.SetActive(false);
    }

    protected override void UpdateChildren()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Basic1") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
        {
            animator.SetBool("basic1", false);
            comboIndex = 0;
        }

        Vector3 direction = Camera.main.transform.forward;
        
        if(!isFps) direction.y = 0;

        if (Time.time >= attackTime)
        {
            var bullet = Instantiate(basicOrb, projectileSpawnpoint.position, transform.rotation);

            bullet.GetComponent<PlayerProjectile>().explosion = explosion;
            bullet.GetComponent<PlayerProjectile>().damage = 35f;
            bullet.GetComponent<Rigidbody>().AddForce(direction * 380000 * Time.deltaTime);

            AudioManager.Instance.Attack("projectile");

            attackTime = float.PositiveInfinity;
        }
        else if ((animator.GetCurrentAnimatorStateInfo(0).IsName("Basic1") || comboIndex == 1) && !isFps)
        { 
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            Quaternion surfaceRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, toRotation.eulerAngles.y, transform.rotation.eulerAngles.z);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, surfaceRotation, rotationSpeed * Time.deltaTime);
        }
    }

    protected override void FixedUpdateChildren()
    {
        if (isFps)
        {
            FpsMovement();
        }
    }

    protected override void BasicAttack()
    {
        if (comboIndex == 0)
        {
            attackTime = Time.time + 0.55f;
            animator.SetBool("basic1", true);
            comboIndex = 1;
        }
    }

    protected override void HeavyAttack()
    {
        if (Time.time < fpsTime) return;

        if (!isFps)
        {
            isFps = true;
            fpsCam.m_Priority = 5;
            cinemachineCamera.m_Priority = 0;
            fpsTime = Time.time + 0.3f;
            crosshair.SetActive(true);
            return;
        }
        
        isFps = false;
        cinemachineCamera.m_Priority = 10;
        fpsCam.m_Priority = 0;
        fpsTime = Time.time + 0.3f;
        crosshair.SetActive(false);
    }

    protected override void SpecialAttack()
    {
        if (!DeductMana(5f))
        {
            return;
        }

        var bullet = Instantiate(specialProjectile, projectileSpawnpoint.position, transform.rotation);
        Vector3 direction = Camera.main.transform.forward;

        bullet.GetComponent<PlayerProjectile>().explosion = specialExplosion;
        bullet.GetComponent<PlayerProjectile>().damage = 40f;
        bullet.GetComponent<Rigidbody>().AddForce(direction * 140000 * Time.deltaTime);
        AudioManager.Instance.Attack("projectile");
    }

    private void FpsMovement()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * 100;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * 100;

        fpsYrotate += mouseX;
        fpsXrotate = Mathf.Clamp(fpsXrotate - mouseY, -90f, 90f);

        transform.rotation = Quaternion.Euler(0, fpsYrotate, 0);
        fpsCam.transform.rotation = Quaternion.Euler(fpsXrotate, fpsYrotate, 0);
    }
}
