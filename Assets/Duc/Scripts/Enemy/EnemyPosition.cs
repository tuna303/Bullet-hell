using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyAI : MonoBehaviour
{
    
    [SerializeField] private Transform player;

    [SerializeField] private float maxAttackRange = 6f; 
    [SerializeField] private float minAttackRange = 4f; 
    [SerializeField] private float pacingRadius = 1.5f; 
    [SerializeField] private float pacingTimer = 1.2f;  
    private float currentPaceTime;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

   
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        currentPaceTime = pacingTimer;
    }

    void Update()
    {
        if (player == null || !agent.isOnNavMesh) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

       
        if (distanceToPlayer > maxAttackRange)
        {
            agent.SetDestination(player.position);
        }
       
        else if (distanceToPlayer < minAttackRange)
        {
          
            Vector2 directionAway = (transform.position - player.position).normalized;
            
           
            float distanceToReposition = minAttackRange - distanceToPlayer;
            
          
            Vector2 repositionPoint = (Vector2)transform.position + (directionAway * distanceToReposition);
            
            agent.SetDestination(repositionPoint);
        }
        
        else
        {
         
            currentPaceTime -= Time.deltaTime;
            
            if (currentPaceTime <= 0)
            {
          
                Vector2 randomOffset = Random.insideUnitCircle * pacingRadius;
                Vector3 targetPoint = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

              
                NavMeshHit hit;
         
                if (NavMesh.SamplePosition(targetPoint, out hit, pacingRadius, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                }
                
             
                currentPaceTime = Random.Range(0.5f, pacingTimer);
            }
        }
    }
}