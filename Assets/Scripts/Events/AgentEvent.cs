public enum AgentEvent
{
    OnIdle,
    OnAttack,
    OnAttackEnd,
    OnAnimationChanged,
    OnUnlockAnimation,
    OnDamageHitpoints,
    OnRecoverHitpoints,
    OnHitpointsChanged
}

public enum MovementEvent
{
    OnMove,
    OnDisableMovement,
    OnEnableMovement,
    OnLockDirection,
    OnUnlockDirection,
    OnDisableJump,
    OnEnableJump,
    OnJump,
    OnJumpBegin,
    OnFalling,
    OnLanded,
}

public enum UiEvent
{
    PlayerHealthChanged
}