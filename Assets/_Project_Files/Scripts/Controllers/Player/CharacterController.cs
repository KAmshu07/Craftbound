using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    private CharacterController characterController;
    private HealthComponent healthComponent;
    private Transform cam;

    [SerializeField] private float speed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float crouchHeight = 1f;
    private CuttingTree cuttingTree;


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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Tree"))
        {
            Debug.Log("Player has collided with the tree!");
            cuttingTree = hit.collider.gameObject.GetComponent<CuttingTree>();
            if(Input.GetKeyDown(KeyCode.C))cuttingTree.StartCutting();
        }
    }

    void OnEnable()
    {
        EventManager.OnHealthChanged += HandleHealthChanged;
        EventManager.OnEntityDeath += HandleEntityDeath;
    }

    private void OnDisable()
    {
        EventManager.OnHealthChanged -= HandleHealthChanged;
        EventManager.OnEntityDeath -= HandleEntityDeath;
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
