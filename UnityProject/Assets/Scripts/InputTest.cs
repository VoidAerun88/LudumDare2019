using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

public class InputTest : MonoBehaviour, GamepadControls.ITestActions
{
    public GamepadControls Controls;

    // Start is called before the first frame update
    void Awake()
    {
        Controls = new GamepadControls();
        Controls.Test.SetCallbacks(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnable()
    {
        Controls.Test.Enable();
    }

    public void OnDisable()
    {
        Controls.Test.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector3 input = context.ReadValue<Vector2>();

        this.transform.position += input * Time.deltaTime;
    }
}
