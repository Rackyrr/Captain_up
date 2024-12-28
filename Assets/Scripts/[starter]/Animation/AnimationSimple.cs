using UnityEngine;

namespace BUT
{
    public class AnimationSimple : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField]
        Animator m_Animator;

        [SerializeField]
        float m_SpeedMultiplier;

        public void Moving(bool moving)
        {
            m_Animator?.SetBool("Move", moving);
        }

        public void ChangeSpeed(float speed)
        {
            m_Animator?.SetFloat("Speed", speed * m_SpeedMultiplier);
        }

        public void ChangeGrounded(bool grounded)
        {
            m_Animator?.SetBool("IsGrounded", grounded);
        }

        public void Die()
        {
            m_Animator?.SetTrigger("Die");

        }
    }
}
