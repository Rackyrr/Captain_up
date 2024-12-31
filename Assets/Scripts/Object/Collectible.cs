using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinCoin : MonoBehaviour
{
    public CoinManager CoinManager;
    public GameObject onCollectEffect;

    // Update is called once per frame
    void Update()
    { 
        transform.Rotate(0, 0.3f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        Instantiate(onCollectEffect, transform.position, transform.rotation);
        CoinManager.AddCoin();
    }
}
