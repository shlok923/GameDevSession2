using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 100f;
    [SerializeField] float attackSpeed = 1f;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float moveSpeed = 1f;
    private PlayerController player;

    private Animator animator;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Bullet.OnEnemyHit += HandleEnemyHit;
        animator = GetComponent<Animator>();
        player = FindFirstObjectByType<PlayerController>();
    }

    private IEnumerator PauseMovement(float seconds)
    {
        agent.isStopped = true;

        yield return new WaitForSeconds(seconds);

        agent.isStopped = false;
    }

    private void HandleEnemyHit(GameObject zombie)
    {
        if (zombie == gameObject)
        {
            animator.SetTrigger("hit");
            PauseMovement(1.5f);
            health -= 10;

            if (health <= 0)
            {
                Debug.Log("DED");
                animator.SetTrigger("die");
                PauseMovement(3f);
                Die();
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
        HandleAnimations();
        AttackPlayer();
    }

    private void FollowPlayer()
    {
        if (player != null)
        {
            agent.SetDestination(player.gameObject.transform.position);
            // Debug.Log("destination: " + player.transform.position);
        }
        else
        {
            Debug.LogWarning("Player is not assigned or found!");
        }
    }

    private void AttackPlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
        {
            animator.SetTrigger("attack");
            PauseMovement(1f);
        }


    }

    private void HandleAnimations()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        bool isMoving = agent.remainingDistance > agent.stoppingDistance;
        animator.SetBool("isWalking", isMoving);
    }

    private void OnDestroy()
    {
        Bullet.OnEnemyHit -= HandleEnemyHit;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

}
