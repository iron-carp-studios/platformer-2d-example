using IronCarpStudios.AES.Agents;
using UnityEngine;

public class DamageZoneComponent : AgentComponent
{
    public int DamageAmount;
    public SortingLayer InteractionLayer;

    public override void OnEnable()
    {
        base.OnEnable();
        gameObject.layer = LayerMask.NameToLayer(InteractionLayer.ToString());
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        Agent agent = coll.gameObject.GetComponent<Agent>();

        if (agent != null)
        {
            agent.Broadcast(AgentEvent.OnDamageHitpoints.ToString(), new DamageEventArgs(DamageAmount));
        }
    }
}
