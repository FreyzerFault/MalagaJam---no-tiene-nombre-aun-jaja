using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Controllers
{
    public class PlayerController : Singleton<PlayerController>
    {
        public float speed = 5;
        public float runMultiplier = 2;
        public float gravity = -9.18f;
        public float maxFallSpeed = 2f;
        public float jumpHeight = 3f;

        [SerializeField] private Transform groundCheckT;
        [SerializeField] private  LayerMask groundMask;

        private Vector3 velocity;
        private bool isRunning;
        private Vector2 moveInput;
        private CharacterController controller;
        private CharacterController Controller => controller ??= GetComponent<CharacterController>();

        public bool enabledMovement = true;

        protected override void Awake()
        {
            base.Awake();

            camController = GetComponent<FPSCamController>();
            interactionController = GetComponent<InteractionController>();
            maskController = GetComponent<MaskController>();

            enabledMovement = true;
        }

        private void FixedUpdate()
        {
            if (!enabledMovement) return;
            
            float speedMultiplier = isRunning ? runMultiplier : 1;
            Vector3 moveInput3D = new(moveInput.x, 0, moveInput.y);
        
            Controller.Move(transform.rotation * moveInput3D * (speed * Time.fixedDeltaTime * speedMultiplier));
        
            ApplyGravity();

            Controller.Move(velocity * Time.deltaTime);
        }
    
        private void Jump()
        {
            if (!enabledMovement) return;
            
            velocity.y = Controller.isGrounded ? Mathf.Sqrt(jumpHeight * -2f * gravity) : velocity.y;
        }

        private void ApplyGravity()
        {
            velocity.y += gravity * Time.deltaTime;
        
            if (Controller.isGrounded && velocity.y < 0)
                velocity.y = -maxFallSpeed;
        }

        private void OnMove(InputValue value) => moveInput = value.Get<Vector2>();
        private void OnRun(InputValue value) => isRunning = value.Get<float>() > 0.1f;
        private void OnJump() => Jump();

        
        #region OTHER CONTROLLERS

        [HideInInspector] public FPSCamController camController;
        [HideInInspector] public InteractionController interactionController;
        [HideInInspector] public MaskController maskController;
        [HideInInspector] public PlayerSfxController sfxController;

        #endregion
        
    
        #region DEBUGGING

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Controller.isGrounded ? Color.red : Color.aquamarine;
            Gizmos.DrawWireCube(groundCheckT.position, Vector3.one * Controller.stepOffset);
        }

        #endregion
    }
}
