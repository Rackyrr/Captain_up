using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace BUT
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        private bool Isdead = false;

        [SerializeField]
        Movement m_Movement;

        float m_CurrentSpeed;
        public float CurrentSpeed
        {
            set
            {
                if (m_CurrentSpeed == value) return;
                m_CurrentSpeed = value;
                OnSpeedChange?.Invoke(m_CurrentSpeed);
            }
            get => m_CurrentSpeed;
        }

        bool m_IsSprinting;
        public bool IsSprinting { set => m_IsSprinting = value; get => m_IsSprinting; }

        private bool m_IsMoving;
        public bool IsMoving
        {
            set
            {
                if (m_IsMoving == value) return;
                m_IsMoving = value;
                OnMovingChange?.Invoke(m_IsMoving);
            }
            get => m_IsMoving;
        }

        private Vector3 m_Direction;
        public Vector3 Direction { set => m_Direction = value; get => m_Direction; }
        public Vector3 FullDirection => (GroundRotationOffset * Direction * CurrentSpeed + Vector3.up * GravityVelocity);

        private Quaternion m_GroundRotationOffset;
        public Quaternion GroundRotationOffset { set => m_GroundRotationOffset = value; get => m_GroundRotationOffset; }

        public const float GRAVITY = -9.31f;

        private float m_GravityVelocity;
        public float GravityVelocity { set => m_GravityVelocity = value; get => m_GravityVelocity; }

        private int m_JumpNumber;
        public int JumpNumber { set => m_JumpNumber = value; get => m_JumpNumber; }

        [SerializeField]
        float m_RayLenght;
        [SerializeField]
        LayerMask m_RayMask;

        RaycastHit m_Hit;

        private bool m_IsGrounded;
        public bool IsGrounded
        {
            set
            {
                if (IsGrounded == value) return;
                m_IsGrounded = value;
                OnGroundedChange?.Invoke(m_IsGrounded);
            }
            get => m_IsGrounded;
        }

        private CharacterController m_CharacterController;
        private Vector2 m_MovementInput;
        private Vector3 m_MovementDirection;

        public UnityEvent<float> OnSpeedChange;
        public UnityEvent<bool> OnMovingChange;
        public UnityEvent<bool> OnGroundedChange;
        public UnityEvent OnPlayerDeath;

        private void Awake()
        {
            m_CharacterController = GetComponent<CharacterController>();
        }

        private void OnDisable()
        {
            IsMoving = false;
        }

        private void OnEnable()
        {
            StartCoroutine(Moving());
        }

        IEnumerator Moving()
        {
            while (enabled)
            {
                if (Isdead) yield return new WaitForFixedUpdate(); // Arrêter les mouvements si mort

                if (m_MovementInput.magnitude > 0.1f)
                {
                    if (!IsMoving) IsMoving = true;
                    m_MovementInput = Vector3.ClampMagnitude(m_MovementInput, 1);
                }
                else if (IsMoving)
                {
                    IsMoving = false;
                }

                ManageDirection();
                ManageGravity();
                if (IsMoving) ApplyRotation();
                ApplyMovement();

                yield return new WaitForFixedUpdate();
            }
        }

        public void SetInputMove(InputAction.CallbackContext _context)
        {
            if (Isdead) return;
            m_MovementInput = _context.ReadValue<Vector2>();
        }

        public void SetInputJump(InputAction.CallbackContext _context)
        {
            if (Isdead) return;
            if (!_context.started || (!m_CharacterController.isGrounded && JumpNumber >= m_Movement.MaxJumpNumber)) return;

            if (JumpNumber == 0) StartCoroutine(WaitForLanding());
            JumpNumber++;

            GravityVelocity += m_Movement.MinimazeJumpPower
                ? m_Movement.JumpPower / JumpNumber
                : m_Movement.JumpPower;
        }

        IEnumerator WaitForLanding()
        {
            yield return new WaitUntil(() => !m_CharacterController.isGrounded);
            yield return new WaitUntil(() => m_CharacterController.isGrounded);
            JumpNumber = 0;
        }

        public void SetInputSprint(InputAction.CallbackContext _context)
        {
            if (Isdead) return;
            IsSprinting = _context.started || _context.performed;
        }

        private void ManageDirection()
        {
            if (Isdead) return;

            m_MovementDirection = new Vector3(m_MovementInput.x, 0, m_MovementInput.y);
            m_MovementDirection = Camera.main.transform.TransformDirection(m_MovementDirection);
            m_MovementDirection.y = transform.forward.y;

            if (Physics.Raycast(transform.position, -transform.up, out m_Hit, m_RayLenght, m_RayMask))
            {
                IsGrounded = true;
                float angleOffset = Vector3.SignedAngle(transform.up, m_Hit.normal, transform.right);
                GroundRotationOffset = Quaternion.AngleAxis(angleOffset, transform.right);
            }
            else
            {
                IsGrounded = m_CharacterController.isGrounded;
                GroundRotationOffset = Quaternion.identity;
            }

            m_MovementDirection.Normalize();
            Direction = m_MovementDirection;

            CurrentSpeed = ((IsSprinting) ? m_Movement.SprintFactor : 1) * m_Movement.MaxSpeed * m_Movement.SpeedFactor.Evaluate(m_MovementInput.magnitude);
        }

        public void ApplyRotation()
        {
            if (Isdead || !IsMoving) return;

            Quaternion targetRotation = Quaternion.LookRotation(Direction, transform.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,
                m_Movement.MaxAngularSpeed * Mathf.Deg2Rad * m_Movement.AngularSpeedFactor.Evaluate(Direction.magnitude) * Time.deltaTime);
        }

        public void ApplyMovement()
        {
            if (Isdead) return;
            m_CharacterController.Move(FullDirection * Time.deltaTime);
        }

        private void ManageGravity()
        {
            if (Isdead) return;

            if (m_CharacterController.isGrounded && GravityVelocity < 0.0f)
            {
                GravityVelocity = -1;
            }
            else
            {
                GravityVelocity += GRAVITY * m_Movement.GravityMultiplier * Time.deltaTime;
            }
        }

        public void SetIsDead(bool isDead)
        {
            Isdead = isDead;
        }
    }
}
