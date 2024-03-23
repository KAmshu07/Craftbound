using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class InputManager : MonoBehaviour
{
    // Singleton instance for easy access
    public static InputManager Instance { get; private set; }

    // Events
    [FoldoutGroup("Events"), LabelText("On Move Input")]
    public event UnityAction<float, float> OnMoveInput;

    [FoldoutGroup("Events"), LabelText("On Jump Input")]
    public event UnityAction OnJumpInput;

    [FoldoutGroup("Events"), LabelText("On Crouch Input")]
    public event UnityAction OnCrouchInput;

    [FoldoutGroup("Events"), LabelText("On Interact Input")]
    public event UnityAction OnInteractInput;

    [FoldoutGroup("Events"), LabelText("On Heal Input")]
    public event UnityAction OnHealInput;

    [FoldoutGroup("Events"), LabelText("On Damage Input")]
    public event UnityAction OnDamageInput;

    // Input states with Odin attributes
    [FoldoutGroup("Input States"), ShowInInspector, ReadOnly, LabelText("Horizontal Input")]
    private float horizontalInput;

    [FoldoutGroup("Input States"), ShowInInspector, ReadOnly, LabelText("Vertical Input")]
    private float verticalInput;

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // Update input values and invoke events
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        OnMoveInput?.Invoke(horizontalInput, verticalInput);

        if (Input.GetButtonDown("Jump"))
        {
            OnJumpInput?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            OnCrouchInput?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            OnInteractInput?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            OnHealInput?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            OnDamageInput?.Invoke();
        }
    }
}
