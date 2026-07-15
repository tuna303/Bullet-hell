using UnityEngine;

[System.Serializable]
public class EnemyPistolState : EnemyWeaponBaseState
{
    public float fireRate = 1.5f;
    public float bulletSpeed = 15f;
    public int damage = 1;

    private float fireTimer;

    public override void EnterState(EnemyWeaponStateManager manager)
    {
        if (manager.pistolObject != null)
        {
            manager.pistolObject.SetActive(true);
            manager.currentWeaponSR = manager.pistolObject.GetComponent<SpriteRenderer>();
        }
        
        manager.currentMuzzle = manager.pistolMuzzle;
        fireTimer = fireRate;
    }

    public override void UpdateState(EnemyWeaponStateManager manager)
    {
        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            Shoot(manager);
            fireTimer = fireRate;
        }
    }

    private void Shoot(EnemyWeaponStateManager manager)
    {
        // if (manager.currentMuzzle == null) return;

        // Bullet bullet = BulletPool.Instance.GetBullet();
        // if (bullet == null) return;

        // bullet.transform.position = manager.currentMuzzle.position;
        // bullet.transform.rotation = manager.currentMuzzle.rotation;

        // bullet.Fire(manager.currentMuzzle.right, bulletSpeed, damage);

        if (manager.currentMuzzle == null || manager.targetPlayer == null) return;

        EnemyBullet bullet = EnemyBulletPool.Instance.GetBullet();
        if (bullet == null) return;

        bullet.transform.position = manager.currentMuzzle.position;

        // 1. Tính toán hướng đạn bay chính xác 100% từ nòng súng tới Player
        Vector2 fireDirection = (manager.targetPlayer.position - manager.currentMuzzle.position).normalized;

        // 2. Xoay hình ảnh viên đạn hướng theo góc bắn đó (tránh việc đạn bay ngang nhưng hình lại chĩa dọc)
        float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        // 3. Bắn thẳng theo hướng vừa tính!
        bullet.Fire(fireDirection, bulletSpeed, damage);
    }

    public override void ExitState(EnemyWeaponStateManager manager)
    {
        if (manager.pistolObject != null)
        {
            manager.pistolObject.SetActive(false);
        }
    }
}

// using UnityEngine;

// [System.Serializable]
// public class EnemyPistolState : EnemyWeaponBaseState
// {
//     public float fireRate = 1.5f;
//     public float bulletSpeed = 15f;
//     public int damage = 1;

//     private float fireTimer;

//     public override void EnterState(EnemyWeaponStateManager manager)
//     {
//         if (manager.pistolObject != null)
//         {
//             manager.pistolObject.SetActive(true);
//             manager.currentWeaponSR = manager.pistolObject.GetComponent<SpriteRenderer>();
//         }
        
//         manager.currentMuzzle = manager.pistolMuzzle;
//         fireTimer = fireRate;
//     }

//     public override void UpdateState(EnemyWeaponStateManager manager)
//     {
//         fireTimer -= Time.deltaTime;

//         if (fireTimer <= 0f)
//         {
//             Shoot(manager);
//             fireTimer = fireRate;
//         }
//     }

//     private void Shoot(EnemyWeaponStateManager manager)
//     {
//         if (manager.currentMuzzle == null || manager.targetPlayer == null) return;

//         EnemyBullet bullet = EnemyBulletPool.Instance.GetBullet();
//         if (bullet == null) return;

//         bullet.transform.position = manager.currentMuzzle.position;

//         Vector2 fireDirection = (manager.targetPlayer.position - manager.currentMuzzle.position).normalized;
//         float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
//         bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

//         bullet.Fire(fireDirection, bulletSpeed, damage);
//     }

//     public override void ExitState(EnemyWeaponStateManager manager)
//     {
//         if (manager.pistolObject != null)
//         {
//             manager.pistolObject.SetActive(false);
//         }
//     }
// }