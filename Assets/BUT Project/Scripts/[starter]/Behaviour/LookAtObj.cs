using System.Collections;
using UnityEngine;

namespace BUT
{
    // rotate towards a target to look at it
    public class LookAtObj : MonoBehaviour
    {
        [SerializeField]
        Transform m_Target;

        private void OnEnable()
        {
            StartCoroutine(Looking());
        }

        IEnumerator Looking()
        {
            while (enabled)
            {
                // modify rotation to look at a transform
                transform.LookAt(m_Target);
                yield return null;
            }
        }
    }
}