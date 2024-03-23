using Sirenix.OdinInspector;
using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    public static event Action OnPlayerDeath;

    public enum PlayerState
    {
        Idle,
        Moving,
        Jumping,
        Crouching,
        Interacting,
        Dead
    }

    [ShowInInspector, ReadOnly, TabGroup("State")]
    private PlayerState currentState = PlayerState.Idle;

    private CharacterController characterController;
    private HealthComponent healthComponent;
    private Transform cam;

    [SerializeField, TabGroup("Movement")]
    private float speed = 6f;

    [SerializeField, TabGroup("Movement")]
    private float turnSmoothTime = 0.1f;

    [SerializeField, TabGroup("Movement")]
    private float gravity = -9.81f;

    [SerializeField, TabGroup("Movement")]
    private float jumpHeight = 2f;

    [SerializeField, TabGroup("Crouch")]
    private float crouchHeight = 1f;

    private float turnSmoothVelocity;
    private Vector3 velocity;
    private bool isCrouching = false;

    [SerializeField, TabGroup("Extra")]
    private float interactRange = 2f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        healthComponent = GetComponent<HealthComponent>();
        cam = Camera.main.transform;
    }

    void OnEnable()
    {
        InputManager.Instance.OnMoveInput += HandleMovement;
        InputManager.Instance.OnJumpInput += HandleJump;
        InputManager.Instance.OnCrouchInput += HandleCrouch;
        InputManager.Instance.OnInteractInput += TryInteract;
        InputManager.Instance.OnHealInput += () => healthComponent.Heal(10);
        InputManager.Instance.OnDamageInput += () => healthComponent.TakeDamage(10);
        HealthManager.OnEntityDeath += HandleEntityDeath;
    }

    void OnDisable()
    {
        InputManager.Instance.OnMoveInput -= HandleMovement;
        InputManager.Instance.OnJumpInput -= HandleJump;
        InputManager.Instance.OnCrouchInput -= HandleCrouch;
        InputManager.Instance.OnInteractInput -= TryInteract;
        InputManager.Instance.OnHealInput -= () => healthComponent.Heal(10);
        InputManager.Instance.OnDamageInput -= () => healthComponent.TakeDamage(10);
        HealthManager.OnEntityDeath -= HandleEntityDeath;
    }

    void Update()
    {
        if (currentState == PlayerState.Dead) return;

        UpdateCharacterController();
    }

    void HandleMovement(float horizontal, float vertical)
    {
        if (currentState == PlayerState.Dead) return;

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            currentState = PlayerState.Moving;

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        else
        {
            currentState = PlayerState.Idle;
        }
    }

    void HandleJump()
    {
        if (currentState == PlayerState.Dead || !characterController.isGrounded) return;

        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        currentState = PlayerState.Jumping;
    }

    void HandleCrouch()
    {
        if (currentState == PlayerState.Dead) return;

        isCrouching = !isCrouching;
        characterController.height = isCrouching ? crouchHeight : 2f;
        currentState = isCrouching ? PlayerState.Crouching : PlayerState.Idle;
    }

    void UpdateCharacterController()
    {
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void TryInteract()
    {
        if (currentState == PlayerState.Dead) return;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactRange))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            IPickable pickable = hit.collider.GetComponent<IPickable>();

            if (interactable != null)
            {
                interactable.Interact();
                if (hit.collider.CompareTag("Tree"))
                {
                    Debug.Log("Interacting with a tree!");
                }
                else if (hit.collider.CompareTag("Rock"))
                {
                    Debug.Log("Interacting with a rock!");
                }
            }
            else if (pickable != null)
            {
                pickable.Pickup(gameObject);
                Destroy(hit.collider.gameObject);
            }
            else
            {
                Debug.Log("No interactable found on the object.");
            }
        }
    }

    void HandleEntityDeath(GameObject entity)
    {
        if (entity == this.gameObject)
        {
            Debug.Log("Player died");
            currentState = PlayerState.Dead;
            OnPlayerDeath?.Invoke();
            PlayerDeath();
        }
    }

    void PlayerDeath()
    {
        this.gameObject.SetActive(false);
    }

    public PlayerState GetCurrentState()
    {
        return currentState;
    }
}

