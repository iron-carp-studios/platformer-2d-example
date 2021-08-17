using IronCarpStudios.AES.Agents;
using IronCarpStudios.AES.Events;
using UnityEngine;

public class HitpointsComponent : AgentComponent
{
    public int CurrentHitpoints;
    public int MaximumHitpoints;

    void Start()
    {
        CurrentHitpoints = MaximumHitpoints;
    }

    protected override void Subscribe()
    {
        agent.AddListener(new EventRegistrationData(AgentEvent.OnDamageHitpoints.ToString(), OnDamage));
        agent.AddListener(new EventRegistrationData(AgentEvent.OnRecoverHitpoints.ToString(), OnRecoverHitpoints));
    }


    protected override void Unsubscribe()
    {
        agent.RemoveListener(new EventRegistrationData(AgentEvent.OnDamageHitpoints.ToString(), OnDamage));
        agent.AddListener(new EventRegistrationData(AgentEvent.OnRecoverHitpoints.ToString(), OnRecoverHitpoints));
    }

    private void OnDamage(Agent sender, AgentEventArgs args)
    {

        var parameters = args as DamageEventArgs;
        var damage = parameters?.DamageAmount ?? 0;

        CurrentHitpoints = Mathf.Clamp(CurrentHitpoints - damage, 0, MaximumHitpoints);
        GlobalEvent.Broadcast(UiEvent.PlayerHealthChanged.ToString(), this.agent, new UiBarChangedEventArgs(CurrentHitpoints, MaximumHitpoints));
    }

    private void OnRecoverHitpoints(Agent sender, AgentEventArgs args)
    {
        HitpointsRecoveryEventArgs parameters = args as HitpointsRecoveryEventArgs;
        var damage = parameters?.Amount ?? 0;

        CurrentHitpoints = Mathf.Clamp(CurrentHitpoints + damage, 0, MaximumHitpoints);
        GlobalEvent.Broadcast(UiEvent.PlayerHealthChanged.ToString(), this.agent, new UiBarChangedEventArgs(CurrentHitpoints, MaximumHitpoints));
    }
}
