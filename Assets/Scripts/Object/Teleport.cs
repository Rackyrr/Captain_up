using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform player, destination;
    public GameObject playerg;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerg.SetActive(false);
            player.position = destination.position;
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false; // Désactive pour réinitialiser
                controller.transform.position = destination.position; // Assurer la position exacte
                controller.enabled = true;  // Réactive le CharacterController
            }

            playerg.SetActive(true);
        }
    }

}
