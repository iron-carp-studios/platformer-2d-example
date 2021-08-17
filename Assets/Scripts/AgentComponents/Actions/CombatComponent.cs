using IronCarpStudios.AES.Agents;
using IronCarpStudios.AES.Events;
using System.Collections;
using UnityEngine;

public class CombatComponent : AgentComponent
{
    public int CombatStep;
    public bool IsAttacking;
    public bool CombatEnabled;
    public bool InAir;
    public float ComboTimeout;

    public override void OnEnable()
    {
        base.OnEnable();
        CombatEnabled = true;
        InAir = false;
    }

    protected override void Subscribe()
    {
        agent.AddListener(new EventRegistrationData(AgentEvent.OnAttackEnd.ToString(), OnAttackEnd));
        agent.AddListener(new EventRegistrationData(AgentEvent.OnAttack.ToString(), OnAttack));
        agent.AddListener(new EventRegistrationData(MovementEvent.OnJumpBegin.ToString(), OnInAir));
        agent.AddListener(new EventRegistrationData(MovementEvent.OnFalling.ToString(), OnInAir));
        agent.AddListener(new EventRegistrationData(MovementEvent.OnLanded.ToString(), OnLanded));
    }

    protected void OnInAir(Agent sender, AgentEventArgs args)
    {
        InAir = true;
    }

    protected void OnLanded(Agent sender, AgentEventArgs args)
    {
        InAir = false;
    }

    protected override void Unsubscribe()
    {
        agent.RemoveListener(new EventRegistrationData(AgentEvent.OnAttackEnd.ToString(), OnAttackEnd));
        agent.AddListener(new EventRegistrationData(AgentEvent.OnAttack.ToString(), OnAttack));
    }

    private void OnAttackEnd(Agent sender, AgentEventArgs args)
    {
        IsAttacking = false;
        StartCoroutine(CombatTimeoutRoutine());
        agent.Broadcast(AgentEvent.OnAnimationChanged.ToString(), new AnimationEventArgs(0));
    }

    private void OnAttack(Agent sender, AgentEventArgs args)
    {
        if (!IsAttacking && CanAttack())
        {
            CombatStep += 1;
            IsAttacking = true;
            agent.Broadcast(AgentEvent.OnAnimationChanged.ToString(), new AnimationEventArgs(100, true));
            agent.Broadcast(AgentEvent.OnAttack.ToString(), new DamageEventArgs(0));
        }
    }

    private bool CanAttack()
    {
        if (CombatEnabled && !InAir)
        {
            return true;
        }

        return false;
    }

    private IEnumerator CombatTimeoutRoutine()
    {
        float time = ComboTimeout;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        CombatStep = 0;
    }
}