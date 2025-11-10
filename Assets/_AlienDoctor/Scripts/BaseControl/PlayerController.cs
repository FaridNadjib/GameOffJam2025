using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] Transform viewCamera;

    Rigidbody rb;
    float forwardInput;
    float sidewaysInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody> ();
    }

    // Update is called once per frame
    void Update()
    {
        forwardInput = Keyboard.current.wKey.isPressed ? 1f:Keyboard.current.sKey.isPressed ? -1f : 0f;
        sidewaysInput = Keyboard.current.dKey.isPressed ? 1f:Keyboard.current.aKey.isPressed ? -1f : 0f;
        
    }

    private void FixedUpdate()
    {
        Vector3 forwardFacing = viewCamera.forward;
        forwardFacing.y = 0f;
        Vector3 sidewaysFacing = viewCamera.right;
        sidewaysFacing.y = 0f;

        rb.linearVelocity = (forwardFacing * forwardInput + sidewaysFacing * sidewaysInput).normalized * moveSpeed;
    }
}
