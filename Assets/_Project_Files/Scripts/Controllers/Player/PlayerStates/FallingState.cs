//using UnityEngine;

//public class FallingState : BaseState<PlayerStates>
//{
//    private PlayerStateManager playerManager;
//    private Vector3 velocity;

//    public FallingState() : base(PlayerStates.Falling) { }

//    public override void Initialize(StateManager<PlayerStates> stateManager)
//    {
//        playerManager = stateManager as PlayerStateManager;
//    }

//    public override void EnterState()
//    {
//        playerManager.Animator.SetBool("isFalling", true);
//        velocity.y = playerManager.CharacterController.velocity.y;
//    }

//    public override void UpdateState()
//    {
//        velocity.y += playerManager.Gravity * Time.deltaTime;
//        playerManager.CharacterController.Move(velocity * Time.deltaTime);

//        if (playerManager.CharacterController.isGrounded)
//        {
//            playerManager.TransitionToState(PlayerStates.Landing);
//        }
//    }

//    public override void ExitState()
//    {
//        playerManager.Animator.SetBool("isFalling", false);
//        velocity.y = 0;
//    }

//    public override PlayerStates GetNextState()
//    {
//        return StateKey; 
//    }

//    public override void OnTriggerEnter(Collider other) { }
//    public override void OnTriggerStay(Collider other) { }
//    public override void OnTriggerExit(Collider other) { }
//    public override void HandleEvent(object eventData) { }
//    public override bool ShouldTransition() { return true; } 
//}
