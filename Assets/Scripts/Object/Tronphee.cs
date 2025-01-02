using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tronphee : MonoBehaviour
{

    public GameObject onCollectEffect;
    public AudioClip tronpheeSound;
    public UiManager _UiManager;

    public string playerTag = "Player";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0.1f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Destroy(gameObject);
            Instantiate(onCollectEffect, transform.position, transform.rotation);
            if (tronpheeSound != null)
            {
                AudioSource.PlayClipAtPoint(tronpheeSound, Vector3.zero);
            }
            _UiManager.ShowEndGame();
        }
    }
}
