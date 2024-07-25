using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntEnemy : Enemy
{
    private PlayerManager players;
    private Grid grid;

    protected override void StartChildren()
    {
        grid = FindObjectOfType<Grid>();
        players = PlayerManager.Instance;
        weapon = transform.Find("Hips/Axe").gameObject.GetComponent<EnemyWeapon>();
    }

    protected override void Attack()
    {
        if (target == null) return;

        if (Vector3.Distance(transform.position, target.position) > 1.6f)
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        animator.SetBool("basic1", true);
        weapon.SetDamage(15f);
    }

    protected override void UpdateChildren()
    {

    }

    // Get nearest player
    protected override void GetTarget()
    {
        int minDist = int.MaxValue;

        foreach (Player p in players.GetPlayers())
        {
            int dist = Pathfinding.Heuristic(grid.NodeFromPos(p.gameObject.transform.position), grid.NodeFromPos(transform.position));
            if (dist < minDist)
            {
                minDist = dist;
                target = p.gameObject.transform;
            }
        }
    }
}
