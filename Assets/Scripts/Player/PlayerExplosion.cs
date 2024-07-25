using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExplosion : MonoBehaviour
{
    public float damage;
    public float spawnTime = 0f;

    private void Start()
    {
        spawnTime = Time.time + 1.2f;
    }

    private void Update()
    {
        gameObject.GetComponent<Audio3D>().Explode();
        if (Time.time - spawnTime > 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().DeductHp(damage);
        }
    }
}
