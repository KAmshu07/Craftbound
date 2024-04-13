using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : EventReceiver
{
    private CharacterController characterController;
    private HealthComponent healthComponent;
    private Transform cam;

    [SerializeField, FoldoutGroup("Movement")]
    private float speed = 6f;

    [SerializeField, FoldoutGroup("Movement")]
    private float turnSmoothTime = 0.1f;

    [SerializeField, FoldoutGroup("Movement")]
    private float gravity = -9.81f;

    [SerializeField, FoldoutGroup("Movement")]
    private float jumpHeight = 2f;

    [SerializeField, FoldoutGroup("Crouch")]
    private float crouchHeight = 1f;

    private float turnSmoothVelocity;
    private Vector3 velocity;
    private bool isCrouching = false;

    [SerializeField, FoldoutGroup("Extra")]
    private float interactRange = 2f;
    private Inventory inventory;

    [SerializeField, FoldoutGroup("Inventory")]
    private GameObject inventoryUI;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        healthComponent = GetComponent<HealthComponent>();
        cam = Camera.main.transform;
        inventory = GetComponent<Inventory>();
    }

    protected void OnEnable()
    {
        Subscribe<MoveInputEvent>(HandleMoveInput, "player");
        Subscribe<JumpInputEvent>(HandleJumpInput, "player");
        Subscribe<CrouchInputEvent>(HandleCrouchInput, "player");
        Subscribe<InteractInputEvent>(HandleInteractInput, "player");
        Subscribe<HealInputEvent>(HandleHealInput, "player");
        Subscribe<DamageInputEvent>(HandleDamageInput, "player");
        Subscribe<InventoryInputEvent>(HandleInventoryInput, "player");
        HealthManager.OnHealthChanged += HandleHealthChanged;
        HealthManager.OnEntityDeath += HandleEntityDeath;
    }

    protected void OnDisable()
    {
        HealthManager.OnHealthChanged -= HandleHealthChanged;
        HealthManager.OnEntityDeath -= HandleEntityDeath;
    }

    private void Update()
    {
        UpdateCharacterController();
    }

    private void UpdateCharacterController()
    {
        if (!characterController.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandleMoveInput(MoveInputEvent inputEvent)
    {
        float horizontal = inputEvent.Horizontal;
        float vertical = inputEvent.Vertical;
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

    private void HandleJumpInput(JumpInputEvent inputEvent)
    {
        if (characterController.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void HandleCrouchInput(CrouchInputEvent inputEvent)
    {
        if (characterController.isGrounded)
        {
            isCrouching = !isCrouching;
            characterController.height = isCrouching ? crouchHeight : 2f;
        }
    }

    private void HandleInteractInput(InteractInputEvent inputEvent)
    {
        TryInteract();
    }

    private void HandleHealInput(HealInputEvent inputEvent)
    {
        healthComponent.Heal(10);
    }

    private void HandleDamageInput(DamageInputEvent inputEvent)
    {
        healthComponent.TakeDamage(10);
    }

    private void HandleInventoryInput(InventoryInputEvent inputEvent)
    {
        ToggleInventory();
    }

    private void TryInteract()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactRange))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            IPickable pickable = hit.collider.GetComponent<IPickable>();

            if (interactable != null)
            {
                interactable.Interact();
            }
            else if (pickable != null)
            {
                pickable.Pickup(gameObject);
                Destroy(hit.collider.gameObject);
            }
        }
    }

    private void ToggleInventory()
    {
        InventoryUIManager uiManager = FindObjectOfType<InventoryUIManager>();
        if (uiManager != null)
        {
            uiManager.ToggleInventory();
        }
    }

    [FoldoutGroup("Health Events")]
    void HandleHealthChanged(GameObject entity, int newHealth)
    {
        if (entity == this.gameObject)
        {
            if (newHealth <= 0)
            {
                HealthManager.RaiseEntityDeath(this.gameObject);
            }
        }
    }

    void HandleEntityDeath(GameObject entity)
    {
        if (entity == this.gameObject)
        {
            Debug.Log("Player died");
            this.gameObject.SetActive(false);
        }
    }
}
