using UnityEngine;

public class IdleState : BaseState<PlayerStates>
{
    private Animator animator;

    public IdleState(Animator animator) : base(PlayerStates.Idle)
    {
        this.animator = animator;
    }

    public override void Initialize(StateManager<PlayerStates> stateManager)
    {
        // Initialization logic, if any
    }

    public override void EnterState()
    {
        animator.SetBool("isIdle", true);
    }

    public override void UpdateState()
    {
        Debug.Log("Hello from Idle State Update");
    }

    public override void ExitState()
    {
        animator.SetBool("isIdle", false);
    }

    public override PlayerStates GetNextState()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            return PlayerStates.Walking;
        }
        else if (Input.GetButtonDown("Jump"))
        {
            return PlayerStates.Jumping;
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            return PlayerStates.Crouching;
        }
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void HandleEvent(object eventData) { }
    public override bool ShouldTransition() { return true; }
}
