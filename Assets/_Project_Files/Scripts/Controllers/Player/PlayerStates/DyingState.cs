//using UnityEngine;

//public class DyingState : BaseState<PlayerStates>
//{
//    private PlayerStateManager playerManager;
//    private float dyingDuration = 1f; 
//    private float timer;

//    public DyingState() : base(PlayerStates.Dying) { }

//    public override void Initialize(StateManager<PlayerStates> stateManager)
//    {
//        playerManager = stateManager as PlayerStateManager;
//    }

//    public override void EnterState()
//    {
//        playerManager.Animator.SetBool("isDying", true);
//        timer = 0f;
//    }

//    public override void UpdateState()
//    {
//        timer += Time.deltaTime;

//        if (timer >= dyingDuration)
//        {
//            playerManager.TransitionToState(PlayerStates.Dead);
//        }
//    }

//    public override void ExitState()
//    {
//        playerManager.Animator.SetBool("isDying", false);
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
