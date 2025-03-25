using System;
using UnityEngine.Events;
using UnityEngine;
using Unity.VisualScripting;

public class InputManager : MonoBehaviour
{
    public UnityEvent OnSpacePressed = new();
    public UnityEvent OnMouseLeftPressed = new();
    public UnityEvent OnMouseRightPressed = new();
    public UnityEvent<Vector2> OnShiftPressed = new();
    public UnityEvent<Vector2> OnMove = new();
    public UnityEvent<Vector2> OnLook = new();

    //public UnityEvent OnResetPressed = new UnityEvent();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSpacePressed?.Invoke();
        }

        Vector2 input = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            input += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            input += Vector2.right;
        }
        if (Input.GetKey(KeyCode.W))
        {
            input += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            input += Vector2.down;
        }

        OnMove?.Invoke(input);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            OnShiftPressed?.Invoke(input);
        }

        Vector2 look = Vector2.zero;
        if (Input.GetAxisRaw("Mouse X") != 0)
        {
            look.x = Input.GetAxisRaw("Mouse X");
        }
        if (Input.GetAxisRaw("Mouse Y") != 0)
        {
            look.y = Input.GetAxisRaw("Mouse Y");
        }

        OnLook?.Invoke(look);

        if (Input.GetMouseButton(0))
        {
            OnMouseLeftPressed?.Invoke();
        }
        if (Input.GetMouseButton(1))
        {
            OnMouseRightPressed?.Invoke();
        }
    }
}