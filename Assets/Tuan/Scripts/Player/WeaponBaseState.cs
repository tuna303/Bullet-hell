using UnityEngine;

public abstract class WeaponBaseState
{
    public abstract void EnterState(WeaponStateManager manager);

    public abstract void UpdateState(WeaponStateManager manager);

    public abstract void ExitState(WeaponStateManager manager);
}
