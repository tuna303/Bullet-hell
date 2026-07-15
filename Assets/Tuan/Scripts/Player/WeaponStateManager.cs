using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponStateManager : MonoBehaviour
{
    WeaponBaseState currentState;    
    [SerializeField] public PistolState pistolState = new PistolState();
    [SerializeField] public ShotgunState shotgunState = new ShotgunState();
    [SerializeField] public RifleState rifleState = new RifleState();

    [Header("Gắn 3 GameObject súng từ Hierarchy vào đây")]
    public GameObject pistolObject;
    public GameObject shotgunObject;
    public GameObject rifleObject;

    public SpriteRenderer currentWeaponSR;
    
    public Transform currentMuzzle;
    public Transform pistolMuzzle;
    public Transform shotgunMuzzle;
    public Transform rifleMuzzle;
    void Start()
    {
        pistolObject.SetActive(false);
        shotgunObject.SetActive(false);
        rifleObject.SetActive(false);

        currentState = pistolState;
        currentState.EnterState(this); 
    }

    void Update()
    {
        currentState.UpdateState(this);

        if (Keyboard.current != null)
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame && currentState != pistolState) 
                SwitchState(pistolState);
            else if (Keyboard.current.digit2Key.wasPressedThisFrame && currentState != shotgunState) 
                SwitchState(shotgunState);
            else if (Keyboard.current.digit3Key.wasPressedThisFrame && currentState != rifleState) 
                SwitchState(rifleState);
        }
        
        HandleGunRotation();
    }

    private void HandleGunRotation()
    {
        if (Mouse.current == null || Camera.main == null) return;

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        Vector2 lookDirection = mouseWorldPosition - (Vector2)transform.position;

        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        HandleFlip(angle);
    }

    private void HandleFlip(float angle)
    {
        bool flip = angle > 90 || angle < -90;

        if (currentWeaponSR != null)
        {
            currentWeaponSR.flipY = flip;
        }
    }

    public void SwitchState(WeaponBaseState state)
    {
        currentState.ExitState(this); 
        currentState = state;         
        currentState.EnterState(this); 
    }
}