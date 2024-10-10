using UnityEngine;

namespace BUT
{
    [CreateAssetMenu(fileName = "Movement", menuName = "BUT/Movement", order = 1)]
    public class Movement : ScriptableObject
    {
        [SerializeField]
        float m_MaxSpeed = 8;
        public float MaxSpeed => m_MaxSpeed;

        [SerializeField]
        AnimationCurve m_SpeedFactor;
        public AnimationCurve SpeedFactor => m_SpeedFactor;

        [SerializeField]
        float m_SprintFactor;
        public float SprintFactor => m_SprintFactor;

        [SerializeField]
        float m_MaxAngularSpeed = 180;
        public float MaxAngularSpeed => m_MaxAngularSpeed;

        [SerializeField]
        AnimationCurve m_AngularSpeedFactor;
        public AnimationCurve AngularSpeedFactor => m_AngularSpeedFactor;

        [SerializeField]
        float m_GravityMultiplier = 3.0f;
        public float GravityMultiplier => m_GravityMultiplier;

        [SerializeField]
        float m_JumpPower;
        public float JumpPower => m_JumpPower;

        [SerializeField]
        int m_MaxJumpNumber;
        public int MaxJumpNumber => m_MaxJumpNumber;

        [SerializeField]
        private bool m_MinimazeJumpPower;
        public bool MinimazeJumpPower => m_MinimazeJumpPower;
    }
}