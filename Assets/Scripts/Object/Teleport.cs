using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;


[RequireComponent(typeof(AudioSource), typeof(Collider))]

public class TeleportComponent : MonoBehaviour
{
    public TeleportComponent destination;
    public ParticleSystem teleportEffect;
    private AudioSource m_AudioSource;
    private Coroutine m_Coroutine; // Pour stocker la coroutine en cours

    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return; // Vérifie si l'objet est le joueur
        // Démarre la téléportation avec un délai
        m_Coroutine = StartCoroutine(TeleportAfterDelay(other));
        teleportEffect.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || m_Coroutine == null) return;
        StopCoroutine(m_Coroutine);
        teleportEffect.Stop();
        m_Coroutine = null;
    }

    private IEnumerator TeleportAfterDelay(Collider player)
    {
        yield return new WaitForSeconds(2f); // Attend 2 secondes

        // Vérifie que la coroutine n'a pas été annulée
        if (player)
        {
            destination.m_AudioSource.Play();
            player.transform.position = destination.transform.position;
        }
        m_Coroutine = null; // Réinitialise la coroutine
        teleportEffect.Stop();
    }
}
