using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
   

    public Rigidbody2D rb;
    
    public Vector2 velocity;
    InputAction moveAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
        moveAction = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
  

    void Update()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>() * velocity;
        
        if (moveAction.IsPressed())
        {
             rb.MovePosition(rb.position + moveValue * Time.fixedDeltaTime);
        }
        
       
    }
    
}
