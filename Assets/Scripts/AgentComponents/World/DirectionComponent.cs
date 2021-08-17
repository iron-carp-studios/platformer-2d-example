using IronCarpStudios.AES.Agents;
using IronCarpStudios.AES.Events;
using UnityEngine;

public class DirectionComponent : AgentComponent
{
    private SpriteRenderer sprite;

    public override void OnEnable()
    {
        base.OnEnable();
        sprite = agent.gameObject.GetComponentInChildren<SpriteRenderer>();

        if(sprite == null)
        {
            this.enabled = false;
        }
    }

    public override void OnDisable()
    {
        sprite = null;
        base.OnDisable();
    }

    protected override void Subscribe()
    {
        agent.AddListener(new EventRegistrationData(MovementEvent.OnMove.ToString(), OnMoveEvent));
    }

    protected override void Unsubscribe()
    {
        agent.RemoveListener(new EventRegistrationData(MovementEvent.OnMove.ToString(), OnMoveEvent));
    }

    private void OnMoveEvent(Agent sender, AgentEventArgs args)
    {
        MovementEventArgs parameter = args as MovementEventArgs;

        if(parameter.Speed == 0)
        {
            return;
        }

        if(parameter.Direction < 0)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
    }
}
