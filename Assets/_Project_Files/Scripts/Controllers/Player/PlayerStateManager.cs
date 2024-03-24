using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerStateManager : StateManager<PlayerStates>
{
    [FoldoutGroup("Components")]
    public Animator Animator;

    [FoldoutGroup("Components")]
    public CharacterController CharacterController;

    [FoldoutGroup("Components")]
    public Transform Cam;

    [FoldoutGroup("Movement Settings")]
    public float Speed = 6f;

    [FoldoutGroup("Movement Settings")]
    public float TurnSmoothTime = 0.1f;

    [FoldoutGroup("Movement Settings")]
    public float Gravity = -9.81f;

    [FoldoutGroup("Crouching Settings")]
    public float CrouchHeight = 1f;

    [FoldoutGroup("State Info"), ShowInInspector, ReadOnly]
    private string CurrentStateName => CurrentState != null ? CurrentState.ToString() : "None";

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        CharacterController = GetComponent<CharacterController>();
        Cam = Camera.main.transform;
    }

    protected override void InitializeStates()
    {
        AddState(PlayerStates.Idle, new IdleState(Animator));
        AddState(PlayerStates.Walking, new WalkingState(Animator, CharacterController, Cam, Speed, TurnSmoothTime));
        AddState(PlayerStates.Running, new RunningState(Animator, CharacterController, Cam, Speed, TurnSmoothTime));
        //AddState(PlayerStates.Jumping, new JumpingState(Animator, CharacterController, Gravity));
        //AddState(PlayerStates.Falling, new FallingState(Animator, CharacterController, Gravity));
        //AddState(PlayerStates.Landing, new LandingState(Animator, CharacterController));
        //AddState(PlayerStates.Crouching, new CrouchingState(Animator, CharacterController, CrouchHeight));
        //AddState(PlayerStates.StandingUp, new StandingUpState(Animator, CharacterController));
        //AddState(PlayerStates.Climbing, new ClimbingState(Animator, CharacterController));
        //AddState(PlayerStates.Dying, new DyingState(Animator, CharacterController));
        //AddState(PlayerStates.Dead, new DeadState(Animator, CharacterController));

        CurrentState = States[PlayerStates.Idle];
    }
}
