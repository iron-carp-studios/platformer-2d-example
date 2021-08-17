using IronCarpStudios.AES.Agents;
using IronCarpStudios.AES.Events;
using UnityEngine;

public class MovementComponent : AgentComponent
{
    public bool MovementEnabled;
    private Rigidbody2D rigidbodyRef;
    private int MoveDirection;
    private float xVelocity;
    private float MoveSpeed;
    private bool IsInAir;
    private bool IsMoving;
    private float aerialAdjustmentStrength;

    public override void OnEnable()
    {
        base.OnEnable();

        rigidbodyRef = GetComponent<Rigidbody2D>();
        aerialAdjustmentStrength = 2.5f;
        IsMoving = false;
        IsInAir = false;
        MovementEnabled = true;

        if (rigidbodyRef == null)
        {
            this.enabled = false;
        }
    }

    public override void OnDisable()
    {

        rigidbodyRef = null;
        base.OnDisable();
    }

    protected override void Subscribe()
    {
        agent.AddListener(new EventRegistrationData(MovementEvent.OnMove.ToString(), OnMoveEvent, EventQueuePriority.Low));
        agent.AddListener(new EventRegistrationData(MovementEvent.OnJumpBegin.ToString(), OnJumpBegin, EventQueuePriority.Low));
        agent.AddListener(new EventRegistrationData(MovementEvent.OnLanded.ToString(), OnLanded, EventQueuePriority.Low));
        agent.AddListener(new EventRegistrationData(MovementEvent.OnDisableMovement.ToString(), OnDisableMovement, EventQueuePriority.Low));
        agent.AddListener(new EventRegistrationData(MovementEvent.OnEnableMovement.ToString(), OnEnableMovement, EventQueuePriority.Low));
    }

    protected override void Unsubscribe()
    {
        agent.RemoveListener(new EventRegistrationData(MovementEvent.OnMove.ToString(), OnMoveEvent, EventQueuePriority.Low));
        agent.RemoveListener(new EventRegistrationData(MovementEvent.OnJumpBegin.ToString(), OnJumpBegin, EventQueuePriority.Low));
        agent.RemoveListener(new EventRegistrationData(MovementEvent.OnLanded.ToString(), OnLanded, EventQueuePriority.Low));
    }

    private void OnDisableMovement(Agent sender, AgentEventArgs args)
    {
        MovementEnabled = false;
    }

    private void OnEnableMovement(Agent sender, AgentEventArgs args)
    {
        MovementEnabled = true;
    }

    private void OnJumpBegin(Agent sender, AgentEventArgs args)
    {
        IsInAir = true;
    }

    private void OnLanded(Agent sender, AgentEventArgs args)
    {
        IsInAir = false;
    }

    private void OnMoveEvent(Agent sender, AgentEventArgs args)
    {
        MovementEventArgs parameter = args as MovementEventArgs;

        if(parameter != null)
        {
            if (MovementEnabled)
            {
                MoveDirection = parameter.Direction;
                MoveSpeed = parameter.Speed;
            }
            else
            {
                MoveSpeed = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        TickMovement();
    }

    private void TickMovement()
    {
        if (IsInAir)
        {
            TickAerialMovement();
        }
        else
        {
            TickGroundMovement();
        }

        TickUpdateAnimationState();

    }

    private void TickAerialMovement()
    {
        xVelocity = CalculateXVelocity();
        if (xVelocity == 0)
        {
            return;
        }

        rigidbodyRef.AddForce(new Vector2(xVelocity * aerialAdjustmentStrength, 0));
        rigidbodyRef.velocity = ClampMaxVelocity();
    }

    private Vector2 ClampMaxVelocity()
    {
        Vector2 currentVelocity = rigidbodyRef.velocity;
        float velocityCap = Mathf.Abs(xVelocity); 
        float maxX = Mathf.Clamp(currentVelocity.x, -velocityCap, velocityCap);
        return new Vector2(maxX, currentVelocity.y);
    }

    private void TickGroundMovement()
    {
        xVelocity = CalculateXVelocity();
        rigidbodyRef.velocity = new Vector2(xVelocity, rigidbodyRef.velocity.y);
    }

    private float CalculateXVelocity()
    {
        return MoveDirection * MoveSpeed * Time.deltaTime * 50;
    }

    private void TickUpdateAnimationState()
    {
        if (MoveSpeed > 0 && !IsMoving)
        {
            IsMoving = true;
            agent.Broadcast(AgentEvent.OnAnimationChanged.ToString(), new AnimationEventArgs(1));
        }
        else if (MoveSpeed == 0 && IsMoving)
        {
            IsMoving = false;
            agent.Broadcast(AgentEvent.OnAnimationChanged.ToString(), new AnimationEventArgs(0)) ;
        }
    }
}