using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    public float spawnTime = 0f;

    private void Awake()
    {
        spawnTime = Time.time + 0.9f;
    }

    private void Update()
    {
        if (Time.time - spawnTime > 0)
        {
            Destroy(gameObject);
        }
    }
}
