using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{

    public float health = 100f;
    public float maxHealth = 100f;
    public bool IsDead = false;

    public Image healthBarImage;

    public UnityEvent Death;

    public AudioClip DeathSound;

    public UiManager _UiManager;

    public MonoBehaviour movementScript;


    // Update is called once per frame
    void Update()
    {
        healthBarImage.fillAmount = health / maxHealth;
    }

    public void TakeDammage(int dammage)
    {
        if (health < dammage)
        {
            health = 0;
        }
        else
        {
            health -= dammage;

        }
        
        if (health <= 0)
        {
            die();
        }
    }

    public void TakeHeal(int heal) 
    {
        if (health + heal > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += heal;
        }
    }


    public void die()
    {
        Death?.Invoke();
        if (!IsDead)
        {
            IsDead = true;
            if (movementScript != null)
            {
                movementScript.enabled = false; // Désactive le script de mouvement
            }
            if (DeathSound != null)
            {
                AudioSource.PlayClipAtPoint(DeathSound, Vector3.zero);
            }
            _UiManager.ShowDeathScreen();
            StartCoroutine(RespawnAfterDelay(8f));
        }
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (movementScript != null)
        {
            movementScript.enabled = true;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }
}
