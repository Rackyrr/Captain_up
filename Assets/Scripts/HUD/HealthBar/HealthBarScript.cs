using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BUT;

public class HealthBarScript : MonoBehaviour
{

    public float health = 100f;
    public float maxHealth = 100f;
    public bool IsDead = false;

    public Image healthBarImage;

    public UnityEvent Death;

    public AudioClip DeathSound;

    public UiManager _UiManager;

    //Pour arrêter les inputs
    public PlayerInteract _playerInteract;
    public PlayerCombatSystem _playerCombatSystem;
    public PlayerMovement _playerMovement;


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
            _playerInteract.SetIsDead(true);
            _playerCombatSystem.SetIsDead(true);
            _playerMovement.SetIsDead(true);
            if (DeathSound != null)
            {
                PlayDeathSoundOnCamera();
            }
            _UiManager.ShowDeathScreen();
            StartCoroutine(RespawnAfterDelay(8f));
        }
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void PlayDeathSoundOnCamera()
    {
        // Récupérer la caméra principale
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            // Vérifier si la caméra a déjà un AudioSource
            AudioSource audioSource = mainCamera.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                // Ajouter un AudioSource si absent
                audioSource = mainCamera.gameObject.AddComponent<AudioSource>();
            }

            // Configurer et jouer le son
            audioSource.clip = DeathSound;
            audioSource.volume = 0.1f; // Régle le volume
            audioSource.spatialBlend = 0f; // Son 2D (non-spatialisé)
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Aucune caméra principale n'est assignée dans la scène !");
        }
    }
}
