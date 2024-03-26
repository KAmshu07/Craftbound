using UnityEngine;

public class RunningState : BaseState<PlayerStates>
{
    private Animator animator;
    private CharacterController characterController;
    private Transform cam;
    private float speed;
    private float turnSmoothTime;
    private float turnSmoothVelocity;

    public RunningState(Animator animator, CharacterController characterController, Transform cam, float speed, float turnSmoothTime)
        : base(PlayerStates.Running)
    {
        this.animator = animator;
        this.characterController = characterController;
        this.cam = cam;
        this.speed = speed;
        this.turnSmoothTime = turnSmoothTime;
    }

    public override void Initialize(StateManager<PlayerStates> stateManager)
    {
        // Initialization logic, if any
    }

    public override void EnterState()
    {
        animator.SetBool("isRunning", true);
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        animator.SetBool("isRunning", false);
    }

    public override PlayerStates GetNextState()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude < 0.1f)
        {
            return PlayerStates.Idle;
        }
        else if (Input.GetButtonDown("Jump"))
        {
            return PlayerStates.Jumping;
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            return PlayerStates.Crouching;
        }
        else if (!Input.GetKey(KeyCode.LeftShift))
        {
            return PlayerStates.Walking;
        }
        else if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(characterController.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            characterController.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDir.normalized * speed * 1.5f * Time.deltaTime); // Increase speed by 1.5x for running
        }
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void HandleEvent(object eventData) { }
    public override bool ShouldTransition() { return true; }
}