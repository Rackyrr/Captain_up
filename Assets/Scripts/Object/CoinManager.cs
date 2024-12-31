using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public int NbCoin = 0;
    public TextMeshProUGUI CoinText;
    public AudioClip coinSound; 

    void Update()
    {
        CoinText.text = NbCoin + " G";
    }

    public void AddCoin()
    {
        NbCoin += 1;

        if (coinSound != null)
        {
            AudioSource.PlayClipAtPoint(coinSound, Vector3.zero);
        }
    }
}
