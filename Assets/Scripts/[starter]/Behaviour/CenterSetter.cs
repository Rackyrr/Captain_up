using System.Collections;
using UnityEngine;

namespace BUT
{
    /* Calculate center of the game according to Player and 3D center */
    public class CenterSetter : MonoBehaviour
    {
        [SerializeField]
        Transform m_Player;

        // center of the game zone
        [SerializeField]
        Transform m_GameCenter;

        // speed to adjust position
        [SerializeField]
        float m_Speed;

        // target position
        Vector3 m_TargetPosition;

        void Awake()
        {
            InitializeCenter();
        }

        private void OnEnable()
        {
            UpdateCenter();
        }

        // snap to calculated center
        public void InitializeCenter()
        {
            m_TargetPosition = (m_Player.position + m_GameCenter.position) / 2;
            m_TargetPosition.y = m_Player.position.y;
            transform.position = m_TargetPosition;
        }

        // start updating calculated center
        private void UpdateCenter()
        {
            StartCoroutine(MoveToMiddle());
        }

        // move to calculated center
        IEnumerator MoveToMiddle()
        {
            while (enabled)
            {
                // calcuate average between player and center of the game zone
                m_TargetPosition = (m_Player.position + m_GameCenter.position) / 2;
                // adjust height with player height
                m_TargetPosition.y = m_Player.position.y;
                // lerp toward target position
                transform.position = Vector3.Lerp(transform.position, m_TargetPosition, m_Speed * Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}