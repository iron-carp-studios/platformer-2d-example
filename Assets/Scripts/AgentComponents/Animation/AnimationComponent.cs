using IronCarpStudios.AES.Agents;
using IronCarpStudios.AES.Events;
using UnityEngine;

public class AnimationComponent : AgentComponent
{
    private Animator animator;
    public bool LockAnimation;

    public override void OnEnable()
    {
        base.OnEnable();
        animator = agent.gameObject.GetComponent<Animator>();
        LockAnimation = false;

        if (animator == null)
        {
            this.enabled = false;
        }
    }

    public override void OnDisable()
    {

        animator = null;
        base.OnDisable();
    }

    protected override void Subscribe()
    {
        agent.AddListener(new EventRegistrationData(AgentEvent.OnAnimationChanged.ToString(), SetAnimationState, EventQueuePriority.Low));
        agent.AddListener(new EventRegistrationData(MovementEvent.OnJumpBegin.ToString(), OnJumpBegin, EventQueuePriority.Low));
        agent.AddListener(new EventRegistrationData(MovementEvent.OnFalling.ToString(), OnFalling, EventQueuePriority.Low));
        agent.AddListener(new EventRegistrationData(MovementEvent.OnLanded.ToString(), OnLanded, EventQueuePriority.Low));
        agent.AddListener(new EventRegistrationData(AgentEvent.OnUnlockAnimation.ToString(), UnlockAnimation, EventQueuePriority.Low));
    }

    private void OnFalling(Agent sender, AgentEventArgs args)
    {
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsFalling", true);
    }

    private void OnLanded(Agent sender, AgentEventArgs args)
    {
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsFalling", false);
    }

    private void OnJumpBegin(Agent sender, AgentEventArgs args)
    {
        animator.SetBool("IsJumping", true);
        animator.SetBool("IsFalling", false);
    }

    protected override void Unsubscribe()
    {
        agent.RemoveListener(new EventRegistrationData(AgentEvent.OnAnimationChanged.ToString(), SetAnimationState));
        agent.RemoveListener(new EventRegistrationData(AgentEvent.OnAnimationChanged.ToString(), SetAnimationState));
        agent.RemoveListener(new EventRegistrationData(AgentEvent.OnAnimationChanged.ToString(), SetAnimationState));
        agent.RemoveListener(new EventRegistrationData(AgentEvent.OnUnlockAnimation.ToString(), UnlockAnimation));
    }

    private void SetAnimationState(Agent sender, AgentEventArgs args)
    {

        AnimationEventArgs parameter = args as AnimationEventArgs;

        if (parameter.ForceAnimation)
        {
            LockAnimation = parameter.LockAnimation;
            animator.SetInteger("AnimationId", parameter.AnimationId);
        }
        else if (!LockAnimation)
        {
            LockAnimation = parameter.LockAnimation;
            animator.SetInteger("AnimationId", parameter.AnimationId);
        }
    }

    private void UnlockAnimation(Agent sender, AgentEventArgs args)
    {
        LockAnimation = false;
    }
}
