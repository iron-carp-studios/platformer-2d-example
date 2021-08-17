using IronCarpStudios.AES.Agents;
using UnityEngine;

public class PlayerInputComponent : AgentComponent
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            agent.Broadcast(AgentEvent.OnAttack.ToString(), null);
            return;
        }

        if (Input.GetKey(KeyCode.D))
        {
            agent.Broadcast(MovementEvent.OnMove.ToString(), new MovementEventArgs(1, 5));
        }
        else if (Input.GetKey(KeyCode.A))
        {
            agent.Broadcast(MovementEvent.OnMove.ToString(), new MovementEventArgs(-1, 5));
        }
        else
        {
            agent.Broadcast(MovementEvent.OnMove.ToString(), new MovementEventArgs(0, 0));
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            agent.Broadcast(MovementEvent.OnJump.ToString(), new MovementEventArgs(0, 0));
        }


    }
}
