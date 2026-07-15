using UnityEngine;

public abstract class EnemyWeaponBaseState
{
    public abstract void EnterState(EnemyWeaponStateManager manager);
    public abstract void UpdateState(EnemyWeaponStateManager manager);
    public abstract void ExitState(EnemyWeaponStateManager manager);
}