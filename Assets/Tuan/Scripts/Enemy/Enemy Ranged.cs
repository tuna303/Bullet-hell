using UnityEngine;
using UnityEngine.AI;

public class EnemyRanged : MonoBehaviour
{
    private NavMeshAgent agent;
    public Animator animator;
    
    [Header("Mục tiêu để ngắm")]
    public Transform player; 
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);

        if (player != null)
        {
            if (player.position.x > transform.position.x && transform.localScale.x < 0)
            {
                Flip();
            }
            else if (player.position.x < transform.position.x && transform.localScale.x > 0)
            {
                Flip();
            }
        }
        else
        {
            if (agent.velocity.x > 0 && transform.localScale.x < 0 || 
                agent.velocity.x < 0 && transform.localScale.x > 0)
            {
                Flip();
            }
        }
    }

    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;    
    }
}