using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionsManager : MonoBehaviour
{
    private InputActionAsset _actions;

    private void Awake()
    {
        _actions = gameObject.GetComponent<PlayerInput>().actions;
        if (_actions is null)
        {
            Debug.LogError("PlayerActionsManager has no InputActionAsset assigned!");
        }
    }

    private void OnChangeRollBtn()
    {
        Debug.Log("Press any button to set Roll action");
        
        var action = _actions.FindAction("Roll");
        if (action is null)
        {
            return;
        }
        
        action.Disable();
        action.PerformInteractiveRebinding(0)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(_ =>
            {
                Debug.Log($"Now to Roll press {action.bindings.First().effectivePath}");
                action.Enable();
            })
            .OnCancel(_ =>
            {
                Debug.Log($"Cancelled");
                action.Enable();
            })
            .Start();
    }
}
