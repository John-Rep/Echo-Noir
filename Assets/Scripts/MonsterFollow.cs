using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterFollow : MonoBehaviour
{
    [Header("Réglages du joueur")]
    // public Transform player;
    public float detectionRadius = 15f;
    public float attackRadius = 2f;

    [Header("Vitesse")]
    public float chaseSpeed = 3.5f;      // Vitesse rapide en poursuite
    public float wanderSpeed = 1.5f;     // Vitesse lente en errance

    [Header("Errance aléatoire")]
    public float wanderRadius = 10f;
    public float wanderInterval = 5f;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isAttacking = false, isChasing = false;
    private float wanderTimer, chaseCooldown;
    private Vector3 spawnPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        agent.speed = wanderSpeed; // Par défaut en mode errance
        spawnPosition = transform.position;
        wanderTimer = wanderInterval;

        /* if (player == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go != null)
                player = go.transform;
        } */
    }

    void Update()
    {
        // if (player == null) return;

        // float distance = Vector3.Distance(transform.position, player.position);

        // Réglage de la vitesse de l'animator selon la situation (optionnel)
        if (animator != null)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }

        
        if (isChasing && agent.remainingDistance < agent.stoppingDistance)
        {
            EndChase();
        }
        if (chaseCooldown > 0)
        {
            chaseCooldown -= Time.deltaTime;
        }
        if (!isChasing && chaseCooldown <= 0)
        {
            agent.speed = wanderSpeed;

            wanderTimer += Time.deltaTime;

            if (wanderTimer >= wanderInterval || !agent.hasPath || agent.remainingDistance <= agent.stoppingDistance)
            {
                Vector3 newPos = RandomNavSphere(spawnPosition, wanderRadius);
                agent.SetDestination(newPos);
                wanderTimer = 0f;
            }
        }
        /* if (isAttacking && player != null)
        {
            Vector3 lookDirection = player.position - transform.position;
            lookDirection.y = 0;
            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 7f);
            }
        } */
    }

    public void NewChase(Vector3 destination)
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(destination);
        if (animator != null)
        {
            animator.SetBool("IsChasing", true);
            animator.speed = 1f; // cadence normale pour poursuite
        }
        isChasing = true;
    }

    void EndChase()
    {
        isChasing = false;
        chaseCooldown = 2f;
        if (animator != null)
        {
            animator.SetBool("IsChasing", false);
            animator.speed = 0.7f; // cadence plus lente pour la marche
        }
    }

    void EndAttack()
    {
        isAttacking = false;
    }

    Vector3 RandomNavSphere(Vector3 origin, float dist)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist + origin;
        randDirection.y = origin.y;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, NavMesh.AllAreas);
        return navHit.position;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Application.isPlaying ? spawnPosition : transform.position, wanderRadius);
    }
}
