using UnityEngine;
using UnityEngine.AI;

public class MonsterFollow : MonoBehaviour
{
    [Header("R�glages du suivi")]
    [Tooltip("Transform du joueur. Si laiss� vide, on cherche automatiquement l'objet tagg� 'Player'.")]
    public Transform player;
    [Tooltip("Distance maximale pour commencer � poursuivre le joueur.")]
    public float detectionRadius = 15f;
    [Tooltip("Vitesse de d�placement du monstre.")]
    public float speed = 3.5f;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;

        // Si on n�a pas assign� manuellement le player, on le cherche par tag
        if (player == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go != null)
                player = go.transform;
            else
                Debug.LogWarning("MonsterFollow : aucun objet tagg� 'Player' trouv� dans la sc�ne.");
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
            // Arr�te la poursuite (reste sur place)
            if (agent.hasPath)
                agent.ResetPath();
        }
    }

    // Optionnel : visualiser le rayon de d�tection dans l��diteur
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
