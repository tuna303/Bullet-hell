using UnityEngine;
using UnityEngine.InputSystem;

public class ShotgunState : WeaponBaseState
{
    public override void EnterState(WeaponStateManager manager)
    {
        manager.shotgunObject.SetActive(true);
        manager.currentWeaponSR = manager.shotgunObject.GetComponent<SpriteRenderer>();
        manager.currentMuzzle = manager.shotgunMuzzle;
        Debug.Log("[STATE] Đã chuyển sang SHOTGUN");
    }

    public override void UpdateState(WeaponStateManager manager)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Shotgun: Bắn 3 viên");
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            for (int i = -1; i <= 1; i++)
            {
                Bullet bullet = BulletPool.Instance.GetBullet();
                if (bullet == null)
                    continue; 

                bullet.transform.position = manager.shotgunMuzzle.position;

                Quaternion spread =
                    Quaternion.Euler(0,0,i * 10);

                Vector2 dir =
                    spread * manager.shotgunMuzzle.right;

                bullet.Fire(dir, 15f, 8f);
            }
        }
    }

    public override void ExitState(WeaponStateManager manager)
    {
        manager.shotgunObject.SetActive(false); 
    }
}