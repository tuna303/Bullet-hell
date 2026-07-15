using UnityEngine;
using UnityEngine.AI;
public class Ranged : MonoBehaviour
{
    
    private NavMeshAgent agent;
    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
      
       
         if (agent.velocity.x > 0 && transform.localScale.x < 0 || agent.velocity.x < 0 && transform.localScale.x > 0)
        {
            Flip();
            
        }

         animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }
    void FixedUpdate()
    {
        
       
    }
    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;    
    }
}
