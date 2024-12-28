using UnityEngine;

public class LockCursor : MonoBehaviour
{
    [SerializeField]
    CursorLockMode m_LockMode;

    private void OnEnable()
    {
        Cursor.lockState = m_LockMode;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
