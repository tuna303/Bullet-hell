using UnityEngine;
using UnityEngine.InputSystem;

public class RifleState : WeaponBaseState
{
    public override void EnterState(WeaponStateManager manager)
    {
        manager.rifleObject.SetActive(true);
        manager.currentWeaponSR = manager.rifleObject.GetComponent<SpriteRenderer>();
        manager.currentMuzzle = manager.rifleMuzzle;
        Debug.Log("[STATE] Đã chuyển sang RIFLE");
    }

    public override void UpdateState(WeaponStateManager manager)
    {
        if (Input.GetKey(KeyCode.Space)) 
        {
            Debug.Log("Rifle: bắn nhanh, 1 viên");
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Bullet bullet = BulletPool.Instance.GetBullet();
            if (bullet == null)
                return;

            bullet.transform.position = manager.rifleMuzzle.position;

            bullet.transform.rotation = manager.rifleMuzzle.rotation;

            bullet.Fire(
                manager.rifleMuzzle.right,
                25f,
                20f
            );
        }
    }

    public override void ExitState(WeaponStateManager manager)
    {
        manager.rifleObject.SetActive(false);
    }
}