using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerInteract : MonoBehaviour
{
    public UnityEvent OnInteractInput;

    private bool _isInteracting;

    [SerializeField]
    private InteractArea _interactArea;

    public void OnInteract(InputAction.CallbackContext context){
        if(!context.performed)return;

        if(_isInteracting) return;
        OnInteractInput?.Invoke();
        StartCoroutine("Interact");
    }

    private IEnumerator Interact(){
        _isInteracting = true;
        yield return new WaitForSeconds(0.5f);
        foreach (var interactivesInArea in _interactArea.Interactives)
        {
            interactivesInArea.Interact();
        }
        yield return new WaitForSeconds(0.5f);
        _isInteracting = false;
    }
}
