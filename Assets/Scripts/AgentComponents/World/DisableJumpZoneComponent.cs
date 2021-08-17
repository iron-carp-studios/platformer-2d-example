using IronCarpStudios.AES.Agents;
using UnityEngine;

public class DisableJumpZoneComponent : AgentComponent
{
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
            agent.Broadcast(MovementEvent.OnDisableJump.ToString(), null);
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        Agent agent = coll.gameObject.GetComponent<Agent>();

        if (agent != null)
        {
            agent.Broadcast(MovementEvent.OnEnableJump.ToString(), null);
        }
    }
}