using UnityEngine;
using System.Collections;
public class NewMonoBehaviourScript : MonoBehaviour
{
    public float speed = 3f;
    public Transform pointA;
    public Transform pointB;
    private Transform currentTarget;
    private Animator anim;
    private bool isStunned = false;
    private Coroutine currentHitRoutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTarget = pointB;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStunned) return;
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);
        float distance = Vector3.Distance(transform.position, currentTarget.position);
        if (distance <= 0.1f)
        {
            currentTarget = (currentTarget == pointB) ? pointA : pointB;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
           
            
            if (currentHitRoutine != null)
            {
                StopCoroutine(currentHitRoutine);
            }
          
          currentHitRoutine = StartCoroutine(HitStunRoutine());
        }
    }
    private IEnumerator HitStunRoutine()
    {
        isStunned = true;
        anim.Play("Hit", -1, 0f);
        yield return new WaitForSeconds(1f);
       
        isStunned = false;

        anim.Play("Idle");
       
    }
}
