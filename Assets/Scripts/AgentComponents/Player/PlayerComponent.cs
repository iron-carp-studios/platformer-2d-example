using IronCarpStudios.AES.Agents;
using UnityEngine;

public class PlayerComponent : AgentComponent
{
    public override void OnEnable()
    {
        base.OnEnable();
        gameObject.layer = LayerMask.NameToLayer(SortingLayer.PlayerAgents.ToString());
    }
}