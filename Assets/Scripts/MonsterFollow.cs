using UnityEngine;
using UnityEngine.AI;

public class MonsterFollow : MonoBehaviour
{
    [Header("Réglages du suivi")]
    [Tooltip("Transform du joueur. Si laissé vide, on cherche automatiquement l'objet taggé 'Player'.")]
    public Transform player;
    [Tooltip("Distance maximale pour commencer à poursuivre le joueur.")]
    public float detectionRadius = 15f;
    [Tooltip("Vitesse de déplacement du monstre.")]
    public float speed = 3.5f;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;

        // Si on n’a pas assigné manuellement le player, on le cherche par tag
        if (player == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go != null)
                player = go.transform;
            else
                Debug.LogWarning("MonsterFollow : aucun objet taggé 'Player' trouvé dans la scène.");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRadius)
        {
            // Poursuite
            agent.SetDestination(player.position);
        }
        else
        {
            // Arrête la poursuite (reste sur place)
            if (agent.hasPath)
                agent.ResetPath();
        }
    }

    // Optionnel : visualiser le rayon de détection dans l’éditeur
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
