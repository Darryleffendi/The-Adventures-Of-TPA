using Cinemachine;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerBot : MonoBehaviour
{

    // A* Attributes
    private Transform target;
    private int targetIndex;
    private Vector3 prevTargetPos = new Vector3(-10000, -10000, -10000);
    private float prevRequestTime = 0f;
    private EnemyManager enemies;
    private Grid grid;
    Vector3[] path;

    private void Start()
    {
        grid = FindObjectOfType<Grid>();
        enemies = EnemyManager.Instance;
    }

    /* ====== Astar Scripts ====== */
    public void BotMovement()
    {
        if (EnemyManager.Instance.Count() <= 0) return;

        GetNearestTarget();

        if (target == null) return;

        // Limit the number of requests to improve performance
        if (prevTargetPos != target.position && Time.time - prevRequestTime > 0.4f)
        {
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
            prevTargetPos = target.position;
            prevRequestTime = Time.time + 0.4f;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 300f * Time.deltaTime);
    }

    protected void GetNearestTarget()
    {
        int minDist = int.MaxValue;

        foreach (Enemy e in enemies.GetEmemies())
        {
            int dist = Pathfinding.Heuristic(grid.NodeFromPos(e.gameObject.transform.position), grid.NodeFromPos(transform.position));
            if (dist < minDist)
            {
                minDist = dist;
                target = e.gameObject.transform;
            }
        }
    }
    public void OnPathFound(Vector3[] newPath, bool pathSuccess)
    {
        if (!pathSuccess) return;
        path = newPath;
        targetIndex = 0;
        StopCoroutine("FollowPath");
        StartCoroutine("FollowPath");
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint;

        if (path.Length > 0)
        {
            currentWaypoint = path[0];
            while (true)
            {
                if (Vector3.Distance(transform.position, currentWaypoint) <= 0.3f)
                {
                    targetIndex++;

                    if (targetIndex >= path.Length)
                    {
                        yield break;
                    }

                    currentWaypoint = path[targetIndex];
                }

                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, (180f / 45) * Time.deltaTime);

                yield return null;
            }
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}

