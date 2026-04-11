using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "StayIn/Input Reader")]
public class InputReader : ScriptableObject, StayInInput.IPlayerActions {

    public event UnityAction<Vector2> MoveEvent = delegate { };
    public event UnityAction InteractEvent = delegate { };
    public event UnityAction InteractCanceledEvent = delegate { };

    private StayInInput GameInput;

    private void OnEnable() {
        if(GameInput == null) {
            GameInput = new StayInInput();
            GameInput.Player.SetCallbacks(this);
        }
        GameInput.Player.Enable();
    }

    private void OnDisable() {
        GameInput.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context) {
        MoveEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnInteract(InputAction.CallbackContext context) {
        if(context.phase == InputActionPhase.Performed) {
            InteractEvent.Invoke();
        } else if (context.phase == InputActionPhase.Canceled) {
            InteractCanceledEvent.Invoke();
        }
    }

    
}
