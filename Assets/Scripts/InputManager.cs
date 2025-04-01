using UnityEngine.Events;
using UnityEngine;
using System.Collections; 
using TMPro;

public class InputManager : MonoBehaviour
{
    public UnityEvent OnSpacePressed = new();
    public UnityEvent OnShiftPressed = new();
    public UnityEvent<char> OnMousePressed = new();
    public UnityEvent<Vector2> OnMove = new();
    public UnityEvent<Vector2> OnLook = new();
    [SerializeField] private TMP_Text slowMoMessage;
    private bool hasUsedSlowMo = false;

    //public UnityEvent OnResetPressed = new UnityEvent();
    private void LateUpdate()
    {
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSpacePressed?.Invoke();
        }
        if (Input.GetMouseButton(0))
        {
            OnMousePressed?.Invoke('a');
        }
        if (Input.GetMouseButton(1))
        {
            OnMousePressed?.Invoke('b');
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            OnShiftPressed?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Q) && !hasUsedSlowMo) 
        {
            StartCoroutine(SlowMo());
        }

    }
    void FixedUpdate()
    {
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
    }

    IEnumerator SlowMo()
    {
        hasUsedSlowMo = true; 

        Time.timeScale = 0.5f; 
        yield return new WaitForSecondsRealtime(3); 
        Time.timeScale = 1f; 

        if (slowMoMessage != null)
        {
            slowMoMessage.text = "Slowmo used: 0 remaining";
            slowMoMessage.enabled = true;

            for (int i = 0; i < 4; i++)
            {
                slowMoMessage.enabled = !slowMoMessage.enabled;
                yield return new WaitForSeconds(0.5f);
            }

            slowMoMessage.enabled = false;
        }
        else
        {
            Debug.LogError("SlowMoMessage is not assigned in the Inspector!");
        }
    }
}