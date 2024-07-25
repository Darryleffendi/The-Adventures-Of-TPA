using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    [SerializeField]
    protected Transform gruntSpawnpoint, gruntSpawnpoint2, golemSpawnpoint, minotaurSpawnpoint, enemyParent, crystal;
    [SerializeField]
    protected GameObject grunt, golem, minotaur, blood;

    private int gruntSpawntime = 14;
    private int golemSpawntime = 12;
    private int minotaurSpawntime = 16;
    private bool hasSpawned = false;
    private int prevSpawnTime = 0;
    private List<Enemy> enemyList = new List<Enemy>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        SpawnGrunt();
        SpawnGolem();
        SpawnMinotaur();
    }

    void Update()
    {
        if (Time.time - prevSpawnTime > 1f)
            hasSpawned = false;

        if (hasSpawned || (int)Time.time == 0) return;

        else if ((int)Time.time % gruntSpawntime == 0)
            SpawnGrunt();
        else if ((int)Time.time % golemSpawntime == 0)
            SpawnGolem();
        else if ((int)Time.time % minotaurSpawntime == 0)
            SpawnMinotaur();
    }

    private GameObject SpawnEnemy(GameObject enemyPrefab, Transform spawnpoint)
    {
        if (enemyList.Count >= 6) return null;

        hasSpawned = true;
        prevSpawnTime = (int)Time.time;

        Vector3 spawn = new Vector3(spawnpoint.position.x + Random.Range(-10, 10), spawnpoint.position.y, spawnpoint.position.z + Random.Range(-10, 10));
        spawn.y = GetTerrainHeight(spawn);

        GameObject enemy = Instantiate(enemyPrefab, spawn, spawnpoint.rotation, enemyParent);
        enemy.GetComponent<Enemy>().blood = blood;
        enemyList.Add(enemy.GetComponent<Enemy>());
        return enemy;
    }

    private GameObject SpawnEnemy(GameObject enemyPrefab, Transform spawnpoint1, Transform spawnpoint2)
    {
        if (Random.Range(0, 2) == 1)
        {
            return SpawnEnemy(enemyPrefab, spawnpoint1);
        }
        else
        {
            return SpawnEnemy(enemyPrefab, spawnpoint2);
        }
    }

    private void SpawnGrunt()
    {
        SpawnEnemy(grunt, gruntSpawnpoint, gruntSpawnpoint2);
    }

    private void SpawnGolem()
    {
        GameObject enemy = SpawnEnemy(golem, golemSpawnpoint);
        if(enemy != null)
        {
            ((GolemEnemy)enemy.GetComponent<Enemy>()).SetCrystal(crystal);
        }
    }

    private void SpawnMinotaur()
    {
        GameObject enemy = SpawnEnemy(minotaur, minotaurSpawnpoint);
        if (enemy != null)
        {
            ((MinotaurEnemy)enemy.GetComponent<Enemy>()).SetCrystal(crystal);
        }
    }

    public List<Enemy> GetEmemies()
    {
        return enemyList;
    }

    public void Remove(Enemy enemy)
    {
        enemyList.Remove(enemy);
    }

    public int Count()
    {
        return enemyList.Count;
    }
    private float GetTerrainHeight(Vector3 worldPos)
    {
        float maxY = 0;
        foreach (Terrain terrain in Terrain.activeTerrains)
        {
            float curY = terrain.SampleHeight(worldPos);

            if (curY > maxY)
                maxY = curY;
        }
        return maxY;
    }
}
