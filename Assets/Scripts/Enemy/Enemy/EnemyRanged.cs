using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    public float speed = 2.5f;
    public float stoppingDistance = 5f;
    public float retreatDistance = 3f;

    [Header("Wall Avoidance")]
    public LayerMask wallLayer;             // Gan layer cua Wall vao day
    public float wallCheckDistance = 0.5f;  // Khoang cach check truoc mat

    private Transform player;
    private Animator anim;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > stoppingDistance)
        {
            // Di chuyen ve phia player
            Vector2 dirToPlayer = ((Vector2)player.position - (Vector2)transform.position).normalized;
            if (!IsWallInDirection(dirToPlayer))
            {
                Vector2 newPos = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                MoveToPosition(newPos);
            }
            if (anim != null) anim.SetBool("isRunning", true);
        }
        else if (distance < retreatDistance)
        {
            // Lui ra xa player
            Vector2 retreatDir = ((Vector2)transform.position - (Vector2)player.position).normalized;
            if (!IsWallInDirection(retreatDir))
            {
                Vector2 newPos = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
                MoveToPosition(newPos);
            }
            if (anim != null) anim.SetBool("isRunning", true);
        }
        else
        {
            if (anim != null) anim.SetBool("isRunning", false);
        }
        FlipTowardsPlayer();
    }

    /// <summary>
    /// Di chuyen enemy bang Rigidbody2D.MovePosition de ton trong physics.
    /// </summary>
    private void MoveToPosition(Vector2 targetPos)
    {
        if (rb != null)
        {
            rb.MovePosition(targetPos);
        }
        else
        {
            transform.position = targetPos;
        }
    }

    /// <summary>
    /// Kiem tra xem co tuong (tag "Wall") o huong di chuyen khong.
    /// </summary>
    private bool IsWallInDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, wallCheckDistance, wallLayer);
        if (hit.collider != null && hit.collider.CompareTag("Wall"))
        {
            return true;
        }
        return false;
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
