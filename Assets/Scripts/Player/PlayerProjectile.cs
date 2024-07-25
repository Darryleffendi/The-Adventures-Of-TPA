using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float damage = 0f;
    public GameObject explosion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") ||
            other.CompareTag("Projectile") || 
            other.CompareTag("PlayerWeapon")) return;

        var explode = Instantiate(explosion, this.transform.position, this.transform.rotation);
        explode.GetComponent<PlayerExplosion>().damage = damage;
        Destroy(gameObject);
    }
}
