using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;


public class PlayerCombatSystem : MonoBehaviour
{
    public UnityEvent OnAttackInput;

    private bool _isAttacking;

    [SerializeField]
    private float DamageAfterTime;

    [SerializeField]
    private int NbDamage;

    [SerializeField]
    private AttackArea _attackArea;

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed)return;
        
        if(_isAttacking) return;
        OnAttackInput?.Invoke();
        StartCoroutine("Hit");
    }

    private IEnumerator Hit(){
        _isAttacking = true;
        yield return new WaitForSeconds(DamageAfterTime);
        foreach (var attackAreaDamageable in _attackArea.Damageables)
        {
            attackAreaDamageable.Damage(NbDamage);
        }
        yield return new WaitForSeconds(DamageAfterTime);
        _isAttacking = false;
    }
}