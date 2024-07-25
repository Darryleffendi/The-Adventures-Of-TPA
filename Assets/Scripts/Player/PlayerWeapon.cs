using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private float damage = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().DeductHp(damage);
        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
