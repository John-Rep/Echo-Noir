using UnityEngine;
using UnityEngine.AI;

public class MonsterFollow : MonoBehaviour
{
    [Header("Réglages du joueur")]
    public Transform player;
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
    private bool isAttacking = false;
    private float wanderTimer;
    private Vector3 spawnPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        agent.speed = wanderSpeed; // Par défaut en mode errance
        spawnPosition = transform.position;
        wanderTimer = wanderInterval;

        if (player == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go != null)
                player = go.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Réglage de la vitesse de l'animator selon la situation (optionnel)
        if (animator != null)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }

        if (distance <= detectionRadius)
        {
            agent.speed = chaseSpeed; // 🟢 poursuite rapide
            agent.SetDestination(player.position);
            if (animator != null)
            {
                animator.SetBool("IsChasing", true);
                animator.speed = 1.0f; // vitesse normale pour les anims (optionnel)
            }

            if (distance <= attackRadius && !isAttacking)
            {
                if (animator != null)
                    animator.SetTrigger("Attack");
                isAttacking = true;
                Invoke(nameof(EndAttack), 1.2f);
            }
            else if (distance > attackRadius)
            {
                isAttacking = false;
            }
            wanderTimer = wanderInterval;
        }
        else
        {
            agent.speed = wanderSpeed; // 🟡 balade lente
            if (animator != null)
            {
                animator.SetBool("IsChasing", false);
                animator.speed = 0.7f; // cadence plus lente pour la marche (optionnel)
            }

            wanderTimer += Time.deltaTime;

            if (wanderTimer >= wanderInterval || !agent.hasPath || agent.remainingDistance <= agent.stoppingDistance)
            {
                Vector3 newPos = RandomNavSphere(spawnPosition, wanderRadius);
                agent.SetDestination(newPos);
                wanderTimer = 0f;
            }
            isAttacking = false;
        }
        if (isAttacking && player != null)
        {
            Vector3 lookDirection = player.position - transform.position;
            lookDirection.y = 0;
            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 7f);
            }
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
