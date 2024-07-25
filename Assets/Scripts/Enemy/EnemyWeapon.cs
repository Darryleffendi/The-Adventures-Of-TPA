using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    private float damage = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().DeductHp(damage);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Crystal"))
        {
            other.gameObject.GetComponent<Crystal>().DeductHp(damage);
        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
