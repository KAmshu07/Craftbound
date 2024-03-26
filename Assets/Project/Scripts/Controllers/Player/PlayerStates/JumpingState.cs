//using UnityEngine;

//public class JumpingState : BaseState<PlayerStates>
//{
//    private PlayerStateManager playerManager;
//    private float jumpForce = 10f;
//    private Vector3 velocity;

//    public JumpingState() : base(PlayerStates.Jumping) { }

//    public override void Initialize(StateManager<PlayerStates> stateManager)
//    {
//        playerManager = stateManager as PlayerStateManager;
//    }

//    public override void EnterState()
//    {
//        playerManager.Animator.SetBool("isJumping", true);
//        velocity.y = jumpForce;
//    }

//    public override void UpdateState()
//    {
//        velocity.y += playerManager.Gravity * Time.deltaTime;
//        playerManager.CharacterController.Move(velocity * Time.deltaTime);

//        if (velocity.y < 0)
//        {
//            playerManager.TransitionToState(PlayerStates.Falling);
//        }
//        else if (playerManager.CharacterController.isGrounded)
//        {
//            playerManager.TransitionToState(PlayerStates.Landing);
//        }
//    }

//    public override void ExitState()
//    {
//        playerManager.Animator.SetBool("isJumping", false);
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
