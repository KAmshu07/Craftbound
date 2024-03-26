//using UnityEngine;

//public class DeadState : BaseState<PlayerStates>
//{
//    private PlayerStateManager playerManager;

//    public DeadState() : base(PlayerStates.Dead) { }

//    public override void Initialize(StateManager<PlayerStates> stateManager)
//    {
//        playerManager = stateManager as PlayerStateManager;
//    }

//    public override void EnterState()
//    {
//        playerManager.Animator.SetBool("isDead", true);
//        // Disable player control and other components as needed
//        playerManager.CharacterController.enabled = false;
//        // Additional logic for handling death, e.g., showing game over screen
//    }

//    public override void UpdateState()
//    {
//        // The player is dead, so there's no need to update anything in this state
//    }

//    public override void ExitState()
//    {
//        // This state should not be exited once entered, but you can add logic here if needed
//    }

//    public override PlayerStates GetNextState()
//    {
//        return StateKey; // Not used in this implementation
//    }

//    public override void OnTriggerEnter(Collider other) { }
//    public override void OnTriggerStay(Collider other) { }
//    public override void OnTriggerExit(Collider other) { }
//    public override void HandleEvent(object eventData) { }
//    public override bool ShouldTransition() { return false; } // Not used in this implementation
//}
