//using UnityEngine;

//public class LandingState : BaseState<PlayerStates>
//{
//    private PlayerStateManager playerManager;
//    private float landingDuration = 0.2f; // Duration of the landing animation
//    private float timer;

//    public LandingState() : base(PlayerStates.Landing) { }

//    public override void Initialize(StateManager<PlayerStates> stateManager)
//    {
//        playerManager = stateManager as PlayerStateManager;
//    }

//    public override void EnterState()
//    {
//        playerManager.Animator.SetBool("isLanding", true);
//        timer = 0f; 
//    }

//    public override void UpdateState()
//    {
//        timer += Time.deltaTime;

//        if (timer >= landingDuration)
//        {
//            playerManager.ReturnToPreviousState();
//        }
//    }

//    public override void ExitState()
//    {
//        playerManager.Animator.SetBool("isLanding", false);
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
