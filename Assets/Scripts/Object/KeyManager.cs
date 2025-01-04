using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyManager : MonoBehaviour
{

    public int NbKeys = 0;
    public TextMeshProUGUI KeyText;
    public AudioClip KeySound;

    void Update()
    {
        KeyText.text = NbKeys + " Keys";
    }

    public void AddKeys()
    {
        NbKeys += 1;

        if (KeySound != null)
        {
            AudioSource.PlayClipAtPoint(KeySound, Vector3.zero);
        }
    }

    public int GetNbKeys(){
        return NbKeys;
    }

    public void SetNbKeys(int nbKeys){
        NbKeys = nbKeys;
    }
}
