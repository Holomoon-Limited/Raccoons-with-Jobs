using Holo.Input;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadControls : MonoBehaviour
{
    public static GamepadControls Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public IGamepadLocation activeLocation = null;

    [SerializeField] InputManager input;

    [SerializeField][Min(0f)] private float highlightCooldown = 0.5f;
    private float timeSinceHighlight = Mathf.Infinity;

    private void Start()
    {
        if (InputSystem.GetDevice<Gamepad>() != null)
        {
            input.EnableGamepadControls();
            if (activeLocation != null)
            {
                activeLocation.OnControllerActivated();
            }
        }
    }

    private void OnEnable()
    {
        InputSystem.onDeviceChange += (device, change) =>
        {
            switch (change)
            {
                case InputDeviceChange.Added:
                    //New device
                    if (InputSystem.GetDevice<Gamepad>() != null)
                    {
                        input.EnableGamepadControls();
                        if (activeLocation != null)
                        {
                            activeLocation.OnControllerActivated();
                        }
                    }
                    break;
                case InputDeviceChange.Disconnected:
                    //Device got unplugged
                    if (InputSystem.GetDevice<Gamepad>() == null)
                    {
                        input.EnableKeyboardControls();
                    }
                    break;
                case InputDeviceChange.Reconnected:
                    //Plugged back in
                    if (InputSystem.GetDevice<Gamepad>() != null)
                    {
                        input.EnableGamepadControls();
                        if (activeLocation != null)
                        {
                            activeLocation.OnControllerActivated();
                        }
                    }
                    break;
                case InputDeviceChange.Removed:
                    //Remove from Input System entirelu; by default Devices stay in the system once discovered
                    if (InputSystem.GetDevice<Gamepad>() == null)
                    {
                        input.EnableKeyboardControls();
                    }
                    break;
                default:
                    //See InputDeviceChange reference for other event types
                    break;
            }
        };
    }

    private void Update()
    {
        float navigation = input.Controls.Gamepad.Navigate.ReadValue<float>();
        if (Mathf.Abs(navigation) > Mathf.Epsilon && timeSinceHighlight > highlightCooldown)
        {
            if (activeLocation != null)
            {
                activeLocation.OnNavigate(navigation);
                timeSinceHighlight = 0;
            }
        }
        timeSinceHighlight += Time.deltaTime;
    }
}
