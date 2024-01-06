using UnityEngine;
using Sirenix.OdinInspector;

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

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        healthComponent = GetComponent<HealthComponent>();
        cam = Camera.main.transform;
    }

    void Update()
    {
        HandleJump();
        HandleCrouch();
        HandleMovement();
        UpdateCharacterController();
        HandleInput();
    }

    [FoldoutGroup("Collision Events")]
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Tree"))
        {
            Debug.Log("Player has collided with the tree!");
            cuttingTree = hit.collider.gameObject.GetComponent<CuttingTree>();
            if (Input.GetKeyDown(KeyCode.C)) cuttingTree.StartCutting();
        }
    }

    void OnEnable()
    {
        EventManager.OnHealthChanged += HandleHealthChanged;
        EventManager.OnEntityDeath += HandleEntityDeath;
    }

    void OnDisable()
    {
        EventManager.OnHealthChanged -= HandleHealthChanged;
        EventManager.OnEntityDeath -= HandleEntityDeath;
    }

    [FoldoutGroup("Movement")]
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

    [FoldoutGroup("Movement")]
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

    [FoldoutGroup("Crouch")]
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
                EventManager.RaiseEntityDeath(this.gameObject);
            }
        }
    }

    [FoldoutGroup("Health Events")]
    void HandleEntityDeath(GameObject entity)
    {
        if (entity == this.gameObject)
        {
            Debug.Log("Player died");
            this.gameObject.SetActive(false);
        }
    }

    [FoldoutGroup("Input")]
    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.C) && cuttingTree != null)
        {
            // Initiate cutting the tree
            cuttingTree.StartCutting();
        }

        // Check for healing button (H)
        if (Input.GetKeyDown(KeyCode.H))
        {
            healthComponent.Heal(10); // Adjust the amount as needed
        }

        // Check for damaging button (D)
        if (Input.GetKeyDown(KeyCode.D))
        {
            healthComponent.TakeDamage(10); // Adjust the damage amount as needed
        }
    }
}
