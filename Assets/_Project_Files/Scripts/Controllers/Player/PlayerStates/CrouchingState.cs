//using UnityEngine;

//public class CrouchingState : BaseState<PlayerStates>
//{
//    private PlayerStateManager playerManager;

//    public CrouchingState() : base(PlayerStates.Crouching) { }

//    public override void Initialize(StateManager<PlayerStates> stateManager)
//    {
//        playerManager = stateManager as PlayerStateManager;
//    }

//    public override void EnterState()
//    {
//        playerManager.Animator.SetBool("isCrouching", true);
//        // Reduce the character controller height
//        playerManager.CharacterController.height = playerManager.CrouchHeight;
//    }

//    public override void UpdateState()
//    {
//        if (!Input.GetKey(KeyCode.LeftControl))
//        {
//            playerManager.TransitionToState(PlayerStates.StandingUp);
//        }
//        else
//        {
//            float horizontal = Input.GetAxisRaw("Horizontal");
//            float vertical = Input.GetAxisRaw("Vertical");
//            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

//            if (direction.magnitude >= 0.1f)
//            {
//                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerManager.Cam.eulerAngles.y;
//                float angle = Mathf.SmoothDampAngle(playerManager.transform.eulerAngles.y, targetAngle, ref playerManager.TurnSmoothVelocity, playerManager.TurnSmoothTime);
//                playerManager.transform.rotation = Quaternion.Euler(0f, angle, 0f);

//                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
//                playerManager.CharacterController.Move(moveDir.normalized * (playerManager.Speed * 0.5f) * Time.deltaTime);
//            }
//        }
//    }


//    public override void ExitState()
//    {
//        playerManager.Animator.SetBool("isCrouching", false);
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
