using IronCarpStudios.AES.Agents;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitpointsRecoveryZoneComponent : AgentComponent
{
    public int RecoveryAmount;
    public float RecoveryRate;
    public SortingLayer InteractionLayer;

    private Dictionary<Agent, float> agentTimers;

    public override void OnEnable()
    {
        base.OnEnable();
        gameObject.layer = LayerMask.NameToLayer(InteractionLayer.ToString());
        agentTimers = new Dictionary<Agent, float>();
    }

    public override void OnDisable()
    {
        agentTimers = null;
        base.OnDisable();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        Agent agent = coll.gameObject.GetComponent<Agent>();
        TryAddAgent(agent);
    }

    public void TryAddAgent(Agent agent)
    {
        if (agent == null)
        {
            return;
        }

        if (!agentTimers.ContainsKey(agent))
        {
            agentTimers.Add(agent, 0);
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        Agent agent = coll.gameObject.GetComponent<Agent>();
        TryRemoveAgent(agent);
    }

    public void TryRemoveAgent(Agent agent)
    {
        if (agent == null)
        {
            return;
        }

        if (agentTimers.ContainsKey(agent))
        {
            agentTimers.Remove(agent);
        }
    }

    private void Update()
    {
        if (agentTimers.Count == 0)
        {
            return;
        }

        TickRecoveryTimer();
    }

    private void TickRecoveryTimer()
    {
        List<Agent> keys = agentTimers.Keys.ToList();
        foreach (Agent agent in keys)
        {
            var timeRemaining = agentTimers[agent];
            var newTimeRemaining = timeRemaining - Time.deltaTime;

            if (newTimeRemaining <= 0)
            {
                agent.Broadcast(AgentEvent.OnRecoverHitpoints.ToString(), new HitpointsRecoveryEventArgs(RecoveryAmount));
                newTimeRemaining = RecoveryRate;
            }

            agentTimers[agent] = newTimeRemaining;
        }
    }
}
