using UnityEngine;

public class EnemyChaser : MonoBehaviour
{
    private Animator anim;
    public float speed = 2.5f;
    public float chaseRange = 7f;
    public float attackRange = 1.2f;
    private Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= chaseRange && distanceToPlayer > attackRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            if (anim != null) anim.SetBool("isRunning", true);
        }
        else if (distanceToPlayer <= attackRange)
        {
            transform.position = this.transform.position;
            if (anim != null) anim.SetBool("isRunning", false);
        }
        else
        {
            
            if (anim != null) anim.SetBool("isRunning", false);
        }
        FlipTowardsPlayer();
    }
    private void FlipTowardsPlayer()
    {
        Vector3 currentScale = transform.localScale;
        if (player.position.x > transform.position.x)
        {
            currentScale.x = Mathf.Abs(currentScale.x); 
        }
        else if (player.position.x < transform.position.x)
        {
            currentScale.x = -Mathf.Abs(currentScale.x);
        }
        transform.localScale = currentScale;
    }

}