using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController_Simple : MonoBehaviour
{
    [SerializeField] private Transform groundCheckPointT;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private string groundLayerName = "Ground";
    
    public float speed = 5;
    public float runMultiplier = 2;
    public float maxFallSpeed = 2f;
    public float jumpHeight = 3f;

    private Vector2 moveInput;
    
    private bool isRunning;
    private int GroundLayerMask => LayerMask.GetMask(groundLayerName);
    private bool IsGrounded => Physics.CheckBox(
            groundCheckPointT.position,
            Vector3.one * groundCheckRadius,
            transform.rotation,
            GroundLayerMask
        );
    
    private Rigidbody rb;

    private void Awake()
    {
        rb  = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = maxFallSpeed;
    }
    
    private void FixedUpdate() => Move();

    
    #region MOVEMENT

    private void Move()
    {
        if (moveInput == Vector2.zero) return;

        float speedMultiplier = isRunning ? runMultiplier : 1;
        
        Vector3 moveInput3D = new(moveInput.x, 0, moveInput.y);
        transform.Translate(moveInput3D * (speed * Time.fixedDeltaTime * speedMultiplier), Space.Self);
    }
    
    private void Jump()
    {
        Vector3 velocity = rb.linearVelocity;
        velocity.y = IsGrounded ? jumpHeight * 2f : velocity.y;
        rb.linearVelocity = velocity;
    }

    #endregion
    
    
    private void OnMove(InputValue value) => moveInput = value.Get<Vector2>();
    private void OnRun(InputValue value) => isRunning = value.Get<float>() > 0.1f;
    private void OnJump() => Jump();
    
    
    
    #region DEBUGGING

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = IsGrounded ? Color.red : Color.aquamarine;
        Gizmos.DrawWireCube(groundCheckPointT.position, Vector3.one * groundCheckRadius);
    }

    #endregion
}
