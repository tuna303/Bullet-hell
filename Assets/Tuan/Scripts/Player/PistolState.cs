using UnityEngine;
using UnityEngine.InputSystem;

public class PistolState : WeaponBaseState
{
    public override void EnterState(WeaponStateManager manager)
    {
        manager.pistolObject.SetActive(true);
        manager.currentWeaponSR = manager.pistolObject.GetComponent<SpriteRenderer>();
        manager.currentMuzzle = manager.pistolMuzzle;
        Debug.Log("[STATE] Đã chuyển sang PISTOL");
    }

    public override void UpdateState(WeaponStateManager manager)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Pistol: Bắn chậm, 1 viên");
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Bullet bullet = BulletPool.Instance.GetBullet();

            if (bullet == null)
                return;

            Debug.Log("Shoot");
            bullet.transform.position = manager.pistolMuzzle.position;

            bullet.transform.rotation = manager.pistolMuzzle.rotation;

            bullet.Fire(
                manager.pistolMuzzle.right,
                15f,
                20f
            );
        }
    }

    public override void ExitState(WeaponStateManager manager)
    {
        manager.pistolObject.SetActive(false);
    }
}