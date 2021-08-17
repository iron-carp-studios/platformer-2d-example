using IronCarpStudios.AES.Agents;
using IronCarpStudios.AES.Events;
using System.Collections;
using UnityEngine;
public class JumpComponent : AgentComponent
{
    public float JumpPower;
    public float TickRate;

    private Rigidbody2D rigidbodyRef;

    public bool JumpEnabled;
    private bool CanJump;
    private bool IsJumping;
    private bool IsFalling;

    private Vector3 currentPosition;
    private float lastHeight;
    private float currentHeight;
    private float heightOffGround;
    private float heightDifference;

    private RaycastHit2D hitResult;

    public override void OnEnable()
    {
        base.OnEnable();

        rigidbodyRef = GetComponent<Rigidbody2D>();

        if (rigidbodyRef == null)
        {
            this.enabled = false;
        }

        JumpEnabled = true;
        CanJump = true;
        IsJumping = false;
        IsFalling = false;
        StartCoroutine(JumpStateRoutine());
    }

    public override void OnDisable()
    {
        StopAllCoroutines();
        rigidbodyRef = null;
        base.OnDisable();
    }

    protected override void Subscribe()
    {
        agent.AddListener(new EventRegistrationData(MovementEvent.OnJump.ToString(), OnJumpEvent));
        agent.AddListener(new EventRegistrationData(MovementEvent.OnEnableJump.ToString(), OnEnableJump));
        agent.AddListener(new EventRegistrationData(MovementEvent.OnDisableJump.ToString(), OnDisableJump));
    }

    protected override void Unsubscribe()
    {
        agent.RemoveListener(new EventRegistrationData(MovementEvent.OnJump.ToString(), OnJumpEvent));
        agent.RemoveListener(new EventRegistrationData(MovementEvent.OnEnableJump.ToString(), OnEnableJump));
        agent.RemoveListener(new EventRegistrationData(MovementEvent.OnDisableJump.ToString(), OnDisableJump));
    }

    private void OnEnableJump(Agent sender, AgentEventArgs args)
    {
        JumpEnabled = true;
    }

    private void OnDisableJump(Agent sender, AgentEventArgs args)
    {
        JumpEnabled = false;
    }

    private void OnJumpEvent(Agent sender, AgentEventArgs args)
    {

        if (CanJump && JumpEnabled)
        {
            rigidbodyRef.AddForce(rigidbodyRef.velocity + new Vector2(0, JumpPower));
            CanJump = false;
        }
    }

    private IEnumerator JumpStateRoutine()
    {
        while (true)
        {
            TickUpdateJumpStateInternal();

            if (heightOffGround == 0 && heightDifference < .1f)
            {
                if(IsJumping || IsFalling)
                {
                    IsJumping = false;
                    IsFalling = false;
                    CanJump = true;
                    agent.Broadcast(MovementEvent.OnLanded.ToString(), null);
                }
            }
            else if (currentHeight > lastHeight)
            {
                if (!IsJumping)
                {
                    IsJumping = true;
                    IsFalling = false;
                    agent.Broadcast(MovementEvent.OnJumpBegin.ToString(), null);
                }

                Debug.DrawLine(currentPosition, hitResult.point, Color.green);
            }
            else if(currentHeight < lastHeight)
            {
                if (!IsFalling)
                {
                    IsJumping = false;
                    IsFalling = true;
                    CanJump = false;
                    agent.Broadcast(MovementEvent.OnFalling.ToString(), null);
                }

                Debug.DrawLine(currentPosition, hitResult.point, Color.red);
            }

            yield return new WaitForSeconds(TickRate);
        }
    }

    private float GetHeightOffGround()
    {
        if(hitResult.collider == null)
        {
            return -1f;
        }
        var rawHeight = currentPosition.y - hitResult.point.y;

        if (rawHeight <= .1f)
        {
            return 0;
        }

        return rawHeight;
    }

    private bool InAir()
    {
        if(IsFalling || IsJumping && heightOffGround > 0)
        {
            return true;
        }

        return false;
    }

    private void TickUpdateJumpStateInternal()
    {
        currentPosition = agent.transform.position;
        lastHeight = currentHeight;
        currentHeight = currentPosition.y;
        hitResult = Physics2D.Raycast(currentPosition, new Vector2(0, -1), 500, LayerMask.GetMask(SortingLayer.Default.ToString()));
        heightOffGround = GetHeightOffGround();
        heightDifference = Mathf.Abs(currentHeight - lastHeight);
    }
}