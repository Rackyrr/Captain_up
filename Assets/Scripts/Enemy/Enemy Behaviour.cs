using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Interfaces;

public class Enemy : MonoBehaviour, IDamageable
{
    public NavMeshAgent navAgent;
    public Transform player;
    public LayerMask groundLayer, playerLayer;
    public float health;
    public float walkPointRange;
    public float timeBetweenAttacks;
    public float sightRange;
    public float attackRange; // Utilisé pour dessiner la portée
    public int damage;
    public Animator animator;
    public ParticleSystem hitEffect;

    private Vector3 walkPoint;
    private bool walkPointSet;
    private bool alreadyAttacked;
    private bool takeDamage;
    private bool isDead;
    private Collider attackZone; // Nouveau composant pour détecter les cibles

    [SerializeField]
    public HealthBarScript healthHUD;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player").transform;
        navAgent = GetComponent<NavMeshAgent>();

        // Ajoute un collider pour la zone d'attaque si inexistant
        attackZone = GetComponent<SphereCollider>();
        if (attackZone == null)
        {
            attackZone = gameObject.AddComponent<SphereCollider>();
            attackZone.isTrigger = true;
            ((SphereCollider)attackZone).radius = attackRange;
        }
    }

    private void Update()
    {
        if (isDead) return;

        if (health <= 0)
        {
            DestroyEnemy();
            return;
        }

        // Vérifie si le joueur est visible et entièrement dans la zone d'attaque
        bool playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        bool playerFullyInAttackRange = PlayerFullyInAttackRange();

        if (!playerInSightRange && !playerFullyInAttackRange)
        {
            Patroling();
        }
        else if (playerInSightRange && !playerFullyInAttackRange)
        {
            ChasePlayer();
        }
        else if (playerFullyInAttackRange)
        {
            AttackPlayer();
        }
    }


    private void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            navAgent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        animator.SetFloat("Velocity", 0.2f);

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        navAgent.SetDestination(player.position);
        animator.SetFloat("Velocity", 0.6f);
        navAgent.isStopped = false;
    }

    private void AttackPlayer()
    {
        navAgent.SetDestination(transform.position); // Arrête l'ennemi

        if (!alreadyAttacked)
        {
            transform.LookAt(player.position);
            alreadyAttacked = true;
            animator.SetBool("Attack", true);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

            if (PlayerIsFullyInAttackZone())
            {
                Debug.Log("Player fully inside attack zone.");
                if (healthHUD != null)
                {
                    healthHUD.TakeDammage(damage);
                }
            }
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        animator.SetBool("Attack", false);
    }

    private bool PlayerIsFullyInAttackZone()
    {
        // Vérifie si la position du joueur est entièrement dans la zone d'attaque
        if (attackZone.bounds.Contains(player.position))
        {
            Debug.Log("Player is completely within attack zone.");
            return true;
        }
        return false;
    }

    public void Damage(int damageAmount)
    {
        if (isDead) return;

        health -= damageAmount;
        Debug.Log($"Enemy took {damageAmount} damage. Remaining health: {health}");
    }

    private void DestroyEnemy()
    {
        if (isDead) return;

        isDead = true;
        navAgent.isStopped = true;
        navAgent.enabled = false;

        StartCoroutine(DestroyEnemyCoroutine());
    }

    private IEnumerator DestroyEnemyCoroutine()
    {
        animator.SetBool("Dead", true);
        yield return new WaitForSeconds(1.8f);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    private bool PlayerFullyInAttackRange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, playerLayer);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                Bounds enemyAttackBounds = new Bounds(transform.position, new Vector3(attackRange * 2, attackRange * 2, attackRange * 2));
                Bounds playerBounds = collider.bounds;

                // Vérifie que toutes les limites du joueur sont contenues dans la zone d'attaque
                if (enemyAttackBounds.Contains(playerBounds.min) && enemyAttackBounds.Contains(playerBounds.max))
                {
                    return true;
                }
            }
        }
        return false;
    }

}
