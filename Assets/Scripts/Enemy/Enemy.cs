using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    protected float speed = 2f;
    Vector3[] path = new Vector3[0];

    protected Transform target;
    protected int targetIndex;
    protected Vector3 prevTargetPos = new Vector3(-10000,-10000,-10000);
    private Vector3 prevPos = new Vector3(-10000, -10000, -10000);
    protected float prevRequestTime = 0;
    private float animateTime = 0;
    protected bool findCrystal = false;
    protected Animator animator;
    protected Audio3D audio3D;
    protected Transform healthBar;
    protected EnemyWeapon weapon;
    public GameObject blood;

    // Child Attributes
    protected float hp = 380f;
    protected float maxHp = 380f;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        audio3D = gameObject.GetComponent<Audio3D>();
        healthBar = transform.Find("Healthbar/Health");
        StartChildren();
    }
    protected abstract void GetTarget();
    protected abstract void StartChildren();
    protected abstract void UpdateChildren();
    protected abstract void Attack();

    private void Update()
    {
        GetTarget();
        AnimateMovement();
        UpdateChildren();

        if (!findCrystal && path.Length <= 0)
            Attack();

        if (target == null) return;

        // Limit the number of requests to improve performance
        if (prevTargetPos != target.position && Time.time - prevRequestTime > 0.3f)
        {
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound, findCrystal);
            prevTargetPos = target.position;
            prevRequestTime = Time.time + 0.3f;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 300f * Time.deltaTime);

    }

    public void DeductHp(float damage)
    {
        if (damage <= 0) return;

        audio3D.Gore();
        Instantiate(blood, transform.position, transform.rotation);
        hp -= damage;
        UpdateHealth();
        
        if (hp <= 0)
        {
            QuestScene.Instance.EnemyKill();
            EnemyManager.Instance.Remove(this);
            StopAllCoroutines();
            Destroy(this.gameObject);
        }
    }

    private void UpdateHealth()
    {
        healthBar.localScale = new Vector3((hp / maxHp) * 0.218f, 0.029f, 1f);
        if (hp > maxHp * 2 / 3)
        {
            healthBar.GetComponent<Image>().color = Color.green;
        }
        else if (hp > maxHp * 1 / 3)
        {
            healthBar.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            healthBar.GetComponent<Image>().color = Color.red;
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
        
        if(path.Length > 0)
        {
            animator.SetBool("basic1", false);
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
            
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);

                yield return null;
            }
        }
    }
     
    private void AnimateMovement()
    {
        if (Time.time - animateTime > 0.3)
        {
            animateTime = Time.time + 0.3f;

            if (Vector3.Distance(prevPos, transform.position) > 0.15)
            {
                prevPos = transform.position;
                animator.SetBool("isRunning", true);
            }
            else
            {
                Attack();
            }
        }

        if (animator.GetBool("isRunning"))
        {
            audio3D.Walk(speed);
        }

    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.red;
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
