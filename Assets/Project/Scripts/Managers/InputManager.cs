using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private void Awake()
    {
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
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("Jump"))
        {
            EventDispatcher.Publish(new JumpInputEvent(), "player");
        }

        if (horizontal != 0 || vertical != 0)
        {
            EventDispatcher.Publish(new MoveInputEvent(horizontal, vertical), "player");
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            EventDispatcher.Publish(new CrouchInputEvent(), "player");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            EventDispatcher.Publish(new InteractInputEvent(), "player");
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            EventDispatcher.Publish(new HealInputEvent(), "player");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            EventDispatcher.Publish(new DamageInputEvent(), "player");
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            EventDispatcher.Publish(new InventoryInputEvent(), "player");
        }
    }
}

public class MoveInputEvent
{
    public float Horizontal { get; }
    public float Vertical { get; }

    public MoveInputEvent(float horizontal, float vertical)
    {
        Horizontal = horizontal;
        Vertical = vertical;
    }
}

public class JumpInputEvent { }
public class CrouchInputEvent { }
public class InteractInputEvent { }
public class HealInputEvent { }
public class DamageInputEvent { }
public class InventoryInputEvent { }
