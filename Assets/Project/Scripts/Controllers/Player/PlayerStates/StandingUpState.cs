//using UnityEngine;

//public class StandingUpState : BaseState<PlayerStates>
//{
//    private PlayerStateManager playerManager;
//    private float standingUpDuration = 0.5f; // Duration of the standing up animation
//    private float timer;

//    public StandingUpState() : base(PlayerStates.StandingUp) { }

//    public override void Initialize(StateManager<PlayerStates> stateManager)
//    {
//        playerManager = stateManager as PlayerStateManager;
//    }

//    public override void EnterState()
//    {
//        playerManager.Animator.SetBool("isStandingUp", true);
//        timer = 0f;
//    }

//    public override void UpdateState()
//    {
//        timer += Time.deltaTime;

//        if (timer >= standingUpDuration)
//        {
//            playerManager.ReturnToPreviousState();
//        }
//    }

//    public override void ExitState()
//    {
//        playerManager.Animator.SetBool("isStandingUp", false);
//        playerManager.CharacterController.height = 2f;
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
