using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    private CharacterController characterController;
    private HealthComponent healthComponent;
    private Transform cam;
    private CuttingTree cuttingTree;

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

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        healthComponent = GetComponent<HealthComponent>();
        cam = Camera.main.transform;
        inventory = GetComponent<Inventory>(); // Add this line
    }


    void Update()
    {
        HandleJump();
        HandleCrouch();
        HandleMovement();
        UpdateCharacterController();
        HandleInput();
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //{
        //    Debug.Log("Player has collided with" + hit.gameObject.name);
        //    if (Input.GetKeyDown(KeyCode.E))
        //    {
        //        TryInteract();
        //    }
        //}
    }

    void OnEnable()
    {
        HealthManager.OnHealthChanged += HandleHealthChanged;
        HealthManager.OnEntityDeath += HandleEntityDeath;
    }

    void OnDisable()
    {
        HealthManager.OnHealthChanged -= HandleHealthChanged;
        HealthManager.OnEntityDeath -= HandleEntityDeath;
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
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

    void HandleJump()
    {
        if (characterController.isGrounded)
        {
            velocity.y = -2f;

            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
    }

    void HandleCrouch()
    {
        if (characterController.isGrounded && Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;
            characterController.height = isCrouching ? crouchHeight : 2f;
        }
    }

    void UpdateCharacterController()
    {
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
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

    void HandleInput()
    {

        if (Input.GetKeyDown(KeyCode.H))
        {
            healthComponent.Heal(10);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            healthComponent.TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenInventory();
        }
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
                if (hit.collider.CompareTag("Tree"))
                {
                    Debug.Log("Interacting with a tree!");
                }

                else if (hit.collider.CompareTag("Rock"))
                {
                    Debug.Log("Interacting with an rock!");
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

    private void OpenInventory()
    {
        Debug.Log("Opening Inventory:");
        foreach (var category in System.Enum.GetValues(typeof(ItemCategory)))
        {
            var items = inventory.GetItemsByCategory((ItemCategory)category);
            if (items.Count > 0)
            {
                Debug.Log($"{category}:");
                foreach (var item in items)
                {
                    Debug.Log($"- {item.ItemName} x{item.Quantity}");
                }
            }
        }
    }
}