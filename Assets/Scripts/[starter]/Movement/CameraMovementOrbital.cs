using UnityEngine;
using UnityEngine.InputSystem;

namespace BUT
{
    /* Handle Camera's rotation */
    public class CameraMovementOrbital : MonoBehaviour
    {
        [SerializeField]
        float m_SpeedRotation;

        public float SpeedRotation => Mathf.Deg2Rad * m_SpeedRotation * Time.deltaTime;

        [SerializeField]
        float m_SpeedFactorGamepad = 2.0f;

        public float SpeedFactorGamepad => m_SpeedFactorGamepad;

        [SerializeField]
        Vector2 m_RotationXLimits = new Vector2(0f, 25f);

        public Vector2 RotationXLimits => m_RotationXLimits;

        Vector3 m_CurrentRotation;

        private void Start(){
            // Init rotation
            m_CurrentRotation = transform.rotation.eulerAngles;
        }

        private void Update(){
            // Vérifie si un Gamepad est connecté
            if (Gamepad.current != null) {

                // Lecture de l'input du joystick droit
                Vector2 input = Gamepad.current.rightStick.ReadValue();
                RotateCamera(input, SpeedFactorGamepad);
            }
        }

        private void RotateCamera(Vector2 input, float speedFactor){
            // Vérifie si l'input est significatif
            if (input.magnitude > 0.1f){
                // Ajout de rotation
                m_CurrentRotation.y += input.x * SpeedRotation * speedFactor; 
                m_CurrentRotation.x += -input.y * SpeedRotation * speedFactor;

                // Limiter la rotation sur l'axe X
                m_CurrentRotation.x = Mathf.Clamp(m_CurrentRotation.x, m_RotationXLimits.x, m_RotationXLimits.y);
                
                // Appliquer la rotation
                transform.rotation = Quaternion.Euler(m_CurrentRotation.x, m_CurrentRotation.y, m_CurrentRotation.z);
            }
        }

        // Callback pour l'input system : surtout pour la souris
        public void Rotate(InputAction.CallbackContext _context){
            Vector2 input = _context.ReadValue<Vector2>();
            RotateCamera(input, 5.0f);
        }
    }
}