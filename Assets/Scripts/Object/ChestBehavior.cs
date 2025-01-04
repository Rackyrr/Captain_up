using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class ChestBehavior : MonoBehaviour, Iinteractive
{
    public Animator _animator;
    
    public int CoinsInChest;

    public CoinManager CoinManager;

    public KeyManager KeyManager;

    public void Interact(){
        int nbKeys = KeyManager.GetNbKeys();
        if (nbKeys <= 0){
            Debug.Log("Clochard t as pas de clé");
            return;
        }
        else if (nbKeys > 0){
            KeyManager.SetNbKeys(nbKeys - 1);
            _animator?.SetBool("Open", true);
            StartCoroutine("Wait");
            CoinManager.AddMultipleCoins(CoinsInChest);
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
    }
}
