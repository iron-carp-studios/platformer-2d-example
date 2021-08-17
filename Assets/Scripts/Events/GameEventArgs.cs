using IronCarpStudios.AES.Agents;
using IronCarpStudios.AES.Events;
using UnityEngine;

public class MovementEventArgs : AgentEventArgs
{
    public int Direction { get; set; }
    public float Speed { get; set; }
    public bool LockVelocity { get; set; }

    public MovementEventArgs(int direction, float speed)
    {
        Direction = direction;
        Speed = Mathf.Clamp(speed, 0, 100);
        LockVelocity = false;
    }
}

public class AnimationEventArgs : AgentEventArgs
{
    public int AnimationId { get; set; }
    public bool LockAnimation { get; set; }
    public bool ForceAnimation { get; set; }

    public AnimationEventArgs(int animationId, bool lockAnimation = false, bool forceAnimation = false)
    {
        AnimationId = animationId;
        LockAnimation = lockAnimation;
        ForceAnimation = forceAnimation;
    }
}

public class HitpointsRecoveryEventArgs: AgentEventArgs
{
    public int Amount { get; set; }

    public HitpointsRecoveryEventArgs(int amount)
    {
        Amount = amount;
    }
}

public class DamageEventArgs : AgentEventArgs 
{
    public int DamageAmount { get; set; }
    public Agent Sender { get; set; }

    public DamageEventArgs(int damageAmount)
    {
        DamageAmount = damageAmount;
    }
}

public class UiBarChangedEventArgs : AgentEventArgs
{
    public int CurrentValue { get; set; }
    public int MaximumValue { get; set; }

    public UiBarChangedEventArgs(int currentValue, int maximumValue)
    {
        CurrentValue = currentValue;
        MaximumValue = maximumValue;
    }
}
