using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public Transform player;
    public float hearingDistance = 15f;

    private NavMeshAgent agent;
    private PlayerMovement playerMovement;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < hearingDistance && playerMovement.noiseLevel > 0f)
        {
            agent.SetDestination(player.position);
        }
    }
}
